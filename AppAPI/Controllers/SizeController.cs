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
    public class SizeController : ControllerBase
    {
        private readonly IAllRepositories<Size> repos;
        ShopDBContext context = new ShopDBContext();
        DbSet<Size> _size;
        public SizeController()
        {
            _size = context.Sizes;
            AllRepositories<Size> all = new AllRepositories<Size>(context, _size);
            repos = all;
        }
        // GET: api/<SizeController>
        [HttpGet("Get-Size")]
        public IEnumerable<Size> Get()
        {
           return repos.GetAll();
        }

        // GET api/<SizeController>/5
        [HttpGet("{name}")]
        public IEnumerable<Size> Get(string name)
        {
            return repos.GetAll().Where(p => p.Name.Contains(name));
        }

        // POST api/<SizeController>
        [HttpPost("Create-size")]
        public bool CreateSize(string name, int status )
        {
            Size sz = new Size();
            sz.Name = name;
            sz.Status = status;
            sz.SizeID = Guid.NewGuid();
            return repos.AddItem(sz);
        }

        // PUT api/<SizeController>/5
        [HttpPut("Update-Size")]
        public bool Put(Guid sizeID, string name, int status)
        {
            var sz = repos.GetAll().First(p => p.SizeID == sizeID);
            sz.Name = name;
            sz.Status = status;
            return repos.EditItem(sz);
        }

        // DELETE api/<SizeController>/5
        [HttpDelete("Delete-Size")]
        public bool Delete(Guid sizeID)
        {
            var sz = repos.GetAll().First(p => p.SizeID == sizeID);
            return repos.RemoveItem(sz);
        }
    }
}
