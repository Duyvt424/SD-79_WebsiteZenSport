using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
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

        // GET: api/<CouponController1>
        [HttpGet("get-coupon")]
        public IEnumerable<Coupon> GetAll()
        {
            return _repos.GetAll();
        }

        // GET api/<CouponController1>/5
        //[HttpGet("find-customer")]
        //public IEnumerable<Customer> GetAll(string name)
        //{
        //    return _repos.GetAll().Where(c => c.UserName.ToLower().Contains(name.ToLower())).ToList();
        //}



        // POST api/<CouponController1>
        [HttpPost("create-coupon")]
        public bool CreateCoupon(string CouponCode, decimal CouponValue, int MaxUsage, int RemainingUsage, DateTime ExpirationDate, int Status)
        {
            Coupon coupon = new Coupon();
            coupon.CouponCode = CouponCode;
            coupon.CouponValue = CouponValue;
            coupon.MaxUsage = MaxUsage;
            coupon.RemainingUsage = RemainingUsage;
            //coupon.ExpirationDate = Convert.ToDateTime(ExpirationDate);
            coupon.ExpirationDate = ExpirationDate;
            coupon.CouponID = Guid.NewGuid();
            return _repos.AddItem(coupon);
           
        }


        // PUT api/<CouponController1>/5
        [HttpPut("update-coupon")]
        public bool Put( Guid CouponID,string CouponCode, decimal CouponValue, int MaxUsage, int RemainingUsage, DateTime ExpirationDate, int status)
        {
            var coupon = _repos.GetAll().First(c => c.CouponID == CouponID);
            coupon.CouponCode = CouponCode;
            coupon.CouponValue = CouponValue;
            coupon.MaxUsage = MaxUsage;
            coupon.RemainingUsage= RemainingUsage;
            //coupon.ExpirationDate = Convert.ToDateTime(ExpirationDate);
            coupon.ExpirationDate = ExpirationDate;
            coupon.Status= status;
           
            return _repos.EditItem(coupon);


        }

        // DELETE api/<CouponController1>/5
        [HttpDelete("delete-coupon")]
        public bool Delete(Guid id)
        {
            var coupon = _repos.GetAll().First(c => c.CouponID == id);
            return _repos.RemoveItem(coupon);
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
