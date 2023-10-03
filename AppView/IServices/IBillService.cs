using AppData.Models;

namespace AppView.IServices
{
	public interface IBillService
	{
		public bool CreateBill(Bill i);
		public bool UpdateBill(Bill i);
		public bool DeleteBill(Guid id);
		public List<Bill> GetAllBills();
		public Bill GetBillById(Guid id);
	}
}
