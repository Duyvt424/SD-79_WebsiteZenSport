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
    public class BillDetailsController : ControllerBase
    {
        private readonly IAllRepositories<BillDetails> _repos;
        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<BillDetails> _billdetails;
        public BillDetailsController()
        {
            _billdetails = _dbContext.BillDetails;
            AllRepositories<BillDetails> all = new AllRepositories<BillDetails>(_dbContext, _billdetails);
            _repos = all;
        }
        // GET: api/<BillDetailsController>
        [HttpGet("get-billdetails")]
        public IEnumerable<BillDetails> GetBillDetails()
        {
            return _repos.GetAll();
        }

        [HttpPost("create-billdetails")]
        public string CreateBilldetails(int Quantity, decimal Price, int Status, Guid ShoesDetails_SizeId, Guid BillID)
        {
            var billDT = _repos.GetAll().FirstOrDefault(c => c.BillID == BillID && c.ShoesDetails_SizeID == ShoesDetails_SizeId);
            var shoesDT = _dbContext.ShoesDetails_Sizes.FirstOrDefault(c => c.ID == ShoesDetails_SizeId).ShoesDetailsId;
            if (billDT != null)
            {
                var newQuantity = billDT.Quantity += Quantity;
                //if (newQuantity > shoesDT.AvailableQuantity)
                //{
                //    return "Số lượng sản phẩm đã hết";
                //}
                billDT.Quantity = newQuantity;
                billDT.Price += Price;
                billDT.Status = Status;
                if (_repos.EditItem(billDT))
                {
                    return "Cộng dồn thành công";
                }
                else
                {
                    return "Cộng dồn thất bại";
                }
            }
            else
            {
                BillDetails billDetails = new BillDetails();
                billDetails.ID = Guid.NewGuid();
                billDetails.Quantity = Quantity;
                billDetails.Price = Price;
                billDetails.Status = Status;
                billDetails.BillID = BillID;
                billDetails.ShoesDetails_SizeID = ShoesDetails_SizeId;
                if (_repos.AddItem(billDetails))
                {
                    return "Thêm thành công";
                }
                else
                {
                    return "Thêm thất bại";
                }
            }
        }

        // PUT api/<BillDetailsController>/5
        [HttpPut("update-billdetails")]
        public string UpdateBillDetails(Guid id, int Quantity, decimal Price, int Status, Guid ShoesDetails_SizeId, Guid BillID)
        {
            var billDetails = _repos.GetAll().First(c => c.ID == id);
            billDetails.Quantity = Quantity;
            billDetails.Price = Price;
            billDetails.Status = Status;
            billDetails.ShoesDetails_SizeID = ShoesDetails_SizeId;
            billDetails.BillID = BillID;
            if (_repos.EditItem(billDetails))
            {
                return "Sửa thành công";
            }
            else
            {
                return "Sửa thất bại";
            }
        }

        // DELETE api/<BillDetailsController>/5
        [HttpDelete("delete-billDetails")]
        public string Delete(Guid id)
        {
            var billDT = _repos.GetAll().First(c => c.ID == id);
            if (_repos.RemoveItem(billDT))
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
