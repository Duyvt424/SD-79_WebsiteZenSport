using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppView.Models.DashBoardViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppView.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly IAllRepositories<Bill> _repos;
        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<Bill> _bill;
        public DashBoardController()
        {
            _bill = _dbContext.Bills;
            AllRepositories<Bill> all = new AllRepositories<Bill>(_dbContext, _bill);
            _repos = all;
        }
        public IActionResult index()
        {
            return View();
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
    }
}
