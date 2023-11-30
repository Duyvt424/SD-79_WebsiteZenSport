﻿using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Owin.BuilderProperties;
using Newtonsoft.Json;
using Address = AppData.Models.Address;

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

        [HttpPost]
        public IActionResult SetDefaultAddress(Guid addressId)
        {
            // Kiểm tra xem địa chỉ có tồn tại không
            var address = _dbContext.Addresses.FirstOrDefault(c => c.AddressID == addressId);
            if (address == null)
            {
                return NotFound();
            }
            var userIdString = HttpContext.Session.GetString("UserId");
            var customerId = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
            // Lấy địa chỉ mặc định hiện tại của người dùng
            var currentDefaultAddress = _dbContext.Addresses.FirstOrDefault(c => c.CumstomerID == customerId && c.IsDefaultAddress);
            if (currentDefaultAddress != null)
            {
                // Hủy đặt làm mặc định cho địa chỉ hiện tại
                currentDefaultAddress.IsDefaultAddress = false;
            }
            // Đặt làm mặc định cho địa chỉ mới
            address.IsDefaultAddress = true;
            // Lưu thay đổi vào cơ sở dữ liệu
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IActionResult GetDefaultAddress()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            var customerId = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
            // Lấy địa chỉ mặc định của người dùng
            var defaultAddress = _dbContext.Addresses.FirstOrDefault(c => c.CumstomerID == customerId && c.IsDefaultAddress == true);
            if (defaultAddress == null)
            {
                // Không tìm thấy địa chỉ mặc định
                return NotFound();
            }
            // Trả về thông tin địa chỉ mặc định
            return Ok(new { addressId = defaultAddress.AddressID });
        }

        [HttpPost]
        public IActionResult UpdatePriceShipping(Guid addressId, decimal shippingCost)
        {
            var objAddress = _dbContext.Addresses.First(c => c.AddressID == addressId);
            if (objAddress == null)
            {
                return NotFound();
            }
            objAddress.ShippingCost = shippingCost;
            _dbContext.Update(objAddress);
            _dbContext.SaveChanges();
            return Ok(new { success = true });
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
            string apiUrl = $"https://localhost:7036/api/Address/create-address?Street={address.Street}&Commune={address.Commune}&District={address.District}&Province={address.Province}&IsDefaultAddress={address.IsDefaultAddress}&ShippingCost={address.ShippingCost}&Status={address.Status}&DateCreated={DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}&CumstomerID={address.CumstomerID}";
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
            string apiUrl = $"https://localhost:7036/api/Address/update-address?AddressID={address.AddressID}&Street={address.Street}&Commune={address.Commune}&District={address.District}&Province={address.Province}&IsDefaultAddress={address.IsDefaultAddress}&ShippingCost={address.ShippingCost}&Status={address.Status}&DateCreated={DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}&CumstomerID={address.CumstomerID}";
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
