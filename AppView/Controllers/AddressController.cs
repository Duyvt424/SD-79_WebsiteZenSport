using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AppView.Controllers
{
	public class AddressController : Controller
	{
        private readonly IAllRepositories<Address> _repos;
        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<Address> _address;
        public AddressController()
        {
            _address = _dbContext.Addresses;
            AllRepositories<Address> all = new AllRepositories<Address>(_dbContext, _address);
            _repos = all;
        }
        public async Task<IActionResult> GetAllAddress()
		{
            string apiUrl = "https://localhost:7036/api/Address/get-address";
            var httpClient = new HttpClient(); // tạo ra để callApi
            var response = await httpClient.GetAsync(apiUrl);// Lấy dữ liệu ra
            string apiData = await response.Content.ReadAsStringAsync();
            var address = JsonConvert.DeserializeObject<List<Address>>(apiData);
            return View(address);
        }
        [HttpGet]
        public async Task<IActionResult> CreateAddress()
        {
            using (ShopDBContext shopDBContext = new ShopDBContext())
            {
                var customer = shopDBContext.Customers.ToList();
                SelectList selectListCustomer = new SelectList(customer, "CumstomerID", "UserName");
                ViewBag.CustomerList = selectListCustomer;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAddress(Address address)
        {
            HttpClient httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Address/create-address?Street={address.Street}&Commune={address.Commune}&District={address.District}&Province={address.Province}&Status={address.Status}&CumstomerID={address.CumstomerID}";
            var response = await httpClient.PostAsync(apiUrl, null);
            return RedirectToAction("GetAllAddress");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateAddress(Guid id)
        {
            Address address = _repos.GetAll().FirstOrDefault(c => c.AddressID == id);
            using (ShopDBContext shopDBContext = new ShopDBContext())
            {
                var customer = shopDBContext.Customers.ToList();
                SelectList selectListCustomer = new SelectList(customer, "CumstomerID", "UserName");
                ViewBag.CustomerList = selectListCustomer;
            }
            return View(address);
        }
        public async Task<IActionResult> UpdateAddress(Address address)
        {
            HttpClient httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Address/update-address?AddressID={address.AddressID}&Street={address.Street}&Commune={address.Commune}&District={address.District}&Province={address.Province}&Status={address.Status}&CumstomerID={address.CumstomerID}";
            var response = await httpClient.PutAsync(apiUrl, null);
            return RedirectToAction("GetAllAddress");
        }
        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            var address = _repos.GetAll().FirstOrDefault(c => c.AddressID == id);
            HttpClient httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Address/delete-address?id={id}";
            var response = await httpClient.DeleteAsync(apiUrl);
            return RedirectToAction("GetAllAddress");
        }
    }
}
