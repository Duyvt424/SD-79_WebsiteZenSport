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
    public class ProductController : ControllerBase
    {
        private readonly IAllRepositories<Product> repos;
        ShopDBContext context = new ShopDBContext();
        DbSet<Product> product;
        // GET: api/<ImageController>
        public ProductController()
        {
            product = context.Products;
            AllRepositories<Product> all = new AllRepositories<Product>(context, product);
            repos = all;
        }
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return repos.GetAll();
        }

        // GET api/<ImageController>/5
        [HttpGet("{name}")]

        public IEnumerable<Product> Get(string name) // Tìm theo tên
        {
            return repos.GetAll().Where(p => p.Name.Contains(name));
        }
        // POST api/<ImageController>
        [HttpPost("Create-Product")]
        public bool CreateImage(string name, int status)
        {
            Product product = new Product();
            product.Name = name;

            product.Status = status;
            product.ProductID = Guid.NewGuid();
            return repos.AddItem(product);

        }

        // PUT api/<ImageController>/5
        [HttpPut("{Update}")]
        public bool Put(Guid id, string name, int status)
        {
            var image = repos.GetAll().FirstOrDefault(c => c.ProductID == id);
            image.Name = name;

            image.Status = status;
            return repos.EditItem(image);
        }


        // DELETE api/<ImageController>/5
        [HttpDelete("{delete}")]
        public bool Delete(Guid id)
        {
            var sp = repos.GetAll().FirstOrDefault(c => c.ProductID == id);
            return repos.RemoveItem(sp);
        }
    }
}
