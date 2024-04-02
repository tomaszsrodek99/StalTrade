using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StalTradeAPI.Dtos;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;
using System.Globalization;

namespace StalTradeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IDepositRepository _depositRepository;
        public ExpenseController(IMapper mapper, IExpenseRepository expenseRepository, IDepositRepository depositRepository)
        {
            _mapper = mapper;
            _expenseRepository = expenseRepository;
            _depositRepository = depositRepository;
        }

        [HttpGet("GetExpenses")]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpenses()
        {
            try
            {
                var expenses = await _expenseRepository.GetAllAsync();
                if (!expenses.Any())
                {
                    return BadRequest("Nie znaleziono zapisanych kosztów.");
                }
                var records = _mapper.Map<List<ExpenseDto>>(expenses);

                return Ok(records.OrderByDescending(x=>x.Date));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetMonthlyExpenses")]
        public async Task<ActionResult<IEnumerable<MonthlyExpenseViewModel>>> GetMonthlyExpenses()
        {
            try
            {
                var expenses = await _expenseRepository.GetAllAsync();
                var expensesDto = _mapper.Map<List<ExpenseDto>>(expenses.OrderBy(x => x.DateOfPayment));

                var monthlyExpenses = expensesDto
                        .GroupBy(e => e.DateOfPayment.ToString("MMMM yyyy"))
                        .Select(group => new MonthlyExpenseViewModel
                        {
                            Month = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(group.Key),
                            TotalBrutto = group.Sum(e => e.Brutto),
                            Expenses = group.ToList()
                        })
                        .OrderByDescending(m => m.Month)
                        .Take(12);

                return Ok(monthlyExpenses);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateExpense")]
        public async Task<IActionResult> CreateExpense(ExpenseDto dto)
        {
            try
            {
                await _expenseRepository.AddAsync(_mapper.Map<Expense>(dto));

                return Ok(new { success = true, message = "Wydatek został pomyślnie dodany!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateExpense")]
        public async Task<IActionResult> UpdateExpense(ExpenseDto dto)
        {
            try
            {
                await _expenseRepository.UpdateAsync(_mapper.Map<Expense>(dto));

                return Ok(new { success = true, message = "Wydatek został pomyślnie edytowany!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteExpense{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            try
            {
                await _expenseRepository.DeleteAsync(id);

                return Ok(new { success = true, message = "Wydatek został pomyślnie usunięty!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetExpense{id}")]
        public async Task<ActionResult<ExpenseDto>> GetExpense(int id)
        {
            try
            {
                var expense = await _expenseRepository.GetAsync(id);
                var record = _mapper.Map<ExpenseDto>(expense);
                return Ok(record);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetDeposites")]
        public async Task<ActionResult<IEnumerable<DepositDto>>> GetDeposites()
        {
            try
            {
                var deposites = await _depositRepository.GetAllAsync();
                if (!deposites.Any())
                {
                    return BadRequest("Nie znaleziono zapisanych wpłat.");
                }
                var records = _mapper.Map<List<DepositDto>>(deposites);

                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateDeposit")]
        public async Task<IActionResult> CreateDeposit(DepositDto deposit)
        {
            try
            {
                await _depositRepository.AddAsync(_mapper.Map<Deposit>(deposit));
                return Ok(new { success = true, message = "Udało się dokonać wpłatę do kasy!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteDeposit{id}")]
        public async Task<IActionResult> DeleteDeposit(int id)
        {
            try
            {
                await _depositRepository.DeleteAsync(id);
                return Ok(new { success = true, message = "Wpłata została anulowana!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ChangePaidStatus")]
        public async Task<IActionResult> ChangePaidStatus([FromBody] int id)
        {
            try
            {
                var expense = await _expenseRepository.GetAsync(id);
                expense.Paid = true;

                await _expenseRepository.UpdateAsync(expense);

                return Ok(new { success = true, message = $"Zaktualizowano status płatności dla {expense.Contractor}!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AutocompleteContractor")]
        public IActionResult AutocompleteContractor([FromBody] string term)
        {
            var contractors = _expenseRepository.GetContractorsFromDatabase(term);
            return Json(contractors.Distinct());
        }

        [HttpPost("AutocompleteDescription")]
        public IActionResult AutocompleteDescription([FromBody] string term)
        {
            var descriptions = _expenseRepository.GetDescriptionsFromDatabase(term);
            return Json(descriptions.Distinct());
        }

        [HttpPost("AutocompleteEventType")]
        public IActionResult AutocompleteEventType([FromBody] string term)
        {
            var eventTypes = _expenseRepository.GetEventTypesFromDatabase(term);
            return Json(eventTypes.Distinct());
        }
    }
}
