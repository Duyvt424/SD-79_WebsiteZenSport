using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillStatusHistoryController : ControllerBase
    {
        private readonly IAllRepositories<BillStatusHistory> _repos;
        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<BillStatusHistory> _billStatusHistories;
        public BillStatusHistoryController()
        {
            _billStatusHistories = _dbContext.BillStatusHistories;
            AllRepositories<BillStatusHistory> all = new AllRepositories<BillStatusHistory>(_dbContext, _billStatusHistories);
            _repos = all;
        }

        [HttpGet("get-billStatusHistory")]
        public IEnumerable<BillStatusHistory> GetBillStatusHistory()
        {
            return _repos.GetAll();
        }

        [HttpPost("add-billStatusHistory")]
        public string AddBillStatusHistory(int Status, DateTime ChangeDate, string Note, Guid BillID, Guid EmployeeID)
        {
            BillStatusHistory billStatusHistory = new BillStatusHistory();
            billStatusHistory.BillStatusHistoryID = Guid.NewGuid();
            billStatusHistory.Status = Status;
            billStatusHistory.ChangeDate = ChangeDate;
            billStatusHistory.Note = Note;
            billStatusHistory.BillID = BillID;
            billStatusHistory.EmployeeID = EmployeeID;
            if (_repos.AddItem(billStatusHistory))
            {
                return "Thêm thành công";
            }
            else
            {
                return "Thêm thất bại";
            }
        }

        [HttpPut("update-billStatusHistory")]
        public string UpdateBillStatusHistory(Guid BillStatusHistoryID, int Status, DateTime ChangeDate, string Note, Guid BillID, Guid EmployeeID)
        {
            var objBillStatusHistory = _dbContext.BillStatusHistories.First(c => c.BillStatusHistoryID == BillStatusHistoryID);
            if (objBillStatusHistory != null)
            {
                objBillStatusHistory.Status = Status;
                objBillStatusHistory.ChangeDate = ChangeDate;
                objBillStatusHistory.Note = Note;
                objBillStatusHistory.BillID = BillID;
                objBillStatusHistory.EmployeeID = EmployeeID;
                if (_repos.EditItem(objBillStatusHistory))
                {
                    return "Sửa thành công";
                }
                else
                {
                    return "Sửa thất bại";
                }
            }
            else
            {
                return "Not found object";
            }
        }

        [HttpDelete("delete-billStatusHistory")]
        public string Delete(Guid BillStatusHistoryID)
        {
            var obj = _dbContext.BillStatusHistories.FirstOrDefault(c => c.BillStatusHistoryID == BillStatusHistoryID);
            if (obj != null)
            {
                obj.Status = 1;
                if (_repos.EditItem(obj))
                {
                    return "Xóa thành công";
                }
                else
                {
                    return "Xóa thất bại";
                }
            }
            else
            {
                return "Not found object";
            }
        }
    }
}
