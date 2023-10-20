using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StalTradeApi.Dtos;
using StalTradeAPI.Dtos;
using StalTradeUi.Models;
using System.Diagnostics;
using System.Net;
using StalTradeApi.Models;
using Microsoft.AspNetCore.Http;

namespace StalTradeUi.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        public UserController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7279/")
            };
        }

        [HttpGet]
        public IActionResult LoginView()
        {
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Auth/Login", request);
                if (response.IsSuccessStatusCode)
                {
                    var responseDto = await response.Content.ReadFromJsonAsync<UserLoginResponseDto>();

                    var jwtCookie = new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddMinutes(10)
                    };
                    Response.Cookies.Append("JWTToken", responseDto.Token, jwtCookie);

                    var user = responseDto.User;

                    var userIdCookie = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    };
                    Response.Cookies.Append("UserId", user.Id.ToString(), userIdCookie);                   

                    return RedirectToAction("Index");
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = await response.Content.ReadFromJsonAsync<ValidationError>();
                    if(error != null)
                    {
                        ModelState.AddModelError(error.State, error.Message);
                        return View("Login", request);
                    }
                    ViewBag.ErrorMessage = await response.Content.ReadAsStringAsync();
                    return View("Error");
                }
                else
                {
                    ViewBag.ErrorMessage = await response.Content.ReadAsStringAsync();
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        public IActionResult Index()
        {
            return View("Index");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("JWTToken");
            Response.Cookies.Delete("UserId");
            return RedirectToAction("LoginView");
        }
    }
}