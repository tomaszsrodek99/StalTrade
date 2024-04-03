using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StalTradeAPI.Dtos;
using StalTradeUI.Helpers;

namespace StalTradeUI.Controllers
{
    [Authorize]
    public class ProductUIController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        public ProductUIController(IHttpClientFactory httpContext)
        {
            _httpClientFactory = httpContext;
            _httpClient = httpContext.CreateClient("MyHttpContext");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("api/Product/GetProducts");
                if (response.IsSuccessStatusCode)
                {
                    var responseDto = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
                    return View("Products", responseDto);
                }
                return View("Products", new List<ProductDto>());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDto dto)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Product/CreateProduct", dto);
                ResponseHandler.HandleResponse(response, this);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Błąd serwera. {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateProductView(int? id)
        {
            if (id == null)
                return View("CreateProduct", new ProductDto());
            else
            {
                try
                {
                    HttpResponseMessage product = await _httpClient.GetAsync($"api/Product/GetProduct{id}");

                    if (product.IsSuccessStatusCode)
                    {
                        var productDto = await product.Content.ReadFromJsonAsync<ProductDto>();
                        return View("CreateProduct", productDto);
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
        }

        [HttpPost]
        public async Task<IActionResult> PutProduct(ProductDto dto)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync("api/Product/UpdateProduct", dto);
                ResponseHandler.HandleResponse(response, this);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Nie udało się dodać metody płatności. {ex.Message}";
                return RedirectToAction("CreateProductView");
            }
        }
        public async Task<IActionResult> RemoveProduct(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Product/DeleteProduct{id}");
                ResponseHandler.HandleResponse(response, this);
                return RedirectToAction("Index");              
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Nie udało się usunąć metody płatności. {ex.Message}";
                return RedirectToAction("CreateProductView");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProductExists([FromQuery] string companyDrawingNumber, [FromQuery] int productId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"api/Product/IsProductUnique{productId}", companyDrawingNumber);
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
                return RedirectToAction("CreateProductView");
            }
        }
    }
}
