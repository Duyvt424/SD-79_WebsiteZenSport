using AppData.IRepositories;
using AppData.IServices;
using AppData.Models;
using AppData.Repositories;
using AppData.Services;
using AppView.IServices;
using AppView.Models;
using AppView.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Security.Claims;

namespace AppView.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private ShopDBContext _dBContext;
        private readonly IShoesDetailsService _shoesDT;
        private readonly IProductService _product;
        private readonly IImageService _image;
        private readonly IBillService _bill;
        public CartController(ILogger<CartController> logger)
        {
            _logger = logger;
            _dBContext = new ShopDBContext();
            _shoesDT = new ShoesDetailsService();
            _product = new ProductService();
            _image = new ImageService();
            _bill = new BillService();
        }
        public IActionResult Cart()
        {
            var userIdString = HttpContext.Session.GetString("UserId"); // lấy id khi người dùng đăng nhập
            var customerId = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty; // ép kiểu thành guid
            if (customerId != Guid.Empty) // nếu ng dùng tồn tại
            {
                var loggedInUser = _dBContext.Customers.FirstOrDefault(c => c.CumstomerID == customerId);
                if (loggedInUser != null)
                {
                    var cartItemList = _dBContext.CartDetails
                        .Where(cd => cd.CumstomerID == loggedInUser.CumstomerID && cd.ShoesDetails_Size != null)
                        .Select(cd => new CartItemViewModel
                        {
                            ShoesDetailsID = cd.ShoesDetails_Size.ShoesDetailsId,
                            Quantity = cd.Quantity,
                            ProductName = _dBContext.Products.FirstOrDefault(p => p.ProductID == cd.ShoesDetails_Size.ShoesDetails.ProductID).Name,
                            Price = cd.ShoesDetails_Size.ShoesDetails.Price,
                            Description = cd.ShoesDetails_Size.ShoesDetails.Description,
                            Size = cd.ShoesDetails_Size.Size.Name,
                            ProductImage = _dBContext.Images.FirstOrDefault(i => i.ShoesDetailsID == cd.ShoesDetails_Size.ShoesDetails.ShoesDetailsId).Image1,
                            MaHD = ""
                        })
                        .ToList();
                    return View(cartItemList);
                }
            }
            // Nếu không có người dùng đăng nhập, lấy giỏ hàng từ session
            var cartItems = SessionServices.GetObjFromSession(HttpContext.Session, "Cart") as List<CartItemViewModel>;
            return View(cartItems);
        }
        public IActionResult GetCartItemCount()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            var customerId = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
            int itemCount = 0;
            if (customerId != Guid.Empty)
            {
                var loggedInUser = _dBContext.Customers.FirstOrDefault(c => c.CumstomerID == customerId);
                if (loggedInUser != null)
                {
                    itemCount = _dBContext.CartDetails
                        .Where(cd => cd.CumstomerID == loggedInUser.CumstomerID)
                        .Sum(cd => cd.Quantity);
                }
            }
            else
            {
                var cartItems = SessionServices.GetObjFromSession(HttpContext.Session, "Cart") as List<CartItemViewModel>;
                if (cartItems != null)
                {
                    itemCount = cartItems.Sum(item => item.Quantity);
                }
            }
            return Json(new { itemCount });
        }
        public IActionResult AddToCart(Guid id, string size)
        {
            var objSize = _dBContext.Sizes.FirstOrDefault(c => c.Name == size)?.SizeID; // tạo 1 biến chọc vào table size để tìm xem có size name nào = size từ trên form chọn
            var shoesDT_Size = _dBContext.ShoesDetails_Sizes.FirstOrDefault(c => c.ShoesDetailsId == id && c.SizeID == objSize); // tạo biến để chọc vào bảng shoesDetails_Size với đk ShoesDetailsID giống như trên form chọn ++ size id == objsize đã chọn
            if (shoesDT_Size == null) // nếu chưa tồn tại bản ghi nào thỏa mãn đk thì báo lỗi
            {
                return Content("Error");
            }
            var ShoesDT = _shoesDT.GetAllShoesDetails().FirstOrDefault(c => c.ShoesDetailsId == id); // biến này chỉ để phục vụ ng dùng chưa đăng nhập
            if (ShoesDT == null)
            {
                return Content("Sản phẩm không tồn tại");
            }
            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            var userIdString = HttpContext.Session.GetString("UserId"); // lấy id customer từ session khi đăng nhập
            var CustomerID = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty; // nếu ko null or rỗng thì ép kiểu thành Guid

            if (CustomerID != Guid.Empty) // nếu đã có Guid, tức có ng dùng đăng nhập
            {
                var loggedInUser = _dBContext.Customers.FirstOrDefault(c => c.CumstomerID == CustomerID); // kiểm tra trong db table customer xem có cus nào có id bằng id đăng nhập ko

                if (loggedInUser != null) // nếu có ng dùng đó tồn tại thật
                {
                    var existingCart = _dBContext.Carts.FirstOrDefault(c => c.CumstomerID == loggedInUser.CumstomerID); // biến để kiểm tra ng dùng đã có giỏ hàng trước đó chưa

                    if (existingCart != null) // nếu ng dùng đã có giỏ hàng trước rồi
                    {
                        // Bản ghi đã tồn tại, bạn có thể cập nhật nó thay vì thêm mới
                        existingCart.Description = "Updated cart description";
                        _dBContext.Carts.Update(existingCart);
                    }
                    else
                    {
                        // Bản ghi chưa tồn tại, thêm mới bình thường
                        var cart = new Cart
                        {
                            CumstomerID = loggedInUser.CumstomerID,
                            Description = "Cart for logged in user"
                        };
                        _dBContext.Carts.Add(cart);
                    }
                    // Kiểm tra 2 điều kiện: nếu cùng 1 customer và cùng 1 ShoesDetails_Size (tức là đôi giày này + size đã có trong giỏ hàng rôi)
                    var existingCartItem = _dBContext.CartDetails.FirstOrDefault(c => c.CumstomerID == loggedInUser.CumstomerID && c.ShoesDetails_SizeID == shoesDT_Size.ID);
                    if (existingCartItem != null) // Sản phẩm đã tồn tại trong giỏ hàng, tăng số lượng lên 1
                    {
                        existingCartItem.Quantity++;
                        _dBContext.CartDetails.Update(existingCartItem);
                    }
                    else
                    {
                        // Sản phẩm chưa tồn tại trong giỏ hàng, thêm mới vào giỏ hàng
                        var cartDetails = new CartDetails
                        {
                            CartDetailsId = Guid.NewGuid(),
                            CumstomerID = loggedInUser.CumstomerID,
                            ShoesDetails_SizeID = shoesDT_Size.ID,
                            Quantity = 1
                        };
                        _dBContext.CartDetails.Add(cartDetails);
                    }
                    _dBContext.Update(shoesDT_Size);
                    shoesDT_Size.Quantity--;
                    _dBContext.SaveChanges();
                }
            }
            else // nếu ng dùng chưa đăng nhập thì lưu bằng session
            {
                var cartItems = SessionServices.GetObjFromSession(HttpContext.Session, "Cart") as List<CartItemViewModel>;
                if (cartItems == null)
                {
                    cartItems = new List<CartItemViewModel>();
                }
                var cartItem = cartItems.FirstOrDefault(c => c.ShoesDetailsID == ShoesDT.ShoesDetailsId && c.Size == size);
                if (cartItem == null)
                {
                    // Nếu chưa có, thêm sản phẩm vào giỏ hàng với số lượng là 1
                    cartItems.Add(new CartItemViewModel
                    {
                        ShoesDetailsID = ShoesDT.ShoesDetailsId,
                        Quantity = 1,
                        ProductName = _product.GetAllProducts().FirstOrDefault(c => c.ProductID == ShoesDT.ProductID)?.Name,
                        Price = ShoesDT.Price,
                        Description = ShoesDT.Description,
                        Size = size,
                        ProductImage = _image.GetAllImages().FirstOrDefault(c => c.ShoesDetailsID == ShoesDT.ShoesDetailsId)?.Image1,
                        MaHD = ""
                    });
                }
                else
                {
                    // Nếu sản phẩm đã có trong giỏ hàng, tăng số lượng lên 1
                    cartItem.Quantity++;
                }
                SessionServices.SetObjToSession(HttpContext.Session, "Cart", cartItems);
            }
            return RedirectToAction("Cart");
        }
        public IActionResult RemoveCartItem(Guid id, string sizeName)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            var CustomerID = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
            var objSize = _dBContext.Sizes.FirstOrDefault(c => c.Name == sizeName)?.SizeID;
            var ShoesDT_Size = _dBContext.ShoesDetails_Sizes.FirstOrDefault(c => c.ShoesDetailsId == id && c.SizeID == objSize);
            if (CustomerID != Guid.Empty)
            {
                var cartItem = _dBContext.CartDetails.FirstOrDefault(c => c.ShoesDetails_SizeID == ShoesDT_Size.ID);
                ShoesDT_Size.Quantity += cartItem.Quantity;
                _dBContext.CartDetails.Remove(cartItem);
                _dBContext.SaveChanges();
            }
            else
            {
                // Lấy thông tin giỏ hàng từ session
                List<CartItemViewModel> cartItems = SessionServices.GetObjFromSession(HttpContext.Session, "Cart") as List<CartItemViewModel>;
                // Tìm kiếm sản phẩm cần xóa
                CartItemViewModel itemToRemove = cartItems.FirstOrDefault(c => c.ShoesDetailsID == id);
                // Nếu sản phẩm tồn tại trong giỏ hàng, thực hiện xóa
                if (itemToRemove != null)
                {
                    cartItems.Remove(itemToRemove);
                    // Lưu lại thông tin giỏ hàng mới vào session
                    SessionServices.SetObjToSession(HttpContext.Session, "Cart", cartItems);
                }
            }
            _dBContext.Update(ShoesDT_Size);
            _dBContext.SaveChanges();
            return RedirectToAction("Cart");
        }
        private string GenerateBillCode()
        {
            // Tạo mã đơn hàng dựa trên số lượng đơn hàng đã có trong cơ sở dữ liệu
            var count = _dBContext.Bills.Count();
            var billCode = $"HD{count + 1:000}";
            return billCode;
        }
        [HttpPost]
        public IActionResult CheckoutOk(List<CartItemViewModel> viewModel, string HinhThucThanhToan, int shippingFee)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            var CustomerID = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
            var HTThanhToan = _dBContext.PurchaseMethods.FirstOrDefault(c => c.MethodName == HinhThucThanhToan).PurchaseMethodID;
            if (CustomerID == Guid.Empty)
            {
                return RedirectToAction("Login", "Customer");
            }

            // Thêm sản phẩm vào bảng BillDetail và cập nhật số lượng sản phẩm
            //var cartItems = _dBContext.CartDetails
            //    .Where(c => c.CumstomerID == CustomerID)
            //    .Include(c => c.ShoesDetails)
            //    .ToList();
            // Tính tổng giá tiền cho cả hóa đơn
            decimal totalPrice = 0;
            //foreach (var item in cartItems)
            //{
            //    totalPrice += (item.ShoesDetails.Price * item.Quantity) + shippingFee;
            //}
            // Tạo đơn hàng
            var bill = new Bill
            {
                BillID = Guid.NewGuid(),
                BillCode = GenerateBillCode(), // Tạo mã đơn hàng (ví dụ: HD001, HD002, ...)
                CustomerID = CustomerID,
                CreateDate = DateTime.Now,
                Status = 1,
                Note = "",
                SuccessDate = DateTime.Now,
                ShippingCosts = shippingFee,
                DeliveryDate = DateTime.Now,
                CancelDate = DateTime.Now,
                TotalPrice = totalPrice,
                EmployeeID = Guid.Parse("779ae3d8-6a02-46f1-bde2-960140a0e585"),
                VoucherID = Guid.Parse("9f86a42e-cfbe-4043-8d2d-8ebcea7f8fcd"),
                PurchaseMethodID = HTThanhToan
            };
            _dBContext.Bills.Add(bill);

            //foreach (var item in cartItems)
            //{
            //    var billDetail = new BillDetails
            //    {
            //        ID = Guid.NewGuid(),
            //        BillID = bill.BillID,
            //        ShoesDetailsId = item.ShoesDetailsId,
            //        Quantity = item.Quantity,
            //        Price = item.ShoesDetails.Price
            //    };
            //    _dBContext.BillDetails.Add(billDetail);
            //}

            // Lưu thay đổi vào cơ sở dữ liệu
            _dBContext.SaveChanges();

            // Xóa giỏ hàng của người dùng
            //_dBContext.CartDetails.RemoveRange(cartItems);
            _dBContext.SaveChanges();

            return RedirectToAction("ViewBill");
        }
        [HttpPost]
        public IActionResult UpdateCartItemQuantity(Guid shoesDetailsId, int quantity, string size)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            var CustomerID = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
            var objSize = _dBContext.Sizes.FirstOrDefault(c => c.Name == size)?.SizeID;
            var ShoesDT_Size = _dBContext.ShoesDetails_Sizes.FirstOrDefault(c => c.ShoesDetailsId == shoesDetailsId && c.SizeID == objSize);
            var loggedInUser = _dBContext.Customers.FirstOrDefault(c => c.CumstomerID == CustomerID);
            if (loggedInUser != null)
            {
                var cartItem = _dBContext.CartDetails.FirstOrDefault(cd => cd.CumstomerID == loggedInUser.CumstomerID && cd.ShoesDetails_SizeID == ShoesDT_Size.ID);
                if (cartItem != null)
                {
                    //lưu số lượng trước đó ví dụ 1
                    var previousQuantity = cartItem.Quantity;
                    //Cập nhật số lượng trong giỏ hàng ví dụ 5
                    cartItem.Quantity = quantity;
                    _dBContext.SaveChanges();

                    //Cập nhật số lượng tồn của sản phẩm: ShoesDT.AvailableQuantity += 1 - 5; (số lượng trước đó - số lượng mới)
                    ShoesDT_Size.Quantity += previousQuantity - quantity;
                    _dBContext.Update(ShoesDT_Size);
                    _dBContext.SaveChanges();
                }
            }
            // Các xử lý khác (nếu cần)
            return RedirectToAction("Cart");
        }
        public int GetMaxQuantityForProduct(Guid shoesDetailsId, string size)
        {
            var objSize = _dBContext.Sizes.FirstOrDefault(c => c.Name == size)?.SizeID;
            var maxQuantity = _dBContext.ShoesDetails_Sizes.FirstOrDefault(p => p.ShoesDetailsId == shoesDetailsId && p.SizeID == objSize)?.Quantity ?? 0;
            return maxQuantity;
        }

        public IActionResult ViewBill()
        {
            List<Bill> lstBills = _bill.GetAllBills();
            return View(lstBills);
        }
    }
}
//if (!HttpContext.Session.TryGetValue("EmployeeID", out _))
//{
//    // Người dùng chưa đăng nhập, chuyển hướng đến trang đăng nhập hoặc hiển thị thông báo lỗi.
//    return RedirectToAction("Login", "Employee");
//}