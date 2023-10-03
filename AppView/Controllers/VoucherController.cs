using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AppView.Controllers
{
	public class VoucherController:Controller
	{
		private readonly IAllRepositories<Voucher> repos;
		private ShopDBContext context = new ShopDBContext();
		private DbSet<Voucher> voucher;

		public VoucherController()
		{
			voucher = context.Vouchers;
			AllRepositories<Voucher> all = new AllRepositories<Voucher>(context, voucher);
			repos = all;
		}

		public async Task<IActionResult> GetAllVouchers()
		{
			string apiUrl = "https://localhost:7036/api/Voucher/get-voucher";
			var httpClient = new HttpClient(); // tạo ra để callApi
			var response = await httpClient.GetAsync(apiUrl);// Lấy dữ liệu ra
															 // Lấy dữ liệu Json trả về từ Api được call dạng string
			string apiData = await response.Content.ReadAsStringAsync();
			// Lấy kqua trả về từ API
			// Đọc từ string Json vừa thu được sang List<T>
			var styles = JsonConvert.DeserializeObject<List<Voucher>>(apiData);
			return View(styles);
		}

		public async Task<IActionResult> CreateVoucher()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateVoucher(Voucher voucher)
		{
			string apiUrl = $"https://localhost:7036/api/Customer/create-voucher?code={voucher.VoucherCode}&status={voucher.Status}&value={voucher.VoucherValue}&maxUse={voucher.MaxUsage}&remainUse={voucher.RemainingUsage}&expireDate={voucher.ExpirationDate}";
			var httpClient = new HttpClient();
			var response = await httpClient.GetAsync(apiUrl);
			string apiData = await response.Content.ReadAsStringAsync();
			// Cập nhật thông tin từ apiData vào đối tượng customer
			var newStyle = JsonConvert.DeserializeObject<Voucher>(apiData);
			repos.AddItem(voucher);
			return RedirectToAction("GetAllVouchers");
		}

		[HttpGet]
		public IActionResult EditVoucher(Guid id) // Khi ấn vào Create thì hiển thị View
		{
			// Lấy Product từ database dựa theo id truyền vào từ route
			Voucher voucher = repos.GetAll().FirstOrDefault(c => c.VoucherID == id);
			return View(voucher);
		}

		public IActionResult EditVoucher(Voucher voucher) // Thực hiện việc Tạo mới
		{
			if (repos.EditItem(voucher))
			{
				return RedirectToAction("GetAllVouchers");
			}
			else return BadRequest();
		}

		public IActionResult DeleteVoucher(Guid id)
		{
			var voucher = repos.GetAll().First(c => c.VoucherID == id);
			if (repos.RemoveItem(voucher))
			{
				return RedirectToAction("GetAllVouchers");
			}
			else return Content("Error");
		}
	}
}
