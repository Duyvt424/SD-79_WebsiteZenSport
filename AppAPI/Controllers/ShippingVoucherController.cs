using System.Data;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ShippingVoucherController
	{
		private readonly IAllRepositories<ShippingVoucher> repos;
		private ShopDBContext context = new ShopDBContext();
		private DbSet<ShippingVoucher> voucher;

		public ShippingVoucherController()
		{
			voucher = context.ShippingVoucher;
			AllRepositories<ShippingVoucher> all = new AllRepositories<ShippingVoucher>(context, voucher);
			repos = all;
		}

		[HttpGet("get-shippingVoucher")]
		public IEnumerable<ShippingVoucher> GetAll()
		{
			return repos.GetAll().Where(x => x.ExpirationDate > DateTime.Now && x.QuantityShip > 0);
		}

		[HttpGet("find-shippingVoucher")]
		public IEnumerable<ShippingVoucher> GetVoucher(string code)
		{
			var voucher = repos.GetAll().Where(x => x.VoucherShipCode == code);
			/*Voucher voucher1 = (Voucher)voucher;

			if (voucher1.DateCreated > DateTime.Now)
			{
				return BadRequestResult();
			}*/
			return voucher;
		}

		[HttpPost("create-shippingVoucher")]
		public bool CreateVoucher( DateTime expireDate, DateTime DateCreated, string? code, decimal? MaxShippingDiscount, decimal? ShippingDiscount, int QuantityShip, int IsShippingVoucher)
		{
			ShippingVoucher voucher = new ShippingVoucher();
			voucher.ShippingVoucherID = Guid.NewGuid();
			voucher.ExpirationDate= expireDate;
			voucher.DateCreated= DateCreated;
			voucher.VoucherShipCode= code;
			voucher.MaxShippingDiscount= MaxShippingDiscount;
			voucher.ShippingDiscount= ShippingDiscount;
			voucher.QuantityShip=QuantityShip;
			voucher.IsShippingVoucher= IsShippingVoucher;
			return repos.AddItem(voucher);
		}

		[HttpPut("update-shippingVoucher")]
		public bool UpdateVoucher(Guid id, DateTime expireDate, DateTime DateCreated, string? VoucherShipCode, decimal? MaxShippingDiscount, decimal? ShippingDiscount, int QuantityShip, int IsShippingVoucher)
		{
			ShippingVoucher voucher = repos.GetAll().First(x => x.ShippingVoucherID == id);
			
			voucher.ExpirationDate = expireDate;
			voucher.DateCreated = DateCreated;
			voucher.VoucherShipCode = VoucherShipCode;
			voucher.MaxShippingDiscount = MaxShippingDiscount;
			voucher.ShippingDiscount = ShippingDiscount;
			voucher.QuantityShip = QuantityShip;
			voucher.IsShippingVoucher = IsShippingVoucher;
			return repos.EditItem(voucher);
		}

		[HttpDelete("delete-voucher")]
		public bool DeleteVoucher(Guid id)
		{
			var ship = repos.GetAll().First(c => c.ShippingVoucherID == id);
			
			return repos.RemoveItem(ship);
		}
		//[HttpGet("update-quantity")]
		//public bool UpdateVoucherQuantity([FromQuery(Name = "voucherId")] Guid voucherId)
		//{
		//	try
		//	{
		//		// Lấy voucher từ cơ sở dữ liệu dựa trên voucherId
		//		var voucher = repos.GetAll().FirstOrDefault(c => c.VoucherID == voucherId);

		//		if (voucher != null && voucher.RemainingUsage > 0)
		//		{
		//			// Giảm số lượng voucher đi 1
		//			voucher.RemainingUsage -= 1;

		//			// Lưu thay đổi vào cơ sở dữ liệu
		//			bool updateResult = repos.EditItem(voucher);

		//			return updateResult;
		//		}
		//		else
		//		{
		//			// Trả về false nếu không tìm thấy voucher hoặc số lượng còn lại là 0
		//			return false;
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		// Xử lý ngoại lệ, log lỗi, và trả về false nếu có lỗi
		//		return false;
		//	}




	}
}