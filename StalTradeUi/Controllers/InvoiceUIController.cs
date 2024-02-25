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

        [HttpGet]
        public async Task<IActionResult> CreatePurchase()
        {
            try
            {
                HttpResponseMessage companyResponse = await _httpClient.GetAsync("api/Company/GetCompanies");
                HttpResponseMessage productsResponse = await _httpClient.GetAsync("api/Invoice/GetProductsWithLatestPrices");

                if (!(companyResponse.IsSuccessStatusCode && productsResponse.IsSuccessStatusCode))
                {
                    throw new Exception(message: "Błąd pobierania danych. Upewnij się, że w bazie danych posiadasz zapisane firmy oraz produkty.");
                }
                ViewBag.Companies = await companyResponse.Content.ReadFromJsonAsync<IEnumerable<CompanyDto>>();

                var products = await productsResponse.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();

                foreach (var product in products)
                {
                    product.Prices = product.Prices.Where(price => price.IsPurchase).ToList();
                }
                ViewBag.Products = products;

                return PartialView("_CreatePurchaseInvoice", new InvoiceDto());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateSale()
        {
            try
            {
                HttpResponseMessage companyResponse = await _httpClient.GetAsync("api/Company/GetCompanies");
                HttpResponseMessage productsResponse = await _httpClient.GetAsync("api/Invoice/GetProductsWithLatestPrices");

                if (!(companyResponse.IsSuccessStatusCode && productsResponse.IsSuccessStatusCode))
                {
                    throw new Exception(message: "Błąd pobierania danych. Upewnij się, że w bazie danych posiadasz zapisane firmy oraz produkty.");
                }
                ViewBag.Companies = await companyResponse.Content.ReadFromJsonAsync<IEnumerable<CompanyDto>>();

                var products = await productsResponse.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();

                foreach (var product in products)
                {
                    product.Prices = product.Prices.Where(price => !price.IsPurchase).ToList();
                }
                ViewBag.Products = products;
                return PartialView("_CreateSaleInvoice", new InvoiceDto());
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
                List<InvoiceProductDto> productsList = dto.ProductsList.Where(x => x.Quantity != 0).ToList();
                dto.ProductsList = productsList;
                if(!dto.ProductsList.Any())
                {
                    throw new Exception(message: "Nie wybrano produktów.");
                }
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
                List<InvoiceProductDto> productsList = dto.ProductsList.Where(x =>x.Quantity != 0).ToList();
                dto.ProductsList = productsList;
                if (!dto.ProductsList.Any())
                {
                    throw new Exception(message: "Nie wybrano produktów.");
                }
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
        public async Task<IActionResult> RemoveInvoice(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Invoice/DeleteInvoice{id}");

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");
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
