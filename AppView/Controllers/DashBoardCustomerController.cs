using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppView.Models;
using AppView.Models.DashBoardViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AppView.Controllers
{
    public class DashBoardCustomerController : Controller
    {
        private readonly IAllRepositories<Bill> _repos;
        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<Bill> _bill;
        public DashBoardCustomerController()
        {
            _bill = _dbContext.Bills;
            AllRepositories<Bill> all = new AllRepositories<Bill>(_dbContext, _bill);
            _repos = all;
        }

        //private bool CheckUserRole()
        //{
        //    var CustomerRole = HttpContext.Session.GetString("UserId");
        //    var EmployeeNameSession = HttpContext.Session.GetString("RoleName");
        //    var EmployeeName = EmployeeNameSession != null ? EmployeeNameSession.Replace("\"", "") : null;
        //    if (CustomerRole != null || EmployeeName != "Quản lý")
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        public IActionResult index()
        {
            return RedirectToAction("Forbidden", "Home");
        }

        [HttpGet]
        public IActionResult tables()
        {

            var listBill = _dbContext.Bills.Select(c => new tablesViewModel
            {
                BillID = c.BillID,
                BillCode = c.BillCode,
                TotalShoes = _dbContext.BillDetails.Where(z => z.BillID == c.BillID).Sum(s => s.Quantity),
                Price = c.TotalPriceAfterDiscount,
                CustomerID = c.CustomerID,
                FullNameCus = _dbContext.Customers.First(z => z.CumstomerID == c.CustomerID).FullName,
                PhoneNumber = _dbContext.Customers.First(z => z.CumstomerID == c.CustomerID).PhoneNumber,
                CreateDate = c.CreateDate,
                PurchasePayMent = _dbContext.PurchaseMethods.First(z => z.PurchaseMethodID == c.PurchaseMethodID).MethodName,
                Status = c.Status,
            }).ToList();
            return View(listBill);
        }

        [HttpGet]
        public IActionResult findTables(string billCode)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            var customerIdSession = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
            var objCustomer = _dbContext.Customers.FirstOrDefault(c => c.CumstomerID == customerIdSession);
            var objBill = _dbContext.BillDetails.Include(c => c.Bill).Include(c => c.ShoesDetails_Size).ThenInclude(sds => sds.ShoesDetails).ThenInclude(sd => sd.Product)
                .Where(c => c.Bill.CustomerID == objCustomer.CumstomerID && (c.Bill.BillCode.ToLower().Contains(billCode) || c.ShoesDetails_Size.ShoesDetails.Product.Name.ToLower().Contains(billCode))).Select(c => c.BillID).ToList();
            var billUser = _dbContext.BillDetails.Where(c => objBill.Contains(c.BillID)); //Lấy ra tất cả BillDetails mà BillID của nó nằm trong danh sách objBill.
            var listBill = billUser.Select(c => new tablesViewModel
            {
                BillID = c.BillID,
                CustomerID = c.Bill.CustomerID,
                TotalShoes = c.Quantity,
                Price = c.Price * c.Quantity,
                TotalPrice = _dbContext.Bills.FirstOrDefault(b => b.BillID == c.BillID).TotalPriceAfterDiscount,
                NameProduct = _dbContext.Products.FirstOrDefault(p => p.ProductID == c.ShoesDetails_Size.ShoesDetails.ProductID).Name,
                Description = _dbContext.ShoesDetails.FirstOrDefault(p => p.ShoesDetailsId == c.ShoesDetails_Size.ShoesDetailsId).Description,
                ImageUrl = _dbContext.Images.FirstOrDefault(i => i.ShoesDetailsID == c.ShoesDetails_Size.ShoesDetailsId).Image1,
                ImageUser = objCustomer.ImageUser,
                CreateDate = _dbContext.Bills.FirstOrDefault(z => z.BillID == c.BillID).CreateDate,
                PurchasePayMent = _dbContext.PurchaseMethods.First(z => z.PurchaseMethodID == c.Bill.PurchaseMethodID).MethodName,
                Status = c.Status,
                SizeName = _dbContext.Sizes.FirstOrDefault(s => s.SizeID == c.ShoesDetails_Size.SizeID).Name,
            }).ToList();
            return View(listBill);
        }

        [HttpGet]
        public IActionResult Filter(int nameStatus)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            var customerIdSession = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
            var objCustomer = _dbContext.Customers.FirstOrDefault(c => c.CumstomerID == customerIdSession);
            var objBill = _dbContext.BillDetails.Include(c => c.Bill).ThenInclude(c => c.Customer).Include(c => c.ShoesDetails_Size).ThenInclude(c => c.ShoesDetails).ThenInclude(c => c.Product).Where(c => c.Bill.CustomerID == objCustomer.CumstomerID).ToList();
            if (nameStatus != 8)
            {
                objBill = _dbContext.BillDetails.Include(c => c.Bill).ThenInclude(c => c.Customer).Include(c => c.ShoesDetails_Size).ThenInclude(c => c.ShoesDetails).ThenInclude(c => c.Product).Where(c => c.Bill.Status == nameStatus && c.Bill.CustomerID == objCustomer.CumstomerID).ToList();
            }
            var listBill = objBill.Select(c => new tablesViewModel
            {
                BillID = c.BillID,
                CustomerID = c.Bill.CustomerID,
                TotalShoes = c.Quantity,
                Price = c.Price * c.Quantity,
                TotalPrice = _dbContext.Bills.FirstOrDefault(b => b.BillID == c.BillID).TotalPriceAfterDiscount,
                NameProduct = _dbContext.Products.FirstOrDefault(p => p.ProductID == c.ShoesDetails_Size.ShoesDetails.ProductID).Name,
                Description = _dbContext.ShoesDetails.FirstOrDefault(p => p.ShoesDetailsId == c.ShoesDetails_Size.ShoesDetailsId).Description,
                ImageUrl = _dbContext.Images.FirstOrDefault(i => i.ShoesDetailsID == c.ShoesDetails_Size.ShoesDetailsId).Image1,
                ImageUser = objCustomer.ImageUser,
                CreateDate = _dbContext.Bills.FirstOrDefault(z => z.BillID == c.BillID).CreateDate,
                PurchasePayMent = _dbContext.PurchaseMethods.First(z => z.PurchaseMethodID == c.Bill.PurchaseMethodID).MethodName,
                Status = c.Status,
                SizeName = _dbContext.Sizes.FirstOrDefault(s => s.SizeID == c.ShoesDetails_Size.SizeID).Name,
            }).ToList();
            return Json(listBill);
        }

        [HttpGet]
        public IActionResult tables1()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            var customerIdSession = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
            var objCustomer = _dbContext.Customers.FirstOrDefault(c => c.CumstomerID == customerIdSession);
            var listBill = _dbContext.BillDetails.Include(c => c.Bill).Include(c => c.ShoesDetails_Size).ThenInclude(sds => sds.ShoesDetails).ThenInclude(sd => sd.Product).Where(c => c.Bill.CustomerID == objCustomer.CumstomerID).ToList().Select(c => new tablesViewModel
            {
                BillID = c.BillID,
                CustomerID = c.Bill.CustomerID,
                TotalShoes = c.Quantity,
                Price = c.Price * c.Quantity,
                TotalPrice = _dbContext.Bills.FirstOrDefault(b => b.BillID == c.BillID).TotalPriceAfterDiscount,
                NameProduct = _dbContext.Products.FirstOrDefault(p => p.ProductID == c.ShoesDetails_Size.ShoesDetails.ProductID).Name,
                Description = _dbContext.ShoesDetails.FirstOrDefault(p => p.ShoesDetailsId == c.ShoesDetails_Size.ShoesDetailsId).Description,
                ImageUrl = _dbContext.Images.FirstOrDefault(i => i.ShoesDetailsID == c.ShoesDetails_Size.ShoesDetailsId).Image1,
                ImageUser = objCustomer.ImageUser,
                CreateDate = _dbContext.Bills.FirstOrDefault(z => z.BillID == c.BillID).CreateDate,
                PurchasePayMent = _dbContext.PurchaseMethods.First(z => z.PurchaseMethodID == c.Bill.PurchaseMethodID).MethodName,
                Status = c.Status,
                SizeName = _dbContext.Sizes.FirstOrDefault(s => s.SizeID == c.ShoesDetails_Size.SizeID).Name,
            }).ToList();
            //For address
            var AddressList = _dbContext.Addresses.Where(a => a.CumstomerID == objCustomer.CumstomerID).Select(address => new AddressViewModel
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
            }).ToList();
            CustomerDashboardViewModel customerDashboard = new CustomerDashboardViewModel()
            {
                BillList = listBill,
                AddressList = AddressList
            };
            return View(customerDashboard);
        }

        [HttpPost]
        public async Task<IActionResult> updateInformationUser(Guid inputUserID, [Bind(Prefix = "chooseFile")] IFormFile chooseFile, string inputUsername, string inputFullname, string phoneNumberUser, string emailUser, int sex, string inputPassword)
        {
            var userUpdate = _dbContext.Customers.FirstOrDefault(c => c.CumstomerID == inputUserID);
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Customer/update-customer?CumstomerID={userUpdate.CumstomerID}&FullName={inputFullname}&UserName={inputUsername}&Password={userUpdate.Password}&Email={emailUser}&Sex={sex}&ResetPassword=0000&PhoneNumber={phoneNumberUser}&Status={userUpdate.Status}&RankID={userUpdate.RankID}&DateCreated={userUpdate.DateCreated.ToString("yyyy-MM-ddTHH:mm:ss")}";
            var response = await httpClient.PutAsync(apiUrl, null);
            //update image
            if (chooseFile != null && chooseFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", chooseFile.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await chooseFile.CopyToAsync(stream);
                }
                userUpdate.ImageUser = chooseFile.FileName;
                _dbContext.Customers.Update(userUpdate);
                _dbContext.SaveChanges();
            }
            // cập nhật lại vào session
            HttpContext.Session.SetString("UserId", JsonConvert.SerializeObject(userUpdate.CumstomerID.ToString()));
            HttpContext.Session.SetString("UserName", JsonConvert.SerializeObject(inputUsername));
            HttpContext.Session.SetString("FullName", JsonConvert.SerializeObject(inputFullname));
            HttpContext.Session.SetString("Email", JsonConvert.SerializeObject(emailUser));
            HttpContext.Session.SetString("Sex", JsonConvert.SerializeObject(sex));
            HttpContext.Session.SetString("PhoneNumber", JsonConvert.SerializeObject(phoneNumberUser));
            HttpContext.Session.SetString("ImageUser", JsonConvert.SerializeObject(userUpdate.ImageUser));
            return RedirectToAction("tables1");
        }

        [HttpPost]
        public IActionResult ChangePassword(Guid inputUserID, string passUser, string newPass, string renewPass)
        {
            var user = _dbContext.Customers.FirstOrDefault(c => c.CumstomerID == inputUserID);
            if (user != null && user.Password == passUser)
            {
                if (newPass == renewPass)
                {
                    user.Password = newPass;
                    _dbContext.Customers.Update(user);
                    _dbContext.SaveChanges();
                    HttpContext.Session.SetString("Password", JsonConvert.SerializeObject(user.Password));
                }
                else
                {
                    return Json(new { success = false, message = "Mật khẩu mới không khớp, vui lòng nhập lại!" });
                }
            }
            else
            {
                return Json(new { success = false, message = "Vui lòng nhập đúng mật khẩu!" });
            }
            return Json(new { success = true, message = "Đổi mật khẩu thành công!" });
        }
    }
}
