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
        public IActionResult DetailsBill()
        {
            return View();
        }
    }
}
