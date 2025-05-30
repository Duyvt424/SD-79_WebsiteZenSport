﻿using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using AppData.IRepositories;
using AppData.IServices;
using AppData.Models;
using AppData.Repositories;
using AppData.Services;
using AppView.IServices;
using AppView.Models;
using AppView.Models.DetailsBillViewModel;
using AppView.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Owin.BuilderProperties;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using BillStatusHistoryViewModel = AppView.Models.DetailsBillViewModel.BillStatusHistoryViewModel;
using IShoesDetailsService = AppView.IServices.IShoesDetailsService;
using ProductViewModel = AppView.Models.DetailsBillViewModel.ProductViewModel;

namespace AppView.Controllers
{
    public class BillController : Controller
    {
        private readonly IAllRepositories<Bill> _repos;
        private readonly IAllRepositories<Customer> customer;
        private readonly IAllRepositories<Voucher> voucher;
        private readonly IAllRepositories<ShippingVoucher> shippingvoucher;
        private readonly IAllRepositories<Employee> employee;
        private readonly IAllRepositories<PurchaseMethod> purchaseMethod;
        private readonly IShoesDetailsService _shoesDT;
        private readonly IProductService _product;
        private readonly IImageService _image;
        private readonly IStyleService _style;
        // private readonly IAllRepositories<Supplier> supplierRepos;
        ShopDBContext _dbContext = new ShopDBContext();
        DbSet<Employee> _employee;
        DbSet<Customer> _cu;
        DbSet<Voucher> _voucher;
        DbSet<PurchaseMethod> _pu;
        DbSet<Bill> _bill;
        DbSet<ShippingVoucher> _shipVoucher;
        public BillController()
        {
            _bill = _dbContext.Bills;
            AllRepositories<Bill> all = new AllRepositories<Bill>(_dbContext, _bill);
            _repos = all;

            _employee = _dbContext.Employees;
            AllRepositories<Employee> em = new AllRepositories<Employee>(_dbContext, _employee);
            employee = em;

            _cu = _dbContext.Customers;
            AllRepositories<Customer> cu = new AllRepositories<Customer>(_dbContext, _cu);
            customer = cu;

            _voucher = _dbContext.Vouchers;
            AllRepositories<Voucher> vc = new AllRepositories<Voucher>(_dbContext, _voucher);
            voucher = vc;

            _pu = _dbContext.PurchaseMethods;
            AllRepositories<PurchaseMethod> pu = new AllRepositories<PurchaseMethod>(_dbContext, _pu);
            purchaseMethod = pu;


            _shipVoucher = _dbContext.ShippingVoucher;
            AllRepositories<ShippingVoucher> pu1 = new AllRepositories<ShippingVoucher>(_dbContext, _shipVoucher);
            shippingvoucher = pu1;

            _shoesDT = new ShoesDetailsService();
            _product = new ProductService();
            _image = new ImageService();
            _style = new StyleService();
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
        private string GenerateBillCode()
        {
            var lastProduct = _dbContext.Bills.OrderByDescending(c => c.BillCode).FirstOrDefault();
            if (lastProduct != null)
            {
                var lastNumber = int.Parse(lastProduct.BillCode.Substring(2)); // Lấy phần số cuối cùng từ ColorCode
                var nextNumber = lastNumber + 1; // Tăng giá trị cuối cùng
                var newProductCode = "HD" + nextNumber.ToString("D3");
                return newProductCode;
            }
            return "HD001"; // Trường hợp không có ColorCode trong cơ sở dữ liệu, trả về giá trị mặc định "CL001"
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBill()
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            string apiUrl = "https://localhost:7036/api/Bill/get-bill";
            var httpClient = new HttpClient(); // tạo ra để callApi
            var response = await httpClient.GetAsync(apiUrl);// Lấy dữ liệu ra
            string apiData = await response.Content.ReadAsStringAsync();
            var bill = JsonConvert.DeserializeObject<List<Bill>>(apiData);
            var cus = customer.GetAll();
            var em = employee.GetAll();
            var pu = purchaseMethod.GetAll();
            var vc = voucher.GetAll();
            var sv = shippingvoucher.GetAll();
            //  var materials = materialRepos.GetAll();

            // Tạo danh sách ProductViewModel với thông tin Supplier và Material
            var billViewModels = bill.Select(bills => new BillViewModel
            {
                BillID = bills.BillID,
                BillCode = bills.BillCode,
                CreateDate = bills.CreateDate,
                ConfirmationDate = bills.ConfirmationDate,
                SuccessDate = bills.SuccessDate,
                DeliveryDate = bills.DeliveryDate,
                CancelDate = bills.CancelDate,
                UpdateDate = bills.UpdateDate,
                TotalPrice = bills.TotalPrice,
                ShippingCosts = bills.ShippingCosts,
                TotalPriceAfterDiscount = bills.TotalPriceAfterDiscount,
                Note = bills.Note,
                Status = bills.Status,
                CustomerName = cus.FirstOrDefault(s => s.CumstomerID == bills.CustomerID)?.FullName,
                VoucherName = vc.FirstOrDefault(c => c.VoucherID == bills.VoucherID).VoucherCode,
                ShippingVoucherName = sv.FirstOrDefault(c => c.ShippingVoucherID == bills.ShippingVoucherID).VoucherShipCode,
                EmployeeName = em.FirstOrDefault(e => e.EmployeeID == bills.EmployeeID).FullName,
                PurchaseMethodName = pu.FirstOrDefault(p => p.PurchaseMethodID == bills.PurchaseMethodID).MethodName,

                //  MaterialName = materials.VoucherName(m => m.MaterialId == product.MaterialId)?.Name
            }).ToList();

            return View(billViewModels);
        }
        [HttpGet]
        public async Task<IActionResult> CreateBill()
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            using (ShopDBContext dBContext = new ShopDBContext())
            {
                var cus = dBContext.Customers.Where(c => c.Status == 0).ToList();
                SelectList selectListCustomer = new SelectList(cus, "CumstomerID", "FullName");
                ViewBag.CusList = selectListCustomer;

                var em = dBContext.Employees.Where(c => c.Status == 0).ToList();
                SelectList selectListEm = new SelectList(em, "EmployeeID", "FullName");
                ViewBag.EmList = selectListEm;

                var vc = dBContext.Vouchers.Where(c => c.Status == 0).ToList();
                SelectList selectListVC = new SelectList(vc, "VoucherID", "VoucherCode");
                ViewBag.VCList = selectListVC;

                var sv = dBContext.ShippingVoucher.Where(c => c.IsShippingVoucher == 0).ToList();
                SelectList selectListSVC = new SelectList(vc, "ShippingVoucherID", "VoucherShipCode");
                ViewBag.SVCList = selectListVC;

                var pu = dBContext.PurchaseMethods.Where(c => c.Status == 0).ToList();
                SelectList selectListPu = new SelectList(pu, "PurchaseMethodID", "MethodName");
                ViewBag.PuList = selectListPu;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBill(Bill bill)
        {
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Bill/create-bill?BillCode={GenerateBillCode()}&CreateDate={bill.CreateDate}&SuccessDate={bill.SuccessDate}&DeliveryDate={bill.DeliveryDate}&CancelDate={bill.CancelDate}&TotalPrice={bill.TotalPrice}&ShippingCosts={bill.ShippingCosts}&Note={bill.Note}&Status={bill.Status}&CustomerID={bill.CustomerID}&VoucherID={bill.VoucherID}&EmployeeID={bill.EmployeeID}&PurchaseMethodID={bill.PurchaseMethodID}&ShippingVoucherID={bill.ShippingVoucherID}";
            var response = await httpClient.PostAsync(apiUrl, null);
            return RedirectToAction("GetAllBill");
        }

        [HttpGet]
        public async Task<IActionResult> EditBill(Guid id)
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            Bill product = _repos.GetAll().FirstOrDefault(c => c.BillID == id);
            using (ShopDBContext dBContext = new ShopDBContext())
            {
                var cus = dBContext.Customers.Where(c => c.Status == 0).ToList();
                SelectList selectListCustomer = new SelectList(cus, "CumstomerID", "FullName");
                ViewBag.CusList = selectListCustomer;

                var em = dBContext.Employees.Where(c => c.Status == 0).ToList();
                SelectList selectListEm = new SelectList(em, "EmployeeID", "FullName");
                ViewBag.EmList = selectListEm;

                var vc = dBContext.Vouchers.Where(c => c.Status == 0).ToList();
                SelectList selectListVC = new SelectList(vc, "VoucherID", "VoucherCode");
                ViewBag.VCList = selectListVC;

                var sv = dBContext.ShippingVoucher.Where(c => c.IsShippingVoucher == 0).ToList();
                SelectList selectListSVC = new SelectList(vc, "ShippingVoucherID", "VoucherShipCode");
                ViewBag.SVCList = selectListVC;

                var pu = dBContext.PurchaseMethods.Where(c => c.Status == 0).ToList();
                SelectList selectListPu = new SelectList(pu, "PurchaseMethodID", "MethodName");
                ViewBag.PuList = selectListPu;
            }
            return View(product);
        }
        public async Task<IActionResult> EditBill(Bill product)
        {
            if (_repos.EditItem(product))
            {
                return RedirectToAction("GetAllBill");
            }
            else return BadRequest();
        }

        public IActionResult DeleteBill(Guid id)
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            var voucher = _repos.GetAll().First(c => c.BillID == id);
            if (_repos.RemoveItem(voucher))
            {
                return RedirectToAction("GetAllBill");
            }
            else return Content("Error");
        }

