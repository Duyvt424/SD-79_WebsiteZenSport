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
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AppView.Controllers
{ 
    public class CustomerController : Controller
    {
        private readonly IAllRepositories<Customer> _repos;
		private readonly IAllRepositories<Rank> _repos1;
		private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<Customer> _customer;
		private DbSet<Rank> _rank;
		public CustomerController()
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
            var customer = _repos.GetAll().Where(c =>c.UserName.ToLower().Contains(searchQuery.ToLower()));
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
				HttpContext.Session.SetString("FullName", JsonConvert.SerializeObject(loggedInUser.FullName));
				HttpContext.Session.SetString("Email", JsonConvert.SerializeObject(loggedInUser.Email));
                HttpContext.Session.SetString("Password", JsonConvert.SerializeObject(loggedInUser.Password));
                HttpContext.Session.SetString("Sex", JsonConvert.SerializeObject(loggedInUser.Sex));
                HttpContext.Session.SetString("PhoneNumber", JsonConvert.SerializeObject(loggedInUser.PhoneNumber));
                

                TempData["SignUpSuccess"] = "Đăng nhập thành công!";
                return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
            }
            else
            {
                return Json(new { success = false, message = "Vui lòng nhập đúng thông tin tài khoản" });
            }
        }


		[HttpGet]
		public async Task<IActionResult> GetTotalSpentByCustomer1()
		{
			// Kiểm tra xem có customerId được lưu trong Session không
			if (HttpContext.Session.TryGetValue("UserId", out byte[] userIdBytes) &&
				Guid.TryParse(Encoding.UTF8.GetString(userIdBytes), out Guid customerId))
			{
				string apiUrl = "https://localhost:7036/api/Bill/get-bill";
				var httpClient = new HttpClient();
				var response = await httpClient.GetAsync(apiUrl);
				string apiData = await response.Content.ReadAsStringAsync();
				var bill = JsonConvert.DeserializeObject<List<Bill>>(apiData);

				// Tính tổng tiền của khách hàng đang đăng nhập
				decimal totalSpent = bill
					.Where(b => b.CustomerID == customerId)
					.Sum(b => b.TotalPriceAfterDiscount);

				// Gọi API để cập nhật RankID cho khách hàng đang đăng nhập
				string rankId = GetRankId(totalSpent);

				string updateRankApiUrl = $"https://localhost:7036/api/Customer/update-customer-by-rankid?customerId={customerId}&rankId={rankId}";

				var updateRankResponse = await httpClient.PutAsync(updateRankApiUrl, null);

				if (!updateRankResponse.IsSuccessStatusCode)
				{
					// Xử lý lỗi nếu cần thiết
					return StatusCode((int)updateRankResponse.StatusCode);
				}
				ViewBag.CustomerId = customerId;
				return View();
			}
			else
			{
				// Redirect hoặc xử lý khi không có customerId trong Session
				return RedirectToAction("Login"); // hoặc thực hiện hành động khác tùy thuộc vào yêu cầu của bạn
			}
		}




		private string GetRankId(decimal totalSpent)
		{
			// Xác định Rank dựa trên totalSpent
			if (totalSpent >= 20000000) // Hạng kim cương
			{
				return "b76ed681-23ef-4591-8c03-70b287d716ac";
			}
			else if (totalSpent >= 10000000) // Hạng vàng
			{
				return "34dc614c-9604-4288-b964-3d011afd3b76";
			}
			else if (totalSpent >= 5000000) // Hạng bạc
			{
				return "49865ee6-9562-46b9-bf6c-d2af8f8c2685";
			}
			else
			{
				return "29e837e2-1875-434c-a3f9-628db0e033e5";
			}
		}

		public async Task<IActionResult> SignUp()
        {  
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(Customer customer, string ConfirmPassword, string rank, int sex)
        {
            var RankUser = _dbContext.Ranks.FirstOrDefault(c => c.Name == rank).RankID;
            var userKhach = _dbContext.Customers.FirstOrDefault(c => c.PhoneNumber == customer.PhoneNumber && c.ResetPassword == "" && c.Status == 1);
            if (customer.Password != ConfirmPassword)
            {
                return View();
            }
            if (_repos.GetAll().Any(c => c.UserName == customer.UserName))
            {
                return View();
            }
            if (userKhach != null)
            {
                var httpClient = new HttpClient();
                string apiUrl = $"https://localhost:7036/api/Customer/update-customer?CumstomerID={userKhach.CumstomerID}&FullName={customer.FullName}&UserName={customer.UserName}&Password={customer.Password}&Email={customer.Email}&Sex={customer.Sex}&ResetPassword={0000}&PhoneNumber={customer.PhoneNumber}&Status={0}&RankID={RankUser}&DateCreated={DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}";
                var response = await httpClient.PutAsync(apiUrl, null);
                return RedirectToAction("Login");
            }
            else
            {
                var httpClient = new HttpClient();
                string apiUrl = $"https://localhost:7036/api/Customer/create-customer?FullName={HttpUtility.UrlEncode(customer.FullName)}&UserName={HttpUtility.UrlEncode(customer.UserName)}&Password={HttpUtility.UrlEncode(customer.Password)}&Email={HttpUtility.UrlEncode(customer.Email)}&Sex={sex}&ResetPassword={0000}&PhoneNumber={HttpUtility.UrlEncode(customer.PhoneNumber)}&Status={0}&RankID={RankUser}&DateCreated={DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}";
                var response = await httpClient.PostAsync(apiUrl, null);
                return RedirectToAction("Login");
            }
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
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(Customer customer, string newpass, string enewpass)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            var customerIdSession = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
            var objCustomer = _dbContext.Customers.FirstOrDefault(c => c.CumstomerID == customerIdSession);
            if (customer.Password != objCustomer.Password)
            {
                return RedirectToAction("ChangePassword");
            }

            if (objCustomer != null && newpass != "" && enewpass != "")
            {
                objCustomer.Password = enewpass;
            }
            _dbContext.Customers.Update(objCustomer);
            _dbContext.SaveChanges();
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Login");
        }

        public async Task LoginByGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse")
                });
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });
            var nameIdentifier = result.Principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            var fullName = result.Principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
            var givenName = result.Principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")?.Value;
            var emailAddress = result.Principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            var userDb = _dbContext.Customers.FirstOrDefault(c => c.Email == emailAddress);
            var rank = _dbContext.Ranks.First(c => c.Name == "Không").RankID;
            if (userDb == null)
            {
                var User = new Customer()
                {
                    CumstomerID = Guid.NewGuid(),
                    FullName = fullName,
                    UserName = givenName,
                    Password = nameIdentifier,
                    Email = emailAddress,
                    Sex = 2,
                    ResetPassword = "",
                    PhoneNumber = "",
                    Status = 2,
                    DateCreated = DateTime.Now,
                    RankID = rank
                };
                _dbContext.Customers.Add(User);
                _dbContext.SaveChanges();
                userDb = User;
            }
            HttpContext.Session.SetString("UserId", JsonConvert.SerializeObject(userDb.CumstomerID.ToString()));
            HttpContext.Session.SetString("UserName", JsonConvert.SerializeObject(userDb.UserName));
            HttpContext.Session.SetString("FullName", JsonConvert.SerializeObject(userDb.FullName));
            HttpContext.Session.SetString("Email", JsonConvert.SerializeObject(userDb.Email));
            HttpContext.Session.SetString("Password", JsonConvert.SerializeObject(userDb.Password));
            HttpContext.Session.SetString("Sex", JsonConvert.SerializeObject(userDb.Sex));
            HttpContext.Session.SetString("PhoneNumber", JsonConvert.SerializeObject(userDb.PhoneNumber));
            HttpContext.Session.SetInt32("IsEmailLogin", userDb.Status);
            return RedirectToAction("Index", "Home");
        }
    }
}
