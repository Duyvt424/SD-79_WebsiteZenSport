using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppView.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AppView.Controllers
{
    public class FavoriteShoesController : Controller
    {
        private readonly IAllRepositories<FavoriteShoes> repos;
        private readonly IAllRepositories<Customer> customer;
        private readonly IAllRepositories<ShoesDetails_Size> shoesDetails_Size;
        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<FavoriteShoes> favoriteShoes;
        private DbSet<ShoesDetails_Size> _shoesDetailsSize;
        private DbSet<Customer> _customers; 
        public FavoriteShoesController()
        {
            favoriteShoes = _dbContext.FavoriteShoes;
            AllRepositories<FavoriteShoes> all = new AllRepositories<FavoriteShoes>(_dbContext, favoriteShoes);
            repos = all;
            _customers = _dbContext.Customers;
            AllRepositories<Customer> cu = new AllRepositories<Customer>(_dbContext, _customers);
            customer = cu;
            _shoesDetailsSize = _dbContext.ShoesDetails_Sizes;
            AllRepositories<ShoesDetails_Size> shd = new AllRepositories<ShoesDetails_Size>(_dbContext, _shoesDetailsSize);
            shoesDetails_Size = shd;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFavoriteShoes()
        {
            string apiUrl = "https://localhost:7036/api/FavoriteShoes/get-favoriteshoes";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var FavoriteShoes = JsonConvert.DeserializeObject<List<FavoriteShoes>>(apiData);
            return View(FavoriteShoes);
        }

        [HttpGet]
        public async Task<IActionResult> CreateFavoriteShoes()
        {
            using (ShopDBContext dBContext = new ShopDBContext())
            {
                var cus = dBContext.Customers.Where(c => c.Status == 0).ToList();
                SelectList selectListCustomer = new SelectList(cus, "CumstomerID", "FullName");
                ViewBag.CusList = selectListCustomer;

                var shoesDTSize = dBContext.ShoesDetails_Sizes;
                SelectList selectListShoesDT_Size = new SelectList(shoesDTSize, "ID", "ID");
                ViewBag.ShoesDTList = selectListShoesDT_Size;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateFavoriteShose(FavoriteShoes favoriteShoes)
        {
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/FavoriteShoes/create-favoriteshoes?CumstomerID={favoriteShoes.CumstomerID}&ShoesDetails_SizeID={favoriteShoes.ShoesDetails_SizeId}&AddedDate={favoriteShoes.AddedDate.ToString("yyyy-MM-ddTHH:mm:ss")}&Status=0";
            var response = await httpClient.PostAsync(apiUrl, null);
            return RedirectToAction("GetAllFavoriteShoes");
        }

        [HttpGet]
        public async Task<IActionResult> EditFavoriteShoes(Guid id)
        {
            FavoriteShoes favoriteShoes = repos.GetAll().FirstOrDefault(c => c.FavoriteShoesID == id);
            using (ShopDBContext dBContext = new ShopDBContext())
            {
                var cus = dBContext.Customers.Where(c => c.Status == 0).ToList();
                SelectList selectListCustomer = new SelectList(cus, "CumstomerID", "FullName");
                ViewBag.CusList = selectListCustomer;

                var shoesDTSize = dBContext.ShoesDetails_Sizes;
                SelectList selectListShoesDT_Size = new SelectList(shoesDTSize, "ID", "ID");
                ViewBag.ShoesDTList = selectListShoesDT_Size;
            }
            return View(favoriteShoes);
        }
        public async Task<IActionResult> EditFavoriteShoes(FavoriteShoes favoriteShoes)
        {
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/FavoriteShoes/update-favoritshoes?FavoriteShoesID={favoriteShoes.FavoriteShoesID}&CumstomerID={favoriteShoes.CumstomerID}&ShoesDetails_SizeID={favoriteShoes.ShoesDetails_SizeId}&AddedDate={favoriteShoes.AddedDate}&Status={favoriteShoes.Status}";
            var response = await httpClient.PutAsync(apiUrl, null);
            return RedirectToAction("GetAllFavoriteShoes");
        }

        public async Task<IActionResult> DeleteFavoriteShoes(Guid id)
        {
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/FavoriteShoes/delete-favoritshoes?id={id}";
            var response = await httpClient.DeleteAsync(apiUrl);
            return RedirectToAction("GetAllFavoriteShoes");
        }
    }
}
