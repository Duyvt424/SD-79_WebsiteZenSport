using AppView.DTO;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppView.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace AppView.Controllers
{
    public class ShoesDetailController : Controller
    {
        private readonly IAllRepositories<ShoesDetails> _repos;
        private readonly IAllRepositories<Color> _colorRepos;
        private readonly IAllRepositories<Product> _productRepos;
        private readonly IAllRepositories<Sole> _soleRepos;
        private readonly IAllRepositories<Style> _styleRepos;
        private readonly IAllRepositories<Sex> _sexRepos;
        private ShopDBContext _context = new ShopDBContext();
        private DbSet<ShoesDetails> _shoesdt;
        private DbSet<Color> _color;
        private DbSet<Product> _product;
        private DbSet<Sole> _sole;
        private DbSet<Style> _style;
        private DbSet<Sex> _sex;

        public ShoesDetailController()
        {
            _shoesdt = _context.ShoesDetails;
            AllRepositories<ShoesDetails> all = new AllRepositories<ShoesDetails>(_context, _shoesdt);
            _repos = all;

            _color = _context.Colors;
            AllRepositories<Color> colorAll = new AllRepositories<Color>(_context, _color);
            _colorRepos = colorAll;

            _product = _context.Products;
            AllRepositories<Product> productAll = new AllRepositories<Product>(_context, _product);
            _productRepos = productAll;

            _sole = _context.Soles;
            AllRepositories<Sole> soleAll = new AllRepositories<Sole>(_context, _sole);
            _soleRepos = soleAll;

            _style = _context.Styles;
            AllRepositories<Style> styleAll = new AllRepositories<Style>(_context, _style);
            _styleRepos = styleAll;

            _sex = _context.Sex;
            AllRepositories<Sex> sexAll = new AllRepositories<Sex>(_context, _sex);
            _sexRepos = sexAll;
        }

        private bool CheckUserRole()
        {
            var CustomerRole = HttpContext.Session.GetString("UserId");
            var EmployeeNameSession = HttpContext.Session.GetString("RoleName");
            var EmployeeName = EmployeeNameSession != null ? EmployeeNameSession.Replace("\"", "") : null;
            if (CustomerRole != null || EmployeeName != "Quản lý")
            {
                return false;
            }
            return true;
        }
        private string GenerateShoesDetailsCode()
        {
            var lastShoesDetails = _context.ShoesDetails.OrderByDescending(c => c.ShoesDetailsCode).FirstOrDefault();
            if (lastShoesDetails != null)
            {
                var lastNumber = int.Parse(lastShoesDetails.ShoesDetailsCode.Substring(3)); // Lấy phần số cuối cùng từ ColorCode
                var nextNumber = lastNumber + 1; // Tăng giá trị cuối cùng
                var newShoesDetailsCode = "SDT" + nextNumber.ToString("D3");
                return newShoesDetailsCode;
            }
            return "SDT001";
        }
        public async Task<IActionResult> GetAllShoesDetails()
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            string apiUrl = "https://localhost:7036/api/ShoesDetails/get-shoesdetails";
            var httpClient = new HttpClient(); // tạo ra để callApi
            var response = await httpClient.GetAsync(apiUrl);// Lấy dữ liệu ra
            string apiData = await response.Content.ReadAsStringAsync();
            var shoesdt = JsonConvert.DeserializeObject<List<shoesDetailsDTO>>(apiData);
            var color = _colorRepos.GetAll();
            var product = _productRepos.GetAll();
            var sole = _soleRepos.GetAll();
            var style = _styleRepos.GetAll();
            var sex = _sexRepos.GetAll();

            // Tạo danh sách ProductViewModel với thông tin Supplier và Material
            var shoesDetailsViewModels = shoesdt.Select(shoesdt => new ShoesDetailsViewModel
            {
                ShoesDetailsId = shoesdt.ShoesDetailsId,
                ShoesDetailsCode = shoesdt.ShoesDetailsCode,
                DateCreated = shoesdt.DateCreated,
                Price = shoesdt.Price,
                ImportPrice = shoesdt.ImportPrice,
                Description = shoesdt.Description,
                Status = shoesdt.Status,
                ColorName = color.FirstOrDefault(s => s.ColorID == shoesdt.ColorID)?.Name,
                ProductName = product.FirstOrDefault(m => m.ProductID == shoesdt.ProductID)?.Name,
                SoleName = sole.FirstOrDefault(c => c.SoleID == shoesdt.SoleID)?.Name,
                StyleName = style.FirstOrDefault(m => m.StyleID == shoesdt.StyleID)?.Name,
                SexName = sex.FirstOrDefault(m => m.SexID == shoesdt.SexID)?.SexName
            }).ToList();
            return View(shoesDetailsViewModels);
        }
        public async Task<IActionResult> CreateShoesDetails()
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            using (ShopDBContext shopDBContext = new ShopDBContext())
            {
                var color = shopDBContext.Colors.Where(c => c.Status == 0).ToList();
                SelectList selectListColor = new SelectList(color, "ColorID", "Name");
                ViewBag.ColorList = selectListColor;

                var product = shopDBContext.Products.Where(c => c.Status == 0).ToList();
                SelectList selectListProduct = new SelectList(product, "ProductID", "Name");
                ViewBag.ProductList = selectListProduct;

                var sole = shopDBContext.Soles.Where(c => c.Status == 0).ToList();
                SelectList selectListSole = new SelectList(sole, "SoleID", "Name");
                ViewBag.SoleList = selectListSole;

                var style = shopDBContext.Styles.Where(c => c.Status == 0).ToList();
                SelectList selectListStyle = new SelectList(style, "StyleID", "Name");
                ViewBag.StyleList = selectListStyle;

                var sex = _sexRepos.GetAll();
                SelectList selectListSex = new SelectList(sex, "SexID", "SexName");
                ViewBag.SexList = selectListSex;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateShoesDetails(ShoesDetails shoesdt)
        {
            HttpClient httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/ShoesDetails/create-shoesdetail?shoesdetailsCode={GenerateShoesDetailsCode()}&dateCreated={shoesdt.DateCreated.ToString("yyyy-MM-ddTHH:mm:ss")}&price={shoesdt.Price}&importprice={shoesdt.ImportPrice}&description={shoesdt.Description}&status={shoesdt.Status}&colorId={shoesdt.ColorID}&productId={shoesdt.ProductID}&soleId={shoesdt.SoleID}&styleId={shoesdt.StyleID}&SexID={shoesdt.SexID}";
            var response = await httpClient.PostAsync(apiUrl, null);
            return RedirectToAction("GetAllShoesDetails");
        }

        [HttpGet]
        public IActionResult UpdateShoesDetails(Guid id) // Khi ấn vào Create thì hiển thị View
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            // Lấy Product từ database dựa theo id truyền vào từ route
            ShoesDetails shoesdt = _repos.GetAll().FirstOrDefault(c => c.ShoesDetailsId == id);

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

                var sex = _sexRepos.GetAll();
                SelectList selectListSex = new SelectList(sex, "SexID", "SexName");
                ViewBag.SexList = selectListSex;
            }
            return View(shoesdt);
        }
        public async Task<IActionResult> UpdateShoesDetails(ShoesDetails shoesdt)
        {
            HttpClient httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/ShoesDetails/edit-shoesdetail?id={shoesdt.ShoesDetailsId}&shoesdetailsCode={shoesdt.ShoesDetailsCode}&dateCreated={shoesdt.DateCreated.ToString("yyyy-MM-ddTHH:mm:ss")}&price={shoesdt.Price}&importprice={shoesdt.ImportPrice}&description={shoesdt.Description}&status={shoesdt.Status}&colorId={shoesdt.ColorID}&productId={shoesdt.ProductID}&soleId={shoesdt.SoleID}&styleId={shoesdt.StyleID}&SexID={shoesdt.SexID}";
            var response = await httpClient.PutAsync(apiUrl, null);
            return RedirectToAction("GetAllShoesDetails");
        }
        public async Task<IActionResult> DeleteShoesDetails(Guid id)
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            var shoesdt = _repos.GetAll().First(c => c.ShoesDetailsId == id);
            HttpClient httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/ShoesDetails/delete-shoesdetail?id={id}";
            var response = await httpClient.DeleteAsync(apiUrl);
            return RedirectToAction("GetAllShoesDetails");
        }
        public async Task<IActionResult> FindShoesDetails(string searchQuery)
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            var shoesDT = _repos.GetAll().Where(c => c.ShoesDetailsCode.ToLower().Contains(searchQuery.ToLower()));
            var color = _colorRepos.GetAll();
            var product = _productRepos.GetAll();
            var sole = _soleRepos.GetAll();
            var style = _styleRepos.GetAll();
            // Tạo danh sách ProductViewModel với thông tin Supplier và Material
            var shoesDetailsViewModels = shoesDT.Select(shoesDT => new ShoesDetailsViewModel
            {
                ShoesDetailsId = shoesDT.ShoesDetailsId,
                ShoesDetailsCode = shoesDT.ShoesDetailsCode,
                DateCreated = shoesDT.DateCreated,
                Price = shoesDT.Price,
                ImportPrice = shoesDT.ImportPrice,
                Description = shoesDT.Description,
                Status = shoesDT.Status,
                ColorName = color.FirstOrDefault(s => s.ColorID == shoesDT.ColorID)?.Name,
                ProductName = product.FirstOrDefault(m => m.ProductID == shoesDT.ProductID)?.Name,
                SoleName = sole.FirstOrDefault(c => c.SoleID == shoesDT.SoleID)?.Name,
                StyleName = style.FirstOrDefault(m => m.StyleID == shoesDT.StyleID)?.Name
            }).ToList();
            return View(shoesDetailsViewModels);
        }
    }
}