        public async Task<IActionResult> FindBill(string searchQuery)
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            var bill = _repos.GetAll().Where(c => c.BillCode.ToLower().Contains(searchQuery.ToLower()));
            var cus = customer.GetAll();
            var em = employee.GetAll();
            var vc = voucher.GetAll();
            var sv = shippingvoucher.GetAll();
            var pu = purchaseMethod.GetAll();
            // Tạo danh sách ProductViewModel với thông tin Supplier và Material
            var billViewModels = bill.Select(bills => new BillViewModel
            {
                BillID = bills.BillID,
                BillCode = bills.BillCode,
                CreateDate = bills.CreateDate,
                ConfirmationDate = bills.ConfirmationDate,
                SuccessDate = bills.SuccessDate,
                DeliveryDate = bills.DeliveryDate,
                CancelDate = bills.CancelDate,
                UpdateDate = bills.UpdateDate,
                TotalPrice = bills.TotalPrice,
                ShippingCosts = bills.ShippingCosts,
                TotalPriceAfterDiscount = bills.TotalPriceAfterDiscount,
                Note = bills.Note,
                Status = bills.Status,
                CustomerName = cus.FirstOrDefault(s => s.CumstomerID == bills.CustomerID)?.FullName,
                VoucherName = vc.FirstOrDefault(c => c.VoucherID == bills.VoucherID).VoucherCode,
                EmployeeName = em.FirstOrDefault(e => e.EmployeeID == bills.EmployeeID).FullName,
                PurchaseMethodName = pu.FirstOrDefault(p => p.PurchaseMethodID == bills.PurchaseMethodID).MethodName,
                ShippingVoucherName = sv.FirstOrDefault(c => c.ShippingVoucherID == bills.ShippingVoucherID).VoucherShipCode,

                //  MaterialName = materials.VoucherName(m => m.MaterialId == product.MaterialId)?.Name
            }).ToList();

