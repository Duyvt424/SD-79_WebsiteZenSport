using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppView.IServices;
using AppView.Models;
using AppView.Models.DetailsBillViewModel;
using AppView.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Owin.BuilderProperties;
using Newtonsoft.Json;
using BillStatusHistoryViewModel = AppView.Models.DetailsBillViewModel.BillStatusHistoryViewModel;
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
            var voucher = _repos.GetAll().First(c => c.BillID == id);
            if (_repos.RemoveItem(voucher))
            {
                return RedirectToAction("GetAllBill");
            }
            else return Content("Error");
        }

        public async Task<IActionResult> FindBill(string searchQuery)
        {
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
                            Products = new List<ProductViewModel>
                            {
                                new ProductViewModel
                                {
                                    ProductID = c.ShoesDetails_Size.ShoesDetailsId,
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
                            }).ToList()
                        })
                        .ToListAsync();

					var sharedData = new { BillId = billId, DetailsBill = detailsBill };
					var sharedDataString = JsonConvert.SerializeObject(sharedData);
					HttpContext.Session.SetString("SharedDataKey", sharedDataString);
					return View(detailsBill);
				}
            }
            return View(new List<OrderDetailsViewModel>()); // Trả về danh sách rỗng nếu không có dữ liệu
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
            }
            _dbContext.Update(objBill);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
