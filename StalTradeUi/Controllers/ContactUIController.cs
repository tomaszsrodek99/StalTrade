using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using StalTradeAPI.Dtos;
using StalTradeAPI.Models;
using StalTradeUI.Helpers;
using System.ComponentModel.Design;
using System.Net.Http.Headers;

namespace StalTradeUI.Controllers
{
    [Authorize]
    public class ContactUIController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        public ContactUIController(IHttpClientFactory httpContext)
        {
            _httpClientFactory = httpContext;
            _httpClient = httpContext.CreateClient("MyHttpContext");
        }

        public IActionResult CreateContactView(int id)
        {
            return View("CreateContact", new ContactDto { CompanyID = id });
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(ContactDto dto)
        {
            try
            {
                if(dto.Phone1 == dto.Phone2)
                {
                    ModelState.AddModelError("Phone2", "Numery telefonu nie mogą być takie same.");
                    return View("CreateContact", dto);
                }
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Contact/CreateContact", dto);
                ResponseHandler.HandleResponse(response, this);
                return RedirectToAction("Index", "CompanyUI");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Nie udało się dodać kontaktu. {ex.Message}";
                return RedirectToAction("Index", "CompanyUI");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateContactView(int id)
        {
            try
            {
                HttpResponseMessage contact = await _httpClient.GetAsync($"api/Contact/GetContact{id}");
                var contactDto = await contact.Content.ReadFromJsonAsync<ContactDto>();
                return View("CreateContact", contactDto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Błąd serwera. {ex.Message}";
                return RedirectToAction("Index", "CompanyUI");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PutContact(ContactDto dto)
        {
            try
            {
                if (dto.Phone1 == dto.Phone2)
                {
                    ModelState.AddModelError("Phone2", "Numery telefonu nie mogą być takie same.");
                    return View("CreateContact", dto);
                }
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync("api/Contact/UpdateContact", dto);
                ResponseHandler.HandleResponse(response, this);
                return RedirectToAction("Index", "CompanyUI");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Nie udało się edytować kontaktu. {ex.Message}";
                return RedirectToAction("Index", "CompanyUI");
            }
        }

        public async Task<IActionResult> RemoveContact(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Contact/DeleteContact{id}");
                ResponseHandler.HandleResponse(response, this);
                return RedirectToAction("Index", "CompanyUI");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Nie udało się usunąć kontaktu. {ex.Message}";
                return RedirectToAction("Index", "CompanyUI");
            }
        }
    }
}
