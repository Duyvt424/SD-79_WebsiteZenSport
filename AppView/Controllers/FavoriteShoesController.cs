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
        // GET: FavoriteShoesController
        private readonly IAllRepositories<FavoriteShoes> repos;
        private readonly IAllRepositories<Customer> customer;
        private readonly IAllRepositories<ShoesDetails> shoesDetail;
        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<FavoriteShoes> favorite;
        private DbSet<ShoesDetails> _shd;
        private DbSet<Customer> _cu; 
        public FavoriteShoesController()
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

        //private string GenerateFavoriteCode()
        //{
        //    var lastProduct = _dbContext.FavoriteShoes.OrderByDescending(c => c.FavoriteCode).FirstOrDefault();
        //    if (lastProduct != null)
        //    {
        //        var lastNumber = int.Parse(lastProduct.FavoriteCode.Substring(2)); // Lấy phần số cuối cùng từ ColorCode
        //        var nextNumber = lastNumber + 1; // Tăng giá trị cuối cùng
        //        var newProductCode = "FS" + nextNumber.ToString("D3");
        //        return newProductCode;
        //    }
        //    return "FS001"; // Trường hợp không có ColorCode trong cơ sở dữ liệu, trả về giá trị mặc định "CL001"
        //}
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
                ViewBag.CusList = selectListCustomer;

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
                ViewBag.CusList = selectListCustomer;

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


        //public async Task<IActionResult> FindFavoriteShoes(string searchQuery)
        //{
            
        //    //var favorite = repos.GetAll().Where(c => c.FavoriteID.ToLower().Contains(searchQuery.ToLower()));
        //    var FavoShoe = repos.GetAll().Where(c => c.FavoriteID.ToLower().Contains(searchQuery.ToLower()));
        //    var cus = customer.GetAll();
        //    var shoesDT = shoesDetail.GetAll();
        //    var favoriteShoesViewModel = FavoShoe.Select(FavoShoes => new FavoriteShoesViewModel
        //    {
        //        FavoriteID = FavoShoes.FavoriteID,
        //        ShoesDetailsName = shoesDT.FirstOrDefault(s => s.ShoesDetailsId == FavoShoes.ShoesDetailsId)?.ShoesDetailsCode,
        //        CustomerName = cus.FirstOrDefault(s => s.CumstomerID == FavoShoes.CumstomerID)?.FullName,
        //    }).ToList();

        //    return View(favoriteShoesViewModel);
        //}
    }
}
