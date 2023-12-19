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
	public class ShippingVoucherController:Controller
	{
		private readonly IAllRepositories<ShippingVoucher> repos;
		private ShopDBContext context = new ShopDBContext();
		private DbSet<ShippingVoucher> voucher;

		public ShippingVoucherController()
		{
			voucher = context.ShippingVoucher;
			AllRepositories<ShippingVoucher> all = new AllRepositories<ShippingVoucher>(context, voucher);
			repos = all;
		}
		private string GenerateVoucherCode()
		{
			var last = context.ShippingVoucher.OrderByDescending(c => c.VoucherShipCode).FirstOrDefault();
			if (last != null)
			{
				var lastNumber = int.Parse(last.VoucherShipCode.Substring(2)); // Lấy phần số cuối cùng từ ColorCode
				var nextNumber = lastNumber + 1; // Tăng giá trị cuối cùng
				var newCode = "SV" + nextNumber.ToString("D3"); // Tạo ColorCode mới
				return newCode;
			}
			return "SV001"; // Trường hợp không có ColorCode trong cơ sở dữ liệu, trả về giá trị mặc định "CL001"
		}
		public async Task<IActionResult> GetAllShippingVouchers()
		{
			string apiUrl = "https://localhost:7036/api/ShippingVoucher/get-shippingVoucher";
			var httpClient = new HttpClient(); // tạo ra để callApi
			var response = await httpClient.GetAsync(apiUrl);// Lấy dữ liệu ra
			string apiData = await response.Content.ReadAsStringAsync();
			var styles = JsonConvert.DeserializeObject<List<ShippingVoucherViewModel>>(apiData);
			/*ShoppingCartViewModel s = new ShoppingCartViewModel();
			s.Vouchers = styles.ToList();*/
		

			return View(styles);
       
        }

		public async Task<IActionResult> CreateVouchers()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateVouchers(ShippingVoucher voucher)
		{
            var httpClient = new HttpClient();
			string apiUrl = $"https://localhost:7036/api/ShippingVoucher/create-shippingVoucher?code={GenerateVoucherCode()}&MaxShippingDiscount={voucher.MaxShippingDiscount}&ShippingDiscount={voucher.ShippingDiscount}&QuantityShip={voucher.QuantityShip}&IsShippingVoucher={voucher.IsShippingVoucher}&expireDate={voucher.ExpirationDate.ToString("yyyy-MM-ddTHH:mm:ss")}&DateCreated={voucher.DateCreated.ToString("yyyy-MM-ddTHH:mm:ss")}";
			var response = await httpClient.PostAsync(apiUrl, null);
			return RedirectToAction("GetAllShippingVouchers"); 
        }

		[HttpGet]
		public async Task<IActionResult> EditVouchers(Guid id) // Khi ấn vào Create thì hiển thị View
		{
			// Lấy Product từ database dựa theo id truyền vào từ route
			ShippingVoucher voucher = repos.GetAll().FirstOrDefault(c => c.ShippingVoucherID == id);
			return View(voucher);
		}

		public async Task<IActionResult> EditVouchers(ShippingVoucher voucher)
		{
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/ShippingVoucher/update-shippingVoucher?id={voucher.ShippingVoucherID}&MaxShippingDiscount={voucher.MaxShippingDiscount}&ShippingDiscount={voucher.ShippingDiscount}&QuantityShip={voucher.QuantityShip}&IsShippingVoucher={voucher.IsShippingVoucher}&expireDate={voucher.ExpirationDate.ToString("yyyy-MM-ddTHH:mm:ss")}&DateCreated={voucher.DateCreated.ToString("yyyy-MM-ddTHH:mm:ss")}";
            var response = await httpClient.PutAsync(apiUrl, null);
            return RedirectToAction("GetAllShippingVouchers");
        }

		public async Task<IActionResult> DeleteVouchers(Guid id)
		{
            var voucher = repos.GetAll().First(c => c.ShippingVoucherID == id);
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Voucher/delete-voucher?id={id}";
            var response = await httpClient.DeleteAsync(apiUrl);
            return RedirectToAction("GetAllShippingVouchers");
        }
		public async Task<IActionResult> FindVouchers(string searchQuery)
        {
            var color = repos.GetAll().Where(c => c.VoucherShipCode.ToLower().Contains(searchQuery.ToLower()));
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
			var vc = repos.GetAll().FirstOrDefault( c=> c.ShippingVoucherID == Id);
			return View(vc);
		}


        public ActionResult LockVoucher(Guid id)
        {
            // Lấy voucher từ cơ sở dữ liệu dựa trên id
            var voucher = context.Vouchers.Find(id);

            if (voucher != null)
            {
                // Thực hiện logic để khóa voucher
                voucher.Status = 1; // Đặt Status thành 1 để chỉ định đã khóa

                // Cập nhật cơ sở dữ liệu
                context.SaveChanges();
            }

            // Chuyển hướng lại danh sách voucher
            return RedirectToAction("GetAllShippingVouchers");
        }

        public ActionResult UnlockVoucher(Guid id)
        {
            // Lấy voucher từ cơ sở dữ liệu dựa trên id
            var voucher = context.Vouchers.Find(id);

            if (voucher != null)
            {
                // Thực hiện logic để mở khóa voucher
                voucher.Status = 0; // Đặt Status thành 0 để chỉ định đã mở khóa

                // Cập nhật cơ sở dữ liệu
                context.SaveChanges();
            }

            // Chuyển hướng lại danh sách voucher
            return RedirectToAction("GetAllShippingVouchers");
        }
    }
}
