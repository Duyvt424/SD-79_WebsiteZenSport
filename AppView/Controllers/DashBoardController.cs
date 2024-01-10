using System.Data.Entity.Core.Objects;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppView.Models;
using AppView.Models.DashBoardViewModel;
using AppView.Models.DetailsBillViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppView.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly IAllRepositories<Bill> _repos;
        private readonly IAllRepositories<Color> _colo;
        private readonly IAllRepositories<ShoesDetails> _Sho;

        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<Bill> _bill;
        private DbSet<Color> _color;
        private DbSet<ShoesDetails> shoes;
        public DashBoardController()
        {
            _bill = _dbContext.Bills;
            AllRepositories<Bill> all = new AllRepositories<Bill>(_dbContext, _bill);
            _repos = all;

            _color = _dbContext.Colors;
            AllRepositories<Color> all1 = new AllRepositories<Color>(_dbContext, _color);
            _colo = all1;

            shoes = _dbContext.ShoesDetails;
            AllRepositories<ShoesDetails> all2 = new AllRepositories<ShoesDetails>(_dbContext, shoes);
            _Sho = all2;
        }
        // aaaaa

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
        public IActionResult index()
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            return View();
        }


      
        [HttpGet]
        public IActionResult GetSummaryStatistics()
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                int currentMonth = currentDate.Month;

                var viewModel = new SummaryStatisticsViewModel();

                viewModel.MonthlyProfit = _dbContext.BillDetails
                    .Where(bd => bd.Bill.DeliveryDate.Month == currentMonth)
                    .Sum(bd => bd.Bill.TotalPriceAfterDiscount - (bd.Bill.Voucher != null ? bd.Bill.Voucher.VoucherValue : 0));

                DateTime today = DateTime.Today;

                viewModel.TodayRevenue = _dbContext.BillDetails
                    .Where(bd => bd.Bill.DeliveryDate.Date == today)
                    .Sum(bd => bd.Bill.TotalPriceAfterDiscount - (bd.Bill.Voucher != null ? bd.Bill.Voucher.VoucherValue : 0));

                viewModel.MonthlyQuantitySold = _dbContext.BillDetails
                    .Where(bd => bd.Bill.DeliveryDate.Month == currentMonth)
                    .Sum(bd => bd.Quantity);

                Console.WriteLine($"Monthly Profit: {viewModel.MonthlyProfit}");
                Console.WriteLine($"Today Revenue: {viewModel.TodayRevenue}");
                Console.WriteLine($"Monthly Quantity Sold: {viewModel.MonthlyQuantitySold}");

                return Json(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Json(new { ErrorMessage = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }
        [HttpGet]
        public IActionResult GetDailyRevenue(DateTime startDate, DateTime endDate)
        {
            try
            {
                List<DailyRevenueViewModel> dailyRevenueList = new List<DailyRevenueViewModel>();

                // Thực hiện logic lấy dữ liệu từ cơ sở dữ liệu
                var bills = _dbContext.Bills
                    .Include(b => b.Voucher)
                    .Where(b => b.DeliveryDate.Date >= startDate.Date && b.DeliveryDate.Date <= endDate.Date)
                    .ToList();

                foreach (DateTime date in EachDay(startDate, endDate))
                {
                    decimal totalRevenue = bills
                        .Where(b => b.DeliveryDate.Date == date.Date)
                        .Sum(b => b.TotalPriceAfterDiscount - (b.Voucher != null ? b.Voucher.VoucherValue : 0));

                    dailyRevenueList.Add(new DailyRevenueViewModel
                    {
                        Date = date,
                        TotalRevenue = totalRevenue
                    });
                }

                // Log doanh thu theo từng ngày
                foreach (var dailyRevenue in dailyRevenueList)
                {
                    Console.WriteLine($"Date: {dailyRevenue.Date}, Total Revenue: {dailyRevenue.TotalRevenue}");
                }

                // Trả về view hoặc partial view tùy thuộc vào nhu cầu của bạn
                return Json(dailyRevenueList);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                Console.WriteLine($"Lỗi xử lý dữ liệu: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


        private IEnumerable<DateTime> EachDay(DateTime startDate, DateTime endDate)
        {
            for (var day = startDate.Date; day.Date <= endDate.Date; day = day.AddDays(1))
            {
                yield return day;
            }
        }
      
        [HttpGet]
        public IActionResult tables()
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
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

        //[HttpGet]
        //public IActionResult TopSellingProducts()
        //{
        //    var colo = _colo.GetAll();
        //    var topSellingProducts = _dbContext.BillDetails
        //  .GroupBy(bd => bd.ShoesDetails_Size.ShoesDetailsId)
        //  .Select(grouped => new TopSellingProductViewModel
        //   {
        //    ShoesDetailsId = grouped.Key,
        //    ShoesDetailsCode = grouped.FirstOrDefault().ShoesDetails_Size.ShoesDetails.ShoesDetailsCode,
        //   TotalQuantitySold = grouped.Sum(bd => bd.Quantity),
        //   Price = grouped.FirstOrDefault().ShoesDetails_Size.ShoesDetails.Price,
        //   ImportPrice = grouped.FirstOrDefault().ShoesDetails_Size.ShoesDetails.ImportPrice,
        //    Description = grouped.FirstOrDefault().ShoesDetails_Size.ShoesDetails.Description,
        //     Status = grouped.FirstOrDefault().ShoesDetails_Size.ShoesDetails.Status,
        //  Image = grouped.FirstOrDefault().ShoesDetails_Size.ShoesDetails.ImageUrl,

        //  })
        //     .OrderByDescending(p => p.TotalQuantitySold)
        //     .Take(10)
        //     .ToList();

        //    return View(topSellingProducts);
        //}
        [HttpGet]

        public IActionResult TopSellingProducts()
        {
            var topSellingProducts = _dbContext.BillDetails
                .GroupBy(bd => bd.ShoesDetails_Size.ShoesDetailsId)
                .Select(grouped => new TopSellingProductViewModel
                {
                    ShoesDetailsId = grouped.Key,
                    ShoesDetailsCode = grouped.FirstOrDefault().ShoesDetails_Size.ShoesDetails.ShoesDetailsCode,
                    TotalQuantitySold = grouped.Sum(bd => bd.Quantity),
                    Price = grouped.FirstOrDefault().ShoesDetails_Size.ShoesDetails.Price,
                    ImportPrice = grouped.FirstOrDefault().ShoesDetails_Size.ShoesDetails.ImportPrice,
                    Description = grouped.FirstOrDefault().ShoesDetails_Size.ShoesDetails.Description,
                    Status = grouped.FirstOrDefault().ShoesDetails_Size.ShoesDetails.Status,
                    Image = grouped.FirstOrDefault().ShoesDetails_Size.ShoesDetails.ImageUrl,
                })
                .Join(_dbContext.ShoesDetails,
                    grouped => grouped.ShoesDetailsId,
                    shoes => shoes.ShoesDetailsId,
                    (grouped, shoes) => new
                    {
                        TopSellingProduct = grouped,
                        ShoesDetails = shoes
                    })
                .Join(_dbContext.Colors,
                    combined => combined.ShoesDetails.ColorID,
                    color => color.ColorID,
                    (combined, color) => new
                    {
                        combined.TopSellingProduct,
                        combined.ShoesDetails,
                        Color = color
                    })
                .Join(_dbContext.Products,
                    combined => combined.ShoesDetails.ProductID,
                    product => product.ProductID,
                    (combined, product) => new
                    {
                        combined.TopSellingProduct,
                        combined.ShoesDetails,
                        combined.Color,
                        Product = product
                    })
                .Join(_dbContext.Soles,
                    combined => combined.ShoesDetails.SoleID,
                    sole => sole.SoleID,
                    (combined, sole) => new
                    {
                        combined.TopSellingProduct,
                        combined.ShoesDetails,
                        combined.Color,
                        combined.Product,
                        Sole = sole
                    })
                .Join(_dbContext.Styles,
                    combined => combined.ShoesDetails.StyleID,
                    style => style.StyleID,
                    (combined, style) => new
                    {
                        combined.TopSellingProduct,
                        combined.ShoesDetails,
                        combined.Color,
                        combined.Product,
                        combined.Sole,
                        Style = style
                    })
                .Join(_dbContext.Sex,
                    combined => combined.ShoesDetails.SexID,
                    sex => sex.SexID,
                    (combined, sex) => new TopSellingProductViewModel
                    {
                        ShoesDetailsId = combined.TopSellingProduct.ShoesDetailsId,
                        ShoesDetailsCode = combined.TopSellingProduct.ShoesDetailsCode,
                        TotalQuantitySold = combined.TopSellingProduct.TotalQuantitySold,
                        Price = combined.TopSellingProduct.Price,
                        ImportPrice = combined.TopSellingProduct.ImportPrice,
                        Description = combined.TopSellingProduct.Description,
                        Status = combined.TopSellingProduct.Status,
                        Image = combined.TopSellingProduct.Image,
                        ColorName = combined.Color.Name,
                        ProductName = combined.Product.Name,
                        SoleName = combined.Sole.Name,
                        StyleName = combined.Style.Name,
                        SexName = sex.SexName
                        // Add other properties from TopSellingProductViewModel as needed
                    })
                .OrderByDescending(p => p.TotalQuantitySold)
                .Take(10)
                .ToList();

            return View(topSellingProducts);
        }


        [HttpGet]
        public IActionResult findTables(string billCode)
        {
            var objBill = _repos.GetAll().Where(c => c.BillCode.Contains(billCode)).ToList();
            var listBill = objBill.Select(c => new tablesViewModel
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
        public IActionResult searchByDate(DateTime startDate, DateTime endDate)
        {
            var objBill = _repos.GetAll().Where(c => c.CreateDate >= startDate && c.CreateDate <= endDate).ToList();
            var listBill = objBill.Select(c => new tablesViewModel
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
            return Json(listBill);
        }

        [HttpGet]
        public IActionResult Filter(int nameStatus, int priceNewOld, DateTime startDate, DateTime endDate)
        {
            var query = _repos.GetAll();
            if (nameStatus != 8)
            {
                query = query.Where(c => c.Status == nameStatus);
            }
            if (priceNewOld != 8)
            {
                switch (priceNewOld)
                {
                    case 0:
                        query = query.OrderByDescending(c => c.CreateDate);
                        break;
                    case 1:
                        query = query.OrderBy(c => c.CreateDate);
                        break;
                    case 2:
                        query = query.OrderByDescending(c => c.TotalPriceAfterDiscount);
                        break;
                    case 3:
                        query = query.OrderBy(c => c.TotalPriceAfterDiscount);
                        break;
                }
            }
            query = query.Where(c => c.CreateDate >= startDate && c.CreateDate <= endDate);
            var objBill = query.ToList();
            var listBill = objBill.Select(c => new tablesViewModel
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
            return Json(listBill);
        }


		[HttpGet]
        public IActionResult tables1()
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
