using WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

namespace WebApp.Controllers
{
    public class SalaryController : Controller
    {
        private readonly HttpClient _apiClient;

        public SalaryController(IHttpClientFactory httpClientFactory)
        {
            _apiClient = httpClientFactory.CreateClient("ApiWithJwt");
        }

        public async Task<IActionResult> Index()
        {
            List<Salary> list = new List<Salary>();
            HttpResponseMessage response = await _apiClient.GetAsync($"{_apiClient.BaseAddress}/salaries");
            if (response.IsSuccessStatusCode)
            {
                var jstring = await response.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<List<Salary>>(jstring);
                return View(list);
            }
            else
            {
                return View(list);
            }
        }

        public IActionResult Add()
        {
            Salary salary = new Salary();
            return View(salary);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Salary salary)
        {
            if (ModelState.IsValid)
            {
                var jsonSalary = JsonConvert.SerializeObject(salary);
                StringContent content = new StringContent(jsonSalary, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _apiClient.PostAsync($"{_apiClient.BaseAddress}/salaries", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Error", "There is an API error");
                    return View(salary);
                }
            }
            else
            {
                return View(salary);
            }
        }

        public async Task<IActionResult> Update(int Id)
        {
            HttpResponseMessage response = await _apiClient.GetAsync($"{_apiClient.BaseAddress}/salaries/{Id}");
            if (response.IsSuccessStatusCode)
            {
                var jstring = await response.Content.ReadAsStringAsync();
                Salary salary = JsonConvert.DeserializeObject<Salary>(jstring);
                return View(salary);
            }
            else
            {
                return RedirectToAction("Add");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(Salary salary)
        {
            if (ModelState.IsValid)
            {
                var jsonSalary = JsonConvert.SerializeObject(salary);
                StringContent content = new StringContent(jsonSalary, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _apiClient.PutAsync($"{_apiClient.BaseAddress}/salaries", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(salary);
                }
            }
            else
            {
                return View(salary);
            }
        }


        public async Task<IActionResult> Delete(int Id)
        {
            HttpResponseMessage response = await _apiClient.DeleteAsync($"{_apiClient.BaseAddress}/salaries/{Id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
    }
}
