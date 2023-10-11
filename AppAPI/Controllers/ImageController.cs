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
        ShopDBContext context = new ShopDBContext();
        DbSet<Image> image;
        public ImageController()
        {
            image = context.Images;
            AllRepositories<Image> all = new AllRepositories<Image>(context, image);
            repos = all;
        }
        [HttpGet("get-image")]
        public IEnumerable<Image> GetAllImage()
        {
            return repos.GetAll();
        }

        [HttpGet("find-image")]
        public IEnumerable<Image> FindImage(string name) // Tìm theo tên
        {
            return repos.GetAll().Where(p => p.Name.ToLower().Contains(name.ToLower()));
        }

        // POST api/<ImageController>
        [HttpPost("create-image")]
        public bool CreateImage(string imageCode, string name, string image1, string image2, string image3, string image4, int status, DateTime DateCreated, Guid shoesDetailsId)
        {
            Image image = new Image();
            image.ImageID = Guid.NewGuid();
            image.ImageCode = imageCode;
            image.Name = name;
            image.Image1 = image1;
            image.Image2 = image2;
            image.Image3 = image3;
            image.Image4 = image4;
            image.Status = status;
            image.DateCreated = DateCreated;
            image.ShoesDetailsID = shoesDetailsId;
            return repos.AddItem(image);
        }

        [HttpPut("update-image")]
        public bool Put(Guid id, string imageCode, string name, string image1, string image2, string image3, string image4, int status, DateTime DateCreated, Guid shoesDetailsId)
        {
            var image = repos.GetAll().First(c => c.ImageID == id);
            image.ImageCode = imageCode;
            image.Name = name;
            image.Image1 = image1;
            image.Image2 = image2;
            image.Image3 = image3;
            image.Image4 = image4;
            image.Status = status;
            image.DateCreated = DateCreated;
            image.ShoesDetailsID = shoesDetailsId;
            return repos.EditItem(image);
        }

        [HttpDelete("delete-image")]
        public bool Delete(Guid id)
        {
            var image = repos.GetAll().First(c => c.ImageID == id);
            image.Status = 1;
            return repos.EditItem(image);
        }
    }
}
