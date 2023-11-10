using AppData.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductAPIController : ControllerBase
	{
		ShopDBContext db = new ShopDBContext();


		// GET: api/<ProductAPIController>
		[HttpGet]
		public IEnumerable<ShoesDetails> GetAllShoesDetails()
		{
			var sp = (from p in db.ShoesDetails
					  select new ShoesDetails { 
					  Price= p.Price,
					  ColorID= p.ColorID,
					  ProductID= p.ProductID,
					  SoleID= p.SoleID,
					  StyleID= p.StyleID,
					  
					  }).ToList();

			return sp;

		}

		[HttpGet("{Style}")]
		public IEnumerable<ShoesDetails> GetStyleVyCategory(Guid style)
		{
			var sp = (from p in db.ShoesDetails
					  where p.StyleID == style
					  select new ShoesDetails
					  {
						  Price = p.Price,
						  ColorID = p.ColorID,
						  ProductID = p.ProductID,
						  SoleID = p.SoleID,
						  StyleID = p.StyleID,

					  }).ToList();

			return sp;

		}

		[HttpGet("Price")]
		public IEnumerable<ShoesDetails> GetPriceVyCategory(decimal price)
		{
			var sp = (from p in db.ShoesDetails
					  where p.Price == price
					  select new ShoesDetails
					  {
						  Price = p.Price,
						  ColorID = p.ColorID,
						  ProductID = p.ProductID,
						  SoleID = p.SoleID,
						  StyleID = p.StyleID,

					  }).ToList();

			return sp;

		}
		[HttpGet("Sole")]
		public IEnumerable<ShoesDetails> GetSoleVyCategory(Guid sole)
		{
			var sp = (from p in db.ShoesDetails
					  where p.SoleID == sole
					  select new ShoesDetails
					  {
						  Price = p.Price,
						  ColorID = p.ColorID,
						  ProductID = p.ProductID,
						  SoleID = p.SoleID,
						  StyleID = p.StyleID,

					  }).ToList();

			return sp;

		}

		[HttpGet("Color")]
		public IEnumerable<ShoesDetails> GetColorVyCategory(Guid color)
		{
			var sp = (from p in db.ShoesDetails
					  where p.ColorID == color
					  select new ShoesDetails
					  {
						  Price = p.Price,
						  ColorID = p.ColorID,
						  ProductID = p.ProductID,
						  SoleID = p.SoleID,
						  StyleID = p.StyleID,

					  }).ToList();

			return sp;

		}

	}
}
