using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StalTradeAPI.Dtos;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;
using StalTradeAPI.Repositories;

namespace StalTradeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IExpenseRepository _expenseRepository;
        public ExpenseController(IMapper mapper, IExpenseRepository expenseRepository)
        {
            _mapper = mapper;
            _expenseRepository = expenseRepository;
        }

        [HttpGet("GetExpenses")]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpenses()
        {
            try
            {
                var expenses = await _expenseRepository.GetAllAsync();
                if (!expenses.Any())
                {
                    return BadRequest("Nie znaleziono kosztów.");
                }
                var records = _mapper.Map<List<ExpenseDto>>(expenses);
                return Ok(records);
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
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateExpense")]
        public async Task<IActionResult> UpdateExpenset(ExpenseDto dto)
        {
            try
            {
                await _expenseRepository.UpdateAsync(_mapper.Map<Expense>(dto));
                return Ok();
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
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ChangePaidStatus/{id}")]
        public async Task<IActionResult> ChangePaidStatus(int id)
        {
            try
            {
                var expense = await _expenseRepository.GetAsync(id);
                expense.Paid = true;
                await _expenseRepository.UpdateAsync(expense);
                return Ok("Pomyślnie zaktualizowano status.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("AutocompleteContractor")]
        public IActionResult AutocompleteContractor(string term)
        {
            var contractors =  _expenseRepository.GetContractorsFromDatabase(term);
            return Json(contractors.Distinct());
        }

        [HttpGet("AutocompleteDescription")]
        public IActionResult AutocompleteDescription(string term)
        {
            var descriptions = _expenseRepository.GetDescriptionsFromDatabase(term);
            return Json(descriptions.Distinct());
        }

        [HttpGet("AutocompleteEventType")]
        public IActionResult AutocompleteEventType(string term)
        {
            var eventTypes = _expenseRepository.GetEventTypesFromDatabase(term);
            return Json(eventTypes.Distinct());
        }
    }
}
