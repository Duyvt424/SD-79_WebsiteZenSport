using AppData.Models;
using AppView.IServices;

namespace AppView.Services
{
	public class BillService : IBillService
	{
		private readonly ShopDBContext dBContext;
		public BillService()
		{
			dBContext= new ShopDBContext();
		}
		public bool CreateBill(Bill i)
		{
			try
			{
				dBContext.Bills.Add(i);
				dBContext.SaveChanges();
				return true;

			}
			catch (Exception)
			{

				return false;
			}
		}

		public bool DeleteBill(Guid id)
		{
			try
			{
				var bill = dBContext.Bills.FirstOrDefault(p => p.BillID == id);
				if (bill != null)
				{
					dBContext.Bills.Remove(bill);
					dBContext.SaveChanges();
					return true;
				}
				else return false;
			}
			catch (Exception)
			{

				return false;
			}
		}

		public List<Bill> GetAllBills()
		{
			return dBContext.Bills.ToList();
		}

		public Bill GetBillById(Guid id)
		{

			return dBContext.Bills.FirstOrDefault(c => c.BillID == id);
		}

		public bool UpdateBill(Bill i)
		{
			try
			{
				var x = dBContext.Bills.FirstOrDefault(p => p.BillID== i.BillID);
				x.BillCode = i.BillCode;
				x.CreateDate = i.CreateDate;
				x.SuccessDate = i.SuccessDate;
				x.DeliveryDate= i.DeliveryDate;
				x.Status = i.Status;
				x.CancelDate= i.CancelDate;
				x.TotalPrice= i.TotalPrice;
				x.ShippingCosts=i.ShippingCosts;
				x.Note = i.Note;
				x.CouponID= i.CouponID;
				x.EmployeeID= i.EmployeeID;
				x.CustomerID = i.CustomerID;
				x.VoucherID= i.VoucherID;
				return true;
			}
			catch (Exception)
			{
               return false;
			}
		}
	}
}
