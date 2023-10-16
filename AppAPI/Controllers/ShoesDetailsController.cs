using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoesDetailsController : ControllerBase
    {
        private readonly IAllRepositories<ShoesDetails> repos;
        ShopDBContext context = new ShopDBContext();
        DbSet<ShoesDetails> shoesdt;

        public ShoesDetailsController()
        {
            shoesdt = context.ShoesDetails;
            AllRepositories<ShoesDetails> all = new AllRepositories<ShoesDetails>(context, shoesdt);
            repos = all;
        }

        [HttpGet("get-shoesdetails")]
        public IEnumerable<ShoesDetails> GetAllShoesDetails() 
        {
            return repos.GetAll();
        }

        [HttpGet("find-shoesdetails")]
        public IEnumerable<ShoesDetails> FindShoesDetails(string shoesDetailsCode)
        {
            return repos.GetAll().Where(p => p.ShoesDetailsCode == shoesDetailsCode);
        }

        // POST api/<ShoesDetailsController>
        [HttpPost("create-shoesdetail")] 
        public bool CreateShoesDetail(string shoesdetailsCode, DateTime dateCreated, decimal price, decimal importprice, string description, int status, Guid colorId, Guid productId, Guid soleId, Guid styleId) 
        {
            ShoesDetails shoesdt = new ShoesDetails(); 
            shoesdt.ShoesDetailsId = Guid.NewGuid();
            shoesdt.ShoesDetailsCode = shoesdetailsCode;
            shoesdt.DateCreated = dateCreated;
            shoesdt.Price = price;
            shoesdt.ImportPrice = importprice;
            shoesdt.Description = description;
            shoesdt.Status = status;
            shoesdt.ColorID = colorId;
            shoesdt.ProductID = productId;
            shoesdt.SoleID = soleId;
            shoesdt.StyleID = styleId;
            return repos.AddItem(shoesdt);
        }

        // PUT api/<ShoesDetailsController>/5
        [HttpPut("edit-shoesdetail")]
        public bool Put(Guid id, string shoesdetailsCode, DateTime dateCreated, decimal price, decimal importprice, string description, int status, Guid colorId, Guid productId, Guid soleId, Guid styleId) 
        {
            var shoesdt = repos.GetAll().First(p => p.ShoesDetailsId == id);
            shoesdt.ShoesDetailsCode = shoesdetailsCode;
            shoesdt.DateCreated = dateCreated;
            shoesdt.Price = price;
            shoesdt.ImportPrice = importprice;
            shoesdt.Description = description;
            shoesdt.Status = status;
            shoesdt.ColorID = colorId;
            shoesdt.ProductID = productId;
            shoesdt.SoleID = soleId;
            shoesdt.StyleID = styleId;
            return repos.EditItem(shoesdt);
        }

        [HttpDelete("delete-shoesdetail")]
        public bool Delete(Guid id)
        {
            var shoesdt = repos.GetAll().First(p => p.ShoesDetailsId == id);
            shoesdt.Status = 1;
            return repos.EditItem(shoesdt);
        }
    }
}
