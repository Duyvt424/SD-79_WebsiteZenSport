using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AppView.Controllers
{
	public class SupplierController:Controller
	{
		private readonly IAllRepositories<Supplier> repos;
		private ShopDBContext context = new ShopDBContext();
		private DbSet<Supplier> supplier;

		public SupplierController()
		{
			supplier = context.Suppliers;
			AllRepositories<Supplier> all = new AllRepositories<Supplier>(context, supplier);
			repos = all;
		}

		public async Task<IActionResult> GetAllSuppliers()
		{
			string apiUrl = "https://localhost:7036/api/Supplier/get-supplier";
			var httpClient = new HttpClient(); // tạo ra để callApi
			var response = await httpClient.GetAsync(apiUrl);// Lấy dữ liệu ra
															 // Lấy dữ liệu Json trả về từ Api được call dạng string
			string apiData = await response.Content.ReadAsStringAsync();
			// Lấy kqua trả về từ API
			// Đọc từ string Json vừa thu được sang List<T>
			var styles = JsonConvert.DeserializeObject<List<Supplier>>(apiData);
			return View(styles);
		}

		public async Task<IActionResult> CreateSupplier()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateSupplier(Supplier supplier)
		{
			string apiUrl = $"https://localhost:7036/api/Customer/create-supplier?name={supplier.Name}&status={supplier.Status}";
			var httpClient = new HttpClient();
			var response = await httpClient.GetAsync(apiUrl);
			string apiData = await response.Content.ReadAsStringAsync();
			// Cập nhật thông tin từ apiData vào đối tượng customer
			var newStyle = JsonConvert.DeserializeObject<Supplier>(apiData);
			repos.AddItem(supplier);
			return RedirectToAction("GetAllSuppliers");
		}

		[HttpGet]
		public IActionResult EditSupplier(Guid id) // Khi ấn vào Create thì hiển thị View
		{
			// Lấy Product từ database dựa theo id truyền vào từ route
			Supplier supplier = repos.GetAll().FirstOrDefault(c => c.SupplierID == id);
			return View(supplier);
		}

		public IActionResult EditSupplier(Supplier supplier) // Thực hiện việc Tạo mới
		{
			if (repos.EditItem(supplier))
			{
				return RedirectToAction("GetAllSuppliers");
			}
			else return BadRequest();
		}

		public IActionResult DeleteSupplier(Guid id)
		{
			var supplier = repos.GetAll().First(c => c.SupplierID == id);
			if (repos.RemoveItem(supplier))
			{
				return RedirectToAction("GetAllSuppliers");
			}
			else return Content("Error");
		}
	}
}
