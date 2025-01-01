using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteShoesController : ControllerBase
    {
        // GET: api/<FavoritshoesController>
        private readonly IAllRepositories<FavoriteShoes> repos;
        ShopDBContext _dbContext = new ShopDBContext();
        DbSet<FavoriteShoes> _favorite;
        public FavoriteShoesController()
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
        [HttpPost("create-favoriteshoes")]
        public string AddFavoriteShoes(Guid CumstomerID, Guid ShoesDetails_SizeID, DateTime AddedDate, int Status)
        {
            FavoriteShoes favorite = new FavoriteShoes();
            favorite.FavoriteShoesID = Guid.NewGuid();
            favorite.CumstomerID = CumstomerID;
            favorite.ShoesDetails_SizeId = ShoesDetails_SizeID;
            favorite.AddedDate = AddedDate;
            favorite.Status = Status;
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
        public string UpdateFavoritShoes(Guid FavoriteShoesID, Guid CumstomerID, Guid ShoesDetails_SizeID, DateTime AddedDate, int Status)
        {
            var favorite = repos.GetAll().First(c => c.FavoriteShoesID == FavoriteShoesID);
            favorite.CumstomerID = CumstomerID;
            favorite.ShoesDetails_SizeId = ShoesDetails_SizeID;
            favorite.AddedDate = AddedDate;
            favorite.Status = Status;
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
            var favorite = repos.GetAll().First(c => c.FavoriteShoesID == id);
            favorite.Status = 1;
            if (repos.EditItem(favorite))
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
