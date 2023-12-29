using System.Net.Mail;
using System.Net;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.RegularExpressions;
using AppView.Models;
using System.Web;
namespace AppView.Controllers
{
    public class CustomersController : Controller
    {

        private readonly IAllRepositories<Customer> _repos;
        private readonly IAllRepositories<Rank> _repos1;
        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<Customer> _customer;
        private DbSet<Rank> _rank;
        public CustomersController()
        {
            _customer = _dbContext.Customers;
            AllRepositories<Customer> all = new AllRepositories<Customer>(_dbContext, _customer);
            _repos = all;

            _rank = _dbContext.Ranks;
            AllRepositories<Rank> all1 = new AllRepositories<Rank>(_dbContext, _rank);
            _repos1 = all1;
        }

        public async Task<IActionResult> GetAllCustomer()
        {
            //         string apiUrl = "https://localhost:7036/api/Customer/get-customer";
            //         var httpClient = new HttpClient(); // tạo ra để callApi
            //         var response = await httpClient.GetAsync(apiUrl);// Lấy dữ liệu ra
            //                                                          // Lấy dữ liệu Json trả về từ Api được call dạng string
            //         string apiData = await response.Content.ReadAsStringAsync();
            //// Lấy kqua trả về từ API
            // Đọc từ string Json vừa thu được sang List<T>


            string apiUrl = "https://localhost:7036/api/Customer/get-customer";
            var httpClient = new HttpClient(); // tạo ra để callApi
            var response = await httpClient.GetAsync(apiUrl);// Lấy dữ liệu ra
            string apiData = await response.Content.ReadAsStringAsync();

            var customer = JsonConvert.DeserializeObject<List<Customer>>(apiData);
            var rank = _repos1.GetAll();
            var productViewModels = customer.Select(employee => new CustomerViewModel
            {
                CumstomerID = employee.CumstomerID,
                FullName = employee.FullName,
                UserName = employee.UserName,
                Password = employee.Password,
                Email = employee.Email,
                Sex = employee.Sex,
                ResetPassword = employee.ResetPassword,
                PhoneNumber = employee.PhoneNumber,
                Status = employee.Status,
                DateCreated = employee.DateCreated,
                RankName = rank.FirstOrDefault(s => s.RankID == employee.RankID)?.Name,
            }).ToList();
            return View(productViewModels);
        }
        [HttpGet]
        public async Task<IActionResult> CreateCustomer()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Customer/create-customer?FullName={customer.FullName}&UserName={customer.UserName}&Password={customer.Password}&Email={customer.Email}&Sex={customer.Sex}&ResetPassword={customer.ResetPassword}&PhoneNumber={customer.PhoneNumber}&Status={customer.Status}&RankID={customer.RankID}&DateCreated={customer.DateCreated}";
            var response = await httpClient.PostAsync(apiUrl, null);
            return RedirectToAction("GetAllCustomer");
        }
        [HttpGet]
        public async Task<IActionResult> EditCustomer(Guid id) // Khi ấn vào Create thì hiển thị View
        {
            // Lấy Product từ database dựa theo id truyền vào từ route
            Customer customer = _repos.GetAll().FirstOrDefault(c => c.CumstomerID == id);
            return View(customer);
        }
        public async Task<IActionResult> EditCustomer(Customer customer) // Thực hiện việc Tạo mới
        {
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Customer/update-customer?FullName={customer.FullName}&UserName={customer.UserName}&Password={customer.Password}&Email={customer.Email}&Sex={customer.Sex}&ResetPassword={customer.ResetPassword}&PhoneNumber={customer.PhoneNumber}&Status={customer.Status}&RankID={customer.RankID}&DateCreated={customer.DateCreated}&CumstomerID={customer.CumstomerID}";
            var response = await httpClient.PutAsync(apiUrl, null);
            return RedirectToAction("GetAllCustomer");
            //if (_repos.EditItem(customer))
            //{
            //    return RedirectToAction("GetAllCustomer");
            //}
            //else return BadRequest();
        }
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var cus = _repos.GetAll().First(c => c.CumstomerID == id);
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Customer/delete-customer?id={id}";
            var response = await httpClient.DeleteAsync(apiUrl);
            return RedirectToAction("GetAllCustomer");
            //if (_repos.RemoveItem(cus))
            //{
            //    return RedirectToAction("GetAllCustomer");
            //}
            //else return Content("Error");
        }
        public async Task<IActionResult> FindCustomer(string searchQuery)
        {
            var customer = _repos.GetAll().Where(c => c.UserName.ToLower().Contains(searchQuery.ToLower()));
            return View(customer);
        }

        public IActionResult Login()
        {
            ViewBag.SignUpSuccess = TempData["SignUpSuccess"];
            return View();
        }
        [HttpPost]
        public IActionResult Login(Customer customer)
        {
            var loggedInUser = _repos.GetAll().FirstOrDefault(c => c.UserName == customer.UserName && c.Password == customer.Password);
            if (loggedInUser != null)
            {
                HttpContext.Session.SetString("UserId", JsonConvert.SerializeObject(loggedInUser.CumstomerID.ToString()));
                HttpContext.Session.SetString("UserName", JsonConvert.SerializeObject(loggedInUser.UserName));

                TempData["SignUpSuccess"] = "Đăng nhập thành công!";
                return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
            }
            else
            {
                return Json(new { success = false, message = "Vui lòng nhập đúng thông tin tài khoản" });
            }
        }


        public async Task<IActionResult> SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(Customer customer, string ConfirmPassword, string rank, int sex)
        {
            var RankUser = _dbContext.Ranks.FirstOrDefault(c => c.Name == rank);
            if (customer.Password != ConfirmPassword)
            {
                return View();
            }
            if (_repos.GetAll().Any(c => c.UserName == customer.UserName))
            {
                return Content("Da ton tai");
            }
            var httpClient = new HttpClient();

            // Tạo URL API với các tham số cố định
            string apiUrl = $"https://localhost:7036/api/Customer/create-customer?" +
                $"FullName={HttpUtility.UrlEncode(customer.FullName)}" +
                $"&UserName={HttpUtility.UrlEncode(customer.UserName)}" +
                $"&Password={HttpUtility.UrlEncode(customer.Password)}" +
                $"&Email={HttpUtility.UrlEncode(customer.Email)}" +
                $"&Sex={sex}" +
                $"&ResetPassword=0000" +
                $"&PhoneNumber={HttpUtility.UrlEncode(customer.PhoneNumber)}" +
                $"&Status=0" +
                $"&RankID=29e837e2-1875-434c-a3f9-628db0e033e5" +
                $"&DateCreated={DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}";
            var response = await httpClient.PostAsync(apiUrl, null);
            return RedirectToAction("Login");
        }
        public IActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DangNhap(Customer customer)
        {
            if (customer.UserName.Length <= 6 || customer.Password.Length <= 6)
            {
                return Content("User Name va Password phai dai hon 6 ky tu");
            }
            Regex regex = new Regex("^[a-zA-Z0-9]+$");
            if (!regex.IsMatch(customer.UserName) || !regex.IsMatch(customer.Password))
            {
                return Content("User Name và Password chỉ được chứa chữ cái và số.");
            }
            return RedirectToAction("Index", "Home");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(string Email)
        {
            try
            {
                var customer = _repos.GetAll().FirstOrDefault(c => c.Email == Email);
                if (customer == null)
                {
                    return Json(new { success = false, message = "Email không tồn tại" });
                }
                var randomCode = GenerateToken();
                var subject = "Mã xác nhận đổi mật khẩu tài khoản Nike&08Pt";
                var body = $"Mã xác nhận của bạn là: {randomCode}";
                // Gửi email
                GuiMail(customer.Email, subject, body);
                // Cập nhật mã xác nhận mới cho nhân viên
                customer.ResetPassword = randomCode;
                _repos.EditItem(customer);
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
            var customer = _repos.GetAll().FirstOrDefault(c => c.ResetPassword == CodeXacThuc);
            if (customer == null)
            {
                // Nếu không tìm thấy user với mã xác thực này, hiển thị thông báo lỗi.
                TempData["ErrorMessage"] = "Mã xác thực không hợp lệ.";
                return View();
            }
            // Kiểm tra mã xác thực
            if (customer.ResetPassword != CodeXacThuc)
            {
                TempData["ErrorMessage"] = "Mã xác thực không đúng.";
                return View();
            }
            // Nếu mã xác thực đúng, chuyển hướng tới trang cập nhật mật khẩu
            HttpContext.Session.SetString("ResetPasswordUserName", customer.UserName);
            return RedirectToAction("UpdatePassword");
        }
        public IActionResult UpdatePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UpdatePassword(string password)
        {
            var username = HttpContext.Session.GetString("ResetPasswordUserName");
            Console.WriteLine("Username from HttpContext: " + username); // Thêm log để ghi lại thông tin đăng nhập được lấy từ HttpContext
            var customer = _repos.GetAll().FirstOrDefault(c => c.UserName == username);
            if (customer == null)
            {
                TempData["ErrorMessage"] = "User không tồn tại";
                return View();
            }
            customer.Password = password;
            _repos.EditItem(customer);
            TempData["UserName"] = customer.UserName;
            TempData["Password"] = customer.Password;
            TempData["SignUpSuccess"] = "Mật khẩu đã được cập nhật thành công!";
            return RedirectToAction("Login", "Customer");
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Login");
        }

    }
}
