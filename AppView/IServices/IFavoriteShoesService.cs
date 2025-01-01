using AppData.Models;

namespace AppView.IServices
{
    public interface IFavoriteShoesService
    {
        public bool AddFavoritShoes(FavoriteShoes favoriteShoes);
        public bool RemoveFavoritShoes(FavoriteShoes favoriteShoes);
        public bool UpdateFavoritShoes(FavoriteShoes favoriteShoes);
        public List<FavoriteShoes> GetAllFavoriteShoes();
        public FavoriteShoes GetFavoritShoesById(Guid ID);
    }
}
