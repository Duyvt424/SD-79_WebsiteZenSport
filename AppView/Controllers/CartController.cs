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
            var userIdString = HttpContext.Session.GetString("UserId");
            var customerId = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
            if (customerId != Guid.Empty)
            {
                var loggedInUser = _dBContext.Customers.FirstOrDefault(c => c.CumstomerID == customerId);
                if (loggedInUser != null)
                {
                    //var cartItemList = _dBContext.CartDetails
                    //    .Where(cd => cd.CumstomerID == loggedInUser.CumstomerID && cd.ShoesDetails != null)
                    //    .Join(_dBContext.Sizes, cd => cd.ShoesDetails.SizeID, s => s.SizeID, (cd, s) => new CartItemViewModel
                    //    {
                    //        ShoesDetailsID = cd.ShoesDetailsId,
                    //        Quantity = cd.Quantity,
                    //        ProductName = _dBContext.Products.FirstOrDefault(p => p.ProductID == cd.ShoesDetails.ProductID).Name,
                    //        Price = cd.ShoesDetails.Price,
                    //        Description = cd.ShoesDetails.Description,
                    //        Size = s.Name, // Lấy tên kích thước từ bảng Size
                    //        ProductImage = _dBContext.Images.FirstOrDefault(i => i.ShoesDetailsID == cd.ShoesDetails.ShoesDetailsId).Image1,
                    //        MaHD = ""
                    //    })
                    //    .ToList();
                    //return View(cartItemList);
                }
            }
            // Nếu không có người dùng đăng nhập, lấy giỏ hàng từ session
            var cartItems = SessionServices.GetObjFromSession(HttpContext.Session, "Cart") as List<CartItemViewModel>;
            return View(cartItems);
        }
        public IActionResult AddToCart(Guid id, string size)
        {
            var ShoesDT = _shoesDT.GetAllShoesDetails().FirstOrDefault(c => c.ShoesDetailsId == id);
            if (ShoesDT == null)
            {
                return Content("Sản phẩm không tồn tại");
            }
            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            var userIdString = HttpContext.Session.GetString("UserId");
            var CustomerID = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;

            if (CustomerID != Guid.Empty)
            {
                var loggedInUser = _dBContext.Customers.FirstOrDefault(c => c.CumstomerID == CustomerID);

                if (loggedInUser != null)
                {
                    var existingCart = _dBContext.Carts.FirstOrDefault(c => c.CumstomerID == loggedInUser.CumstomerID);

                    if (existingCart != null)
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
                    // Kiểm tra xem người dùng đã chọn kích thước hay chưa
                    var SizeID = _dBContext.Sizes.FirstOrDefault(c => c.Name == size)?.SizeID;
                    var existingCartItem = _dBContext.CartDetails.FirstOrDefault(c => c.CumstomerID == loggedInUser.CumstomerID/* && c.ShoesDetailsId == ShoesDT.ShoesDetailsId && c.ShoesDetails.SizeID == SizeID*/);
                    if (existingCartItem != null)
                    {
                        // Sản phẩm đã tồn tại trong giỏ hàng, tăng số lượng lên 1
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
                            //ShoesDetailsId = ShoesDT.ShoesDetailsId, // Gán ShoesDetailsId để tham chiếu đến thông tin sản phẩm
                            Quantity = 1
                        };
                        _dBContext.CartDetails.Add(cartDetails);
                    }
                    // Cập nhật kích thước của sản phẩm
                    //ShoesDT.SizeID = SizeID;
                    _dBContext.Update(ShoesDT);
                    //ShoesDT.AvailableQuantity--;
                    _dBContext.SaveChanges();
                }
            }
            else
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
        public IActionResult RemoveCartItem(Guid id, string quantity)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            var CustomerID = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
            var ShoesDT = _shoesDT.GetAllShoesDetails().FirstOrDefault(c => c.ShoesDetailsId == id);
            if (CustomerID != Guid.Empty)
            {
                //var cartItem = _dBContext.CartDetails.FirstOrDefault(c => c.ShoesDetailsId == id);
                //ShoesDT.AvailableQuantity += cartItem.Quantity;
                //_dBContext.CartDetails.Remove(cartItem);
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
            _dBContext.Update(ShoesDT);
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
        public IActionResult UpdateCartItemQuantity(Guid shoesDetailsId, int quantity)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            var CustomerID = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
            var ShoesDT = _shoesDT.GetAllShoesDetails().FirstOrDefault(c => c.ShoesDetailsId == shoesDetailsId);
            var loggedInUser = _dBContext.Customers.FirstOrDefault(c => c.CumstomerID == CustomerID);
            if (loggedInUser != null)
            {
                var cartItem = _dBContext.CartDetails.FirstOrDefault(cd => cd.CumstomerID == loggedInUser.CumstomerID/* && cd.ShoesDetailsId == shoesDetailsId*/);
                if (cartItem != null)
                {
                    //lưu số lượng trước đó ví dụ 1
                    var previousQuantity = cartItem.Quantity;
                    //Cập nhật số lượng trong giỏ hàng ví dụ 5
                    cartItem.Quantity = quantity;
                    _dBContext.SaveChanges();

                    //Cập nhật số lượng tồn của sản phẩm: ShoesDT.AvailableQuantity += 1 - 5; (số lượng trước đó - số lượng mới)
                    //ShoesDT.AvailableQuantity += previousQuantity - quantity;
                    _dBContext.Update(ShoesDT);
                    _dBContext.SaveChanges();
                }
            }
            // Các xử lý khác (nếu cần)
            return RedirectToAction("Cart");
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