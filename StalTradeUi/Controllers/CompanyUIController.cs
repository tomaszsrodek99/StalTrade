﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StalTradeAPI.Dtos;
using StalTradeAPI.Models;

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
                HttpResponseMessage companies = await _httpClient.GetAsync("api/Company/GetCompanies");
                HttpResponseMessage methods = await _httpClient.GetAsync("api/PaymentMethod/GetPaymentMethods");
                if (companies.IsSuccessStatusCode && methods.IsSuccessStatusCode)
                {
                    var companiesDto = await companies.Content.ReadFromJsonAsync<IEnumerable<CompanyDto>>();
                    var methodsDto = await methods.Content.ReadFromJsonAsync<IEnumerable<PaymentMethod>>();
                    ViewBag.Methods = methodsDto;
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

        [HttpPost]
        public async Task<IActionResult> AddCompany(CompanyDto dto)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Company/CreateCompany", dto);

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
        public async Task<IActionResult> AddMethod(PaymentMethod dto)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/PaymentMethod/CreatePaymentMethod", dto);

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
        public async Task<IActionResult> AddContact(ContactDto dto)
        {        
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Contact/CreateContact", dto);

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
        public async Task<IActionResult> RemoveMethod(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/PaymentMethod/DeleteMethod{id}");

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
