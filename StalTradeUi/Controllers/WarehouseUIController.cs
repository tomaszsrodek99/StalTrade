using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StalTradeAPI.Dtos;
using System.Net.Http;

namespace StalTradeUI.Controllers
{
    public class WarehouseUIController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public WarehouseUIController(IWebHostEnvironment webHostEnvironment)
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
                HttpResponseMessage response = await _httpClient.GetAsync("api/Warehouse/GetProducts");
                if (response.IsSuccessStatusCode)
                {
                    var responseDto = await response.Content.ReadFromJsonAsync<IEnumerable<StockStatusDto>>();
                    ViewData["PurchasedValue"] = responseDto.Select(x => x.PurchasedValue).Sum();
                    ViewData["SoldValue"] = responseDto.Select(x => x.SoldValue).Sum();
                    ViewData["MarginValue"] = responseDto.Select(x => x.MarginValue).Sum();
                    return View("Warehouse", responseDto);
                }
                return View("Warehouse", new List<StockStatusDto>());
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

                    var latestPrices = responseDto
                        .SelectMany(p => p.Prices ?? Enumerable.Empty<PriceDto>())
                        .Where(price => !price.IsPurchase)
                        .GroupBy(price => new { price.ProductId, price.CompanyId })
                        .Select(group => group.OrderByDescending(price => price.Date).FirstOrDefault());

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
        public async Task<IActionResult> AddPrice(PriceDto dto)
        {
            try
            {
                dto.Date = DateTime.Now;
                dto.IsPurchase = false;
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Warehouse/CreatePrice", dto);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("PriceList");

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
        public async Task<IActionResult> PutPrice(PriceDto dto)
        {
            try
            {
                dto.Date = DateTime.Now;
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync("api/Warehouse/UpdatePrice", dto);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("PriceList");

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

        public async Task<IActionResult> RemovePrice(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Warehouse/DeletePrice{id}");

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("PriceList");
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
