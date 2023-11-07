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
		private DbSet<Employee> _employees;
		// GET: api/<ColorController1>

		public EmployeeController()
		{
			_employees = _dbContext.Employees;
			AllRepositories<Employee> all = new AllRepositories<Employee>(_dbContext, _employees);
			_repos = all;
		}

		[HttpGet("get-employee")]

		public IEnumerable<Employee> GetAll()
		{
			return _repos.GetAll();
		}

		// GET api/<ColorController1>/5
		[HttpGet("find-employee")]
		public IEnumerable<Employee> GetAll(string name)
		{
			return _repos.GetAll().Where(c => c.FullName.ToLower().Contains(name.ToLower())).ToList();
		}

		// POST api/<ColorController1>
		[HttpPost("create-employee")]
		public bool CreateEmployee(string FullName, string UserName, string Password, string Email, int Sex, string ResetPassword,
			 string PhoneNumber, int Status, DateTime DateCreated, Guid RoleID, string Image, string IdentificationCode, string Address)
		{
			Employee emp = new Employee();
			emp.FullName = FullName;
			emp.UserName = UserName;
			emp.Password = Password;
			emp.Email = Email;
			emp.Sex = Sex;
			emp.ResetPassword = ResetPassword;
			emp.PhoneNumber = PhoneNumber;
			emp.Status = Status;
			emp.DateCreated = DateCreated;
			emp.RoleID = RoleID;
			emp.Image = Image;
			emp.IdentificationCode = IdentificationCode;
			emp.Address = Address;
			emp.EmployeeID = Guid.NewGuid();
			return _repos.AddItem(emp);
		}


		// PUT api/<ColorController1>/5
		[HttpPut("update-employee")]
		public bool Put(string FullName, string UserName, string Password, string Email, int Sex, string ResetPassword,
			 string PhoneNumber, int Status, DateTime DateCreated, Guid EmployeeID, Guid RoleID, string Image, string IdentificationCode, string Address)
		{
			var emp = _repos.GetAll().FirstOrDefault(c => c.EmployeeID == EmployeeID);
			emp.FullName = FullName;
			emp.UserName = UserName;
			emp.Password = Password;
			emp.Email = Email;
			emp.Sex = Sex;
			emp.ResetPassword = ResetPassword;
			emp.PhoneNumber = PhoneNumber;
			emp.Status = Status;
			emp.DateCreated = DateCreated;
			emp.RoleID = RoleID;
            emp.Image = Image;
            emp.IdentificationCode = IdentificationCode;
            emp.Address = Address;
            return _repos.EditItem(emp);

		}


		// DELETE api/<ColorController1>/5
		[HttpDelete("delete-employee")]
		public bool Delete(Guid id)
		{
			var role = _repos.GetAll().First(c => c.EmployeeID == id);
			role.Status = 1;
			return _repos.EditItem(role);
		}
	}
}
