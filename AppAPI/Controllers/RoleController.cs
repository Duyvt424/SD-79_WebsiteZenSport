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
    public class RoleController : ControllerBase
    {
		private readonly IAllRepositories<Role> _repos;
		private ShopDBContext _dbContext = new ShopDBContext();
		private DbSet<Role> _role;
		// GET: api/<ColorController1>

		public RoleController()
		{
			_role = _dbContext.Roles;
			AllRepositories<Role> all = new AllRepositories<Role>(_dbContext, _role);
			_repos = all;
		}
		[HttpGet("get-role")]

		public IEnumerable<Role> GetAll()
		{
			return _repos.GetAll();
		}

		// GET api/<ColorController1>/5
		[HttpGet("find-role")]
		public IEnumerable<Role> GetAll(string name)
		{
			return _repos.GetAll().Where(c => c.RoleName.ToLower().Contains(name.ToLower())).ToList();
		}

		// POST api/<ColorController1>
		[HttpPost("create-role")]
		public bool CreateColor(string RoleCode, string RoleName, int Status, DateTime DateCreated)
		{
			Role role = new Role();
			role.RoleCode = RoleCode;
			role.RoleName = RoleName;
			role.Status = Status;
			role.DateCreated = DateCreated;
			role.RoleID = Guid.NewGuid();
			return _repos.AddItem(role);
		}

		// PUT api/<ColorController1>/5
		[HttpPut("update-role")]
		public bool Put(string RoleCode, string RoleName, int Status, DateTime DateCreated, Guid RoleID)
		{
			var role = _repos.GetAll().FirstOrDefault(c => c.RoleID == RoleID);
			role.RoleCode = RoleCode;
			role.RoleName = RoleName;
			role.Status = Status;
			role.DateCreated = DateCreated;
			return _repos.EditItem(role);

		}


		// DELETE api/<ColorController1>/5
		[HttpDelete("delete-role")]
		public bool Delete(Guid id)
		{
			var role = _repos.GetAll().First(c => c.RoleID == id);
			role.Status = 1;
			return _repos.EditItem(role);
		}

	}
}
