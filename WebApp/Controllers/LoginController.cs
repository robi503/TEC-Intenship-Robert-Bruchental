using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;
        private readonly string _api;
        public LoginController(IConfiguration config)
        {
            _config = config;
            _api = _config.GetValue<string>("ApiSettings:ApiUrl") + "/admins";
        }

        public IActionResult Add()
        {
            Admin admin = new Admin();
            return View(admin);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Admin admin)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = new HttpClient();
                var jsonadmin = JsonConvert.SerializeObject(admin);
                StringContent content = new StringContent(jsonadmin, Encoding.UTF8, "application/json");
                HttpResponseMessage message = await client.PostAsync($"{_api}", content);
                if (message.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }
                else if (message.StatusCode == HttpStatusCode.Conflict)
                {
                    ViewData["CredentialError"] = "Username is already taken.";
                    return View(admin);
                }
                else
                {
                    ModelState.AddModelError("Error", "There is an API error");
                    return View(admin);
                }
            }
            else
            {
                return View(admin);
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Admin admin)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage message = await client.GetAsync($"{_api}/{admin.Username}/{admin.Password}");
                if (message.IsSuccessStatusCode)
                {
                    var jstring = await message.Content.ReadAsStringAsync();
                    dynamic tokenResponse = JsonConvert.DeserializeObject(jstring);
                    string token = tokenResponse.token;
                    Response.Cookies.Append("jwtToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTimeOffset.UtcNow.AddMinutes(60),
                    });
                    return RedirectToAction("Index", "Home");
                }
                else if (message.StatusCode == HttpStatusCode.NotFound)
                {
                    ViewData["CredentialError"] = "Invalid credentials. Please try again.";
                    return View(admin);
                }
                else
                {
                    ModelState.AddModelError("Error", "An error occurred while processing your request. Please try again later.");
                    return View(admin);
                }
            }
            else
            {
                return View(admin);
            }
        }
    }
}
