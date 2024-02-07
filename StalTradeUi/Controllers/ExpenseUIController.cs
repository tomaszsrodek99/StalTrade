using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StalTradeAPI.Dtos;
using StalTradeAPI.Models;

namespace StalTradeUI.Controllers
{
    [Authorize]
    public class ExpenseUIController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ExpenseUIController(IWebHostEnvironment webHostEnvironment)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7279/")
            };
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> FixedCosts()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("api/Expense/GetExpenses");
                if (response.IsSuccessStatusCode)
                {
                    var responseDto = await response.Content.ReadFromJsonAsync<IEnumerable<ExpenseDto>>();
                    ViewBag.BruttoExpensesPaid = responseDto.Where(e => e.Paid).Sum(e => e.Brutto);
                    ViewBag.BruttoExpensesUnpaid = responseDto.Where(e => !e.Paid).Sum(e => e.Brutto);
                    return View("Index", responseDto);
                }
                return View("Index", new List<ExpenseDto>());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CashRegister()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("api/Expense/GetExpenses");
                if (response.IsSuccessStatusCode)
                {
                    var responseDto = await response.Content.ReadFromJsonAsync<IEnumerable<ExpenseDto>>();
                    //ViewBag.BruttoExpensesPaid = responseDto.Where(e => e.Paid).Sum(e => e.Brutto);
                    //ViewBag.BruttoExpensesUnpaid = responseDto.Where(e => !e.Paid).Sum(e => e.Brutto);
                    // Grupowanie wydatków według miesiąca
                    var monthlyExpenses = responseDto
                        .GroupBy(e => e.DateOfPayment.ToString("MMMM yyyy"))
                        .Select(group => new MonthlyExpenseViewModel
                        {
                            Month = group.Key,
                            TotalBrutto = group.Sum(e => e.Brutto),
                            Expenses = group.ToList()
                        })
                        .OrderByDescending(m => m.Month)
                        .Take(12);

                    return View("CashRegister", monthlyExpenses);
                }

                return View("CashRegister", new List<MonthlyExpenseViewModel>());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddExpense(ExpenseDto dto)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Expense/CreateExpense", dto);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("FixedCosts");

                var content = await response.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = $"Nie udało się dodać rekordu. {response.ReasonPhrase + " " + content}";
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> PutExpense(ExpenseDto dto)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync("api/Expense/UpdateExpense", dto);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("FixedCosts");

                var content = await response.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = $"Nie udało się edytować rekordu.{response.ReasonPhrase + " " + content}";
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
        public async Task<IActionResult> RemoveExpense(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Expense/DeleteExpense{id}");

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("FixedCosts");
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = $"Nie udało się usunąć rekordu.{response.ReasonPhrase + " " + content}";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
    }
}
