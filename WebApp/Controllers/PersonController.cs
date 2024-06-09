using WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Controllers
{
    public class PersonController(IHttpClientFactory httpClientFactory) : Controller
    {
        private readonly HttpClient _apiClient = httpClientFactory.CreateClient("ApiWithJwt");

        private async Task LoadViewBagData()
        {
            var positionsResponse = await _apiClient.GetAsync($"{_apiClient.BaseAddress}/positions");
            var positionsContent = await positionsResponse.Content.ReadAsStringAsync();
            var positions = JsonConvert.DeserializeObject<List<Position>>(positionsContent);
            ViewBag.Positions = new SelectList(positions, "PositionId", "Name");

            var salariesResponse = await _apiClient.GetAsync($"{_apiClient.BaseAddress}/salaries");
            var salariesContent = await salariesResponse.Content.ReadAsStringAsync();
            var salaries = JsonConvert.DeserializeObject<List<Salary>>(salariesContent);
            ViewBag.Salaries = new SelectList(salaries, "SalaryId", "Amount");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _apiClient.GetAsync($"{_apiClient.BaseAddress}/persons");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<PersonInformation>>(content);
                return View(list);
            }
            else
            {
                return View(new List<PersonInformation>());
            }
        }

        public async Task<IActionResult> Add()
        {
            await LoadViewBagData();
            return View(new Person());
        }

        [HttpPost]
        public async Task<IActionResult> Add(Person person)
        {
            if (ModelState.IsValid)
            {
                var jsonPerson = JsonConvert.SerializeObject(person);
                var content = new StringContent(jsonPerson, Encoding.UTF8, "application/json");
                var response = await _apiClient.PostAsync($"{_apiClient.BaseAddress}/persons", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "There is an API Error");
                    await LoadViewBagData();
                    return View(person);
                }
            }
            else
            {
                await LoadViewBagData();
                return View(person);
            }
        }

        public async Task<IActionResult> Update(int Id)
        {
            var response = await _apiClient.GetAsync($"{_apiClient.BaseAddress}/persons/{Id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var person = JsonConvert.DeserializeObject<Person>(content);

                await LoadViewBagData();
                return View(person);
            }
            else
            {
                return RedirectToAction("Add");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(Person person)
        {
            if (ModelState.IsValid)
            {
                var jsonPerson = JsonConvert.SerializeObject(person);
                var content = new StringContent(jsonPerson, Encoding.UTF8, "application/json");
                var response = await _apiClient.PutAsync($"{_apiClient.BaseAddress}/persons", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            await LoadViewBagData();
            return View(person);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var response = await _apiClient.DeleteAsync($"{_apiClient.BaseAddress}/persons/{Id}");
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
