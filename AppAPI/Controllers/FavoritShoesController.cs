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
    public class FavoritshoesController : ControllerBase
    {
        // GET: api/<FavoritshoesController>
        private readonly IAllRepositories<FavoriteShoes> repos;
        ShopDBContext _dbContext = new ShopDBContext();
        DbSet<FavoriteShoes> _favorite;
        public FavoritshoesController()
        {
            _favorite = _dbContext.FavoriteShoes;
            AllRepositories<FavoriteShoes> all = new AllRepositories<FavoriteShoes>(_dbContext, _favorite);
            repos = all;
        }
        
        // GET api/<FavoritshoesController>/5
        [HttpGet("get-favoriteshoes")]
        public IEnumerable<FavoriteShoes> GetAll()
        {
            return repos.GetAll();
        }
        // POST api/<FavoritshoesController>
        [HttpPost("create-favoritshoes")]
        public string AddFavoritShoes( Guid CumstomerID, Guid ShoesDetailsId)
        {
            FavoriteShoes favorite = new FavoriteShoes();
            favorite.FavoriteID = Guid.NewGuid();
            
            favorite.CumstomerID = CumstomerID;
            favorite.ShoesDetailsId = ShoesDetailsId;
            if (repos.AddItem(favorite))
            {
                return "Thêm thành công";
            }
            else
            {
                return "Thêm thất bại";
            }
        }

        // PUT api/<FavoritshoesController>/5
        [HttpPut("update-favoritshoes")]
        public string UpdateFavoritShoes( Guid FavoriteID, Guid CumstomerID, Guid ShoesDetailsId)
        {
            var favorite = repos.GetAll().First(c => c.FavoriteID == FavoriteID);
            favorite.CumstomerID = CumstomerID;
            favorite.ShoesDetailsId = ShoesDetailsId;
            if (repos.EditItem(favorite))
            {
                return "Sửa thành công";
            }
            else
            {
                return "Sửa thất bại";
            }
        }
        // DELETE api/<FavoritshoesController>/5
        [HttpDelete("delete-favoritshoes")]
        public string DeleteFavoritShoes(Guid id)
        {
            var favorite = repos.GetAll().First(c => c.FavoriteID == id);
            if (repos.RemoveItem(favorite))
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
