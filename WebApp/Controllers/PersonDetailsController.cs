using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class PersonDetailsController : Controller
    {
        private readonly HttpClient _apiClient;

        public PersonDetailsController(IHttpClientFactory httpClientFactory)
        {
            _apiClient = httpClientFactory.CreateClient("ApiWithJwt");
        }

        private async Task LoadViewBagData()
        {
            var personsResponse = await _apiClient.GetAsync($"{_apiClient.BaseAddress}/persons");
            var personsContent = await personsResponse.Content.ReadAsStringAsync();
            var persons = JsonConvert.DeserializeObject<List<Position>>(personsContent);
            ViewBag.Persons = new SelectList(persons, "PositionId", "Name");
        }

        public async Task<IActionResult> Index()
        {
            var list = new List<PersonDetailsInformation>();
            var responseMessage = await _apiClient.GetAsync($"{_apiClient.BaseAddress}/persondetails");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jstring = await responseMessage.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<List<PersonDetailsInformation>>(jstring);
                return View(list);
            }
            else
            {
                return View(list);
            }
        }

        public async Task<IActionResult> Add()
        {
            await LoadViewBagData();
            return View(new PersonDetails());
        }

        [HttpPost]
        public async Task<IActionResult> Add(PersonDetails personDetails)
        {
            if (ModelState.IsValid)
            {
                var jsondepartment = JsonConvert.SerializeObject(personDetails);
                var content = new StringContent(jsondepartment, Encoding.UTF8, "application/json");
                var message = await _apiClient.PostAsync($"{_apiClient.BaseAddress}/persondetails", content);
                if (message.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Error", "There is an API error");
                    return View(personDetails);
                }
            }
            else
            {
                return View(personDetails);
            }
        }

        public async Task<IActionResult> Update(int Id)
        {
            var message = await _apiClient.GetAsync($"{_apiClient.BaseAddress}/persondetails/{Id}");
            if (message.IsSuccessStatusCode)
            {
                var jstring = await message.Content.ReadAsStringAsync();
                var personDetails = JsonConvert.DeserializeObject<PersonDetails>(jstring);

                return View(personDetails);
            }
            else
            {
                return RedirectToAction("Add");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(PersonDetails personDetails)
        {
            if (ModelState.IsValid)
            {
                var jsonperson = JsonConvert.SerializeObject(personDetails);
                var content = new StringContent(jsonperson, Encoding.UTF8, "application/json");
                var message = await _apiClient.PutAsync($"{_apiClient.BaseAddress}/persondetails", content);
                if (message.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    await LoadViewBagData();
                    return View(personDetails);
                }
            }
            else
            {
                await LoadViewBagData();
                return View(personDetails);
            }
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var message = await _apiClient.DeleteAsync($"{_apiClient.BaseAddress}/persondetails/{Id}");
            if (message.IsSuccessStatusCode)
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
