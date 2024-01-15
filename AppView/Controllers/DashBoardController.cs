//using System.Data.Entity.Core.Objects;
using System.Drawing;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppView.Models;
using AppView.Models.DashBoardViewModel;
using AppView.Models.DetailsBillViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

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



		//[HttpGet]
		//public IActionResult GetSummaryStatistics()
		//{
		//	try
		//	{
		//		DateTime currentDate = DateTime.Now;
		//		int currentMonth = currentDate.Month;

		//		var viewModel = new SummaryStatisticsViewModel();

		//		viewModel.MonthlyProfit = _dbContext.Bills
		//			.Where(bd => bd.DeliveryDate.Month == currentMonth)
		//			.Sum(bd => bd.TotalPriceAfterDiscount /*- (bd.Voucher != null ? bd.Voucher.VoucherValue : 0)*/);

		//		DateTime today = DateTime.Today;

		//		viewModel.TodayRevenue = _dbContext.Bills
		//			.Where(bd => bd.DeliveryDate.Date == today)
		//			.Sum(bd => bd.TotalPriceAfterDiscount/* - (bd.Voucher != null ? bd.Voucher.VoucherValue : 0)*/);

		//		viewModel.MonthlyQuantitySold = _dbContext.BillDetails
		//			.Where(bd => bd.Bill.DeliveryDate.Month == currentMonth)
		//			.Sum(bd => bd.Quantity);

		//		Console.WriteLine($"Monthly Profit: {viewModel.MonthlyProfit}");
		//		Console.WriteLine($"Today Revenue: {viewModel.TodayRevenue}");
		//		Console.WriteLine($"Monthly Quantity Sold: {viewModel.MonthlyQuantitySold}");

		//		return Json(viewModel);
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine($"Error: {ex.Message}");
		//		return Json(new { ErrorMessage = $"Có lỗi xảy ra: {ex.Message}" });
		//	}
		//}
		[HttpGet]
		public IActionResult GetSummaryStatistics()
		{
			try
			{
				DateTime currentDate = DateTime.Now;
				int currentMonth = currentDate.Month;

				var viewModel = new SummaryStatisticsViewModel();

				viewModel.MonthlyProfit = _dbContext.Bills
					.Where(bd => bd.DeliveryDate.Month == currentMonth)
					.Sum(bd => bd.TotalPriceAfterDiscount /*- (bd.Voucher != null ? bd.Voucher.VoucherValue : 0)*/);

				DateTime today = DateTime.Today;

				viewModel.TodayRevenue = _dbContext.Bills
					.Where(bd => bd.DeliveryDate.Date == today)
					.Sum(bd => bd.TotalPriceAfterDiscount/* - (bd.Voucher != null ? bd.Voucher.VoucherValue : 0)*/);

				viewModel.MonthlyQuantitySold = _dbContext.BillDetails
					.Where(bd => bd.Bill.DeliveryDate.Month == currentMonth)
					.Sum(bd => bd.Quantity);

				// Tính số lượng đơn hàng tháng này
				viewModel.MonthlyOrderCount = _dbContext.Bills
					.Where(bd => bd.DeliveryDate.Month == currentMonth)
					.Count();

				// Tính số lượng đơn hàng hôm nay
				viewModel.TodayOrderCount = _dbContext.Bills
					.Where(bd => bd.DeliveryDate.Date == today)
					.Count();

				Console.WriteLine($"Monthly Profit: {viewModel.MonthlyProfit}");
				Console.WriteLine($"Today Revenue: {viewModel.TodayRevenue}");
				Console.WriteLine($"Monthly Quantity Sold: {viewModel.MonthlyQuantitySold}");
				Console.WriteLine($"Monthly Order Count: {viewModel.MonthlyOrderCount}");
				Console.WriteLine($"Today Order Count: {viewModel.TodayOrderCount}");

				return Json(viewModel);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
				return Json(new { ErrorMessage = $"Có lỗi xảy ra: {ex.Message}" });
			}
		}


		[HttpGet]
        public IActionResult Top5Provinces()
        {
            var top5Provinces = _dbContext.Bills
                .Include(b => b.Address)
                .GroupBy(b => b.Address.Province)
                .Select(group => new Top5ProvincesViewModel
                {
                    Province = group.Key,
                    TotalOrders = group.Count()
                })
                .OrderByDescending(result => result.TotalOrders)
                .Take(5)
                .ToList();

            // Chuyển dữ liệu top5Provinces đến view
            return View(top5Provinces);
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
                        .Sum(b => b.TotalPriceAfterDiscount /*- (b.Voucher != null ? b.Voucher.VoucherValue : 0)*/);

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
		public IActionResult TopVouchers()
		{
			try
			{
				var topVouchers = _dbContext.Bills
					.Where(b => b.VoucherID != null) // Lọc ra các hóa đơn có sử dụng voucher
					.GroupBy(b => b.VoucherID) // Nhóm theo VoucherID
					.Select(group => new TopVoucherStatisticsViewModel
					{
						VoucherID = group.Key.Value,
						VoucherCode = group.First().Voucher.VoucherCode,
						TotalUsage = group.Count(),
                        VoucherValue = group.First().Voucher.VoucherValue
					})
					.OrderByDescending(result => result.TotalUsage)
					.Take(5)
					.ToList();

				return View(topVouchers);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
				return View("Error", new { ErrorMessage = $"Có lỗi xảy ra: {ex.Message}" });
			}
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

        public IActionResult settings(Guid employeeId)
        {
            Employee employee = _dbContext.Employees.FirstOrDefault(c => c.EmployeeID == employeeId);
            using (ShopDBContext dBContext = new ShopDBContext())
            {
                var role = dBContext.Roles.Where(c => c.Status == 0).ToList();
                SelectList selectListRole = new SelectList(role, "RoleID", "RoleName");
                ViewBag.RoleList = selectListRole;
            }
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> settings(Employee employee, [Bind(Prefix = "image")] IFormFile image)
        {
            string imageName;
            if (image != null && image.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", image.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    image.CopyTo(stream);
                }
                imageName = image.FileName;
            }
            else
            {
                imageName = _dbContext.Employees.FirstOrDefault(c => c.EmployeeID == employee.EmployeeID).Image;
            }
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Employee/update-employee?FullName={employee.FullName}&UserName={employee.UserName}&Password={employee.Password}&Email={employee.Email}&Sex={employee.Sex}&ResetPassword={0000}&PhoneNumber={employee.PhoneNumber}&Status={0}&DateCreated={DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}&EmployeeID={employee.EmployeeID}&RoleID={employee.RoleID}&Image={imageName}&IdentificationCode={employee.IdentificationCode}&Address={employee.Address}";
            var response = await httpClient.PutAsync(apiUrl, null);
            return RedirectToAction("listEmployee");
        }

        public IActionResult listEmployee()
        {
            var listEmployee = _dbContext.Employees.Select(c => new employeeViewModel
            {
                EmployeeId = c.EmployeeID,
                Image = c.Image,
                UserName = c.UserName,
                Email = c.Email,
                Fullname = c.FullName,
                CreateDate = c.DateCreated,
                Status = c.Status,
            }).ToList();
            return View(listEmployee);
        }

        public IActionResult findEmployee(string username)
        {
            var objEmployee = _dbContext.Employees.Where(c => c.UserName.ToLower().Contains(username.ToLower()) || c.FullName.ToLower().Contains(username.ToLower())).ToList();
            var listEmployee = objEmployee.Select(c => new employeeViewModel
            {
                EmployeeId = c.EmployeeID,
                Image = c.Image,
                UserName = c.UserName,
                Email = c.Email,
                Fullname = c.FullName,
                CreateDate = c.DateCreated,
                Status = c.Status,
            }).ToList();
            return View(listEmployee);
        }

        [HttpGet]
        public IActionResult FilterEmployee(int nameStatus, int priceNewOld)
        {
            var query = _dbContext.Employees.ToList();
            if (nameStatus != 8)
            {
                query = query.Where(c => c.Status == nameStatus).ToList();
            }
            if (priceNewOld != 8)
            {
                switch (priceNewOld)
                {
                    case 0:
                        query = query.Where(c => c.Sex == 0).ToList();
                        break;
                    case 1:
                        query = query.Where(c => c.Sex == 1).ToList();
                        break;
                    case 2:
                        query = query.Where(c => c.Sex == 2).ToList();
                        break;
                }
            }
            var objEmployee = query.ToList();
            var listEmployee = objEmployee.Select(c => new employeeViewModel
            {
                EmployeeId = c.EmployeeID,
                Image = c.Image,
                UserName = c.UserName,
                Email = c.Email,
                Fullname = c.FullName,
                CreateDate = c.DateCreated,
                Status = c.Status,
            }).ToList();
            return Json(listEmployee);
        }
    }
}
