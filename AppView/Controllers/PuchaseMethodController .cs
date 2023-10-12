﻿using System.Data;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AppView.Controllers
{
    public class PuchaseMethodController : Controller
    {

        private readonly IAllRepositories<PurchaseMethod> _repos;
        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<PurchaseMethod> _pu;
        public PuchaseMethodController()
        {
            _pu = _dbContext.PurchaseMethods;
            AllRepositories<PurchaseMethod> all = new AllRepositories<PurchaseMethod>(_dbContext, _pu);
            _repos = all;
        }  
   
        public async Task<IActionResult> GetAllPu()
        {

            string apiUrl = "https://localhost:7036/api/PurchaseMethod/get-pu";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            // Lấy kqua trả về từ API
            // Đọc từ string Json vừa thu được sang List<T>
            var color = JsonConvert.DeserializeObject<List<PurchaseMethod>>(apiData);
            return View(color);
        }

        [HttpGet]
        public async Task<IActionResult> CreatePu()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreatePu(PurchaseMethod pu)
        {
            if (_repos.EditItem(pu))
            {
                return RedirectToAction("GetAllPu");
            }
            else return BadRequest();
        }
		[HttpGet]
		public async Task<IActionResult> EditPu(Guid id) // Khi ấn vào Create thì hiển thị View
		{
			// Lấy Product từ database dựa theo id truyền vào từ route
			PurchaseMethod role = _repos.GetAll().FirstOrDefault(c => c.PurchaseMethodID == id);
			return View(role);
		}
		public async Task<IActionResult> EditPu(PurchaseMethod role) // Thực hiện việc Tạo mới
		{
			/*var httpClient = new HttpClient();
			string apiUrl = $"https://localhost:7036/api/Role/update-role?RoleCode={role.RoleCode}&RoleName={role.RoleName}&Status={role.Status}&DateCreated={role.DateCreated}&ColorID={role.RoleID}";
			var response = await httpClient.PutAsync(apiUrl, null);
			return RedirectToAction("GetAllRole");*/
			if (_repos.EditItem(role))
			{
				return RedirectToAction("GetAllPu");
			}
			else return BadRequest();
		}
		public async Task<IActionResult> DeletePu(Guid id)
		{
			var cus = _repos.GetAll().First(c => c.PurchaseMethodID == id);
			var httpClient = new HttpClient();
			string apiUrl = $"https://localhost:7036/api/PurchaseMethod/delete-pu?id={id}";
			var response = await httpClient.DeleteAsync(apiUrl);
			return RedirectToAction("GetAllPu");
		}
		public async Task<IActionResult> FindPu(string searchQuery)
		{
			var color = _repos.GetAll().Where(c => c.MethodName.ToLower().Contains(searchQuery.ToLower()) );
			return View(color);
		}
		
	}
}
