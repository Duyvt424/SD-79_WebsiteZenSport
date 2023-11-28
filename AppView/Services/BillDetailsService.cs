using AppData.Models;
using AppView.IServices;

namespace AppView.Services
{
    public class BillDetailsService : IBillDetailsService
    {
        private readonly ShopDBContext context;
        public BillDetailsService()
        {
            context= new ShopDBContext();
        }
    
        public bool CreateBillDetail(BillDetails i)
        {
            try
            {
                context.BillDetails.Add(i);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteBillDetail(Guid id)
        {
            try
            {
                var bill = context.BillDetails.FirstOrDefault(p => p.ID == id);
                if (bill != null)
                {
                    context.BillDetails.Remove(bill);
                    context.SaveChanges();
                    return true;
                }
                else { return false; }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<BillDetails> GetAllBillDetails()
        {
            return context.BillDetails.ToList();
        }

        public BillDetails GetBillDetailById(Guid id)
        {
            return context.BillDetails.FirstOrDefault(c => c.ID== id);
        }

        public List<BillDetails> GetBillDetailsByID(Guid IDHD)
        {
            return context.BillDetails.Where(c => c.BillID == IDHD).ToList();
        }

        public bool UpdateBillDetail(BillDetails i)
        {
            try
            {
                var x = context.BillDetails.FirstOrDefault(p => p.ID == i.ID);
                x.ID = i.ID;
                x.Quantity = i.Quantity;
                x.Price= i.Price;
                x.Status= i.Status;
                x.ShoesDetails_SizeID= i.ShoesDetails_SizeID;
                x.BillID= i.BillID;
                context.BillDetails.Update(x);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
