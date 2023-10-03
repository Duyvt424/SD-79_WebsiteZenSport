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
    public class BillController : ControllerBase
    {
        private readonly IAllRepositories<Bill> _repos;
        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<Bill> _bill;
        public BillController()
        {
            _bill = _dbContext.Bills;
            AllRepositories<Bill> all = new AllRepositories<Bill>(_dbContext, _bill);
            _repos = all;
        }
        // GET: api/<BillController>
        [HttpGet("get-bill")]
        public IEnumerable<Bill> GetAllBill()
        {
            return _repos.GetAll();
        }

        // GET api/<BillController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<BillController>
        [HttpPost("create-bill")]
        public string CreateBill(string BillCode, DateTime CreateDate, DateTime SuccessDate, DateTime DeliveryDate, DateTime CancelDate, decimal TotalPrice, decimal ShippingCosts, string Note, int Status, Guid CouponID, Guid CustomerID, Guid VoucherID, Guid EmployeeID)
        {
            Bill bill = new Bill();
            bill.BillID = Guid.NewGuid();
            bill.BillCode = BillCode;
            bill.CreateDate = CreateDate;
            bill.SuccessDate = SuccessDate;
            bill.DeliveryDate = DeliveryDate;
            bill.CancelDate = CancelDate;
            bill.TotalPrice = TotalPrice;
            bill.ShippingCosts = ShippingCosts;
            bill.Note = Note;
            bill.Status = Status;
            bill.CouponID = CouponID;
            bill.VoucherID = VoucherID;
            bill.EmployeeID = EmployeeID;
            bill.CustomerID = CustomerID;
            if (_repos.AddItem(bill))
            {
                return "Thêm thành công";
            }
            else
            {
                return "Thêm thất bại";
            }
        }

        // PUT api/<BillController>/5
        [HttpPut("update-bill")]
        public string UpdateBill(Guid BillID, string BillCode, DateTime CreateDate, DateTime SuccessDate, DateTime DeliveryDate, DateTime CancelDate, decimal TotalPrice, decimal ShippingCosts, string Note, int Status, Guid CouponID, Guid CustomerID, Guid VoucherID, Guid EmployeeID)
        {
            var bill = _repos.GetAll().First(c => c.BillID == BillID);
            bill.BillCode = BillCode;
            bill.CreateDate = CreateDate;
            bill.SuccessDate = SuccessDate;
            bill.DeliveryDate = DeliveryDate;
            bill.CancelDate = CancelDate;
            bill.TotalPrice = TotalPrice;
            bill.ShippingCosts = ShippingCosts;
            bill.Note = Note;
            bill.Status = Status;
            bill.CouponID = CouponID;
            bill.CustomerID = CustomerID;
            bill.EmployeeID = EmployeeID;
            bill.VoucherID = VoucherID;
            if (_repos.EditItem(bill))
            {
                return "Sửa thành công";
            }
            else
            {
                return "Sửa thất bại";
            }
        }

        // DELETE api/<BillController>/5
        [HttpDelete("delete-bill")]
        public string DeleteBill(Guid id)
        {
            var bill = _repos.GetAll().First(c => c.BillID == id);
            if (_repos.RemoveItem(bill))
            {
                return "Xóa thành công";
            }
            else
            {
                return "Xóa thất bại";
            }
        }
    }
}
