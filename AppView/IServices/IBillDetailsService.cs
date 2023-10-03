using AppData.Models;

namespace AppView.IServices
{
    public interface IBillDetailsService
    {
        public bool CreateBillDetail(BillDetails i);
        public bool UpdateBillDetail(BillDetails i);
        public bool DeleteBillDetail(Guid id);
        public List<BillDetails> GetAllBillDetails();
        public List<BillDetails> GetBillDetailsByID(Guid IDHD);
        public BillDetails GetBillDetailById(Guid id);
    }
}