            return View(billViewModels);
        }

        public async Task<IActionResult> DetailsBill(Guid billId, Guid? customerID)
        {
            try
            {
                var userIdString = HttpContext.Session.GetString("UserId");
                var customerIdSession = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
                var customerId = customerID != null ? customerID : customerIdSession;
                if (customerId != Guid.Empty)
                {
                    var loggedInUser = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CumstomerID == customerId);

                    if (loggedInUser != null)
                    {
                        var objBill = _dbContext.Bills.FirstOrDefault(c => c.BillID == billId);
                        var detailsBill = await _dbContext.BillDetails
                            .Where(c => c.Bill.CustomerID == loggedInUser.CumstomerID && c.ShoesDetails_Size != null && c.BillID == billId)
                            .Select(c => new OrderDetailsViewModel
                            {
                                BillID = objBill.BillID,
                                ShoesDetails_SizeID = c.ShoesDetails_SizeID,
                                BillCode = objBill.BillCode,
                                CustomerId = objBill.CustomerID,
                                FullName = _dbContext.Customers.First(c => c.CumstomerID == customerId).FullName,
                                PhoneNumber = _dbContext.Customers.First(c => c.CumstomerID == customerId).PhoneNumber,
                                Email = _dbContext.Customers.First(c => c.CumstomerID == customerId).Email,
                                ResetPass = _dbContext.Customers.First(c => c.CumstomerID == customerId).ResetPassword,
                                StatusCustomer = _dbContext.Customers.First(c => c.CumstomerID == customerId).Status,
                                PurchaseMethod = _dbContext.PurchaseMethods.First(x => x.PurchaseMethodID == objBill.PurchaseMethodID).MethodName,
                                Street = _dbContext.Addresses.First(c => c.AddressID == objBill.AddressID).Street,
                                Ward = _dbContext.Addresses.First(x => x.AddressID == objBill.AddressID).Commune,
                                District = _dbContext.Addresses.First(x => x.AddressID == objBill.AddressID).District,
                                Province = _dbContext.Addresses.First(x => x.AddressID == objBill.AddressID).Province,
                                TotalPrice = _dbContext.Bills.First(c => c.BillID == objBill.BillID).TotalPrice,
                                TotalPriceAfterDiscount = _dbContext.Bills.First(c => c.BillID == objBill.BillID).TotalPriceAfterDiscount,
                                PriceVoucher = objBill.VoucherID != null ? _dbContext.Vouchers.First(c => c.VoucherID == objBill.VoucherID).VoucherValue : null,
                                PriceVoucherShip = objBill.ShippingVoucherID != null ? _dbContext.ShippingVoucher.First(c => c.ShippingVoucherID == objBill.ShippingVoucherID).ShippingDiscount : null,
                                PriceProduct = _dbContext.ShoesDetails.First(x => x.ShoesDetailsId == c.ShoesDetails_Size.ShoesDetailsId).Price,
                                IsPaid = objBill.IsPaid,
                                ShippingCost = _dbContext.Bills.First(c => c.BillID == objBill.BillID).ShippingCosts,
                                EmployeeName = _dbContext.Employees.First(c => c.EmployeeID == objBill.EmployeeID).FullName,
                                TotalRefundAmount = _dbContext.Bills.FirstOrDefault(c => c.BillID == objBill.BillID).TotalRefundAmount != 0 ? _dbContext.Bills.FirstOrDefault(c => c.BillID == objBill.BillID).TotalRefundAmount : 0,
                                PriceShippingReturn = _dbContext.ReturnedProducts.FirstOrDefault(c => c.BillId == objBill.BillID && c.TransactionType == 1).ShippingFeeReturned != null ? _dbContext.ReturnedProducts.FirstOrDefault(c => c.BillId == objBill.BillID && c.TransactionType == 1).ShippingFeeReturned : null,
                                Products = new List<ProductViewModel>
                                {
                                new ProductViewModel
                                {
                                    ProductID = c.ShoesDetails_Size.ShoesDetailsId,
                                    ShoesDT_SizeId  = _dbContext.ShoesDetails_Sizes.FirstOrDefault(x => x.ShoesDetailsId == c.ShoesDetails_Size.ShoesDetailsId && x.SizeID == c.ShoesDetails_Size.SizeID).ID,
                                    ImageUrl = _dbContext.Images.First(x => x.ShoesDetailsID == c.ShoesDetails_Size.ShoesDetailsId).Image1,
                                    Name = _dbContext.Products.First(x => x.ProductID == c.ShoesDetails_Size.ShoesDetails.ProductID).Name,
                                    Description = _dbContext.Styles.First(x => x.StyleID == c.ShoesDetails_Size.ShoesDetails.StyleID).Name,
                                    Size = _dbContext.Sizes.First(x => x.SizeID == c.ShoesDetails_Size.SizeID).Name,
                                    Quantity = c.Quantity,
                                    Price = _dbContext.ShoesDetails.First(x => x.ShoesDetailsId == c.ShoesDetails_Size.ShoesDetailsId).Price * c.Quantity
                                }
                                },
                                OrderStatuses = new List<OrderStatusViewModel>
                                {
                                new OrderStatusViewModel
                                {
                                    Status = _dbContext.Bills.First(c => c.BillID == billId).Status,
                                    CreateDate = _dbContext.Bills.First(c => c.BillID == billId).CreateDate,
                                    ConfirmationDate = _dbContext.Bills.First(c => c.BillID == billId).ConfirmationDate,
                                    DeliveryDate = _dbContext.Bills.First(c => c.BillID == billId).DeliveryDate,
                                    SuccessDate = _dbContext.Bills.First(c => c.BillID == billId).SuccessDate,
                                    CancelDate = _dbContext.Bills.First(c => c.BillID == billId).CancelDate,
                                    UpdateDate = _dbContext.Bills.First(c => c.BillID == billId).UpdateDate,
                                    PaymentDate = _dbContext.Bills.First(c => c.BillID == billId).PaymentDay
                                }
                                },
                                AddressViewModels = _dbContext.Addresses.Where(c => c.CumstomerID == objBill.CustomerID).Select(a => new AddressViewModel
                                {
                                    AddressID = a.AddressID,
                                    FullNameCus = _dbContext.Customers.First(c => c.CumstomerID == objBill.CustomerID).FullName,
                                    PhoneNumber = _dbContext.Customers.First(c => c.CumstomerID == customerID).PhoneNumber,
                                    Street = a.Street,
                                    Ward = a.Commune,
                                    District = a.District,
                                    Province = a.Province,
                                    IsDefaultAddress = a.IsDefaultAddress,
                                    ShippingCost = a.ShippingCost,
                                    DistrictId = a.DistrictId,
                                    WardCode = a.WardCode,
                                    ShippingMethodID = a.ShippingMethodID
                                }).ToList(),
                                BillStatusHistories = _dbContext.BillStatusHistories.Where(c => c.BillID == billId).Select(x => new BillStatusHistoryViewModel
                                {
                                    ChangeDate = x.ChangeDate,
                                    StatusName = x.Status,
                                    EmployeeName = _dbContext.Employees.First(c => c.EmployeeID == x.EmployeeID).FullName,
                                    Note = x.Note
                                }).ToList(),
                                HistoryPayMentViewModels = _dbContext.ReturnedProducts
                                .Where(x => x.BillId == billId)
                                .Select(x => new HistoryPayMentViewModel
                                {
                                    ShoesDetailsID = _dbContext.ReturnedProducts.FirstOrDefault(x => x.ShoesDetails_SizeID == c.ShoesDetails_SizeID).ShoesDetails_SizeID,
                                    BillId = _dbContext.ReturnedProducts.FirstOrDefault(c => c.BillId == objBill.BillID).BillId,
                                    NameProduct = _dbContext.Products.FirstOrDefault(c => c.ProductID == x.ShoesDetails_Size.ShoesDetails.ProductID).Name,
                                    ImageUrl = _dbContext.Images.FirstOrDefault(c => c.ShoesDetailsID == x.ShoesDetails_Size.ShoesDetailsId).Image1,
                                    Description = _dbContext.Styles.FirstOrDefault(c => c.StyleID == x.ShoesDetails_Size.ShoesDetails.StyleID).Name,
                                    Size = _dbContext.Sizes.FirstOrDefault(c => c.SizeID == x.ShoesDetails_Size.SizeID).Name,
                                    QuantityReturned = x.QuantityReturned,
                                    Price = x.ReturnedPrice,
                                    TotalPrice = _dbContext.ReturnedProducts.FirstOrDefault(c => c.BillId == x.BillId && c.TransactionType == 1).InitialProductTotalPrice,
                                    PayMentDate = x.CreateDate,
                                    TransactionType = x.TransactionType,
                                    PurChaseMethodName = x.NamePurChaseMethod,
                                    Status = x.Status,
                                    Note = x.Note,
                                    EmployeeName = _dbContext.Employees.FirstOrDefault(e => e.EmployeeID == objBill.EmployeeID).FullName,
                                    Image1 = _dbContext.ReturnedProducts.FirstOrDefault(c => c.BillId == objBill.BillID && c.TransactionType == 1).Image1,
                                    Image2 = _dbContext.ReturnedProducts.FirstOrDefault(c => c.BillId == objBill.BillID && c.TransactionType == 1).Image2,
                                    InitialProductTotalPrice = _dbContext.ReturnedProducts.FirstOrDefault(c => c.BillId == billId && c.TransactionType == 1).InitialProductTotalPrice
                                })
                                .ToList(),
                            }).ToListAsync();

                        var sharedData = new { BillId = billId, DetailsBill = detailsBill };
                        var sharedDataString = JsonConvert.SerializeObject(sharedData);
                        HttpContext.Session.SetString("SharedDataKey", sharedDataString);

                        var shoesList = _shoesDT.GetallShoedetailDtO();
                        ViewBag.NameSP = "";
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

                        return View(detailsBill);
                    }
                }
                return View(new List<OrderDetailsViewModel>());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Có lỗi xảy ra: " + ex.Message;
                return View(new List<OrderDetailsViewModel>());
            }
        }

        public async Task<IActionResult> BillDetail(Guid? customerID)
        {
            // Lấy dữ liệu từ TempData
            var tempBillId = TempData["BillId"] as Guid?;
            var tempDetailsBill = TempData["DetailsBill"] as List<OrderDetailsViewModel>;

            var userIdString = HttpContext.Session.GetString("UserId");
            var customerIdSession = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
            var customerId = customerID ?? customerIdSession;

            var sharedDataString = HttpContext.Session.GetString("SharedDataKey");

            if (!string.IsNullOrEmpty(sharedDataString))
            {
                var sharedData = JsonConvert.DeserializeObject<dynamic>(sharedDataString);

                // Truy xuất dữ liệu
                var billId = (Guid)sharedData.BillId;
                var detailsBill = sharedData.DetailsBill.ToObject<List<OrderDetailsViewModel>>(); // Chuyển đổi thành List<OrderDetailsViewModel>

                // Sử dụng dữ liệu đã lấy để hiển thị hoặc thực hiện các công việc khác cần thiết
                ViewBag.BillId = billId;
                ViewBag.DetailsBill = detailsBill;

                ViewBag.GeneratePdfUrl = Url.Action("GeneratePdf", "PDF", new { billId });


                return View(detailsBill);
            }

            return View(new List<OrderDetailsViewModel>());
        }


        public IActionResult SaveStatusBill(Guid billID)
        {
            string DOCUMENT = "Thanh toán trực tuyến VNPay";
            var objBill = _dbContext.Bills.FirstOrDefault(c => c.BillID == billID);
            var purchaseMethod = _dbContext.PurchaseMethods.First(c => c.MethodName == DOCUMENT).PurchaseMethodID;
            if (purchaseMethod != null)
            {
                objBill.PurchaseMethodID = purchaseMethod;
                objBill.PaymentDay = DateTime.Now;
                objBill.IsPaid = true;
            }
            _dbContext.Update(objBill);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IActionResult UpdatePriceShippingInBill(Guid billId, decimal shippingCost)
        {
            var cartItems = _dbContext.BillDetails.Where(c => c.BillID == billId).Include(x => x.ShoesDetails_Size).ToList();
            // Tính tổng giá tiền cho sản phẩm trong giỏ hàng
            decimal totalProductPrice = cartItems.Sum(item =>
            {
                var shoesDetails = _dbContext.ShoesDetails.FirstOrDefault(c => c.ShoesDetailsId == item.ShoesDetails_Size.ShoesDetailsId);
                return shoesDetails.Price * item.Quantity;
            });
            var objBill = _dbContext.Bills.First(c => c.BillID == billId);
            if (objBill == null)
            {
                return NotFound();
            }
            objBill.ShippingCosts = shippingCost;
            objBill.TotalPrice = totalProductPrice + shippingCost;
            _dbContext.Update(objBill);
            _dbContext.SaveChanges();
            return Ok(new { success = true });
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
            ViewBag.Price = ShoesDT.Price;
            return Json(new
            {
                nameproduct = ViewBag.nameProduct,
                styleproduct = ViewBag.styleProduct,
                imagegolden1 = ViewBag.ImageGolden1,
                price = ViewBag.Price
            });
        }

        public IActionResult AddToBill(Guid ShoesDetailsId, string Size, Guid BillId, Guid ProductId)
        {
            var EmployeeIdString = HttpContext.Session.GetString("EmployeeID");
            var EmployeeID = !string.IsNullOrEmpty(EmployeeIdString) ? JsonConvert.DeserializeObject<Guid>(EmployeeIdString) : Guid.Empty;
            var objSize = _dbContext.Sizes.FirstOrDefault(c => c.Name == Size)?.SizeID;
            var shoesDT_Size = _dbContext.ShoesDetails_Sizes.FirstOrDefault(c => c.ShoesDetailsId == ShoesDetailsId && c.SizeID == objSize);
            var nameProduct = _dbContext.Products.First(c => c.ProductID == ProductId).Name;
            if (shoesDT_Size == null) // nếu chưa tồn tại bản ghi nào thỏa mãn đk thì báo lỗi
            {
                return Content("Error");
            }
            var ShoesDT = _shoesDT.GetAllShoesDetails().FirstOrDefault(c => c.ShoesDetailsId == ShoesDetailsId);
            if (ShoesDT == null)
            {
                return Content("Sản phẩm không tồn tại");
            }
            var existingBillItem = _dbContext.BillDetails.FirstOrDefault(c => c.BillID == BillId && c.ShoesDetails_SizeID == shoesDT_Size.ID);
            if (existingBillItem != null) // Sản phẩm đã tồn tại trong bill, tăng số lượng lên 1
            {
                existingBillItem.Quantity++;
                _dbContext.BillDetails.Update(existingBillItem);
            }
            else
            {
                // Sản phẩm chưa tồn tại trong giỏ hàng, thêm mới vào bill
                var billDetails = new BillDetails
                {
                    ID = Guid.NewGuid(),
                    BillID = BillId,
                    ShoesDetails_SizeID = shoesDT_Size.ID,
                    Quantity = 1
                };
                _dbContext.BillDetails.Add(billDetails);
            }
            var billStatusHis = new BillStatusHistory()
            {
                BillStatusHistoryID = Guid.NewGuid(),
                Status = 5,
                ChangeDate = DateTime.Now,
                Note = $"Thêm sản phẩm [{nameProduct}] - kích cỡ [{Size}] vào đơn hàng",
                BillID = BillId,
                EmployeeID = EmployeeID
            };
            _dbContext.Update(shoesDT_Size);
            shoesDT_Size.Quantity--;
            _dbContext.BillStatusHistories.Add(billStatusHis);
            _dbContext.SaveChanges();
            return Ok(new { success = true });
        }

        public IActionResult RemoveItemInBill(Guid ShoesDetailsId, string sizeName, Guid BillId, string ghiChu, string nameProduct)
        {
            var EmployeeIdString = HttpContext.Session.GetString("EmployeeID");
            var EmployeeID = !string.IsNullOrEmpty(EmployeeIdString) ? JsonConvert.DeserializeObject<Guid>(EmployeeIdString) : Guid.Empty;
            var objSize = _dbContext.Sizes.FirstOrDefault(c => c.Name == sizeName)?.SizeID;
            var ShoesDT_Size = _dbContext.ShoesDetails_Sizes.FirstOrDefault(c => c.ShoesDetailsId == ShoesDetailsId && c.SizeID == objSize);
            var billItem = _dbContext.BillDetails.FirstOrDefault(c => c.ShoesDetails_SizeID == ShoesDT_Size.ID && c.BillID == BillId);
            ShoesDT_Size.Quantity += billItem.Quantity;
            var billStatusHis = new BillStatusHistory()
            {
                BillStatusHistoryID = Guid.NewGuid(),
                Status = 5,
                ChangeDate = DateTime.Now,
                Note = $"{ghiChu} [{nameProduct}] - kích cỡ [{sizeName}] khỏi đơn hàng",
                BillID = BillId,
                EmployeeID = EmployeeID
            };
            _dbContext.BillStatusHistories.Add(billStatusHis);
            _dbContext.BillDetails.Remove(billItem);
            _dbContext.SaveChanges();
            _dbContext.Update(ShoesDT_Size);
            _dbContext.SaveChanges();
            return Ok(new { success = true });
        }

        [HttpPost]
        public IActionResult CheckQuantityInBill(Guid shoesDetailsId, string size, Guid billId)
        {
            var objSize = _dbContext.Sizes.FirstOrDefault(c => c.Name == size)?.SizeID;
            var ShoesDT_Size = _dbContext.ShoesDetails_Sizes.First(c => c.ShoesDetailsId == shoesDetailsId && c.SizeID == objSize);
            var billItem = _dbContext.BillDetails.FirstOrDefault(cd => cd.BillID == billId && cd.ShoesDetails_SizeID == ShoesDT_Size.ID);
            if (billItem != null)
            {
                if (ShoesDT_Size.Quantity <= 0)
                {
                    return Json(new { success = false, message = "Số lượng sản phẩm đã hết!" });
                }
            }
            return Json(new { success = true, message = "AA!" });
        }

        [HttpPost]
        public IActionResult UpdateBillItemQuantity(Guid shoesDetailsId, int quantity, string size, Guid billId)
        {
            var objSize = _dbContext.Sizes.FirstOrDefault(c => c.Name == size)?.SizeID;
            var ShoesDT_Size = _dbContext.ShoesDetails_Sizes.FirstOrDefault(c => c.ShoesDetailsId == shoesDetailsId && c.SizeID == objSize);
            var cartItem = _dbContext.BillDetails.FirstOrDefault(cd => cd.BillID == billId && cd.ShoesDetails_SizeID == ShoesDT_Size.ID);
            if (cartItem != null)
            {
                //lưu số lượng trước đó ví dụ 1
                var previousQuantity = cartItem.Quantity;
                //Cập nhật số lượng trong giỏ hàng ví dụ 5
                cartItem.Quantity = quantity;
                _dbContext.SaveChanges();

                //Cập nhật số lượng tồn của sản phẩm: ShoesDT.AvailableQuantity += 1 - 5; (số lượng trước đó - số lượng mới)
                ShoesDT_Size.Quantity += previousQuantity - quantity;
                _dbContext.Update(ShoesDT_Size);
                _dbContext.SaveChanges();
            }
            return Ok(new { success = true });
        }

        public IActionResult ReturnedProduct(Guid idBill, Guid idShoesDT, string idSize, string ghiChuUpdateSP, int quanity, string nameProduct, decimal priceVoucher, decimal priceShippingReturn, decimal initialProductTotalPrice, [Bind(Prefix = "image1")] IFormFile image1, [Bind(Prefix = "image2")] IFormFile image2)
        {
            var EmployeeIdString = HttpContext.Session.GetString("EmployeeID");
            var EmployeeID = !string.IsNullOrEmpty(EmployeeIdString) ? JsonConvert.DeserializeObject<Guid>(EmployeeIdString) : Guid.Empty;
            var objSize = _dbContext.Sizes.First(c => c.Name == idSize)?.SizeID;
            var ShoesDt_Size = _dbContext.ShoesDetails_Sizes.First(c => c.ShoesDetailsId == idShoesDT && c.SizeID == objSize);
            var billDetails = _dbContext.BillDetails.First(c => c.BillID == idBill && c.ShoesDetails_SizeID == ShoesDt_Size.ID);
            var objBill = _dbContext.Bills.First(c => c.BillID == idBill);
            if (billDetails != null)
            {
                //billDetails.Quantity -= quanity;
                //ShoesDt_Size.Quantity += quanity;
                //_dbContext.SaveChanges();

                if (image1 != null && image1.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", image1.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        image1.CopyTo(stream);
                    }
                }

                if (image2 != null && image2.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", image2.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        image2.CopyTo(stream);
                    }
                }

                var historyBill = new ReturnedProducts()
                {
                    ID = Guid.NewGuid(),
                    CreateDate = DateTime.Now,
                    Note = ghiChuUpdateSP,
                    QuantityReturned = quanity,
                    ReturnedPrice = quanity * billDetails.Price,
                    TransactionType = 1,
                    NamePurChaseMethod = "Tiền mặt",
                    ShippingFeeReturned = priceShippingReturn,
                    InitialProductTotalPrice = initialProductTotalPrice,
                    Image1 = image1.FileName,
                    Image2 = image2.FileName,
                    Status = 1,
                    BillId = objBill.BillID,
                    ShoesDetails_SizeID = ShoesDt_Size.ID
                };

                var billstatusHis = new BillStatusHistory()
                {
                    BillStatusHistoryID = Guid.NewGuid(),
                    Status = 6,
                    ChangeDate = DateTime.Now,
                    Note = $"Đã hoàn trả [{quanity}] sản phẩm [{nameProduct}] kích cỡ [{idSize}]. Lý do: [{ghiChuUpdateSP}]",
                    BillID = idBill,
                    EmployeeID = EmployeeID != Guid.Empty ? EmployeeID : null
                };
                objBill.TotalRefundAmount += historyBill.ReturnedPrice;
                objBill.Status = 6;
                objBill.TotalPrice -= billDetails.Price * quanity;
                objBill.TotalPriceAfterDiscount = priceVoucher != null ? objBill.TotalPrice - priceVoucher : objBill.TotalPrice - 0;
                billDetails.Quantity -= quanity;
                _dbContext.Bills.Update(objBill);
                _dbContext.BillDetails.Update(billDetails);
                //_dbContext.ShoesDetails_Sizes.Update(ShoesDt_Size);
                _dbContext.BillStatusHistories.Add(billstatusHis);
                _dbContext.ReturnedProducts.Add(historyBill);
                _dbContext.SaveChanges();
            }
            return Ok(new { success = true });
        }

        [HttpPost]
        public IActionResult CheckQuantityReturned(Guid shoesDetailsId, string size, Guid billId, int quantity)
        {
            var objSize = _dbContext.Sizes.FirstOrDefault(c => c.Name == size)?.SizeID;
            var ShoesDT_Size = _dbContext.ShoesDetails_Sizes.First(c => c.ShoesDetailsId == shoesDetailsId && c.SizeID == objSize);
            var billItem = _dbContext.BillDetails.FirstOrDefault(cd => cd.BillID == billId && cd.ShoesDetails_SizeID == ShoesDT_Size.ID);
            if (billItem != null)
            {
                if (quantity >= billItem.Quantity)
                {
                    return Json(new { success = false, message = "Số lượng sản phẩm trong đơn hàng đã hết" });
                }
            }
            return Json(new { success = true, message = "Success!" });
        }

        [HttpPost]
        public IActionResult SuccessBillReturn(Guid idBill, string ghiChuReturn, decimal price)
        {
            var objbill = _dbContext.Bills.First(c => c.BillID == idBill);
            var returnObj = _dbContext.ReturnedProducts.First(c => c.BillId == objbill.BillID && c.TransactionType == 1 && c.Status == 1);
            var ShoesDt_Size = _dbContext.ShoesDetails_Sizes.FirstOrDefault(c => c.ID == returnObj.ShoesDetails_SizeID);
            if (returnObj.Status == 1)
            {
                returnObj.CreateDate = DateTime.Now;
                returnObj.Note = ghiChuReturn;
                returnObj.Status = 0;
                returnObj.ReturnedPrice = price;
                ShoesDt_Size.Quantity += returnObj.QuantityReturned;
                _dbContext.Bills.Update(objbill);
                _dbContext.ReturnedProducts.Update(returnObj);
                _dbContext.SaveChanges();
            }
            return Ok(new { success = true });
        }

        [HttpPost]
        public IActionResult CancelReturnedProduct(Guid idBill, string ghiChuDontHT, decimal priceVoucher)
        {
            var objBill = _dbContext.Bills.First(c => c.BillID == idBill);
            var returnedObj = _dbContext.ReturnedProducts.FirstOrDefault(c => c.BillId == idBill && c.TransactionType == 1 && c.Status == 1);
            var billDetails = _dbContext.BillDetails.FirstOrDefault(c => c.BillID == idBill && c.ShoesDetails_SizeID == returnedObj.ShoesDetails_SizeID);
            if (returnedObj?.Status == 1)
            {
                returnedObj.Note = ghiChuDontHT;
                returnedObj.Status = 2;
                objBill.TotalRefundAmount -= returnedObj.ReturnedPrice;
                objBill.TotalPrice += billDetails.Price * returnedObj.QuantityReturned;
                objBill.TotalPriceAfterDiscount = priceVoucher != null ? objBill.TotalPrice - priceVoucher : objBill.TotalPrice - 0;
                objBill.Status = 3;
                billDetails.Quantity += returnedObj.QuantityReturned;
                _dbContext.Bills.Update(objBill);
                _dbContext.BillDetails.Update(billDetails);
                _dbContext.ReturnedProducts.Update(returnedObj);
                _dbContext.SaveChanges();
            }
            return Ok(new { success = true });
        }
    }
}