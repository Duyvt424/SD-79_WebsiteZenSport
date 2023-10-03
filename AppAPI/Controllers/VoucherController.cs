using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
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
			return repos.GetAll();
		}

		[HttpGet("find-voucher")]
		public IEnumerable<Voucher> GetVoucher(string code)
		{
			return repos.GetAll().Where(x => x.VoucherCode == code);
		}

		[HttpPost("create-voucher")]
		public bool CreateVoucher(string code, int status, decimal value, int maxUse, int remainUse, DateTime expireDate)
		{
			Voucher voucher = new Voucher();
			voucher.VoucherID = Guid.NewGuid();
			voucher.VoucherCode = code;
			voucher.Status = status;
			voucher.VoucherValue = value;
			voucher.MaxUsage = maxUse;
			voucher.RemainingUsage = remainUse;
			voucher.ExpirationDate = expireDate;
			return repos.AddItem(voucher);
		}

		[HttpPut("update-voucher")]
		public bool UpdateVoucher(Guid id, string code, int status, decimal value, int maxUse, int remainUse, DateTime dateTime)
		{
			Voucher voucher = repos.GetAll().First(x => x.VoucherID == id);
			voucher.VoucherCode = code;
			voucher.Status = status;
			voucher.VoucherValue = value;
			voucher.MaxUsage = maxUse;
			voucher.RemainingUsage = remainUse;
			voucher.ExpirationDate = dateTime;
			return repos.EditItem(voucher);
		}

		[HttpDelete("delete-voucher")]
		public bool DeleteVoucher(Guid id)
		{
			Voucher sp = repos.GetAll().First(x => x.VoucherID == id);
			return repos.RemoveItem(sp);
		}
	}
}