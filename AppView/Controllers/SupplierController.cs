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
        private string GenerateSupplierCode()
        {
            var lastSupplier = context.Suppliers.OrderByDescending(c => c.SupplierCode).FirstOrDefault();
            if (lastSupplier != null)
            {
                var lastNumber = int.Parse(lastSupplier.SupplierCode.Substring(2)); // Lấy phần số cuối cùng từ ColorCode
                var nextNumber = lastNumber + 1; // Tăng giá trị cuối cùng
                var newSupplierCode = "SP" + nextNumber.ToString("D3");
                return newSupplierCode;
            }
            return "SP001"; // Trường hợp không có ColorCode trong cơ sở dữ liệu, trả về giá trị mặc định "CL001"
        }

        public async Task<IActionResult> GetAllSuppliers()
		{
			string apiUrl = "https://localhost:7036/api/Supplier/get-supplier";
			var httpClient = new HttpClient(); // tạo ra để callApi
			var response = await httpClient.GetAsync(apiUrl);
			string apiData = await response.Content.ReadAsStringAsync();
			var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(apiData);
			return View(suppliers);
		}

		public async Task<IActionResult> CreateSupplier()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateSupplier(Supplier supplier)
		{
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Supplier/create-supplier?supplierCode={GenerateSupplierCode()}&name={supplier.Name}&status={supplier.Status}&dateCreated={supplier.DateCreated}";
			var response = await httpClient.PostAsync(apiUrl, null);
			return RedirectToAction("GetAllSuppliers");
		}

		[HttpGet]
		public async Task<IActionResult> EditSupplier(Guid id) // Khi ấn vào Create thì hiển thị View
		{
			// Lấy Product từ database dựa theo id truyền vào từ route
			Supplier supplier = repos.GetAll().FirstOrDefault(c => c.SupplierID == id);
			return View(supplier);
		}

		public async Task<IActionResult> EditSupplier(Supplier supplier) // Thực hiện việc Tạo mới
		{
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Supplier/update-supplier?id={supplier.SupplierID}&supplierCode={supplier.SupplierCode}&name={supplier.Name}&status={supplier.Status}&dateCreated={supplier.DateCreated}";
            var response = await httpClient.PutAsync(apiUrl, null);
            return RedirectToAction("GetAllSuppliers");
        }

		public async Task<IActionResult> DeleteSupplier(Guid id)
		{
            var supplier = repos.GetAll().First(c => c.SupplierID == id);
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Supplier/delete-supplier?id={id}";
            var response = await httpClient.DeleteAsync(apiUrl);
            return RedirectToAction("GetAllSuppliers");
        }
        public async Task<IActionResult> FindSupplier(string searchQuery)
        {
            var supplier = repos.GetAll().Where(c => c.SupplierCode.ToLower().Contains(searchQuery.ToLower()) || c.Name.ToLower().Contains(searchQuery.ToLower()));
            return View(supplier);
        }
    }
}
