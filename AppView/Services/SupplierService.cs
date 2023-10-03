using AppData.Models;
using AppView.IServices;

namespace AppView.Services
{
	public class SupplierService : ISupplierService
	{
		ShopDBContext _dbContext;
		public SupplierService()
		{
			_dbContext = new ShopDBContext();
		}
		public bool AddSupplier(Supplier supplierName)
		{
			try
			{
				_dbContext.Add(supplierName);
				_dbContext.SaveChanges();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public List<Supplier> GetAllSuppliers()
		{
			return _dbContext.Suppliers.ToList();
		}

		public Supplier GetSupplierById(Guid id)
		{
			return _dbContext.Suppliers.First(x => x.SupplierID == id);
		}

		public bool RemoveSupplier(Supplier supplierName)
		{
			try
			{
				_dbContext.Remove(supplierName);
				_dbContext.SaveChanges();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool UpdateSupplier(Supplier supplierName)
		{
			try
			{
				_dbContext.Update(supplierName);
				_dbContext.SaveChanges();
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
