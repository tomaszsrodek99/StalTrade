using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StalTradeAPI.Dtos;

namespace StalTradeUI.Controllers
{
    [Authorize]
    public class InvoiceUIController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public InvoiceUIController(IWebHostEnvironment webHostEnvironment)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7279/")
            };
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("api/Invoice/GetInvoices");
                HttpResponseMessage companyResponse = await _httpClient.GetAsync("api/Company/GetCompanies");
                HttpResponseMessage productsResponse = await _httpClient.GetAsync("api/Warehouse/GetPrices");
                HttpResponseMessage stocksResponse = await _httpClient.GetAsync("api/Warehouse/GetProducts");
                
                if (!(companyResponse.IsSuccessStatusCode && productsResponse.IsSuccessStatusCode && stocksResponse.IsSuccessStatusCode))
                {
                    throw new Exception(message: "Błąd pobierania danych. Upewnij się, że w bazie danych posiadasz zapisane firmy oraz produkty.");                   
                }
                ViewBag.Companies = await companyResponse.Content.ReadFromJsonAsync<IEnumerable<CompanyDto>>();

                ViewBag.Products = await productsResponse.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();

                ViewBag.Stocks = await stocksResponse.Content.ReadFromJsonAsync<IEnumerable<StockStatusDto>>();
                
                if (response.IsSuccessStatusCode)
                {
                    var responseDto = await response.Content.ReadFromJsonAsync<IEnumerable<InvoiceDto>>();
                    return View("Invoices", responseDto);
                }
                return View("Invoices", new List<InvoiceDto>());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }       

        [HttpPost]
        public async Task<IActionResult> AddSale(InvoiceDto dto)
        {
            try
            {
                dto.IsPurchase = false;
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Invoice/CreateInvoice", dto);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

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
        public async Task<IActionResult> AddPurchase(InvoiceDto dto)
        {
            try
            {
                dto.IsPurchase = true;
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Invoice/CreateInvoice", dto);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

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
    }
}
