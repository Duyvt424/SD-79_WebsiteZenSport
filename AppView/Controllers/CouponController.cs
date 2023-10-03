using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AppView.Controllers
{
    public class CouponController : Controller
    {
        private readonly IAllRepositories<Coupon> _repos;
        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<Coupon> _coupon;
        public CouponController()
        {
            _coupon = _dbContext.Coupons;
            AllRepositories<Coupon> all = new AllRepositories<Coupon>(_dbContext, _coupon);
            _repos = all;
        }
       

        public async Task<IActionResult> GetAllCoupon()
        {
            string apiUrl = "https://localhost:7036/api/Coupon/get-coupon";
            var httpClient = new HttpClient(); // tạo ra để callApi
            var response = await httpClient.GetAsync(apiUrl);// Lấy dữ liệu ra
                                                             // Lấy dữ liệu Json trả về từ Api được call dạng string
            string apiData = await response.Content.ReadAsStringAsync();
            // Lấy kqua trả về từ API
            // Đọc từ string Json vừa thu được sang List<T>
            var coupon = JsonConvert.DeserializeObject<List<Coupon>>(apiData);
            return View(coupon);
        }
        [HttpGet]
        public async Task<IActionResult> CreateCoupon()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCoupon(Coupon coupon)
        {
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Coupon/create-coupon?Couponcode={coupon.CouponCode}&CouponValue={coupon.CouponValue}&MaxUsage={coupon.MaxUsage}&RemainingUsage={coupon.RemainingUsage}&ExpirationDate={coupon.ExpirationDate}&Status={coupon.Status}";
            var response = await httpClient.PostAsync(apiUrl, null);
            return RedirectToAction("GetAllCoupon");
        }

        [HttpGet]
        public async Task<IActionResult> EditCoupon(Guid id) // Khi ấn vào Create thì hiển thị View
        {
            // Lấy Product từ database dựa theo id truyền vào từ route
            Coupon coupon = _repos.GetAll().FirstOrDefault(c => c.CouponID == id);
            return View(coupon);
        }
        public async Task<IActionResult> EditCoupon(Coupon coupon) // Thực hiện việc Tạo mới
        {
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Coupon/update-coupon?CouponID={coupon.CouponID}&CouponCode={coupon.CouponCode}&CouponValue={coupon.CouponValue}&MaxUsage={coupon.MaxUsage}&RemainingUsage={coupon.RemainingUsage}&ExpirationDate={coupon.ExpirationDate}&status={coupon.Status}";
            var response = await httpClient.PutAsync(apiUrl, null);
            return RedirectToAction("GetAllCoupon");

            
            //if (_repos.EditItem(coupon))
            //{
            //    return RedirectToAction("GetAllCoupon");
            //}
            //else return BadRequest();
        }
        public async Task<IActionResult> DeleteCoupon(Guid id)
        {
            var cos = _repos.GetAll().First(c => c.CouponID == id);
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Coupon/delete-coupon?id={id}";
            var response = await httpClient.DeleteAsync(apiUrl);
            return RedirectToAction("GetAllCoupon");


          
            //if (_repos.RemoveItem(cus))
            //var coup = _repos.GetAll().First(c => c.CouponID == id);
            //if (_repos.RemoveItem(coup))
            //{
            //    return RedirectToAction("GetAllCoupon");
            //}
            //else return Content("Error");
        }
    }
}
