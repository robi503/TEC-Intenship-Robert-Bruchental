using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class DepartmentController(IHttpClientFactory httpClientFactory) : Controller
    {
        private readonly HttpClient _apiClient = httpClientFactory.CreateClient("ApiWithJwt");

        public async Task<IActionResult> Index()
        {
            
            var response = await _apiClient.GetAsync($"{_apiClient.BaseAddress}/departments");
            if (response.IsSuccessStatusCode)
            {
                var jstring = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<Department>>(jstring);
                return View(list);
            }
            else
            {
                return View(new List<Department>());
            }
        }

        public IActionResult Add()
        {
            return View(new Department());
        }

        [HttpPost]
        public async Task<IActionResult> Add(Department department)
        {
            if (!ModelState.IsValid)
            {
                return View(department);
            }

            var jsondepartment = JsonConvert.SerializeObject(department);
            var content = new StringContent(jsondepartment, Encoding.UTF8, "application/json");
            var response = await _apiClient.PostAsync($"{_apiClient.BaseAddress}/departments", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("Error", "There is an API error");
                return View(department);
            }
        }

        public async Task<IActionResult> Update(int Id)
        {
            var response = await _apiClient.GetAsync($"{_apiClient.BaseAddress}/departments/{ Id}");
            if (response.IsSuccessStatusCode)
            {
                var jstring = await response.Content.ReadAsStringAsync();
                var department = JsonConvert.DeserializeObject<Department>(jstring);
                return View(department);
            }
            else
            {
                return RedirectToAction("Add");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(Department department)
        {
            if (!ModelState.IsValid)
            {
                return View(department);
            }

            var jsondepartment = JsonConvert.SerializeObject(department);
            var content = new StringContent(jsondepartment, Encoding.UTF8, "application/json");
            var response = await _apiClient.PutAsync($"{_apiClient.BaseAddress}/departments", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(department);
            }
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var response = await _apiClient.DeleteAsync($"{_apiClient.BaseAddress}/departments/{Id}");
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
