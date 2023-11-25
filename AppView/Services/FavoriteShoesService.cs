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
        public bool AddFavoritShoes(FavoriteShoes favoritshose)
        {
            try
            {
                _dbContext.Add(favoritshose);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<FavoriteShoes> GetAllFavoritShoes()
        {
            return _dbContext.FavoriteShoes.ToList();
        }
        public FavoriteShoes GetFavoritShoesById(Guid id)
        {
            return _dbContext.FavoriteShoes.First(x => x.FavoriteID==id);
        }
        public bool RemoveFavoritShoes(FavoriteShoes favoritshose)
        {
            try
            {
               
                _dbContext.FavoriteShoes.Remove(favoritshose);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public bool UpdateFavoritShoes(FavoriteShoes favoritshose)
        {
            try
            {
                _dbContext.Update(favoritshose);
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
