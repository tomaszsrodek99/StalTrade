using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StalTradeAPI.Dtos;
using StalTradeAPI.Models;
using StalTradeUI.Helpers;

namespace StalTradeUI.Controllers
{
    [Authorize]
    public class CompanyUIController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        public CompanyUIController(IHttpClientFactory httpContext)
        {
            _httpClientFactory = httpContext;
            _httpClient = httpContext.CreateClient("MyHttpContext");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("api/Company/GetCompanies");

                if (response.IsSuccessStatusCode)
                {
                    var companiesDto = await response.Content.ReadFromJsonAsync<IEnumerable<CompanyDto>>();
                    return View("Companies", companiesDto);
                }

                return View("Companies", new List<CompanyDto>());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateCompanyView()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("api/PaymentMethod/GetPaymentMethods");
                if (response.IsSuccessStatusCode)
                {
                    var methodsDto = await response.Content.ReadFromJsonAsync<IEnumerable<PaymentMethod>>();
                    ViewBag.Methods = methodsDto;
                    return View("CreateCompany", new CompanyDto());
                }
                TempData["ErrorMessage"] = $"Błąd pobierania danych. Spróbuj ponownie później.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Błąd serwera. {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> NipExists([FromQuery] string nip, [FromQuery] int companyId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"api/Company/IsNIPUnique{companyId}", nip);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Błąd serwera. {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCompany(CompanyDto dto)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Company/CreateCompany", dto);
                ResponseHandler.HandleResponse(response, this);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Nie udało się dodać firmy. {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditCompanyView(int id)
        {
            try
            {
                HttpResponseMessage company = await _httpClient.GetAsync($"api/Company/GetCompany{id}");
                HttpResponseMessage methods = await _httpClient.GetAsync("api/PaymentMethod/GetPaymentMethods");

                if (methods.IsSuccessStatusCode && company.IsSuccessStatusCode)
                {
                    var methodsDto = await methods.Content.ReadFromJsonAsync<IEnumerable<PaymentMethod>>();
                    ViewBag.Methods = methodsDto;              
                    var companyDto = await company.Content.ReadFromJsonAsync<CompanyDto>();
                    return View("CreateCompany", companyDto);
                }

                TempData["ErrorMessage"] = $"Błąd pobierania danych. Spróbuj ponownie później.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Błąd serwera. Spróbuj ponownie później. {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PutCompany(CompanyDto dto)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync("api/Company/UpdateCompany", dto);
                ResponseHandler.HandleResponse(response, this);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Nie udało się edytować firmy. {ex.Message}";
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> RemoveCompany(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Company/DeleteCompany{id}");
                ResponseHandler.HandleResponse(response, this);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Nie udało się usunąć firmy. {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> MethodExists([FromQuery] string request)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"api/PaymentMethod/IsMethodExists", request);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Błąd serwera. {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddMethod(PaymentMethod dto)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/PaymentMethod/CreatePaymentMethod", dto);
                ResponseHandler.HandleResponse(response, this);             
                return RedirectToAction("CreateCompanyView");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Nie udało się dodać metody płatności. {ex.Message}";
                return RedirectToAction("CreateCompanyView");
            }
        }

        public async Task<IActionResult> RemoveMethod(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/PaymentMethod/DeletePaymentMethod{id}");
                ResponseHandler.HandleResponse(response, this);            
                return RedirectToAction("CreateCompanyView");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Nie udało się usunąć metody płatności. {ex.Message}";
                return RedirectToAction("CreateCompanyView");
            }
        }
    }
}
