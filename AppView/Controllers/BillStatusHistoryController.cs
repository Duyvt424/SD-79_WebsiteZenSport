using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppView.Models;

namespace AppView.Controllers
{
    public class BillStatusHistoryController : Controller
    {
        private readonly IAllRepositories<BillStatusHistory> _repos;
        private readonly IAllRepositories<Bill> _billRepos;
        private readonly IAllRepositories<Employee> _employeeRepos;
        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<BillStatusHistory> _billStatusHistories;
        private DbSet<Bill> _bill;
        private DbSet<Employee> _employee;
        public BillStatusHistoryController()
        {
            _billStatusHistories = _dbContext.BillStatusHistories;
            AllRepositories<BillStatusHistory> all = new AllRepositories<BillStatusHistory>(_dbContext, _billStatusHistories);
            _repos = all;

            _bill = _dbContext.Bills;
            AllRepositories<Bill> BillAll = new AllRepositories<Bill>(_dbContext, _bill);
            _billRepos = BillAll;

            _employee = _dbContext.Employees;
            AllRepositories<Employee> EmployeeAll = new AllRepositories<Employee>(_dbContext, _employee);
            _employeeRepos = EmployeeAll;
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
        public async Task<IActionResult> GetAllBillStatusHistory()
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            string apiUrl = "https://localhost:7036/api/BillStatusHistory/get-billStatusHistory";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var billStatusHistory = JsonConvert.DeserializeObject<List<BillStatusHistory>>(apiData);
            var bills = _billRepos.GetAll();
            var employees = _employeeRepos.GetAll();

            // Tạo danh sách ProductViewModel với thông tin Supplier và Material
            var billStatusHistoryViewModels = billStatusHistory.Select(billStatusHistory => new BillStatusHistoryViewModel
            {
                BillStatusHistoryID = billStatusHistory.BillStatusHistoryID,
                Status = billStatusHistory.Status,
                ChangeDate = billStatusHistory.ChangeDate,
                Note = billStatusHistory.Note,
                BillCode = bills.FirstOrDefault(s => s.BillID == billStatusHistory.BillID)?.BillCode,
                EmployeeName = employees.FirstOrDefault(m => m.EmployeeID == billStatusHistory.EmployeeID)?.UserName
            }).ToList();

            return View(billStatusHistoryViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> CreateBillStatusHistory()
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            using (ShopDBContext shopDBContext = new ShopDBContext())
            {
                var bill = shopDBContext.Bills.ToList();
                SelectList selectListBill = new SelectList(bill, "BillID", "BillCode");
                ViewBag.BillList = selectListBill;

                var employee = shopDBContext.Employees.Where(c => c.Status == 0).ToList();
                SelectList selectListEmployee = new SelectList(employee, "EmployeeID", "UserName");
                ViewBag.EmployeeList = selectListEmployee;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBillStatusHistory(BillStatusHistory billStatusHistory)
        {
            HttpClient httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/BillStatusHistory/add-billStatusHistory?Status={billStatusHistory.Status}&ChangeDate={billStatusHistory.ChangeDate.ToString("yyyy-MM-ddTHH:mm:ss")}&Note={billStatusHistory.Note}&BillID={billStatusHistory.BillID}&EmployeeID={billStatusHistory.EmployeeID}";
            var response = await httpClient.PostAsync(apiUrl, null);
            return RedirectToAction("GetAllBillStatusHistory");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateBillStatusHistory(Guid id)
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            BillStatusHistory billStatusHistory = _repos.GetAll().FirstOrDefault(c => c.BillStatusHistoryID == id);
            using (ShopDBContext shopDBContext = new ShopDBContext())
            {
                var bill = shopDBContext.Bills.ToList();
                SelectList selectListBill = new SelectList(bill, "BillID", "BillCode");
                ViewBag.BillList = selectListBill;

                var employee = shopDBContext.Employees.Where(c => c.Status == 0).ToList();
                SelectList selectListEmployee = new SelectList(employee, "EmployeeID", "UserName");
                ViewBag.EmployeeList = selectListEmployee;
            }
            return View(billStatusHistory);
        }

        public async Task<IActionResult> UpdateBillStatusHistory(BillStatusHistory billStatusHistory)
        {
            HttpClient httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/BillStatusHistory/update-billStatusHistory?BillStatusHistoryID={billStatusHistory.BillStatusHistoryID}&Status={billStatusHistory.Status}&ChangeDate={billStatusHistory.ChangeDate.ToString("yyyy-MM-ddTHH:mm:ss")}&Note={billStatusHistory.Note}&BillID={billStatusHistory.BillID}&EmployeeID={billStatusHistory.EmployeeID}";
            var response = await httpClient.PutAsync(apiUrl, null);
            return RedirectToAction("GetAllBillStatusHistory");
        }

        public async Task<IActionResult> DeleteBillStatusHistory(Guid id)
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            var billStatusHistory = _repos.GetAll().FirstOrDefault(c => c.BillStatusHistoryID == id);
            HttpClient httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/BillStatusHistory/delete-billStatusHistory?BillStatusHistoryID={id}";
            var response = await httpClient.DeleteAsync(apiUrl);
            return RedirectToAction("GetAllBillStatusHistory");
        }

        public IActionResult SaveStatusBill(string ghiChu, Guid idBill)
        {
            var EmployeeIdString = HttpContext.Session.GetString("EmployeeID");
            var EmployeeID = !string.IsNullOrEmpty(EmployeeIdString) ? JsonConvert.DeserializeObject<Guid>(EmployeeIdString) : Guid.Empty;
            //đổi trạng thái cho hóa đơn
            var bill = _dbContext.Bills.First(c => c.BillID == idBill);
            if (EmployeeID != null)
            {
                var billStatusHis = new BillStatusHistory()
                {
                    BillStatusHistoryID = Guid.NewGuid(),
                    Status = 1,
                    ChangeDate = DateTime.Now,
                    Note = ghiChu,
                    BillID = idBill,
                    EmployeeID = EmployeeID
                };
                bill.Status = 1;
                bill.ConfirmationDate = DateTime.Now;
                bill.EmployeeID = EmployeeID;
                _dbContext.Bills.Update(bill);
                _repos.AddItem(billStatusHis);
                _dbContext.SaveChanges();
            }
            return Json(new { success = true, message = "Lưu trạng thái thành công" });
        }

        public IActionResult SaveStatusBill1(string ghiChu, Guid idBill)
        {
            var EmployeeIdString = HttpContext.Session.GetString("EmployeeID");
            var EmployeeID = !string.IsNullOrEmpty(EmployeeIdString) ? JsonConvert.DeserializeObject<Guid>(EmployeeIdString) : Guid.Empty;
            //đổi trạng thái cho hóa đơn
            var bill = _dbContext.Bills.First(c => c.BillID == idBill);
            if (EmployeeID != null)
            {
                var billStatusHis = new BillStatusHistory()
                {
                    BillStatusHistoryID = Guid.NewGuid(),
                    Status = 2,
                    ChangeDate = DateTime.Now,
                    Note = ghiChu,
                    BillID = idBill,
                    EmployeeID = EmployeeID
                };
                bill.Status = 2;
                bill.DeliveryDate = DateTime.Now;
                bill.EmployeeID = EmployeeID;
                _dbContext.Bills.Update(bill);
                _repos.AddItem(billStatusHis);
                _dbContext.SaveChanges();
            }
            return Json(new { success = true, message = "Lưu trạng thái thành công" });
        }

        public IActionResult SaveStatusBill2(string ghiChu, Guid idBill)
        {
            var EmployeeIdString = HttpContext.Session.GetString("EmployeeID");
            var EmployeeID = !string.IsNullOrEmpty(EmployeeIdString) ? JsonConvert.DeserializeObject<Guid>(EmployeeIdString) : Guid.Empty;
            //đổi trạng thái cho hóa đơn
            var bill = _dbContext.Bills.First(c => c.BillID == idBill);
            if (EmployeeID != null)
            {
                var billStatusHis = new BillStatusHistory()
                {
                    BillStatusHistoryID = Guid.NewGuid(),
                    Status = 3,
                    ChangeDate = DateTime.Now,
                    Note = ghiChu,
                    BillID = idBill,
                    EmployeeID = EmployeeID
                };
                bill.Status = 3;
                bill.SuccessDate = DateTime.Now;
                bill.EmployeeID = EmployeeID;
                _dbContext.Bills.Update(bill);
                _repos.AddItem(billStatusHis);
                _dbContext.SaveChanges();
            }
            return Json(new { success = true, message = "Lưu trạng thái thành công" });
        }

        public IActionResult SaveSuccessBillCus(Guid idBill)
        {
            var objBill = _dbContext.Bills.FirstOrDefault(c => c.BillID == idBill);
            if (objBill != null)
            {
                objBill.IsPaid = true;
                objBill.PaymentDay = DateTime.Now;
            }
            _dbContext.Update(objBill);
            _dbContext.SaveChanges();
            return Ok(new { success = true });
        }

        public IActionResult SaveSuccessBill(Guid idBill, string httt, string ghiChuTT)
        {
            var EmployeeIdString = HttpContext.Session.GetString("EmployeeID");
            var EmployeeID = !string.IsNullOrEmpty(EmployeeIdString) ? JsonConvert.DeserializeObject<Guid>(EmployeeIdString) : Guid.Empty;
            var namePurchaseMethod = _dbContext.PurchaseMethods.First(c => c.MethodName == httt).PurchaseMethodID;
            var objBill = _dbContext.Bills.First(c => c.BillID == idBill);
            if (objBill != null)
            {
                objBill.EmployeeID = EmployeeID;
                objBill.IsPaid = true;
                objBill.PaymentDay = DateTime.Now;
                objBill.PurchaseMethodID = namePurchaseMethod;
            }
            var historyBill = new ReturnedProducts()
            {
                ID = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                Note = ghiChuTT,
                QuantityReturned = 0,
                ReturnedPrice = objBill.TotalPriceAfterDiscount,
                TransactionType = 0,
                NamePurChaseMethod = httt,
                Status = 0,
                BillId = objBill.BillID,
                ShoesDetails_SizeID = null
            };
            _dbContext.ReturnedProducts.Add(historyBill);
            _dbContext.Bills.Update(objBill);
            _dbContext.SaveChanges();
            return Json(new { success = true, message = "Xác nhận đơn hàng thành công" });
        }

        public IActionResult CancelOrder(string ghiChu, Guid idBill)
        {
            var EmployeeIdString = HttpContext.Session.GetString("EmployeeID");
            var EmployeeID = !string.IsNullOrEmpty(EmployeeIdString) ? JsonConvert.DeserializeObject<Guid>(EmployeeIdString) : Guid.Empty;
            var objBill = _dbContext.Bills.First(c => c.BillID == idBill);
            var billStatusHis = new BillStatusHistory()
            {
                BillStatusHistoryID = Guid.NewGuid(),
                Status = 4,
                ChangeDate = DateTime.Now,
                Note = ghiChu,
                BillID = idBill,
                EmployeeID = EmployeeID != Guid.Empty ? EmployeeID : null
            };
            objBill.Status = 4;
            objBill.CancelDate = DateTime.Now;
            objBill.EmployeeID = EmployeeID != Guid.Empty ? EmployeeID : null;
            _dbContext.Bills.Update(objBill);
            _repos.AddItem(billStatusHis);
            _dbContext.SaveChanges();
            return Json(new { success = true, message = "Lưu trạng thái thành công" });
        }

        public IActionResult UpdateQuantityItem(Guid idBill, Guid idShoesDT, string idSize, string ghiChuUpdateSP, int quanity, string nameProduct)
        {
            var EmployeeIdString = HttpContext.Session.GetString("EmployeeID");
            var EmployeeID = !string.IsNullOrEmpty(EmployeeIdString) ? JsonConvert.DeserializeObject<Guid>(EmployeeIdString) : Guid.Empty;
            var objSize = _dbContext.Sizes.First(c => c.Name == idSize)?.SizeID;
            var ShoesDT_Size = _dbContext.ShoesDetails_Sizes.First(c => c.ShoesDetailsId == idShoesDT && c.SizeID == objSize);
            var billDetails = _dbContext.BillDetails.First(c => c.BillID == idBill && c.ShoesDetails_SizeID == ShoesDT_Size.ID);
            //đổi trạng thái cho hóa đơn
            var bill = _dbContext.Bills.First(c => c.BillID == idBill);
            if (EmployeeID != null)
            {
                var billStatusHis = new BillStatusHistory()
                {
                    BillStatusHistoryID = Guid.NewGuid(),
                    Status = 5,
                    ChangeDate = DateTime.Now,
                    Note = $"{ghiChuUpdateSP} sản phẩm [{nameProduct}] - kích cỡ [{idSize}] với số lượng: [{quanity}]",
                    BillID = idBill,
                    EmployeeID = EmployeeID
                };
                bill.UpdateDate = DateTime.Now;
                bill.EmployeeID = EmployeeID;
                _dbContext.Bills.Update(bill);

                var previousQuantity = billDetails.Quantity;
                billDetails.Quantity = quanity;
                billDetails.Price = billDetails.Price;
                _dbContext.BillDetails.Update(billDetails);

                ShoesDT_Size.Quantity += previousQuantity - quanity;
                _dbContext.ShoesDetails_Sizes.Update(ShoesDT_Size);

                _repos.AddItem(billStatusHis);
                _dbContext.SaveChanges();
            }
            return Json(new { success = true, message = "Lưu trạng thái thành công" });
        }

        public IActionResult UpdateShippinginBill(Guid BillID, decimal ShippingCost)
        {
            var bill = _dbContext.Bills.First(c => c.BillID == BillID);
            var totalPriceBefore = bill.TotalPrice;
            var shippingCostBefore = bill.ShippingCosts;
            var totalPrice = totalPriceBefore - shippingCostBefore;
            var newPrice = totalPrice + ShippingCost;
            bill.TotalPrice = newPrice;
            bill.ShippingCosts = ShippingCost;
            _dbContext.Bills.Update(bill);
            _dbContext.SaveChanges();
            return Ok(new { success = true });
        }
    }
}
