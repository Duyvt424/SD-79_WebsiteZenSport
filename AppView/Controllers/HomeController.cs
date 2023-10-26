using AppData.IServices;
using AppData.Models;
using AppData.Services;
using AppView.IServices;
using AppView.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Diagnostics;
using System.Linq;
using ErrorViewModel = AppView.Models.ErrorViewModel;

namespace AppView.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShopDBContext _shopDBContext;
        private readonly IShoesDetailsService _shoesDT;
        private readonly IProductService _product;
        private readonly IImageService _image;
        private readonly ISizeService _size;
        private readonly ISupplierService _supplier;
        private readonly IStyleService _style;
        private readonly IColorService _color;

        public HomeController()
        {
            _shopDBContext = new ShopDBContext();
            _shoesDT = new ShoesDetailsService();
            _product = new ProductService();
            _image = new ImageService();
            _size = new SizeService();
            _supplier = new SupplierService();
            _style = new StyleService();
            _color = new ColorService();
        }

        public IActionResult Index()
        {
            var shoesList = _shoesDT.GetAllShoesDetails();

            ViewBag.NameSP = ""; // Initialize the ViewBag.NameSP with an empty string before the loop
            Dictionary<Guid, string> productNames = new Dictionary<Guid, string>();
            ViewBag.NameStyle = "";
            Dictionary<Guid, string> productStyles = new Dictionary<Guid, string>();
            foreach (var shoes in shoesList)
            {
                var firstImage = _image.GetAllImages().FirstOrDefault(c => c.ShoesDetailsID == shoes.ShoesDetailsId);
                if (firstImage != null)
                {
                    shoes.ImageUrl = firstImage.Image1;
                }

                var product = _product.GetAllProducts().FirstOrDefault(c => c.ProductID == shoes.ProductID);
                if (product != null)
                {
                    productNames[shoes.ShoesDetailsId] = product.Name;
                }

                var style = _style.GetAllStyles().FirstOrDefault(c => c.StyleID == shoes.StyleID);
                if (style != null)
                {
                    productStyles[shoes.ShoesDetailsId] = style.Name;
                }
            }
            ViewBag.NameStyle = productStyles;
            ViewBag.NameSP = productNames;
            ViewBag.shoesList = shoesList;
            return View();
        }

        [HttpGet]
        public IActionResult GetProductInfo(Guid shoesDetailsId, string shoesDetailsName, string shoesDetailsStyle)
        {
            var ShoesDT = _shoesDT.GetAllShoesDetails().FirstOrDefault(c => c.ShoesDetailsId == shoesDetailsId);
            var img = _image.GetAllImages().FirstOrDefault(c => c.ShoesDetailsID == shoesDetailsId);
            var nameSP = _product.GetAllProducts().FirstOrDefault(c => c.Name == shoesDetailsName);
            var styleSP = _style.GetAllStyles().FirstOrDefault(c => c.Name == shoesDetailsStyle);
            if (img != null)
            {
                ShoesDT.ImageUrl = img.Image2;
            }
            if (nameSP != null)
            {
                ShoesDT.ProductID = nameSP.ProductID;
            }
            if (styleSP != null)
            {
                ShoesDT.StyleID = styleSP.StyleID;
            }
            // Tìm thông tin sản phẩm dựa trên shoesDetailsId hoặc sử dụng dữ liệu có sẵn.
            var productInfo = new
            {
                ImageUrl = ShoesDT.ImageUrl,
                Name = nameSP.Name,
                Description = styleSP.Name,
                Price = ShoesDT.Price
            };
            // Trả về dữ liệu sản phẩm dưới dạng JSON
            return Json(productInfo);
        }

        public IActionResult ListProduct()
        {
            var shoesList = _shoesDT.GetAllShoesDetails();

            ViewBag.NameSP = ""; // Initialize the ViewBag.NameSP with an empty string before the loop
            Dictionary<Guid, string> productNames = new Dictionary<Guid, string>();
            ViewBag.NameStyle = "";
            Dictionary<Guid, string> productStyles = new Dictionary<Guid, string>();
            foreach (var shoes in shoesList)
            {
                var firstImage = _image.GetAllImages().FirstOrDefault(c => c.ShoesDetailsID == shoes.ShoesDetailsId);
                if (firstImage != null)
                {
                    shoes.ImageUrl = firstImage.Image1;
                }

                var product = _product.GetAllProducts().FirstOrDefault(c => c.ProductID == shoes.ProductID);
                if (product != null)
                {
                    productNames[shoes.ShoesDetailsId] = product.Name;
                }

                var style = _style.GetAllStyles().FirstOrDefault(c => c.StyleID == shoes.StyleID);
                if (style != null)
                {
                    productStyles[shoes.ShoesDetailsId] = style.Name;
                }
            }
            ViewBag.NameStyle = productStyles;
            ViewBag.NameSP = productNames;
            ViewBag.shoesList = shoesList;

            return View();
        }

        public IActionResult DetailsProduct(Guid id)
        {
            var ShoesDT = _shoesDT.GetAllShoesDetails().FirstOrDefault(c => c.ShoesDetailsId == id);
            var NameProduct = _product.GetAllProducts().FirstOrDefault(c => c.ProductID == ShoesDT.ProductID);
            var StyleProduct = _style.GetAllStyles().FirstOrDefault(c => c.StyleID == ShoesDT.StyleID);
            if (NameProduct != null)
            {
                ViewBag.nameProduct = NameProduct.Name;
            }
            if (StyleProduct != null)
            {
                ViewBag.styleProduct = StyleProduct.Name;
            }
            var ImageGoldens = _image.GetAllImages().FirstOrDefault(c => c.ShoesDetailsID == id);
            ViewBag.ImageGolden1 = ImageGoldens.Image1;
            ViewBag.ImageGolden2 = ImageGoldens.Image2;
            ViewBag.ImageGolden3 = ImageGoldens.Image3;
            ViewBag.ImageGolden4 = ImageGoldens.Image4;
            return View(ShoesDT);
        }

        [HttpGet]
        public IActionResult GetQuantityForSize(Guid shoesDetailsId, string sizeForm)
        {
            // Sử dụng _shoesDetailsSize hoặc dịch vụ tương tự để lấy số lượng tồn từ bảng ShoesDetails_Size.
            var size = _size.GetAllSizes().FirstOrDefault(c => c.Name == sizeForm);
            if (size != null)
            {
                var shoesDT_Size = _shopDBContext.ShoesDetails_Sizes.FirstOrDefault(c => c.ShoesDetailsId == shoesDetailsId && c.SizeID == size.SizeID);
                if (shoesDT_Size != null)
                {
                    return Json(new { quantity = shoesDT_Size.Quantity });
                }
            }
            return Json(new { quantity = 0 }); // Xử lý trường hợp không tìm thấy size hoặc sản phẩm.
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public IActionResult Filters(string[] minPrice)
        {
            //Filter khoảng giá
            foreach (var price in minPrice)
            {
                if (int.TryParse(price, out int minprice))
                {
                    if (minprice == 0)
                    {
                        var shoesList = _shoesDT.GetAllShoesDetails().Where(x => x.Price > minprice && x.Price <= 1000000).ToList();
                    }
                    else if (minprice == 1001000)
                    {
                        var shoesList = _shoesDT.GetAllShoesDetails().Where(x => x.Price > minprice && x.Price <= 2700000).ToList();
                    }
                    else if (minprice == 2701000)
                    {
                        var shoesList = _shoesDT.GetAllShoesDetails().Where(x => x.Price > minprice && x.Price <= 3999000).ToList();
                    }
                    else if (minprice == 4000000)
                    {
                        var shoesList = _shoesDT.GetAllShoesDetails().Where(x => x.Price >= minprice).ToList();
                    }
                }
            }
            return View("ListProduct");
        }
        //public async Task<IActionResult> Search(string name)
        //{
        //          var searchResults = await _shopDBContext.Products
        //                .Where(p => p.Name.Contains(name)) // Điều kiện tìm kiếm
        //                .Include(p => p.ShoesDetails) // Nếu bạn muốn bao gồm dữ liệu từ ShoesDetails
        //                .ToListAsync();
        //	return View(searchResults);
        //      }
        public IActionResult Search(string name)
        {
            var product = _shopDBContext.Products
                .Include(p => p.ShoesDetails) // Bao gồm dữ liệu từ ShoesDetails nếu cần
                .FirstOrDefault(p => p.Name.ToLower().Contains(name.ToLower()));
            ViewBag.NameSP = product?.Name;
            // Kiểm tra xem sản phẩm có tồn tại không
            if (product != null)
            {
                // Lấy danh sách ShoesDetails
                var shoesDetails = product.ShoesDetails.ToList();
                // Duyệt qua danh sách ShoesDetails để kiểm tra hình ảnh
                foreach (var item in shoesDetails)
                {
                    var image = _shopDBContext.Images.FirstOrDefault(c => c.ShoesDetailsID == item.ShoesDetailsId);
                    if (image != null)
                    {
                        item.ImageUrl = image.Image1;
                    }
                }
                return View(shoesDetails);
            }
            else
            {
                ViewBag.NameSP = name;
                return View(new List<ShoesDetails>());
            }
        }
    }
}