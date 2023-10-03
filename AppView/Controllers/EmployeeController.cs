﻿using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppView.IServices;
using AppView.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AppView.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IAllRepositories<Employee> _repos;
        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<Employee> _customer;
        private readonly IRoleService _roleService;
        public EmployeeController()
        {
            _customer = _dbContext.Employees;
            AllRepositories<Employee> all = new AllRepositories<Employee>(_dbContext, _customer);
            _repos = all;
            _roleService = new RoleService();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEmployee() 
        {
            string apiUrl = "https://localhost:7036/api/Employee/get-employee";
            var httpClient = new HttpClient(); // tạo ra để callApi
            var response = await httpClient.GetAsync(apiUrl);// Lấy dữ liệu ra
                                                             // Lấy dữ liệu Json trả về từ Api được call dạng string
            string apiData = await response.Content.ReadAsStringAsync();
            // Lấy kqua trả về từ API
            // Đọc từ string Json vừa thu được sang List<T>
            var employees = JsonConvert.DeserializeObject<List<Employee>>(apiData);
            return View(employees);
        }
        [HttpGet]
        public async Task<IActionResult> CreateEmployee()
        {
            using (ShopDBContext shopDBContext = new ShopDBContext())
            {
                var role = shopDBContext.Roles.ToList();
                SelectList selectListRole = new SelectList(role, "RoleID", "RoleName");
                ViewBag.RoleList = selectListRole;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Employee employee)
        {
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Employee/create-employee?FullName={employee.FullName}&Password={employee.Password}&Email={employee.Email}&Sex={employee.Sex}&PhoneNumber={employee.PhoneNumber}&Status={employee.Status}&RoleID={employee.RoleID}";
            var response = await httpClient.PostAsync(apiUrl, null);
            return RedirectToAction("GetAllEmployee");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateEmployee(Guid id)
        {
            Employee employee = _repos.GetAll().FirstOrDefault(c => c.EmployeeID == id);
            using (ShopDBContext shopDBContext = new ShopDBContext())
            {
                var role = shopDBContext.Roles.ToList();
                SelectList selectListRole = new SelectList(role, "RoleID", "RoleName");
                ViewBag.RoleList = selectListRole;
            }
            return View(employee);
        }
        public async Task<IActionResult> UpdateEmployee(Employee employee)
        {
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Employee/update-employee?id={employee.EmployeeID}&FullName={employee.FullName}&Password={employee.Password}&Email={employee.Email}&Sex={employee.Sex}&PhoneNumber={employee.PhoneNumber}&Status={employee.Status}&RoleID={employee.RoleID}";
            var response = await httpClient.PutAsync(apiUrl, null);
            return RedirectToAction("GetAllEmployee");
            //if (_repos.EditItem(employee))
            //{
            //    return RedirectToAction("GetAllEmployee");
            //}
            //else return Content("Loi");
        }
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var employee = _repos.GetAll().First(c => c.EmployeeID == id);
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Employee/delete-employee?id={id}";
            var response = await httpClient.DeleteAsync(apiUrl);
            return RedirectToAction("GetAllEmployee");
            //if (_repos.RemoveItem(employee))
            //{
            //    return RedirectToAction("GetAllEmployee");
            //}
            //else
            //{
            //    return Content("Error");
        }

        public IActionResult Login()
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
        }
    }
}
