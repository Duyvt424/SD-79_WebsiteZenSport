using System.Data;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppView.Models;
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
			string apiData = await response.Content.ReadAsStringAsync();
			var styles = JsonConvert.DeserializeObject<List<VoucherViewModel>>(apiData);
			/*ShoppingCartViewModel s = new ShoppingCartViewModel();
			s.Vouchers = styles.ToList();*/
		

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
			string apiUrl = $"https://localhost:7036/api/Voucher/create-voucher?code={GenerateVoucherCode()}&status={voucher.Status}&value={voucher.VoucherValue}&maxUse={voucher.MaxUsage}&remainUse={voucher.RemainingUsage}&expireDate={voucher.ExpirationDate}&DateCreated={voucher.DateCreated}";
			var response = await httpClient.PostAsync(apiUrl, null);
			return RedirectToAction("GetAllVouchers"); 
        }

		[HttpGet]
		public async Task<IActionResult> EditVouchers(Guid id) // Khi ấn vào Create thì hiển thị View
		{
			// Lấy Product từ database dựa theo id truyền vào từ route
			Voucher voucher = repos.GetAll().FirstOrDefault(c => c.VoucherID == id);
			return View(voucher);
		}

		public async Task<IActionResult> EditVouchers(Voucher voucher)
		{
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Voucher/update-voucher?id={voucher.VoucherID}&code={voucher.VoucherCode}&status={voucher.Status}&value={voucher.VoucherValue}&maxUse={voucher.MaxUsage}&remainUse={voucher.RemainingUsage}&dateTime={voucher.ExpirationDate}&DateCreated={voucher.DateCreated}";
            var response = await httpClient.PutAsync(apiUrl, null);
            return RedirectToAction("GetAllVouchers");
        }

		public async Task<IActionResult> DeleteVouchers(Guid id)
		{
            var voucher = repos.GetAll().First(c => c.VoucherID == id);
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Voucher/delete-voucher?id={id}";
            var response = await httpClient.DeleteAsync(apiUrl);
            return RedirectToAction("GetAllVouchers");
        }
		public async Task<IActionResult> FindVouchers(string searchQuery)
        {
            var color = repos.GetAll().Where(c => c.VoucherCode.ToLower().Contains(searchQuery.ToLower()));
            return View(color);
        }

		public IActionResult UseVoucher(string voucherCode)
		{
			var voucher = context.Vouchers.FirstOrDefault(v => v.VoucherCode == voucherCode);

			if (voucher == null)
			{
				// Voucher không tồn tại
				return RedirectToAction("InvalidVoucher");
			}

			if (voucher.ExpirationDate < DateTime.Now)
			{
				// Voucher đã hết hạn
				return RedirectToAction("ExpiredVoucher");
			}

			if (voucher.RemainingUsage <= 0)
			{
				// Voucher đã hết lượt sử dụng
				return RedirectToAction("MaxUsageReached");
			}

			// Giảm số lượt sử dụng còn lại của voucher
			voucher.RemainingUsage -= 1;

			// Lưu các thay đổi vào cơ sở dữ liệu
			context.SaveChanges();

			// Chuyển hướng tới trang thành công
			return RedirectToAction("VoucherUsed");
		}

		public IActionResult InvalidVoucher()
		{
			// Xử lý khi voucher không tồn tại
			return View();
		}

		public IActionResult ExpiredVoucher()
		{
			// Xử lý khi voucher đã hết hạn
			return View();
		}

		public IActionResult MaxUsageReached()
		{
			// Xử lý khi voucher đã hết lượt sử dụng
			return View();
		}

		public IActionResult VoucherUsed()
		{
			// Xử lý khi voucher đã được sử dụng thành công
			return View();
		}

		public async Task<IActionResult> Details(Guid Id)
		{
			var vc = repos.GetAll().FirstOrDefault( c=> c.VoucherID == Id);
			return View(vc);
		}

	}
}
