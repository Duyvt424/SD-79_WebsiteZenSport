using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppView.IServices;
using AppView.Models;
using AppView.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AppView.Controllers
{
    public class BillController : Controller
    {
        private readonly IAllRepositories<Bill> _repos;
        private readonly IAllRepositories<Customer> customer;
        private readonly IAllRepositories<Voucher> voucher;
        private readonly IAllRepositories<Employee> employee;
        private readonly IAllRepositories<PurchaseMethod> purchaseMethod;
        // private readonly IAllRepositories<Supplier> supplierRepos;
        ShopDBContext _dbContext = new ShopDBContext();
        DbSet<Employee> _employee;
        DbSet<Customer> _cu;
        DbSet<Voucher> _voucher;
        DbSet<PurchaseMethod> _pu;
        DbSet<Bill> _bill;
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
        public async Task<IActionResult> GetAllBill() { 
            string apiUrl = "https://localhost:7036/api/Bill/get-bill";
            var httpClient = new HttpClient(); // tạo ra để callApi
            var response = await httpClient.GetAsync(apiUrl);// Lấy dữ liệu ra
            string apiData = await response.Content.ReadAsStringAsync();
            var bill = JsonConvert.DeserializeObject<List<Bill>>(apiData);
            var cus = customer.GetAll();
            var em = employee.GetAll();
            var pu = purchaseMethod.GetAll();
            var vc = voucher.GetAll();
            //  var materials = materialRepos.GetAll();

            // Tạo danh sách ProductViewModel với thông tin Supplier và Material
            var billViewModels = bill.Select(bills => new BillViewModel
            {
                BillID = bills.BillID,
                BillCode = bills.BillCode,
                CreateDate = bills.CreateDate,
                SuccessDate = bills.SuccessDate,
                DeliveryDate = bills.DeliveryDate,
                CancelDate = bills.CancelDate,
                TotalPrice = bills.TotalPrice,
                ShippingCosts = bills.ShippingCosts,
                Note = bills.Note,
                Status = bills.Status,

                CustomerName = cus.FirstOrDefault(s => s.CumstomerID == bills.CustomerID)?.FullName,
                VoucherName = vc.FirstOrDefault(c => c.VoucherID == bills.VoucherID).VoucherCode,
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
              string apiUrl = $"https://localhost:7036/api/Bill/create-bill?BillCode={GenerateBillCode()}&CreateDate={bill.CreateDate}&SuccessDate={bill.SuccessDate}&DeliveryDate={bill.DeliveryDate}&CancelDate={bill.CancelDate}&TotalPrice={bill.TotalPrice}&ShippingCosts={bill.ShippingCosts}&Note={bill.Note}&Status={bill.Status}&CustomerID={bill.CustomerID}&VoucherID={bill.VoucherID}&EmployeeID={bill.EmployeeID}&PurchaseMethodID={bill.PurchaseMethodID}";
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
            var pu = purchaseMethod.GetAll();
            // Tạo danh sách ProductViewModel với thông tin Supplier và Material
            var billViewModels = bill.Select(bills => new BillViewModel
            {
                BillID = bills.BillID,
                BillCode = bills.BillCode,
                CreateDate = bills.CreateDate,
                SuccessDate = bills.SuccessDate,
                DeliveryDate = bills.DeliveryDate,
                CancelDate = bills.CancelDate,
                TotalPrice = bills.TotalPrice,
                ShippingCosts = bills.ShippingCosts,
                Note = bills.Note,
                Status = bills.Status,

                CustomerName = cus.FirstOrDefault(s => s.CumstomerID == bills.CustomerID)?.FullName,
                VoucherName = vc.FirstOrDefault(c => c.VoucherID == bills.VoucherID).VoucherCode,
                EmployeeName = em.FirstOrDefault(e => e.EmployeeID == bills.EmployeeID).FullName,
                PurchaseMethodName = pu.FirstOrDefault(p => p.PurchaseMethodID == bills.PurchaseMethodID).MethodName,

                //  MaterialName = materials.VoucherName(m => m.MaterialId == product.MaterialId)?.Name
            }).ToList();

            return View(billViewModels);
        }
        /*
        [HttpGet]
        public async Task<IActionResult> CreateEmployee()
        {
            using (ShopDBContext dBContext = new ShopDBContext())
            {
                var role = dBContext.Roles.Where(c => c.Status == 0).ToList();
                SelectList selectListRole = new SelectList(role, "RoleID", "RoleName");
                ViewBag.RoleList = selectListRole;

               
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Employee employee)
        {
            if (_repos.AddItem(employee))
            {
                return RedirectToAction("GetAllEmployee");
            }
            else return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> EditEmployee(Guid id)
        {
            Employee employee = _repos.GetAll().FirstOrDefault(c => c.EmployeeID == id);
            using (ShopDBContext dBContext = new ShopDBContext())
            {
                var role = dBContext.Roles.Where(c => c.Status == 0).ToList();
                SelectList selectListRole = new SelectList(role, "RoleID", "RoleName");
                ViewBag.RoleList = selectListRole;


            }
            return View(employee);
        }
		public async Task<IActionResult> EditEmployee(Employee employee)
		{
			if (_repos.EditItem(employee))
			{
				return RedirectToAction("GetAllEmployee");
			}
			else return BadRequest();
		}
		public async Task<IActionResult> DeleteEmployee(Guid id)
		{
			var product = _repos.GetAll().FirstOrDefault(c => c.EmployeeID == id);
			var httpClient = new HttpClient();
			string apiUrl = $"https://localhost:7036/api/Employee/delete-employee?id={id}";
			var response = await httpClient.DeleteAsync(apiUrl);
			return RedirectToAction("GetAllEmployee");
		}
        public async Task<IActionResult> FindEmployee(string searchQuery)
        {
            var employees = _repos.GetAll().Where(c => c.FullName.ToLower().Contains(searchQuery.ToLower()) || c.UserName.ToLower().Contains(searchQuery.ToLower()));
            var roles = role.GetAll();


            // Tạo danh sách ProductViewModel với thông tin Supplier và Material
            var productViewModels = employees.Select(employee => new EmployeeViewModel
            {
                EmployeeID = employee.EmployeeID,
                FullName = employee.FullName,
                UserName = employee.UserName,
                Password = employee.Password,
                Email = employee.Email,
                Sex = employee.Sex,
                ResetPassword = employee.ResetPassword,
                PhoneNumber = employee.PhoneNumber,
                Status = employee.Status,
                DateCreated = employee.DateCreated,
                RoleName = roles.FirstOrDefault(s => s.RoleID == employee.RoleID)?.RoleName,
                //  MaterialName = materials.FirstOrDefault(m => m.MaterialId == product.MaterialId)?.Name
            }).ToList();

            return View(productViewModels);
        }
        /*  public IActionResult Login()
          {
              ViewBag.SignUpSuccess = TempData["SignUpSuccess"];
              return View();
          }
          [HttpPost]
          public IActionResult Login(Employee customer)
          {
              var loggedInUser = _repos.GetAll().FirstOrDefault(c => c.FullName == customer.FullName && c.Password == customer.Password);
              var role = _roleService.GetUserById(loggedInUser.RoleID).RoleName;
              if (loggedInUser != null)
              {
                  HttpContext.Session.SetString("EmployeeID", JsonConvert.SerializeObject(loggedInUser.EmployeeID.ToString()));
                  HttpContext.Session.SetString("FullNameEmployee", JsonConvert.SerializeObject(loggedInUser.FullName));

                  TempData["SignUpSuccess"] = "Đăng nhập thành công!";
                  HttpContext.Session.SetString("role", role);
                  return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") }); 
              }
              else
              {
                  return Json(new { success = false, message = "Vui lòng nhập đúng thông tin tài khoản" });
              }
          }
          public IActionResult SignUp()
          {
              using (ShopDBContext shopDBContext = new ShopDBContext())
              {
                  var role = shopDBContext.Roles.ToList();
                  SelectList selectListsRole = new SelectList(role, "RoleID", "RoleName");
                  ViewBag.ListRole = selectListsRole;
              }
              return View();
          }
          [HttpPost]
          public IActionResult SignUp(Employee employee, string ConfirmPassword)
          {
              if (employee.Password != ConfirmPassword)
              {
                  return View();
              }
              else
              if (_repos.GetAll().Any(c => c.FullName == employee.FullName))
              {
                  return Json(new { success = false, message = "Tên đăng nhập đã tồn tại" });
              }
              else
                  employee.PhoneNumber = "000000000";
              //user.DiaChi = "OK";
              if (_repos.AddItem(employee))
              {
                  TempData["UserName"] = employee.FullName;
                  TempData["Password"] = employee.Password;
                  TempData["SignUpSuccess"] = "Đăng ký tài khoản thành công!";
                  return Json(new { success = true, redirectUrl = Url.Action("Login", "Employee") });
              }
              else return BadRequest();
          }

          public IActionResult ForgotPassword()
          {
              return View();
          }
          [HttpPost]
          public IActionResult ForgotPassword(string Email)
          {
              try
              {
                  var employee = _repos.GetAll().FirstOrDefault(c => c.Email == Email);
                  if (employee == null)
                  {
                      return Json(new { success = false, message = "Email không tồn tại" });
                  }
                  var randomCode = GenerateToken();
                  var subject = "Mã xác nhận đổi mật khẩu tài khoản Nike&08Pt";
                  var body = $"Mã xác nhận của bạn là: {randomCode}";
                  // Gửi email
                  GuiMail(employee.Email, subject, body);
                  // Cập nhật mã xác nhận mới cho nhân viên
                  employee.ResetPassword = randomCode;
                  _repos.EditItem(employee);
                  TempData["SignUpSuccess"] = "Mã xác nhận đã được gửi, vui lòng kiểm tra hòm thư!";
                  //return Json(new { success = true, message = "Gửi mail thành công" });
                  return RedirectToAction("VerificationCode");
              }
              catch
              {
                  return Json(new { success = false, message = "Có lỗi xảy ra khi gửi mail. Hãy thử lại sau" });
              }
          }
          public string GenerateToken()
          {
              var random = new Random();
              return random.Next(100000, 999999).ToString();
          }
          public void GuiMail(string Email, string subject, string body)
          {
              var message = new MailMessage();
              message.From = new MailAddress("vuduy10a7@gmail.com");
              message.To.Add(Email);
              message.Subject = subject;
              message.Body = body;
              message.IsBodyHtml = true;

              var smtpClient = new SmtpClient("smtp.gmail.com", 587);
              smtpClient.UseDefaultCredentials = false;
              smtpClient.EnableSsl = true;
              smtpClient.Credentials = new NetworkCredential("vuduy10a7@gmail.com", "yzcgeyulyergpmjw");

              // Thêm lệnh ghi log
              smtpClient.SendCompleted += (sender, args) =>
              {
                  if (args.Error != null)
                  {
                      // Ghi log lỗi
                      Console.WriteLine($"Lỗi gửi email: {args.Error.Message}");
                  }
                  else if (args.Cancelled)
                  {
                      // Ghi log hủy bỏ gửi email
                      Console.WriteLine("Gửi email bị hủy bỏ.");
                  }
                  else
                  {
                      // Ghi log gửi email thành công
                      Console.WriteLine("Gửi email thành công.");
                  }
              };
              smtpClient.Send(message);
          }
          public IActionResult VerificationCode()
          {
              ViewBag.SignUpSuccess = TempData["SignUpSuccess"];
              return View();
          }
          [HttpPost]
          public IActionResult VerificationCode(string CodeXacThuc)
          {
              var user = _repos.GetAll().FirstOrDefault(c => c.ResetPassword == CodeXacThuc);
              if (user == null)
              {
                  // Nếu không tìm thấy user với mã xác thực này, hiển thị thông báo lỗi.
                  TempData["ErrorMessage"] = "Mã xác thực không hợp lệ.";
                  return View();
              }
              // Kiểm tra mã xác thực
              if (user.ResetPassword != CodeXacThuc)
              {
                  TempData["ErrorMessage"] = "Mã xác thực không đúng.";
                  return View();
              }
              // Nếu mã xác thực đúng, chuyển hướng tới trang cập nhật mật khẩu
              HttpContext.Session.SetString("ResetPassword", user.FullName);
              return RedirectToAction("UpdatePassword");
          }
          public IActionResult UpdatePassword()
          {
              return View();
          }
          [HttpPost]
          public IActionResult UpdatePassword(string password)
          {
              var fullname = HttpContext.Session.GetString("ResetPassword");
              Console.WriteLine("Fullname from HttpContext: " + fullname); // Thêm log để ghi lại thông tin đăng nhập được lấy từ HttpContext
              var user = _repos.GetAll().FirstOrDefault(c => c.FullName == fullname);
              if (user == null)
              {
                  TempData["ErrorMessage"] = "User không tồn tại";
                  return View();
              }
              user.Password = password;
              _repos.EditItem(user);
              TempData["FullName"] = user.FullName;
              TempData["Password"] = user.Password;
              TempData["SignUpSuccess"] = "Mật khẩu đã được cập nhật thành công!";
              return RedirectToAction("Login", "Employee");
          }
          public IActionResult DangNhap()
          {
              return View();
          }
          [HttpPost]
          public IActionResult DangNhap(Employee customer)
          {
              if (customer.FullName.Length <= 6 || customer.Password.Length <= 6)
              {
                  return Content("FullName va Password phai dai hon 6 ky tu");
              }
              Regex regex = new Regex("^[a-zA-Z0-9]+$");
              if (!regex.IsMatch(customer.FullName) || !regex.IsMatch(customer.Password))
              {
                  return Content("User Name và Password chỉ được chứa chữ cái và số.");
              }
              return RedirectToAction("Index", "Home");
          }
          public IActionResult LogOut()
          {
              HttpContext.Session.Remove("FullName");   
              return RedirectToAction("Login");
          }
          [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
          public IActionResult Error()
          {
              return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
          } */
    }
}
