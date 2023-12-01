using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StalTradeAPI.Dtos;
using StalTradeAPI.Models;
using System.Net;

namespace StalTradeUI.Controllers
{
    [Authorize]
    public class CompanyUIController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CompanyUIController(IWebHostEnvironment webHostEnvironment)
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

        [HttpGet]
        public async Task<IActionResult> CreateUpdateContactForm(int contactId)
        {
            ViewBag.Action = "PutContact";
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/Contact/GetContact{contactId}");
                var responseDto = await response.Content.ReadFromJsonAsync<ContactDto>();
                return PartialView("CreateContact", responseDto);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult CreateAddContactForm(int companyId)
        {
            ViewBag.Action = "AddContact";
            var model = new ContactDto { CompanyID = companyId };
            return PartialView("CreateContact", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCompany(CompanyDto dto)
        {
            try
            {
                HttpResponseMessage addCompanyResponse = await _httpClient.PostAsJsonAsync("api/Company/CreateCompany", dto);

                if (addCompanyResponse.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                var content = await addCompanyResponse.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = $"Nie udało się dodać rekordu. {addCompanyResponse.ReasonPhrase + " " + content}";
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(ContactDto dto)
        {        
            try
            {
                HttpResponseMessage addCompanyResponse = await _httpClient.PostAsJsonAsync("api/Contact/CreateContact", dto);

                if (addCompanyResponse.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                var content = await addCompanyResponse.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = $"Nie udało się dodać rekordu. {addCompanyResponse.ReasonPhrase + " " + content}";
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PutCompany(CompanyDto dto)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync("api/Company/UpdateCompany", dto);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");
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
        public async Task<IActionResult> PutContact(ContactDto dto)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync("api/Contact/UpdateContact", dto);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");
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

        public async Task<IActionResult> RemoveCompany(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Company/DeleteCompany{id}");

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

        public async Task<IActionResult> RemoveContact(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Contact/DeleteContact{id}");

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
