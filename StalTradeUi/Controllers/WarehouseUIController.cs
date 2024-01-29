using Microsoft.AspNetCore.Mvc;
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
                HttpResponseMessage response = await _httpClient.GetAsync("api/Company/GetCompanies");
                if (response.IsSuccessStatusCode)
                {
                    var responseDto = await response.Content.ReadFromJsonAsync<IEnumerable<CompanyDto>>();
                    return View("Index", responseDto);
                }
                return View("Index", new List<CompanyDto>());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
    }
}
