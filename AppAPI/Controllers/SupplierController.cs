using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SupplierController
	{
		private readonly IAllRepositories<Supplier> repos;
		private ShopDBContext context = new ShopDBContext();
		private DbSet<Supplier> supplier;
		public SupplierController()
		{
			supplier = context.Suppliers;
			AllRepositories<Supplier> all = new AllRepositories<Supplier>(context, supplier);
			repos = all;
		}

		[HttpGet("get-supplier")]
		public IEnumerable<Supplier> GetAll()
		{
			return repos.GetAll();
		}

		[HttpGet("find-supplier")]
		public IEnumerable<Supplier> GetSupplier(string name)
		{
			return repos.GetAll().Where(x => x.Name == name);
		}

		[HttpPost("create-supplier")]
		public bool CreateSupplier(string name, int status)
		{
			Supplier supplier = new Supplier();
			supplier.SupplierID = Guid.NewGuid();
			supplier.Name = name;
			supplier.Status = status;
			return repos.AddItem(supplier);
		}

		[HttpPut("update-supplier")]
		public bool UpdateSupplier(Guid id, string name, int status)
		{
			Supplier supplier = repos.GetAll().First(x => x.SupplierID == id);
			supplier.Name = name;
			supplier.Status = status;
			return repos.EditItem(supplier);
		}

		[HttpDelete("delete-supplier")]
		public bool DeleteSupplier(Guid id)
		{
			Supplier supplier = repos.GetAll().First(x => x.SupplierID == id);
			return repos.RemoveItem(supplier);
		}
	}
}
