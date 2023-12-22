using System.Data;
using System.Web.Http.Results;
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
	public class VoucherController
	{
		private readonly IAllRepositories<Voucher> repos;
		private ShopDBContext context = new ShopDBContext();
		private DbSet<Voucher> voucher;

		public VoucherController()
		{
			voucher = context.Vouchers;
			AllRepositories<Voucher> all = new AllRepositories<Voucher>(context, voucher);
			repos = all;
		}

		[HttpGet("get-voucher")]
		public IEnumerable<Voucher> GetAll()
		{
			return repos.GetAll().Where(x => x.ExpirationDate > x.DateCreated && x.RemainingUsage > 0);
		}

		[HttpGet("find-voucher")]
		public IEnumerable<Voucher> GetVoucher(string code)
		{
			var voucher = repos.GetAll().Where(x => x.VoucherCode == code);
			/*Voucher voucher1 = (Voucher)voucher;

			if (voucher1.DateCreated > DateTime.Now)
			{
				return BadRequestResult();
			}*/
			return voucher;
		}

		[HttpPost("create-voucher")]
		public bool CreateVoucher(string code, int status, decimal value, int maxUse, int remainUse, DateTime expireDate, DateTime DateCreated, decimal Total, string Exclusiveright, bool IsDel, DateTime CreateDate, int? Type)
		{
			Voucher voucher = new Voucher();
			voucher.VoucherID = Guid.NewGuid();
			voucher.VoucherCode = code;
			voucher.Status = status;
			voucher.VoucherValue = value;
			voucher.MaxUsage = maxUse;
			voucher.RemainingUsage = remainUse;
			voucher.ExpirationDate = expireDate;
			voucher.DateCreated = DateCreated;
			voucher.Total = Total;
			voucher.Exclusiveright = Exclusiveright;
			voucher.IsDel = IsDel;
			voucher.CreateDate = DateTime.Now;
			voucher.Type = Type;
			return repos.AddItem(voucher);
		}

		[HttpPut("update-voucher")]
		public bool UpdateVoucher(Guid id, string code, int status, decimal value, int maxUse, int remainUse, DateTime dateTime, DateTime DateCreated, decimal Total, string Exclusiveright, bool IsDel, DateTime CreateDate, int? Type)
		{
			Voucher voucher = repos.GetAll().First(x => x.VoucherID == id);
			voucher.VoucherCode = code;
			voucher.Status = status;
			voucher.VoucherValue = value;
			voucher.MaxUsage = maxUse;
			voucher.RemainingUsage = remainUse;
			voucher.ExpirationDate = dateTime;
			voucher.DateCreated = DateCreated;
			voucher.Total = Total;
			voucher.Exclusiveright = Exclusiveright;
			voucher.IsDel = IsDel;
			voucher.CreateDate = DateTime.Now;
			voucher.Type = Type;
			return repos.EditItem(voucher);
		}

		[HttpDelete("delete-voucher")]
		public bool DeleteVoucher(Guid id)
		{
			var role = repos.GetAll().First(c => c.VoucherID == id);
			
			return repos.RemoveItem(role);
		}

		[HttpGet("update-quantity")]
		public bool UpdateVoucherQuantity([FromQuery(Name = "voucherId")] Guid voucherId)
		{
			try
			{
				// Lấy voucher từ cơ sở dữ liệu dựa trên voucherId
				var voucher = repos.GetAll().FirstOrDefault(c => c.VoucherID == voucherId);

				if (voucher != null && voucher.RemainingUsage > 0)
				{
					// Kiểm tra nếu MaxUsage đã đạt đến RemainingUsage
					if (voucher.MaxUsage < voucher.RemainingUsage)
					{
						// Tăng số lượng voucher đi 1
						voucher.MaxUsage += 1;

						// Lưu thay đổi vào cơ sở dữ liệu
						bool updateResult = repos.EditItem(voucher);

						return updateResult;
					}
					else
					{
						// Trả về false nếu MaxUsage đã đạt đến RemainingUsage
						return false;
					}
				}
				else
				{
					// Trả về false nếu không tìm thấy voucher hoặc số lượng còn lại là 0
					return false;
				}
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ, log lỗi, và trả về false nếu có lỗi
				return false;
			}
		}

	}
}