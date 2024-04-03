using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StalTradeAPI.Dtos;
using StalTradeUI.Helpers;

namespace StalTradeUI.Controllers
{
    [Authorize]
    public class WarehouseUIController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        public WarehouseUIController(IHttpClientFactory httpContext)
        {
            _httpClientFactory = httpContext;
            _httpClient = httpContext.CreateClient("MyHttpContext");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("api/Warehouse/GetProductsWithStockStatus");
                if (response.IsSuccessStatusCode)
                {
                    var responseDto = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
                    ViewData["PurchasedValue"] = responseDto.Select(x => x.StockStatus.PurchasedValue).Sum();
                    ViewData["SoldValue"] = responseDto.Select(x => x.StockStatus.SoldValue).Sum();
                    ViewData["MarginValue"] = responseDto.Select(x => x.StockStatus.MarginValue).Sum();
                    return View("Warehouse", responseDto);
                }
                return View("Warehouse", new List<ProductDto>());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> PriceList()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("api/Warehouse/GetPrices");
                HttpResponseMessage companyResponse = await _httpClient.GetAsync("api/Company/GetCompanies");
                if (response.IsSuccessStatusCode)
                {
                    var responseDto = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
                    ViewBag.Companies = await companyResponse.Content.ReadFromJsonAsync<IEnumerable<CompanyDto>>();

                    var allPrices = responseDto
                        .SelectMany(p => p.Prices);

                    var latestPurchasePrices = allPrices
                        .Where(price => price.IsPurchase)
                        .GroupBy(price => new { price.ProductId, price.CompanyId })
                        .Select(group => group.OrderByDescending(price => price.Date).FirstOrDefault());

                    var latestSalePrices = allPrices
                        .Where(price => !price.IsPurchase)
                        .GroupBy(price => new { price.ProductId, price.CompanyId })
                        .Select(group => group.OrderByDescending(price => price.Date).FirstOrDefault());

                    var latestPrices = latestPurchasePrices.Concat(latestSalePrices).OrderBy(price => price.CompanyId);
                    ViewBag.LatestPrices = latestPrices;

                    return View("PriceList", responseDto);
                }
                return View("PriceList", new List<ProductDto>());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddSalePrice(PriceDto dto)
        {
            try
            {
                dto.Date = DateTime.Now;
                dto.IsPurchase = false;
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Warehouse/CreatePrice", dto);
                ResponseHandler.HandleResponse(response, this);
                return RedirectToAction("PriceList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Nie udało się dodać ceny sprzedaży. {ex.Message}";
                return RedirectToAction("PriceList");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPurchasePrice(PriceDto dto)
        {
            try
            {
                dto.Date = DateTime.Now;
                dto.IsPurchase = true;
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Warehouse/CreatePrice", dto);
                ResponseHandler.HandleResponse(response, this);
                return RedirectToAction("PriceList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Nie udało się dodać ceny zakupu. {ex.Message}";
                return RedirectToAction("PriceList");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PutPrice(PriceDto dto)
        {
            try
            {
                dto.Date = DateTime.Now;
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync("api/Warehouse/UpdatePrice", dto);
                ResponseHandler.HandleResponse(response, this);
                return RedirectToAction("PriceList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Nie udało się edytować ceny. {ex.Message}";
                return RedirectToAction("PriceList");
            }
        }

        public async Task<IActionResult> RemovePrice(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Warehouse/DeletePrice{id}");
                ResponseHandler.HandleResponse(response, this);
                return RedirectToAction("PriceList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Nie udało się usunąć ceny. {ex.Message}";
                return RedirectToAction("PriceList");
            }
        }
    }
}
