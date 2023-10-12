using System.Data;
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
		private string GenerateVoucherCode()
		{
			var last = context.Vouchers.OrderByDescending(c => c.VoucherCode).FirstOrDefault();
			if (last != null)
			{
				var lastNumber = int.Parse(last.VoucherCode.Substring(2)); // Lấy phần số cuối cùng từ ColorCode
				var nextNumber = lastNumber + 1; // Tăng giá trị cuối cùng
				var newCode = "VC" + nextNumber.ToString("D3"); // Tạo ColorCode mới
				return newCode;
			}
			return "VC001"; // Trường hợp không có ColorCode trong cơ sở dữ liệu, trả về giá trị mặc định "CL001"
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

		public async Task<IActionResult> CreateVouchers()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateVouchers(Voucher voucher)
		{


            var httpClient = new HttpClient();
			string apiUrl = $"https://localhost:7036/api/Voucher/create-voucher?VoucherCode={GenerateVoucherCode}&VoucherValue={voucher.VoucherValue}&MaxUsage={voucher.MaxUsage}&RemainingUsage={voucher.RemainingUsage}&ExpirationDate={voucher.ExpirationDate}&Status={voucher.Status}&DateCreated={voucher.DateCreated}";
			var response = await httpClient.PostAsync(apiUrl, null);
			return RedirectToAction("GetAllVouchers"); 
            /*if (repos.AddItem(voucher))
            {
                return RedirectToAction("GetAllVouchers");
            }
            else return BadRequest();*/
        }
		[HttpGet]

		public IActionResult EditVouchers(Guid id) // Khi ấn vào Create thì hiển thị View
		{
			// Lấy Product từ database dựa theo id truyền vào từ route
			Voucher voucher = repos.GetAll().FirstOrDefault(c => c.VoucherID == id);
			return View(voucher);
		}

		public IActionResult EditVouchers(Voucher voucher) // Thực hiện việc Tạo mới
		{
			if (repos.EditItem(voucher))
			{
				return RedirectToAction("GetAllVouchers");
			}
			else return BadRequest();
		}

		public IActionResult DeleteVouchers(Guid id)
		{
			var voucher = repos.GetAll().First(c => c.VoucherID == id);
			if (repos.RemoveItem(voucher))
			{
				return RedirectToAction("GetAllVouchers");
			}
			else return Content("Error");
		}
		public async Task<IActionResult> FindVouchers(string searchQuery)
        {
            var color = repos.GetAll().Where(c => c.VoucherCode.ToLower().Contains(searchQuery.ToLower()));
            return View(color);
        }
	}
}
