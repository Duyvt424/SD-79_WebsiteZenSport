using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReturnedProductsController : ControllerBase
	{
        private readonly IAllRepositories<ReturnedProducts> _repos;
        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<ReturnedProducts> _returnPro;
        // GET: api/<ColorController1>

        public ReturnedProductsController()
        {
            _returnPro = _dbContext.ReturnedProducts;
            AllRepositories<ReturnedProducts> all = new AllRepositories<ReturnedProducts>(_dbContext, _returnPro);
            _repos = all;
        }

        [HttpGet("get-returnedProducts")]
        public IEnumerable<ReturnedProducts> GetAll()
        {
            return _repos.GetAll();
        }

        // POST api/<ColorController1>
        [HttpPost("create-returnedProducts")]
        public bool CreateReturnedProducts(DateTime CreateDate, string Note, int QuantityReturned, decimal ReturnedPrice, decimal ShippingFeeReturned, decimal InitialProductTotalPrice, int Status, Guid BillId, Guid ShoesDetails_SizeID)
        {
            ReturnedProducts returned = new ReturnedProducts();
            returned.ID = Guid.NewGuid();
            returned.CreateDate = CreateDate;
            returned.Note = Note;
            returned.QuantityReturned = QuantityReturned;
            returned.ReturnedPrice = ReturnedPrice;
            returned.ShippingFeeReturned = ShippingFeeReturned;
            returned.InitialProductTotalPrice = InitialProductTotalPrice;
            returned.Status = Status;
            returned.BillId = BillId;
            returned.ShoesDetails_SizeID = ShoesDetails_SizeID;
            return _repos.AddItem(returned);
        }

        // PUT api/<ColorController1>/5
        [HttpPut("update-returnedProducts")]
        public bool Put(Guid ID ,DateTime CreateDate, string Note, int QuantityReturned, decimal ReturnedPrice, decimal ShippingFeeReturned, decimal InitialProductTotalPrice, int Status, Guid BillId, Guid ShoesDetails_SizeID)
        {
            var returned = _repos.GetAll().First(c => c.ID == ID);
            returned.CreateDate = CreateDate;
            returned.Note = Note;
            returned.QuantityReturned = QuantityReturned;
            returned.ReturnedPrice = ReturnedPrice;
            returned.ShippingFeeReturned = ShippingFeeReturned;
            returned.InitialProductTotalPrice = InitialProductTotalPrice;
            returned.Status = Status;
            returned.BillId = BillId;
            returned.ShoesDetails_SizeID = ShoesDetails_SizeID;
            return _repos.EditItem(returned);

        }


        // DELETE api/<ColorController1>/5
        [HttpDelete("delete-returnedProducts")]
        public bool Delete(Guid ID)
        {
            var returned = _repos.GetAll().First(c => c.ID == ID);
            returned.Status = 1;
            return _repos.EditItem(returned);
        }
    }
}
