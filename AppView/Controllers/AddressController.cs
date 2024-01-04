using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Address = AppData.Models.Address;

namespace AppView.Controllers
{
    public class AddressController : Controller
	{
        private readonly IAllRepositories<Address> _repos;
        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<Address> _address;
        public AddressController()
        {
            _address = _dbContext.Addresses;
            AllRepositories<Address> all = new AllRepositories<Address>(_dbContext, _address);
            _repos = all;
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
        public async Task<IActionResult> GetAllAddress()
		{
            if(CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            string apiUrl = "https://localhost:7036/api/Address/get-address";
            var httpClient = new HttpClient(); // tạo ra để callApi
            var response = await httpClient.GetAsync(apiUrl);// Lấy dữ liệu ra
            string apiData = await response.Content.ReadAsStringAsync();
            var address = JsonConvert.DeserializeObject<List<Address>>(apiData);
            return View(address);
        }

        [HttpPost]
        public IActionResult SetDefaultAddress(Guid addressId, Guid idCus)
        {
            // Kiểm tra xem địa chỉ có tồn tại không
            var address = _dbContext.Addresses.FirstOrDefault(c => c.AddressID == addressId);
            if (address == null)
            {
                return NotFound();
            }
            var userIdString = HttpContext.Session.GetString("UserId");
            var customerIdSession = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
            var customerId = customerIdSession != Guid.Empty ? customerIdSession : idCus;
            // Lấy địa chỉ mặc định hiện tại của người dùng
            var currentDefaultAddress = _dbContext.Addresses.FirstOrDefault(c => c.CumstomerID == customerId && c.IsDefaultAddress);
            if (currentDefaultAddress != null)
            {
                // Hủy đặt làm mặc định cho địa chỉ hiện tại
                currentDefaultAddress.IsDefaultAddress = false;
            }
            // Đặt làm mặc định cho địa chỉ mới
            address.IsDefaultAddress = true;
            // Lưu thay đổi vào cơ sở dữ liệu
            _dbContext.SaveChanges();
            return Ok();
        }


        [HttpPost]
        public IActionResult SetDefaultAddressModal(Guid addressId, Guid idCus, Guid idBill)
        {
            var objBill = _dbContext.Bills.First(c => c.BillID == idBill && c.CustomerID == idCus);
            // Kiểm tra xem địa chỉ có tồn tại không
            var address = _dbContext.Addresses.FirstOrDefault(c => c.AddressID == addressId);
            if (address == null)
            {
                return NotFound();
            }
            var userIdString = HttpContext.Session.GetString("UserId");
            var customerIdSession = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
            var customerId = customerIdSession != Guid.Empty ? customerIdSession : idCus;
            // Lấy địa chỉ mặc định hiện tại của người dùng
            var currentDefaultAddress = _dbContext.Addresses.FirstOrDefault(c => c.CumstomerID == customerId && c.IsDefaultAddress);
            if (currentDefaultAddress != null)
            {
                // Hủy đặt làm mặc định cho địa chỉ hiện tại
                currentDefaultAddress.IsDefaultAddress = false;
            }
            // Đặt làm mặc định cho địa chỉ mới
            address.IsDefaultAddress = true;
            objBill.AddressID = address.AddressID;
            _dbContext.Bills.Update(objBill);
            // Lưu thay đổi vào cơ sở dữ liệu
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IActionResult GetDefaultAddress(Guid idCus)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            var customerIdSession = !string.IsNullOrEmpty(userIdString) ? JsonConvert.DeserializeObject<Guid>(userIdString) : Guid.Empty;
            var customerId = customerIdSession != Guid.Empty ? customerIdSession : idCus;
            // Lấy địa chỉ mặc định của người dùng
            var defaultAddress = _dbContext.Addresses.FirstOrDefault(c => c.CumstomerID == customerId && c.IsDefaultAddress == true);
            if (defaultAddress == null)
            {
                // Không tìm thấy địa chỉ mặc định
                return NotFound();
            }
            // Trả về thông tin địa chỉ mặc định
            return Ok(new { addressId = defaultAddress.AddressID });
        }

        [HttpPost]
        public IActionResult UpdatePriceShipping(Guid addressId, decimal shippingCost)
        {
            var objAddress = _dbContext.Addresses.First(c => c.AddressID == addressId);
            if (objAddress == null)
            {
                return NotFound();
            }
            objAddress.ShippingCost = shippingCost;
            _dbContext.Update(objAddress);
            _dbContext.SaveChanges();
            return Ok(new { success = true });
        }

        [HttpPost]
        public IActionResult UpdatePriceShippingInBill(Guid billId, decimal shippingCost, decimal priceVoucher)
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
            objBill.TotalPriceAfterDiscount = priceVoucher != null ? (totalProductPrice + shippingCost) - priceVoucher : (totalProductPrice + shippingCost) - 0;
            _dbContext.Update(objBill);
            _dbContext.SaveChanges();
            return Ok(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> CreateAddress()
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            using (ShopDBContext shopDBContext = new ShopDBContext())
            {
                var customer = shopDBContext.Customers.ToList();
                SelectList selectListCustomer = new SelectList(customer, "CumstomerID", "UserName");
                ViewBag.CustomerList = selectListCustomer;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAddress(Address address)
        {
            HttpClient httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Address/create-address?Street={address.Street}&Commune={address.Commune}&District={address.District}&Province={address.Province}&IsDefaultAddress={address.IsDefaultAddress}&ShippingCost={address.ShippingCost}&Status={address.Status}&DateCreated={DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}&CumstomerID={address.CumstomerID}";
            var response = await httpClient.PostAsync(apiUrl, null);
            return RedirectToAction("GetAllAddress");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateAddress(Guid id)
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            Address address = _repos.GetAll().FirstOrDefault(c => c.AddressID == id);
            using (ShopDBContext shopDBContext = new ShopDBContext())
            {
                var customer = shopDBContext.Customers.ToList();
                SelectList selectListCustomer = new SelectList(customer, "CumstomerID", "UserName");
                ViewBag.CustomerList = selectListCustomer;
            }
            return View(address);
        }
        public async Task<IActionResult> UpdateAddress(Address address)
        {
            HttpClient httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Address/update-address?AddressID={address.AddressID}&Street={address.Street}&Commune={address.Commune}&District={address.District}&Province={address.Province}&IsDefaultAddress={address.IsDefaultAddress}&ShippingCost={address.ShippingCost}&Status={address.Status}&DateCreated={DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}&CumstomerID={address.CumstomerID}";
            var response = await httpClient.PutAsync(apiUrl, null);
            return RedirectToAction("GetAllAddress");
        }
        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            var address = _repos.GetAll().FirstOrDefault(c => c.AddressID == id);
            HttpClient httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Address/delete-address?id={id}";
            var response = await httpClient.DeleteAsync(apiUrl);
            return RedirectToAction("GetAllAddress");
        }
    }
}
