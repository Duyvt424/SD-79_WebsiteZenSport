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
    public class ShoesDetails_SizeController : ControllerBase
    {
        private readonly IAllRepositories<ShoesDetails_Size> repos;
        ShopDBContext context = new ShopDBContext();
        DbSet<ShoesDetails_Size> shoesDetails_Size;
        public ShoesDetails_SizeController()
        {
            shoesDetails_Size = context.ShoesDetails_Sizes;
            AllRepositories<ShoesDetails_Size> all = new AllRepositories<ShoesDetails_Size>(context, shoesDetails_Size);
            repos = all;
        }

        [HttpGet("get-shoesdetails_size")]
        public IEnumerable<ShoesDetails_Size> GetAllShoesDetails_Size()
        {
            return repos.GetAll();
        }

        [HttpPost("add-shoesdetails_size")]
        public bool CreateShoesDetails_Size(int quantity, Guid ShoesDetailsId, Guid SizeID)
        {
            ShoesDetails_Size shoesDetails_Size = new ShoesDetails_Size();
            shoesDetails_Size.Quantity = quantity;
            shoesDetails_Size.ShoesDetailsId = ShoesDetailsId;
            shoesDetails_Size.SizeID = SizeID;
            return repos.AddItem(shoesDetails_Size);
        }

        [HttpPut("edit-shoesdetails_size")]
        public bool UpdateShoesDetails_Size(Guid id, int quantity, Guid ShoesDetailsId, Guid SizeID)
        {
            var shoesDT_Size = repos.GetAll().First(c => c.ID == id);
            shoesDT_Size.Quantity = quantity;
            shoesDT_Size.ShoesDetailsId = ShoesDetailsId;
            shoesDT_Size.SizeID = SizeID;
            return repos.EditItem(shoesDT_Size);
        }
    }
}
