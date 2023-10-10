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
    public class ShoesDetailsController : ControllerBase
    {
        private readonly IAllRepositories<ShoesDetails> repos;
        private readonly IAllRepositories<Color> colorrp;
        private readonly IAllRepositories<Product> productrp;
        private readonly IAllRepositories<Size> sizerp;
        private readonly IAllRepositories<Sole> solerp;
        private readonly IAllRepositories<Style> stylerp;
        private readonly IAllRepositories<Supplier> supplierrp;
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
        public IEnumerable<ShoesDetails> FindShoesDetails(Guid id)
        {
            return repos.GetAll().Where(p => p.ShoesDetailsId == id);
        }

        // POST api/<ShoesDetailsController>
        [HttpPost("create-shoesdetail")] 
        public bool CreateShoesDetail(string createdate, decimal price, decimal importprice, int availablequantity, string description, int status, Guid colorid, Guid productid, Guid soleid, Guid styleid, Guid supplierid) 
        {
            ShoesDetails shoesdt = new ShoesDetails(); 
            shoesdt.ShoesDetailsId = Guid.NewGuid();
            shoesdt.DateCreated = DateTime.Parse(createdate);
            shoesdt.Price = price;
            shoesdt.ImportPrice = importprice;
            //shoesdt.AvailableQuantity = availablequantity;
            shoesdt.Description = description;
            shoesdt.Status = status;
            shoesdt.ColorID = colorid;
            shoesdt.ProductID = productid;
            shoesdt.SoleID = soleid;
            shoesdt.StyleID = styleid;
            //shoesdt.Color.Name = namecolor;
           // shoesdt.ColorID = colorrp.GetAll().Where(p => p.Name == colorid).Select(p => p.ColorID).FirstOrDefault();
            //shoesdt.ProductID = productrp.GetAll().Where(p => p.Name == nameproduct).Select(p => p.ProductID).FirstOrDefault();
            //shoesdt.SizeID = sizerp.GetAll().Where(p => p.Name == namesize).Select(p => p.SizeID).FirstOrDefault();
            //shoesdt.SoleID = solerp.GetAll().Where(p => p.Name == namesole).Select(p => p.SoleID).FirstOrDefault();
            //shoesdt.StyleID = stylerp.GetAll().Where(p => p.Name == namestyle).Select(p => p.StyleID).FirstOrDefault();
            //shoesdt.SupplierID = supplierrp.GetAll().Where(p => p.Name == namesupplier).Select(p => p.SupplierID).FirstOrDefault();
            return repos.AddItem(shoesdt);
        }

        // PUT api/<ShoesDetailsController>/5
        [HttpPut("edit-shoesdetail")]
        public bool Put(Guid id, string createdate, decimal price, decimal importprice, int availablequantity, string description, int status, Guid colorid, Guid productid, Guid soleid, Guid styleid, Guid supplierid) 
        {
            var shoesdt = repos.GetAll().First(p => p.ShoesDetailsId == id);
            shoesdt.DateCreated = DateTime.Parse(createdate);
            shoesdt.Price = price;
            shoesdt.ImportPrice = importprice;
            //shoesdt.AvailableQuantity = availablequantity;
            shoesdt.Description = description;
            shoesdt.Status = status;
            shoesdt.ColorID = colorid;
            shoesdt.ProductID = productid;
            shoesdt.SoleID = soleid;
            shoesdt.StyleID = styleid;
            
            //shoesdt.ColorID = colorrp.GetAll().Where(p => p.Name == namecolor).Select(p => p.ColorID).FirstOrDefault();
            //shoesdt.ProductID = productrp.GetAll().Where(p => p.Name == nameproduct).Select(p => p.ProductID).FirstOrDefault();
            //shoesdt.SizeID = sizerp.GetAll().Where(p => p.Name == namesize).Select(p => p.SizeID).FirstOrDefault();
            //shoesdt.SoleID = solerp.GetAll().Where(p => p.Name == namesole).Select(p => p.SoleID).FirstOrDefault();
            //shoesdt.StyleID = stylerp.GetAll().Where(p => p.Name == namestyle).Select(p => p.StyleID).FirstOrDefault();
            //shoesdt.SupplierID = supplierrp.GetAll().Where(p => p.Name == namesupplier).Select(p => p.SupplierID).FirstOrDefault();
            return repos.EditItem(shoesdt);
        }

        // DELETE api/<ShoesDetailsController>/5
        [HttpDelete("delete-shoesdetail")]
        public bool Delete(Guid id)
        {
            var shoesdt = repos.GetAll().First(p => p.ShoesDetailsId == id);
            return repos.RemoveItem(shoesdt);
        }
    }
}
