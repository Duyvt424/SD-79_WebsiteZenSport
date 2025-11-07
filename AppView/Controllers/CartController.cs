using AppData.IRepositories;
using AppData.IServices;
using AppData.Models;
using AppData.Repositories;
using AppData.Services;
using AppView.IServices;
using AppView.Models;
using AppView.Services;
//using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Owin.BuilderProperties;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartController(ILogger<CartController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _dBContext = new ShopDBContext();
            _shoesDT = new ShoesDetailsService();
            _product = new ProductService();
            _image = new ImageService();
            _bill = new BillService();
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Cart()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            var customerId = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;

            string apiUrl = "https://localhost:7036/api/Voucher/get-voucher";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var styles = JsonConvert.DeserializeObject<List<VoucherViewModel>>(apiData);

			string apiUrl1 = "https://localhost:7036/api/ShippingVoucher/get-shippingVoucher";
			var httpClient1 = new HttpClient(); // tạo ra để callApi
			var response1 = await httpClient1.GetAsync(apiUrl1);// Lấy dữ liệu ra
			string apiData1 = await response1.Content.ReadAsStringAsync();
			var styles1 = JsonConvert.DeserializeObject<List<ShippingVoucherViewModel>>(apiData1);
			if (customerId != Guid.Empty)
            {
                var loggedInUser = _dBContext.Customers.Include(c => c.Rank).FirstOrDefault(c => c.CumstomerID == customerId);
				string rankName = loggedInUser?.Rank?.Name ?? "Unknown";
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
                            Description = _dBContext.Styles.FirstOrDefault(c => c.StyleID == cd.ShoesDetails_Size.ShoesDetails.StyleID).Name,
                            Size = cd.ShoesDetails_Size.Size.Name,
                            ProductImage = _dBContext.Images.FirstOrDefault(i => i.ShoesDetailsID == cd.ShoesDetails_Size.ShoesDetails.ShoesDetailsId).Image1,
                            MaHD = ""
                        })
                        .ToList();

                    var addressList = _dBContext.Addresses
                        .Where(c => c.CumstomerID == customerId)
                        .Select(address => new AddressViewModel
                        {
                            AddressID = address.AddressID,
                            FullNameCus = address.ReceiverName,
                            PhoneNumber = address.ReceiverPhone,
                            Street = address.Street,
                            Ward = address.Commune,
                            District = address.District,
                            Province = address.Province,
                            IsDefaultAddress = address.IsDefaultAddress,
                            ShippingCost = address.ShippingCost,
                            DistrictId = address.DistrictId,
                            WardCode = address.WardCode,
                            ShippingMethodID = address.ShippingMethodID
                        })
                        .ToList();

                    ShoppingCartViewModel model = new ShoppingCartViewModel
                    {
                        CartItems = cartItemList,
                        AddressList = addressList,
                        Vouchers = styles,
                        ShipVouchers = styles1,
                        RankName = rankName,
                    };

                    return View(model);
                }
            }

            // Nếu không có người dùng đăng nhập, lấy giỏ hàng từ session
            var cartItems = SessionServices.GetObjFromSession(HttpContext.Session, "Cart") as List<CartItemViewModel>;
            ShoppingCartViewModel s = new ShoppingCartViewModel
            {
                CartItems = cartItems,
                Vouchers = styles,
				ShipVouchers = styles1
			};

            return View(s);
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
                var cartItem = cartItems.FirstOrDefault(c => c.ShoesDetailsID == shoesDT_Size.ShoesDetailsId && c.Size == size);
                if (cartItem == null)
                {
                    // Nếu chưa có, thêm sản phẩm vào giỏ hàng với số lượng là 1
                    cartItems.Add(new CartItemViewModel
                    {
                        ShoesDetailsID = ShoesDT.ShoesDetailsId,
                        Quantity = 1,
                        ProductName = _product.GetAllProducts().FirstOrDefault(c => c.ProductID == ShoesDT.ProductID)?.Name,
                        Price = ShoesDT.Price,
                        Description = _dBContext.Styles.FirstOrDefault(c => c.StyleID == ShoesDT.StyleID)?.Name,
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
                shoesDT_Size.Quantity--;
                _dBContext.SaveChanges();
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
                    ShoesDT_Size.Quantity += itemToRemove.Quantity;
                    _dBContext.Update(ShoesDT_Size);
                    _dBContext.SaveChanges();
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
            DateTime currentDateTime = DateTime.Now;
            var newBillCode = currentDateTime.ToString("yyyyMMddHHmmssfff");
            return newBillCode;
        }

        [HttpPost]
        public IActionResult CheckoutOk(List<CartItemViewModel> viewModel, string HinhThucThanhToan, decimal shippingFee, Guid voucherID, string deliveryDateSave, Guid addressIDSave1, string NameKhach, string SdtKhach, string EmailKhach, string ProvinceKhach, string DistrictKhach, string WardKhach, string SoNhaKhach, int DistrictIdKhach, int WardCodeKhach, int ShippingMethodKhach)
        {
            //lấy id ng dùng trên session
            var userIdString = HttpContext.Session.GetString("UserId");
            var CustomerID = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
            if (CustomerID != Guid.Empty)
            {
                //tìm hình thức tt tương ứng
                var HTThanhToan = _dBContext.PurchaseMethods.FirstOrDefault(c => c.MethodName == HinhThucThanhToan).PurchaseMethodID;
                var objVoucher = _dBContext.Vouchers.FirstOrDefault(c => c.VoucherID == voucherID);
                var voucherId = objVoucher?.VoucherID;
                // Thêm sản phẩm vào bảng BillDetail và cập nhật số lượng sản phẩm
                var cartItems = _dBContext.CartDetails
                    .Where(c => c.CumstomerID == CustomerID)
                    .Include(c => c.ShoesDetails_Size)
                    .ToList();
                // Tính tổng giá tiền cho sản phẩm trong giỏ hàng
                decimal totalProductPrice = cartItems.Sum(item =>
                {
                    var shoesDetails = _dBContext.ShoesDetails.FirstOrDefault(c => c.ShoesDetailsId == item.ShoesDetails_Size.ShoesDetailsId);
                    return shoesDetails.Price * item.Quantity;
                });

                // Tính tổng giá tiền cả giỏ hàng và phí vận chuyển
                decimal totalPrice = totalProductPrice + shippingFee;
                decimal totalPriceAfterDiscount = voucherID != Guid.Empty ? (totalProductPrice + shippingFee) - objVoucher.VoucherValue : (totalProductPrice + shippingFee) - 0;
                // Tạo đơn hàng
                var bill = new Bill
                {
                    BillID = Guid.NewGuid(),
                    BillCode = GenerateBillCode(),
                    CustomerID = CustomerID,
                    CreateDate = DateTime.Now,
                    ConfirmationDate = DateTime.Now,
                    Status = 0,
                    Note = "",
                    IsPaid = false,
                    SuccessDate = Convert.ToDateTime(deliveryDateSave),
                    ShippingCosts = shippingFee,
                    DeliveryDate = DateTime.Now,
                    CancelDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    PaymentDay = DateTime.Now,
                    TotalPrice = totalPrice,
                    TotalPriceAfterDiscount = totalPriceAfterDiscount,
                    EmployeeID = null,
                    VoucherID = voucherId,
                    PurchaseMethodID = HTThanhToan,
                    AddressID = addressIDSave1,
                    TransactionType = 0
                };
                _dBContext.Bills.Add(bill);

                var billStatusHistory = new BillStatusHistory
                {
                    BillStatusHistoryID = Guid.NewGuid(),
                    Status = 0,
                    ChangeDate = DateTime.Now,
                    Note = "Người mua tạo đơn hàng",
                    BillID = bill.BillID,
                    EmployeeID = null,
                };
                _dBContext.BillStatusHistories.Add(billStatusHistory);

                foreach (var item in cartItems)
                {
                    var billDetail = new BillDetails
                    {
                        ID = Guid.NewGuid(),
                        BillID = bill.BillID,
                        ShoesDetails_SizeID = item.ShoesDetails_SizeID,
                        Quantity = item.Quantity,
                        Price = item.ShoesDetails_Size.ShoesDetails.Price
                    };
                    _dBContext.BillDetails.Add(billDetail);
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                _dBContext.SaveChanges();

                //Xóa giỏ hàng của người dùng
                _dBContext.CartDetails.RemoveRange(cartItems);
                _dBContext.SaveChanges();

                return RedirectToAction("DetailsBill", "Bill", new { billID = bill.BillID });
            }
            else
            {
                var rank = _dBContext.Ranks.First(c => c.Name == "Không").RankID;

                Customer customer = new Customer()
                {
                    CumstomerID = Guid.NewGuid(),
                    FullName = NameKhach,
                    UserName = NameKhach + GenerateBillCode(),
                    Password = NameKhach + GenerateBillCode(),
                    Email = EmailKhach,
                    Sex = 0,
                    ResetPassword = "",
                    PhoneNumber = SdtKhach,
                    Status = 1,
                    DateCreated = DateTime.Now,
                    RankID = rank
                };
                _dBContext.Customers.Add(customer);

                Address address = new Address()
                {
                    AddressID = Guid.NewGuid(),
                    Street = SoNhaKhach,
                    Commune = WardKhach,
                    District = DistrictKhach,
                    Province = ProvinceKhach,
                    IsDefaultAddress = true,
                    ShippingCost = shippingFee,
                    DistrictId = DistrictIdKhach,
                    WardCode = WardCodeKhach,
                    ShippingMethodID = ShippingMethodKhach,
                    Status = 1,
                    DateCreated = DateTime.Now,
                    CumstomerID = customer.CumstomerID
                };
                _dBContext.Addresses.Add(address);

                // Tìm hình thức thanh toán tương ứng
                //tìm hình thức tt tương ứng
                var HTThanhToan = _dBContext.PurchaseMethods.FirstOrDefault(c => c.MethodName == HinhThucThanhToan).PurchaseMethodID;
                var objVoucher = _dBContext.Vouchers.FirstOrDefault(c => c.VoucherID == voucherID);
                var voucherId = objVoucher?.VoucherID;

                // Lấy dữ liệu giỏ hàng từ Session
                var cartItems = SessionServices.GetObjFromSession(HttpContext.Session, "Cart") as List<CartItemViewModel>;
                // Tính tổng giá tiền cho sản phẩm trong giỏ hàng từ Session
                decimal totalProductPrice = cartItems.Sum(item =>
                {
                    // Làm thêm bất kỳ xử lý nào cần thiết với dữ liệu từ Session
                    return item.Price * item.Quantity;
                });

                // Tính tổng giá tiền cả giỏ hàng và phí vận chuyển
                decimal totalPrice = totalProductPrice + shippingFee;
                decimal totalPriceAfterDiscount = voucherID != Guid.Empty ? (totalProductPrice + shippingFee) - objVoucher.VoucherValue : (totalProductPrice + shippingFee) - 0;

                // Tạo đơn hàng
                var bill = new Bill
                {
                    BillID = Guid.NewGuid(),
                    BillCode = GenerateBillCode(),
                    CustomerID = customer.CumstomerID,
                    CreateDate = DateTime.Now,
                    ConfirmationDate = DateTime.Now,
                    Status = 0,
                    Note = "",
                    IsPaid = false,
                    SuccessDate = Convert.ToDateTime(deliveryDateSave),
                    ShippingCosts = shippingFee,
                    TotalRefundAmount = 0,
                    DeliveryDate = DateTime.Now,
                    CancelDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    PaymentDay = DateTime.Now,
                    TotalPrice = totalPrice,
                    TotalPriceAfterDiscount = totalPriceAfterDiscount,
                    EmployeeID = null,
                    VoucherID = voucherId,
                    PurchaseMethodID = HTThanhToan,
                    AddressID = address.AddressID,
                    TransactionType = 0,
                };
                _dBContext.Bills.Add(bill);

                var billStatusHistory = new BillStatusHistory
                {
                    BillStatusHistoryID = Guid.NewGuid(),
                    Status = 0,
                    ChangeDate = DateTime.Now,
                    Note = "Người mua tạo đơn hàng",
                    BillID = bill.BillID,
                    EmployeeID = null,
                };
                _dBContext.BillStatusHistories.Add(billStatusHistory);

                foreach (var item in cartItems)
                {
                    var objSize = _dBContext.Sizes.First(c => c.Name == item.Size).SizeID;
                    var ShoesDt_Size = _dBContext.ShoesDetails_Sizes.First(c => c.ShoesDetailsId == item.ShoesDetailsID && c.SizeID == objSize);
                    var billDetail = new BillDetails
                    {
                        ID = Guid.NewGuid(),
                        BillID = bill.BillID,
                        ShoesDetails_SizeID = ShoesDt_Size.ID,
                        Quantity = item.Quantity,
                        Price = item.Price // Lấy giá từ dữ liệu Session
                    };
                    _dBContext.BillDetails.Add(billDetail);
                }
                // Lưu thay đổi vào cơ sở dữ liệu
                _dBContext.SaveChanges();
                // Xóa giỏ hàng của người dùng từ Session
                HttpContext.Session.Remove("Cart");
                return RedirectToAction("DetailsBill", "Bill", new { billID = bill.BillID, customerID = bill.CustomerID });
            }
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
            else
            {
                List<CartItemViewModel> cartItems = SessionServices.GetObjFromSession(HttpContext.Session, "Cart") as List<CartItemViewModel>;
                CartItemViewModel itemToUpdate = cartItems.First(c => c.ShoesDetailsID == shoesDetailsId && c.Size == size);
                if (itemToUpdate != null)
                {
                    var previousQuantity = itemToUpdate.Quantity;
                    itemToUpdate.Quantity = quantity;
                    ShoesDT_Size.Quantity += previousQuantity - quantity;
                    _dBContext.Update(ShoesDT_Size);
                    _dBContext.SaveChanges();
                }
                SessionServices.SetObjToSession(HttpContext.Session, "Cart", cartItems);
            }
            return RedirectToAction("Cart");
        }
        public int GetMaxQuantityForProduct(Guid shoesDetailsId, string size)
        {
            var objSize = _dBContext.Sizes.FirstOrDefault(c => c.Name == size)?.SizeID;
            var maxQuantity = _dBContext.ShoesDetails_Sizes.FirstOrDefault(p => p.ShoesDetailsId == shoesDetailsId && p.SizeID == objSize)?.Quantity ?? 0;
            return maxQuantity;
        }

        [HttpPost]
        public IActionResult CheckQuantityCart(Guid shoesDetailsId, string size)
        {
            var objSize = _dBContext.Sizes.FirstOrDefault(c => c.Name == size)?.SizeID;
            var ShoesDT_Size = _dBContext.ShoesDetails_Sizes.First(c => c.ShoesDetailsId == shoesDetailsId && c.SizeID == objSize);
            if (ShoesDT_Size.Quantity <= 0)
            {
                return Json(new { success = false, message = "Số lượng sản phẩm đã hết!" });
            }
            return Json(new { success = true, message = "AA!" });
        }

        public IActionResult ViewBill()
        {
            List<Bill> lstBills = _bill.GetAllBills();
            return View(lstBills);
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(string nameUser, string phoneNumber, string email, string provinceName, string districtName, string wardName, string street, decimal ShippingCost, int DistrictID, int WardCode, int ShippingMethodID, Guid idCustomer)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            var customerIdSession = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
            var customerId = customerIdSession != Guid.Empty ? customerIdSession : idCustomer;
            var userUpdate = _dBContext.Customers.FirstOrDefault(c => c.CumstomerID == customerId);
            var emailGoogle = email != null ? email : userUpdate.Email;
            if (userUpdate != null)
            {
                userUpdate.Email = emailGoogle;
                _dBContext.Update(userUpdate);
                _dBContext.SaveChanges();
            }
            // Lấy địa chỉ mặc định hiện tại của người dùng
            var currentDefaultAddress = _dBContext.Addresses.FirstOrDefault(c => c.CumstomerID == customerId && c.IsDefaultAddress == true);
            if (currentDefaultAddress != null)
            {
                // Hủy đặt làm mặc định cho địa chỉ hiện tại
                currentDefaultAddress.IsDefaultAddress = false;
            }
            _dBContext.SaveChanges();
            HttpClient httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Address/create-address?Street={street}&Commune={wardName}&District={districtName}&Province={provinceName}&IsDefaultAddress={true}&ShippingCost={ShippingCost}&DistrictId={DistrictID}&WardCode={WardCode}&ShippingMethodID={ShippingMethodID}&Status={0}&DateCreated={DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}&CumstomerID={customerId}&ReceiverName={nameUser}&ReceiverPhone={phoneNumber}";
            var response = await httpClient.PostAsync(apiUrl, null);
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public async Task<IActionResult> AddAddressFronDetailsBill(string nameUser, string phoneNumber, string provinceName, string districtName, string wardName, string street, decimal ShippingCost, int DistrictID, int WardCode, int ShippingMethodID, Guid idCustomer, Guid idBill)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            var customerIdSession = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
            var customerId = customerIdSession != Guid.Empty ? customerIdSession : idCustomer;

            var objBill = _dBContext.Bills.First(c => c.BillID == idBill);

            var userUpdate = _dBContext.Customers.FirstOrDefault(c => c.CumstomerID == customerId);
            if (userUpdate != null)
            {
                userUpdate.FullName = nameUser;
                userUpdate.PhoneNumber = phoneNumber;
                _dBContext.Update(userUpdate);
                _dBContext.SaveChanges();
            }
            // Lấy địa chỉ mặc định hiện tại của người dùng
            var currentDefaultAddress = _dBContext.Addresses.FirstOrDefault(c => c.CumstomerID == customerId && c.IsDefaultAddress == true);
            if (currentDefaultAddress != null)
            {
                // Hủy đặt làm mặc định cho địa chỉ hiện tại
                currentDefaultAddress.IsDefaultAddress = false;
            }
            _dBContext.SaveChanges();
            HttpClient httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Address/create-address?Street={street}&Commune={wardName}&District={districtName}&Province={provinceName}&IsDefaultAddress={true}&ShippingCost={ShippingCost}&DistrictId={DistrictID}&WardCode={WardCode}&ShippingMethodID={ShippingMethodID}&Status={0}&DateCreated={DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}&CumstomerID={customerId}";
            var response = await httpClient.PostAsync(apiUrl, null);

            objBill.AddressID = _dBContext.Addresses.First(c => c.IsDefaultAddress == true).AddressID;
            _dBContext.Update(objBill);
            _dBContext.SaveChanges();
            return RedirectToAction("Cart");
        }

        #region Thanh toán vnpay
        public string UrlPayment(int TypePaymentVN, string orderCode)
        {
            var urlPayment = "";
            var order = _dBContext.Bills.FirstOrDefault(x => x.BillCode == orderCode);
            //Get Config Info
            // Get Config Info
            string vnp_Returnurl = "https://localhost:7120/";
            string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
            string vnp_TmnCode = "8CTTMT9O";
            string vnp_HashSecret = "ZVQBBDHXCGSKREWPDKMNFTNBXYOARQNF";

            //Build URL for VNPAY
            // Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            Utils utils = new Utils();
            var Price = (long)order.TotalPrice * 100;
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", Price.ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (TypePaymentVN == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (TypePaymentVN == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (TypePaymentVN == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", utils.GetIpAddress(_httpContextAccessor));
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng :" + order.BillCode);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.BillCode); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            return urlPayment;
        }
        #endregion
    }
}