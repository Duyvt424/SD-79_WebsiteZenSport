using AppData.Models;
using AppView.IServices;

namespace AppView.Services
{
    public class FavoriteShoesService : IFavoriteShoesService
    {
        ShopDBContext _dbContext;
        public FavoriteShoesService()
        {
            _dbContext = new ShopDBContext();
        }
        public bool AddFavoritShoes(FavoriteShoes favoriteShoes)
        {
            try
            {
                _dbContext.Add(favoriteShoes);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<FavoriteShoes> GetAllFavoriteShoes()
        {
            return _dbContext.FavoriteShoes.ToList();
        }

        public FavoriteShoes GetFavoritShoesById(Guid ID)
        {
            return _dbContext.FavoriteShoes.First(x => x.FavoriteShoesID == ID);
        }
        public bool RemoveFavoritShoes(FavoriteShoes favoriteShoes)
        {
            try
            {           
                _dbContext.FavoriteShoes.Remove(favoriteShoes);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdateFavoritShoes(FavoriteShoes favoriteShoes)
        {
            try
            {
                _dbContext.Update(favoriteShoes);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
