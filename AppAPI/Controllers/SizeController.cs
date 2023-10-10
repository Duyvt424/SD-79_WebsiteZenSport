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
        [HttpGet("get-size")]
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
        [HttpPost("create-size")]
        public bool CreateSize(string SizeCode, string Name, int Status, DateTime DateCreated)
        {
            Size sz = new Size();
            sz.SizeCode = SizeCode;
            sz.Name = Name;
            sz.Status = Status;
            sz.DateCreated = DateCreated;
            sz.SizeID = Guid.NewGuid();
            return repos.AddItem(sz);
        }

        // PUT api/<SizeController>/5
        [HttpPut("update-size")]
        public bool Put(Guid sizeID, string SizeCode, string Name, int Status, DateTime DateCreated)
        {
            var sz = repos.GetAll().First(p => p.SizeID == sizeID);
            sz.SizeCode = SizeCode;
            sz.Name = Name;
            sz.Status = Status;
            sz.DateCreated = DateCreated;
            return repos.EditItem(sz);
        }

        // DELETE api/<SizeController>/5
        [HttpDelete("delete-size")]
        public bool Delete(Guid sizeID)
        {
            var sz = repos.GetAll().First(p => p.SizeID == sizeID);
            sz.Status = 1;
            return repos.EditItem(sz);
        }
    }
}
