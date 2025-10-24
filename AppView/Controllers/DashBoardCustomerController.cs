using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppView.Models.DashBoardViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var objBill = _dbContext.BillDetails.Include(c => c.Bill).Include(c => c.ShoesDetails_Size).ThenInclude(sds => sds.ShoesDetails).ThenInclude(sd => sd.Product)
                .Where(c => c.Bill.BillCode.ToLower().Contains(billCode) || c.ShoesDetails_Size.ShoesDetails.Product.Name.ToLower().Contains(billCode)).Select(c => c.BillID).ToList();
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
            var objBill = _dbContext.BillDetails.Include(c => c.Bill).Include(c => c.ShoesDetails_Size).ThenInclude(c => c.ShoesDetails).ThenInclude(c => c.Product).ToList();
            if (nameStatus != 8)
            {
                objBill = _dbContext.BillDetails.Where(c => c.Bill.Status == nameStatus).ToList();
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
            var listBill = _dbContext.BillDetails.Select(c => new tablesViewModel
            {
                BillID = c.BillID,
                CustomerID = c.Bill.CustomerID,
                TotalShoes = c.Quantity,
                Price = c.Price * c.Quantity,
                TotalPrice = _dbContext.Bills.FirstOrDefault(b => b.BillID == c.BillID).TotalPriceAfterDiscount,
                NameProduct = _dbContext.Products.FirstOrDefault(p => p.ProductID == c.ShoesDetails_Size.ShoesDetails.ProductID).Name,
                Description = _dbContext.ShoesDetails.FirstOrDefault(p => p.ShoesDetailsId == c.ShoesDetails_Size.ShoesDetailsId).Description,
                ImageUrl = _dbContext.Images.FirstOrDefault(i => i.ShoesDetailsID == c.ShoesDetails_Size.ShoesDetailsId).Image1,
                CreateDate = _dbContext.Bills.FirstOrDefault(z => z.BillID == c.BillID).CreateDate,
                PurchasePayMent = _dbContext.PurchaseMethods.First(z => z.PurchaseMethodID == c.Bill.PurchaseMethodID).MethodName,
                Status = c.Status,
                SizeName = _dbContext.Sizes.FirstOrDefault(s => s.SizeID == c.ShoesDetails_Size.SizeID).Name,
            }).ToList();
            return View(listBill);
        }
    }
}
