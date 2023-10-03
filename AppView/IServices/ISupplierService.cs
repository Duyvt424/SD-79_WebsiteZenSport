using AppData.Models;

namespace AppView.IServices
{
	public interface ISupplierService
	{
		public bool AddSupplier(Supplier supplierName);
		public bool RemoveSupplier(Supplier supplierName);
		public bool UpdateSupplier (Supplier supplierName);
		public List<Supplier> GetAllSuppliers();
		public Supplier GetSupplierById(Guid id);
	}
}
