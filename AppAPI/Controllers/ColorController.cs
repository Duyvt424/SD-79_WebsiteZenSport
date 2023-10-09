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
    public class ColorController : ControllerBase
    {
        private readonly IAllRepositories<Color> _repos;
        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<Color> _color;
        // GET: api/<ColorController1>
       
        public ColorController()
        {
            _color = _dbContext.Colors;
            AllRepositories<Color> all = new AllRepositories<Color>(_dbContext, _color);
            _repos = all;
        }
        [HttpGet("get-color")]
       
        public IEnumerable<Color> GetAll()
        {
            return _repos.GetAll();
        }

        // GET api/<ColorController1>/5
        [HttpGet("find-color")]
        public IEnumerable<Color> GetAll(string name)
        {
            return _repos.GetAll().Where(c => c.Name.ToLower().Contains(name.ToLower())).ToList();
        }

        // POST api/<ColorController1>
        [HttpPost("create-color")]
        public bool CreateColor(string ColorCode, string Name, int Status, DateTime DateCreated)
        {
            Color color = new Color();
            color.ColorCode = ColorCode;
            color.Name = Name;
            color.Status = Status;
            color.DateCreated = DateCreated;
            color.ColorID = Guid.NewGuid();
           return _repos.AddItem(color);
        }

        // PUT api/<ColorController1>/5
        [HttpPut("update-color")]
        public bool Put(string ColorCode, string Name, int Status, DateTime DateCreated, Guid ColorID)
        {
            var colo = _repos.GetAll().FirstOrDefault(c => c.ColorID == ColorID);
            colo.ColorCode = ColorCode;
            colo.Name = Name;
            colo.Status = Status;
            colo.DateCreated = DateCreated;
            return _repos.EditItem(colo);
          
        }


        // DELETE api/<ColorController1>/5
        [HttpDelete("delete-color")]
        public bool Delete(Guid id)
        {
            var colo = _repos.GetAll().First(c => c.ColorID == id);
            colo.Status = 1;
            return _repos.EditItem(colo);
        }
    }
}
