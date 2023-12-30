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
    public class FavoriteShoessController : Controller
    {
        // GET: FavoriteShoess
        private readonly IAllRepositories<FavoriteShoes> repos;
        private readonly IAllRepositories<Customer> customer;
        private readonly IAllRepositories<ShoesDetails> shoesDetail;
        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<FavoriteShoes> favorite;
        private DbSet<ShoesDetails> _shd;
        private DbSet<Customer> _cu;
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public FavoriteShoessController()
        {
            favorite = _dbContext.FavoriteShoes;
            AllRepositories<FavoriteShoes> all = new AllRepositories<FavoriteShoes>(_dbContext, favorite);
            repos = all;
            _cu = _dbContext.Customers;
            AllRepositories<Customer> cu = new AllRepositories<Customer>(_dbContext, _cu);
            customer = cu;
            _shd = _dbContext.ShoesDetails;
            AllRepositories<ShoesDetails> shd = new AllRepositories<ShoesDetails>(_dbContext, _shd);
            shoesDetail = shd;
        }
        // GET: FavoriteShoess/Details/5
        [HttpGet]
        public async Task<IActionResult> GetAllFavoritShoes()
        {
            string apiUrl = "https://localhost:7036/api/Favoritshoes/get-favoriteshoes";
            var httpClient = new HttpClient(); // tạo ra để callApi
            var response = await httpClient.GetAsync(apiUrl);// Lấy dữ liệu ra
            string apiData = await response.Content.ReadAsStringAsync();
            var FavoShoe = JsonConvert.DeserializeObject<List<FavoriteShoes>>(apiData);
            var shoesDT = shoesDetail.GetAll();
            var cus = customer.GetAll();
            // Tạo danh sách ProductViewModel với thông tin Supplier và Material
            //var favoriteShoesViewModel = FavoShoe.Select(FavoShoes => new FavoriteShoesViewModel
            //{
            //   FavoriteID = FavoShoes.FavoriteID,
            //    ShoesDetailsId =  FavoShoes.ShoesDetailsId,
            //    CumstomerID =  FavoShoes.CumstomerID,

            //}).ToList();

            return View(FavoShoe);
        }
        [HttpGet]


        // GET: FavoriteShoesController/Create
        public async Task<IActionResult> CreateFavoriteShoes()
        {
            using (ShopDBContext dBContext = new ShopDBContext())
            {
                var cus = dBContext.Customers.Where(c => c.Status == 0).ToList();
                SelectList selectListCustomer = new SelectList(cus, "CumstomerID", "FullName");
                ViewBag.CustomerList = selectListCustomer;

                var shoesDT = dBContext.ShoesDetails.Where(c => c.Status == 0).ToList();
                SelectList selectListShoesDT = new SelectList(shoesDT, "ShoesDetailsId", "ShoesDetailsCode");
                ViewBag.ShoesDTList = selectListShoesDT;


            }
            return View();
        }

        // POST: FavoriteShoesController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFavoriteShose(FavoriteShoes favoriteShoes)
        {
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Favoritshoes/create-favoritshoes={favoriteShoes.ShoesDetailsId}&CustomerID={favoriteShoes.CumstomerID}&ShoesDetailsId={favoriteShoes.ShoesDetailsId}";
            var response = await httpClient.PostAsync(apiUrl, null);
            return RedirectToAction("GetAllFavoritShoes");
        }
        [HttpGet]
        // GET: FavoriteShoesController/Edit/5

        public async Task<IActionResult> EditFavoriteShoes(Guid id)
        {
            FavoriteShoes product = repos.GetAll().FirstOrDefault(c => c.FavoriteID == id);
            using (ShopDBContext dBContext = new ShopDBContext())
            {
                var cus = dBContext.Customers.Where(c => c.Status == 0).ToList();
                SelectList selectListCustomer = new SelectList(cus, "CumstomerID", "FullName");
                ViewBag.CustomerList = selectListCustomer;

                var shoesDT = dBContext.ShoesDetails.Where(c => c.Status == 0).ToList();
                SelectList selectListShoesDT = new SelectList(shoesDT, "ShoesDetailsId", "ShoesDetailsCode");
                ViewBag.ShoesDTList = selectListShoesDT; 
            }
            return View(product);
        }
        public async Task<IActionResult> EditFavoriteShoes(FavoriteShoes product)
        {
            if (repos.EditItem(product))
            {
                return RedirectToAction("GetAllFavoritShoes");
            }
            else return BadRequest();
        }


        public async Task<IActionResult> DeleteFavoriteShoes(Guid id)
        {
            var favorite = repos.GetAll().First(c => c.FavoriteID == id);
            if (repos.RemoveItem(favorite))
            {
                return RedirectToAction("GetAllFavoritShoes");
            }
            else return Content("Error");
        }

    }
}

