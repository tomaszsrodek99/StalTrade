using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StalTradeAPI.Dtos;
using StalTradeUI.Helpers;

namespace StalTradeUI.Controllers
{
    [Authorize]
    public class ExpenseUIController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        public ExpenseUIController(IHttpClientFactory httpContext)
        {
            _httpClientFactory = httpContext;
            _httpClient = httpContext.CreateClient("MyHttpContext");
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
                    return View("Expenses", responseDto);
                }
                ViewBag.BruttoExpensesPaid = 0;
                ViewBag.BruttoExpensesUnpaid = 0;
                return View("Expenses", new List<ExpenseDto>());
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
                HttpResponseMessage responseMonthlyExpenses = await _httpClient.GetAsync("api/Expense/GetMonthlyExpenses");
                HttpResponseMessage responseExpenses = await _httpClient.GetAsync("api/Expense/GetExpenses");
                HttpResponseMessage responseDeposit = await _httpClient.GetAsync("api/Expense/GetDeposites");
                if (responseMonthlyExpenses.IsSuccessStatusCode && responseExpenses.IsSuccessStatusCode)
                {
                    var monthlyExpenses = await responseMonthlyExpenses.Content.ReadFromJsonAsync<IEnumerable<MonthlyExpenseViewModel>>();                 
                    var expenses = await responseExpenses.Content.ReadFromJsonAsync<IEnumerable<ExpenseDto>>();                 

                    if (responseDeposit.IsSuccessStatusCode)
                    {
                        var depositesDto = await responseDeposit.Content.ReadFromJsonAsync<IEnumerable<DepositDto>>();
                        ViewData["Deposites"] = depositesDto;
                        ViewData["Cash"] = depositesDto.Select(x => x.Cash).Sum() - expenses.Where(x => x.Paid == true).Select(x => x.Brutto).Sum();
                        return View("CashRegister", monthlyExpenses);
                    }
                    ViewData["Cash"] = -expenses.Where(x => x.Paid == true).Select(x => x.Brutto).Sum();
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

        [HttpGet]
        public async Task<IActionResult> CreateExpenseView(int? id)
        {
            if (id == null)
                return View("CreateExpense", new ExpenseDto { Date = DateTime.Now, DateOfPayment = DateTime.Now });
            else
            {
                try
                {
                    HttpResponseMessage response = await _httpClient.GetAsync($"api/Expense/GetExpense{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var expenseDto = await response.Content.ReadFromJsonAsync<ExpenseDto>();
                        return View("CreateExpense", expenseDto);
                    }

                    TempData["ErrorMessage"] = $"Błąd pobierania danych. Spróbuj ponownie później.";
                    return RedirectToAction("FixedCosts");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Błąd serwera. Spróbuj ponownie później. {ex.Message}";
                    return RedirectToAction("FixedCosts");
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddExpense(ExpenseDto dto)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Expense/CreateExpense", dto);
                ResponseHandler.HandleResponse(response, this);
                return RedirectToAction("FixedCosts");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Nie udało się dodać wydatku. {ex.Message}";
                return RedirectToAction("FixedCosts");
            }
        }      

        [HttpPost]
        public async Task<IActionResult> PutExpense(ExpenseDto dto)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync("api/Expense/UpdateExpense", dto);
                ResponseHandler.HandleResponse(response, this);
                return RedirectToAction("FixedCosts");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Nie udało się edytować wydatku. {ex.Message}";
                return RedirectToAction("FixedCosts");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePaidStatus(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Expense/ChangePaidStatus", id);
                ResponseHandler.HandleResponse(response, this);
                return RedirectToAction("FixedCosts");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Nie udało się zapłacić wydatku. {ex.Message}";
                return RedirectToAction("FixedCosts");
            }
        }

        [HttpGet]
        public async Task<IActionResult> AutocompleteContractor(string term)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Expense/AutocompleteContractor", term);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
                    return Json(new { success = true, data = responseData });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        [HttpGet]
        public async Task<IActionResult> AutocompleteDescription(string term)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Expense/AutocompleteDescription", term);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
                    return Json(new { success = true, data = responseData });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        [HttpGet]
        public async Task<IActionResult> AutocompleteEventType(string term)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Expense/AutocompleteEventType", term);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
                    return Json(new { success = true, data = responseData });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddDeposit(DepositDto deposit)
        {
            try
            {
                deposit.Date = DateTime.Now;
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Expense/CreateDeposit", deposit);
                ResponseHandler.HandleResponse(response, this);
                return RedirectToAction("CashRegister");
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = $"Nie udało się dodać wpłaty. {ex.Message}";
                return RedirectToAction("CashRegister");
            }
        }

        public async Task<IActionResult> RemoveExpense(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Expense/DeleteExpense{id}");
                ResponseHandler.HandleResponse(response, this);
                return RedirectToAction("FixedCosts");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Nie udało się usunąć wydatku. {ex.Message}";
                return RedirectToAction("FixedCosts");
            }
        }
        public async Task<IActionResult> RemoveDeposit(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Expense/DeleteDeposit{id}");
                ResponseHandler.HandleResponse(response, this);
                return RedirectToAction("CashRegister");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Nie udało się usunąć wpłaty. {ex.Message}";
                return RedirectToAction("CashRegister");
            }
        }
    }
}
