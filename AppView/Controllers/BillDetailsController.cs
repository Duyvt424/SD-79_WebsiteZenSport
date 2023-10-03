using System.Drawing.Drawing2D;
using AppData.IServices;
using AppData.Models;
using AppData.Services;
using AppView.IServices;
using AppView.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppView.Controllers
{
	public class BillDetailsController : Controller
	{
		private readonly ILogger<BillDetailsController> _logger;
		private readonly IShoesDetailsService shoesDetailsService;
		private readonly IBillDetailsService billService;
		private readonly IProductService productService;
		private readonly ISizeService sizeService;
		private readonly ShopDBContext dBContext;

		public BillDetailsController(ILogger<BillDetailsController> logger)
		{
			_logger= logger;
			billService = new BillDetailsService();
			shoesDetailsService = new ShoesDetailsService();
			productService = new ProductService();
			sizeService = new SizeService();
			dBContext = new ShopDBContext();

		}
		[HttpGet]
		public IActionResult BillDetails( Guid ID)
		{
			List<ShoesDetails> shoesDetails= new List<ShoesDetails>();
			shoesDetails = shoesDetailsService.GetAllShoesDetails();
			/*	using (ShopDBContext shopDBContext = new ShopDBContext())
				{
					var product = shopDBContext.Products.ToList();
					SelectList selectListProduct = new SelectList(product, "ProductID", "Name");
					ViewBag.ProductList = selectListProduct;

					var size = shopDBContext.Sizes.ToList();
					SelectList selectListSize = new SelectList(size, "SizeID", "Name");
					ViewBag.SizeList = selectListSize;

				}*/
			List<Product> products = new List<Product>();
			products = productService.GetAllProducts();
			List<Size> sizes = new List<Size>();
			sizes = sizeService.GetAllSizes();
			ViewData["Name"] = new SelectList(shoesDetails, "ShoesDetailsId", "ProductID");
			ViewData["Size"] = new SelectList(shoesDetails, "ShoesDetailsId", "SizeID");
			ViewData["Color"] = new SelectList(shoesDetails, "ShoesDetailsId", "ColorID");
			ViewData["Sole"] = new SelectList(shoesDetails, "ShoesDetailsId", "SoleID");
			ViewData["Style"] = new SelectList(shoesDetails, "ShoesDetailsId", "StyleID");
            ViewData["Price"] = new SelectList(shoesDetails, "ShoesDetailsId", "Price");
           
            ViewData["CancelDate"] = new SelectList(shoesDetails, "ShoesDetailsId", "CancelDate");
            var a = billService.GetBillDetailsByID(ID);
			List<BillDetails> list = new(billService.GetAllBillDetails().Where(c => c.BillID == ID));
		

			decimal tongtien = Convert.ToDecimal(list.Sum(c => c.Quantity * c.Price));
			ViewBag.tongtien = tongtien;

			var shoes = dBContext.ShoesDetails.Where(c => c.ShoesDetailsId == ID).FirstOrDefault();
           
           
            return View(a);
        }
	}
}
