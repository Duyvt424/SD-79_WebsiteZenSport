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
        [HttpGet("Get-Sole")]
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
        [HttpPost("Create-Sole")]
        public bool CreateSole(string name, string fabric, int status, int height)
        {
            Sole sl = new Sole();
            sl.Name = name;
            sl.Fabric = fabric;
            sl.Status = status;
            sl.Height = height;
            sl.SoleID = Guid.NewGuid();
            return repos.AddItem(sl);
        }

        // PUT api/<SoleController>/5
        [HttpPut("Update-Sole")]
        public bool Put(Guid soleID, string name, string fabric, int height, int status)
        {
            var sl = repos.GetAll().First(p => p.SoleID == soleID);
            sl.Name = name;
            sl.Fabric = fabric;
            sl.Status = status;
            sl.Height = height;
            return repos.EditItem(sl);
        }

        // DELETE api/<SoleController>/5
        [HttpDelete("Delete-Sole")]
        public bool Delete(Guid soleID)
        {
            var sl = repos.GetAll().First(p => p.SoleID == soleID);
            return repos.RemoveItem(sl);
        }
    }
}
