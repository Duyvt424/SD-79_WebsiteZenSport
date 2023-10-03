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
    public class EmployeeController : ControllerBase
    {
        private readonly IAllRepositories<Employee> _repos;
        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<Employee> _employee;
        public EmployeeController()
        {
            _employee = _dbContext.Employees;
            AllRepositories<Employee> all = new AllRepositories<Employee>(_dbContext, _employee);
            _repos = all;
        }
        // GET: api/<EmployeeController>
        [HttpGet("get-employee")]
        public IEnumerable<Employee> GetAll()
        {
            return _repos.GetAll();
        }

        //// GET api/<EmployeeController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<EmployeeController>
        [HttpPost("create-employee")]
        public string CreateEmployee(string FullName, string Password, string Email, int Sex, string PhoneNumber, int Status, Guid RoleID)
        {
            Employee employee = new Employee();
            employee.FullName = FullName;
            employee.Password = Password;
            employee.Email = Email;
            employee.Sex = Sex;
            employee.PhoneNumber = PhoneNumber;
            employee.Status = Status;
            employee.RoleID = RoleID;
            if (_repos.AddItem(employee))
            {
                return "Thêm thành công";
            }
            else
            {
                return "Error";
            }
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("update-employee")]
        public string UpdateEmployee(Guid id, string FullName, string Password, string Email, int Sex, string PhoneNumber, int Status, Guid RoleID)
        {
            var employ = _repos.GetAll().First(c => c.EmployeeID == id);
            employ.FullName = FullName;
            employ.Password = Password;
            employ.Email = Email;
            employ.Sex = Sex;
            employ.PhoneNumber = PhoneNumber;
            employ.Status = Status;
            employ.RoleID = RoleID;
            if (_repos.EditItem(employ))
            {
                return "Sửa thành công";
            }
            else
            {
                return "Sửa thất bại";
            }
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("delete-employee")]
        public string DeleteEmployee(Guid id)
        {
            var employ = _repos.GetAll().First(c => c.EmployeeID == id);
            if (_repos.RemoveItem(employ))
            {
                return "Xóa thành công";
            }
            return "Xóa thành công";
        }
    }
}
