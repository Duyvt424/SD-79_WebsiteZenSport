using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppView.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoleController : ControllerBase
    {
        private readonly IAllRepositories<Sole> repos;
        ShopDBContext context = new ShopDBContext();
        DbSet<Sole> sole;
        public SoleController()
        {
            sole = context.Soles;
            AllRepositories<Sole> all = new AllRepositories<Sole>(context, sole);
            repos = all;

        }
        // GET: api/<SoleController>
        [HttpGet("get-sole")]
        public IEnumerable<Sole> Get()
        {
            return repos.GetAll();
        }

        // GET api/<SoleController>/5
        [HttpGet("{name}")]
        public IEnumerable<Sole> Get(string name)
        {
            return repos.GetAll().Where(p => p.Name.Contains(name));

        }

        // POST api/<SoleController>
        [HttpPost("create-sole")]
        public bool CreateSole(string SoleCode, string Name, int Status, int Height, DateTime DateCreated)
        {
            Sole sl = new Sole();
            sl.SoleCode = SoleCode;
            sl.Name = Name;
            sl.Status = Status;
            sl.Height = Height;
            sl.DateCreated = DateCreated;
            sl.SoleID = Guid.NewGuid();
            return repos.AddItem(sl);
        }

        // PUT api/<SoleController>/5
        [HttpPut("update-sole")]
        public bool Put(Guid SoleID, string SoleCode, string Name, int Height, int Status, DateTime DateCreated)
        {
            var sl = repos.GetAll().First(p => p.SoleID == SoleID);
            sl.SoleCode = SoleCode;
            sl.Name = Name;
            sl.Status = Status;
            sl.Height = Height;
            sl.DateCreated = DateCreated;
            return repos.EditItem(sl);
        }

        // DELETE api/<SoleController>/5
        [HttpDelete("delete-sole")]
        public bool Delete(Guid soleID)
        {
            var sl = repos.GetAll().First(p => p.SoleID == soleID);
            sl.Status = 1;
            return repos.EditItem(sl);
        }
    }
}
