using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]//
    [ApiController]
    public class ImageController : ControllerBase 
    {
        private readonly IAllRepositories<Image> repos;
        private readonly IAllRepositories<ShoesDetails> repos1;
        ShopDBContext context = new ShopDBContext();
        DbSet<Image> image;
       DbSet<ShoesDetails> shoe;
        // GET: api/<ImageController>
        public ImageController()
        {
            image = context.Images;
            AllRepositories<Image> all = new AllRepositories<Image>(context, image);
            repos = all;
            shoe = context.ShoesDetails;
            AllRepositories<ShoesDetails> all1 = new AllRepositories<ShoesDetails>(context, shoe);
            repos1 = all1;

        }
        [HttpGet]
        public IEnumerable<Image> Get()
        {
            return repos.GetAll();
        }

        // GET api/<ImageController>/5
        [HttpGet("{name}")]

        public IEnumerable<Image> Get(string name) // Tìm theo tên
        {
            return repos.GetAll().Where(p => p.Name.Contains(name));
        }
        // POST api/<ImageController>
        [HttpPost("Create-Image")]
        public bool CreateImage(string name, string ig1, string ig2, string ig3, string ig4, int status, Guid ShoesDetailsid)
        {
            Image image = new Image();
           var shoes = repos1.GetAll().FirstOrDefault(p => p.ShoesDetailsId == ShoesDetailsid);
            image.Name = name;
            image.Image1 = ig1;
            image.Image2 = ig2;
            image.Image3 = ig3;
            image.Image4 = ig4;
            image.Status = status;
           
            image.ImageID = Guid.NewGuid();
            image.ShoesDetailsID = shoes.ShoesDetailsId;
            return repos.AddItem(image);

        }

        // PUT api/<ImageController>/5
        [HttpPut("Update-Image")]
        public bool Put(Guid id, string name, string ig1, string ig2, string ig3, string ig4, int status, Guid ShoesDetailsid)
        {
            var image = repos.GetAll().FirstOrDefault(c => c.ImageID == id);
          //  var shoes = repos1.GetAll().FirstOrDefault(p => p.ShoesDetailsId == ShoesDetailsid);
            image.Name = name;
            image.Image1 = ig1;
            image.Image2 = ig2;
            image.Image3 = ig3;
            image.Image4 = ig4;
            image.Status = status;
            image.ShoesDetailsID = ShoesDetailsid;
            return repos.EditItem(image);
        }


        // DELETE api/<ImageController>/5
        [HttpDelete("{Delete-Image}")]
        public bool Delete(Guid id)
        {
            var sp = repos.GetAll().FirstOrDefault(c => c.ImageID == id);
            return repos.RemoveItem(sp);
        }
    }
}
