using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StalTradeAPI.Dtos;
using System.Globalization;

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
                HttpResponseMessage response = await _httpClient.GetAsync("api/Expense/GetExpenses");
                HttpResponseMessage responseDeposit = await _httpClient.GetAsync("api/Expense/GetDeposites");
                if (response.IsSuccessStatusCode)
                {
                    var responseDto = await response.Content.ReadFromJsonAsync<IEnumerable<ExpenseDto>>();
                    responseDto = responseDto.OrderBy(x => x.DateOfPayment);

                    var monthlyExpenses = responseDto
                        .GroupBy(e => e.DateOfPayment.ToString("MMMM yyyy"))
                        .Select(group => new MonthlyExpenseViewModel
                        {
                            Month = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(group.Key),
                            TotalBrutto = group.Sum(e => e.Brutto),
                            Expenses = group.ToList()
                        })
                        .OrderByDescending(m => m.Month)
                        .Take(12);

                    var chartData = new
                    {
                        labels = monthlyExpenses.SelectMany(m => m.Expenses.Select(e => e.DateOfPayment.ToString("yyyy-MM-dd"))),
                        datasets = new[]
                        {
                            new
                            {
                                label = "Wzrost wydatków",
                                data = monthlyExpenses.SelectMany(m => m.Expenses.Select(e => e.Brutto)).Reverse(),
                                fill = false,
                                borderColor = "rgb(75, 192, 192)",
                                tension = 0.1
                            }
                        }
                    };
                    ViewBag.ChartData = JsonConvert.SerializeObject(chartData);
                    ViewData["Cash"] = 0 - responseDto.Where(x => x.Paid == true).Select(x => x.Brutto).Sum();
                    if (responseDeposit.IsSuccessStatusCode)
                    {
                        var depositesDto = await responseDeposit.Content.ReadFromJsonAsync<IEnumerable<DepositDto>>();
                        ViewData["Deposites"] = depositesDto;
                        ViewData["Cash"] = depositesDto.Select(x => x.Cash).Sum() - responseDto.Where(x => x.Paid == true).Select(x => x.Brutto).Sum();
                        return View("CashRegister", monthlyExpenses);
                    }
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

        [HttpPost]
        public async Task<IActionResult> AddDeposit(DepositDto deposit)
        {
            try
            {
                deposit.Date = DateTime.Now;
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Expense/CreateDeposit", deposit);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("CashRegister");

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
        public async Task<IActionResult> RemoveDeposit(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Expense/DeleteDeposit{id}");

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("CashRegister");
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
