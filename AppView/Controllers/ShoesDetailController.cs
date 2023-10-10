using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace AppView.Controllers
{
    public class ShoesDetailController : Controller
    {
        private readonly IAllRepositories<ShoesDetails> repos;
        private ShopDBContext context = new ShopDBContext();
        private DbSet<ShoesDetails> shoesdt;

        public ShoesDetailController()
        {
            shoesdt = context.ShoesDetails;
            AllRepositories<ShoesDetails> all = new AllRepositories<ShoesDetails>(context, shoesdt);
            repos = all;
        }
        public async Task<IActionResult> GetAllShoesDetails()
        {
            string apiUrl = "https://localhost:7036/api/ShoesDetails";
            var httpClient = new HttpClient(); // tạo ra để callApi
            var response = await httpClient.GetAsync(apiUrl);// Lấy dữ liệu ra
                                                             // Lấy dữ liệu Json trả về từ Api được call dạng string
            string apiData = await response.Content.ReadAsStringAsync();
            // Lấy kqua trả về từ API
            // Đọc từ string Json vừa thu được sang List<T>
            var shoesdt = JsonConvert.DeserializeObject<List<ShoesDetails>>(apiData);
            return View(shoesdt);
        }
        public async Task<IActionResult> CreateShoesDetails()
        {
            using(ShopDBContext shopDBContext = new ShopDBContext())
            {
                var color = shopDBContext.Colors.ToList();
                SelectList selectListColor = new SelectList(color, "ColorID", "Name");
                ViewBag.ColorList = selectListColor;

                var product = shopDBContext.Products.ToList();
                SelectList selectListProduct = new SelectList(product, "ProductID", "Name");
                ViewBag.ProductList = selectListProduct;

                var sole = shopDBContext.Soles.ToList();
                SelectList selectListSole = new SelectList(sole, "SoleID", "Name");
                ViewBag.SoleList = selectListSole;

                var style = shopDBContext.Styles.ToList();
                SelectList selectListStyle = new SelectList(style, "StyleID", "Name");
                ViewBag.StyleList = selectListStyle;

                var supplier = shopDBContext.Suppliers.ToList();
                SelectList selectListSupplier = new SelectList(supplier, "SupplierID", "Name");
                ViewBag.SupplierList = selectListSupplier;
            }
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateShoesDetails(ShoesDetails shoesdt)
        {
            HttpClient httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/ShoesDetails/create-shoesdetail?createdate={shoesdt.DateCreated}&price={shoesdt.Price}&importprice={shoesdt.ImportPrice}&description={shoesdt.Description}&status={shoesdt.Status}&colorid={shoesdt.ColorID}&productid={shoesdt.ProductID}&soleid={shoesdt.SoleID}&styleid={shoesdt.StyleID}";
           
            var response = await httpClient.PostAsync(apiUrl, null);
            
            return RedirectToAction("GetAllShoesDetails");
        }
        [HttpGet]
        public IActionResult UpdateShoesDetails(Guid id) // Khi ấn vào Create thì hiển thị View
        {
            // Lấy Product từ database dựa theo id truyền vào từ route
            ShoesDetails shoesdt = repos.GetAll().FirstOrDefault(c => c.ShoesDetailsId == id);

            using (ShopDBContext shopDBContext = new ShopDBContext())
            {
                var color = shopDBContext.Colors.ToList();
                SelectList selectListColor = new SelectList(color, "ColorID", "Name");
                ViewBag.ColorList = selectListColor;

                var product = shopDBContext.Products.ToList();
                SelectList selectListProduct = new SelectList(product, "ProductID", "Name");
                ViewBag.ProductList = selectListProduct;

                var sole = shopDBContext.Soles.ToList();
                SelectList selectListSole = new SelectList(sole, "SoleID", "Name");
                ViewBag.SoleList = selectListSole;

                var style = shopDBContext.Styles.ToList();
                SelectList selectListStyle = new SelectList(style, "StyleID", "Name");
                ViewBag.StyleList = selectListStyle;

                var supplier = shopDBContext.Suppliers.ToList();
                SelectList selectListSupplier = new SelectList(supplier, "SupplierID", "Name");
                ViewBag.SupplierList = selectListSupplier;
            }
            return View(shoesdt);
        }
        public async Task<IActionResult> UpdateShoesDetails(ShoesDetails shoesdt) // Thực hiện việc Tạo mới
        {
            HttpClient httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/ShoesDetails/edit-shoesdetail?id={shoesdt.ShoesDetailsId}&createdate={shoesdt.DateCreated}&price={shoesdt.Price}&importprice={shoesdt.ImportPrice}&description={shoesdt.Description}&status={shoesdt.Status}&colorid={shoesdt.ColorID}&productid={shoesdt.ProductID}&soleid={shoesdt.SoleID}&styleid={shoesdt.StyleID}";

           var response = await httpClient.PutAsync(apiUrl, null);

            return RedirectToAction("GetAllShoesDetails");
        }
        public async Task<IActionResult> DeleteShoesDetails(Guid id)
        {
            var shoesdt = repos.GetAll().First(c => c.ShoesDetailsId == id);
            HttpClient httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/ShoesDetails/delete-shoesdetail?id={id}";
            var response = await httpClient.DeleteAsync(apiUrl);
            return RedirectToAction("GetAllShoesDetails");
        }
        public IActionResult Details(Guid id)
        {
            ShopDBContext dBContext = new ShopDBContext();
            var shoesdt = repos.GetAll().FirstOrDefault(c => c.ShoesDetailsId == id);
            return View(shoesdt);
        }
    }
}
