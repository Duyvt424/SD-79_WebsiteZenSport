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
        private readonly IAllRepositories<Role> repos;
        ShopDBContext context = new ShopDBContext();
        DbSet<Role> role;

        public RoleController()
        {
            role = context.Roles;
            AllRepositories<Role> all = new AllRepositories<Role>(context, role);
            repos = all;
        }

        // GET: api/<RoleController>
        [HttpGet]
        public IEnumerable<Role> Get()
        {
            return repos.GetAll();
        }

        // GET api/<RoleController>/5
        [HttpGet("{name}")]
        public IEnumerable<Role> Get(string name)
        {
            return repos.GetAll().Where(p => p.RoleName.Contains(name));
        }

        // POST api/<RoleController>
        [HttpPost("create-role")]
        public bool CreateRole( string name, int status)
        {
            Role r = new Role();
            r.RoleName = name;
            r.Status = status;
            r.RoleID = Guid.NewGuid();
            return repos.AddItem(r);
        }

        // PUT api/<RoleController>/5
        [HttpPut("update-role")]
        public bool Put(Guid id, string name, int status)
        {
            var r = repos.GetAll().First(p => p.RoleID == id);
            r.RoleName = name; r.Status = status;
            return repos.EditItem(r);
        }

        // DELETE api/<RoleController>/5
        [HttpDelete("delete-role")]
        public string Deletes(Guid id)
        {
            var r = repos.GetAll().First(p => p.RoleID == id);
            if (repos.RemoveItem(r))
            {
                return "Thêm thành công";
            }
            else
            {
                return "Thêm thất bại";
            }
        }
    }
}
