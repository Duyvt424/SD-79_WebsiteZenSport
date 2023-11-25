using AppData.Models;

namespace AppView.IServices
{
    public interface IFavoriteShoesService
    {
        public bool AddFavoritShoes(FavoriteShoes favoritshose);
        public bool RemoveFavoritShoes(FavoriteShoes favoritshose);
        public bool UpdateFavoritShoes(FavoriteShoes favoritshose);
        public List<FavoriteShoes> GetAllFavoritShoes();
        public FavoriteShoes GetFavoritShoesById(Guid id);
    }
}
