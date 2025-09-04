using AppData.IServices;
using AppData.Models;
using AppView.DTO;
using AppData.Services;
using AppView.IServices;
using AppView.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Packaging;
using System.Diagnostics;
using System.Linq;
using ErrorViewModel = AppView.Models.ErrorViewModel;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using AppView.Models;

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
        private readonly ISoleService _sole;
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
        public IActionResult NotFound()
        {
            return View("~/Views/Shared/NotFound.cshtml");
        }

		public IActionResult Forbidden()
		{
			return View();
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

		public IActionResult Privacy()
		{
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
			var shoesList = _shoesDT.GetallShoedetailDtO();
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

		[HttpPost]
		[HttpGet]
		public async Task<ActionResult> ListProduct1(string[] colors, string[] genders, string[] brands, string[] styles, string[] minPrice)
		{
            // Địa chỉ URL của API
            //string apiUrl = "https://localhost:7036/api/ProductAPI/Price"; // Thay thế URL_CUA_API bằng URL thực tế của API của bạn.
            string apiUrlV1 = "https://localhost:7036/api/ShoesDetails/filter-allProduct";
            var rangeItems = new List<RangeItemcs>();
			using (HttpClient client = new HttpClient())
			{
				string query = "";
				string instanceSex = "", instanceBranch = "", instanceStyle = "", instanceColor = "";
				foreach (var item in genders)
				{
					instanceSex += "instanceSex=" + item.ToString() + "&";
				}
				foreach (var item in brands)
				{
					instanceBranch += "instacneBranch=" + item.ToString() + "&";
				}
				foreach (var item in styles)
				{
					instanceStyle += "instanceStyle=" + item.ToString() + "&";
				}
				foreach (var item in colors)
				{
					instanceColor += "instanceColor=" + item.ToString() + "&";
				}

				foreach (var item in minPrice)
				{
					if (item.Split(",").Length == 1)
					{
						var priceData = new RangeItemcs
						{
							min = int.Parse(item.Split(",")[0]),
							max = decimal.MaxValue,
						};
						rangeItems.Add(priceData);
					}
					else
					{
						var priceData = new RangeItemcs
						{
							min = decimal.Parse(item.Split(",")[0]),
							max = decimal.Parse(item.Split(",")[1])
						};
						rangeItems.Add(priceData);
					}

				}
				query = instanceStyle + instanceColor + instanceBranch + instanceSex;

				var body = JsonConvert.SerializeObject(rangeItems);

				var content = new StringContent(body, Encoding.UTF8, "application/json");
				// Gọi API bằng HTTP GET với giá tiền cần tìm kiếm
				HttpResponseMessage response = client.PostAsync($"{apiUrlV1}?{query}", content).Result;

				if (response.IsSuccessStatusCode)
				{
					// Đọc kết quả từ API
					var rescontent = response.Content.ReadAsStringAsync().Result;
					var result = JsonConvert.DeserializeObject<ProductDTO>(rescontent);
					var shoesDetails = result.Shoe_Details;

					ViewBag.NameSP = ""; // Initialize the ViewBag.NameSP with an empty string before the loop
					Dictionary<Guid, string> productNames = new Dictionary<Guid, string>();
					ViewBag.NameStyle = "";
					Dictionary<Guid, string> productStyles = new Dictionary<Guid, string>();
					foreach (var shoes in shoesDetails)
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


					ViewBag.shoesList = shoesDetails;
					return View("ListProduct");
				}
				else
				{
					// Xử lý trường hợp lỗi
					return View("Error");
				}
			}
		}

		public IActionResult DetailsProduct(Guid id)
		{
            var userIdString = HttpContext.Session.GetString("UserId");
            var customerIdSession = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
			if (customerIdSession != Guid.Empty)
			{
                var ChatUser = _shopDBContext.HistoryChats.Where(c => c.CustomerID == customerIdSession).ToList();
                var ShoesDT = _shoesDT.GetAllShoesDetails().FirstOrDefault(c => c.ShoesDetailsId == id);
                var NameProduct = _product.GetAllProducts().FirstOrDefault(c => c.ProductID == ShoesDT.ProductID);
                var StyleProduct = _style.GetAllStyles().FirstOrDefault(c => c.StyleID == ShoesDT.StyleID);
                var ImageGoldens = _image.GetAllImages().FirstOrDefault(c => c.ShoesDetailsID == id);
				//lấy thông tin để foreach sản phẩm gợi ý
				var listShoesDT = _shoesDT.GetAllShoesDetails();
                ViewBag.NameSP = "";
                Dictionary<Guid, string> productNames = new Dictionary<Guid, string>();
                foreach (var item in listShoesDT)
				{
                    var firstImage = _image.GetAllImages().FirstOrDefault(c => c.ShoesDetailsID == item.ShoesDetailsId);
                    if (firstImage != null)
                    {
                        item.ImageUrl = firstImage.Image1;
                    }

                    var product = _product.GetAllProducts().FirstOrDefault(c => c.ProductID == item.ProductID);
                    if (product != null)
                    {
                        productNames[item.ShoesDetailsId] = product.Name;
                    }
                }

                var detailsProductViewModel = new DetailsProductViewModel()
                {
                    ShoesDT = ShoesDT.ShoesDetailsId,
                    NameProduct = NameProduct.ProductID,
                    StyleProduct = StyleProduct.StyleID,
                    ImageGoldens = ImageGoldens.ImageID,
                    Price = ShoesDT.Price,
                    ShoesDetailsId = ShoesDT.ShoesDetailsId,
                    Description = ShoesDT.Description,
                    ChatHistory = ChatUser
                };
                if (NameProduct != null)
                {
                    ViewBag.nameProduct = NameProduct.Name;
                }
                if (StyleProduct != null)
                {
                    ViewBag.styleProduct = StyleProduct.Name;
                }
				//lưu trữ vào viewbag
                ViewBag.ImageGolden1 = ImageGoldens.Image1;
                ViewBag.ImageGolden2 = ImageGoldens.Image2;
                ViewBag.ImageGolden3 = ImageGoldens.Image3;
                ViewBag.ImageGolden4 = ImageGoldens.Image4;
                ViewBag.NameSP = productNames;
                ViewBag.shoesList = listShoesDT;
                var productUrl = Url.Action("DetailsProduct", "HomeController", new { id = ShoesDT.ProductID });
                ViewBag.ProductUrl = productUrl;
                return View(detailsProductViewModel);
            }
			else
			{
                var historyChatbot = HistoryChatSessionService.GetObjFromSession(HttpContext.Session, "UserChatbot") as List<HistoryChat>;
                var ShoesDT = _shoesDT.GetAllShoesDetails().FirstOrDefault(c => c.ShoesDetailsId == id);
                var NameProduct = _product.GetAllProducts().FirstOrDefault(c => c.ProductID == ShoesDT.ProductID);
                var StyleProduct = _style.GetAllStyles().FirstOrDefault(c => c.StyleID == ShoesDT.StyleID);
                var ImageGoldens = _image.GetAllImages().FirstOrDefault(c => c.ShoesDetailsID == id);
                //lấy thông tin để foreach sản phẩm gợi ý
                var listShoesDT = _shoesDT.GetAllShoesDetails();
                ViewBag.NameSP = "";
                Dictionary<Guid, string> productNames = new Dictionary<Guid, string>();
                foreach (var item in listShoesDT)
                {
                    var firstImage = _image.GetAllImages().FirstOrDefault(c => c.ShoesDetailsID == item.ShoesDetailsId);
                    if (firstImage != null)
                    {
                        item.ImageUrl = firstImage.Image1;
                    }

                    var product = _product.GetAllProducts().FirstOrDefault(c => c.ProductID == item.ProductID);
                    if (product != null)
                    {
                        productNames[item.ShoesDetailsId] = product.Name;
                    }
                }
                var detailsProductViewModel = new DetailsProductViewModel()
                {
                    ShoesDT = ShoesDT.ShoesDetailsId,
                    NameProduct = NameProduct.ProductID,
                    StyleProduct = StyleProduct.StyleID,
                    ImageGoldens = ImageGoldens.ImageID,
                    Price = ShoesDT.Price,
                    ShoesDetailsId = ShoesDT.ShoesDetailsId,
                    Description = ShoesDT.Description,
                    ChatHistory = historyChatbot
                };

                ViewBag.nameProduct = NameProduct?.Name;
                ViewBag.styleProduct = StyleProduct?.Name;
                ViewBag.ImageGolden1 = ImageGoldens?.Image1;
                ViewBag.ImageGolden2 = ImageGoldens?.Image2;
                ViewBag.ImageGolden3 = ImageGoldens?.Image3;
                ViewBag.ImageGolden4 = ImageGoldens?.Image4;
                ViewBag.NameSP = productNames;
                ViewBag.shoesList = listShoesDT;
                return View(detailsProductViewModel);
            }
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

		public IActionResult Filters()
		{
			return View("ListProduct");
		}

		[HttpPost]
		public IActionResult Filters(string[] genders, string[] minPrice, string[] brands, string[] styles, string[] colors)
		{
			if (genders != null && genders.Length > 0
				|| minPrice != null && minPrice.Length > 0
				|| brands != null && brands.Length > 0
				|| styles != null && styles.Length > 0
				|| colors != null && colors.Length > 0)
			{
				var combinedShoesList = new List<ShoesDetails>();
				var comparingShoesList = _shoesDT.GetAllShoesDetails();

				ViewBag.NameSP = ""; // Initialize the ViewBag.NameSP with an empty string before the loop
				Dictionary<Guid, string> productNames = new Dictionary<Guid, string>();
				foreach (var shoes in comparingShoesList)
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
				}
				ViewBag.NameSP = productNames;

				//Filter khoảng giá
				foreach (var price in minPrice)
				{
					if (int.TryParse(price, out int minprice))
					{
						if (minprice == 0)
						{
							var shoesList = _shoesDT.GetAllShoesDetails().Where(x => x.Price > minprice && x.Price <= 1000000).ToList();
							AddRangeIfNotExist(combinedShoesList, shoesList);
						}
						else if (minprice == 1001000)
						{
							var shoesList = _shoesDT.GetAllShoesDetails().Where(x => x.Price > minprice && x.Price <= 2700000).ToList();
							AddRangeIfNotExist(combinedShoesList, shoesList);
						}
						else if (minprice == 2701000)
						{
							var shoesList = _shoesDT.GetAllShoesDetails().Where(x => x.Price > minprice && x.Price <= 3999000).ToList();
							AddRangeIfNotExist(combinedShoesList, shoesList);
						}
						else if (minprice == 4000000)
						{
							var shoesList = _shoesDT.GetAllShoesDetails().Where(x => x.Price >= minprice).ToList();
							AddRangeIfNotExist(combinedShoesList, shoesList);
						}
					}
				}
				
				//Filter style
				var stylesList = _style.GetAllStyles().Where(s => styles.Contains(s.Name.ToLower())).ToList();
				var filteredStylesList = comparingShoesList.Where(shoes => stylesList.Any(style => style.StyleID == shoes.StyleID)).ToList();
				AddRangeIfNotExist(combinedShoesList, (List<ShoesDetails>)filteredStylesList);

				//Filter màu sắc
				var colorsList = _color.GetAllColors().Where(s => colors.Contains(s.Name.ToLower())).ToList();
				var filteredColorsList = comparingShoesList.Where(shoes => colorsList.Any(color => color.ColorID == shoes.ColorID)).ToList();
				AddRangeIfNotExist(combinedShoesList, (List<ShoesDetails>)filteredColorsList);

				//Tổng hợp lịa thành 1 list và gửi lên ViewBag
				ViewBag.shoesList = combinedShoesList;
				return View("ListProduct");
			}
			return RedirectToAction("ListProduct");
		}

		//Lọc xem sản phẩm đã có trong combinedShoesList chưa, nếu chưa thì thêm vào
		private void AddRangeIfNotExist(List<ShoesDetails> targetList, List<ShoesDetails> sourceList)
		{
			foreach (var item in sourceList)
			{
				if (!targetList.Contains(item))
				{
					targetList.Add(item);
				}
			}
		}

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

        public IActionResult Autocomplete(string term)
        {
            // Lấy danh sách suggest từ database hoặc bất kỳ nguồn dữ liệu nào bạn đang sử dụng
            var suggestions = _shopDBContext.Products
                .Where(p => p.Name.ToLower().Contains(term.ToLower()))
                .Select(p => p.Name)
                .ToList();
            return Json(suggestions);
        }

		[HttpPost]
		public async Task<IActionResult> SendPrompt(string prompt)
		{
            var userIdString = HttpContext.Session.GetString("UserId");
            var customerIdSession = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
            var httpClient = new HttpClient();
            string apiKey = "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";
            var apiUrl = "https://openrouter.ai/api/v1/chat/completions";

            var payload = new
            {
                model = "meta-llama/llama-4-maverick:free",
                messages = new[]
                {
					new { role = "user", content = prompt }
				}
            };

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(apiUrl, content);

            var responseString = await response.Content.ReadAsStringAsync();
            dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
            string botMessage = Regex.Replace(jsonResponse.choices[0].message.content.ToString(), @"[#\\*`]+", "");
			if (customerIdSession != Guid.Empty)
			{
                HistoryChat historyChat = new HistoryChat()
                {
                    HistoryChatID = Guid.NewGuid(),
                    Message = prompt,
					Response = botMessage,
                    CreatedAt = DateTime.Now,
                    CustomerID = customerIdSession
                };
				_shopDBContext.HistoryChats.Add(historyChat);
				_shopDBContext.SaveChanges();
            }
			else
			{
                var chatList = HistoryChatSessionService.GetObjFromSession(HttpContext.Session, "UserChatbot") as List<HistoryChat>;
                if (chatList == null)
                    chatList = new List<HistoryChat>();

                HistoryChat historyChatbot = new HistoryChat()
                {
                    HistoryChatID = Guid.NewGuid(),
                    Message = prompt,
                    Response = botMessage,
                    CreatedAt = DateTime.Now,
                    CustomerID = customerIdSession
                };
				chatList.Add(historyChatbot);
				HistoryChatSessionService.SetObjToSession(HttpContext.Session, "UserChatbot", chatList);
            }
            return Json(new { response = botMessage });
        }
    }
}