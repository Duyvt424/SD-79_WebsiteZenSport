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
        [HttpGet("get-product")]
        public IEnumerable<Product> GetProduct()
        {
            return repos.GetAll();
        }

        [HttpGet("find-product")]
        public IEnumerable<Product> FindProduct(string name) // Tìm theo tên
        {
            return repos.GetAll().Where(p => p.Name.ToLower().Contains(name.ToLower()));
        }
        // POST api/<ImageController>
        [HttpPost("create-product")]
        public bool CreateProduct(string productCode, string name, int status, DateTime DateCreated, Guid SupplierID, Guid MaterialId)
        {
            Product product = new Product();
            product.ProductCode = productCode;
            product.Name = name;
            product.Status = status;
            product.DateCreated = DateCreated;
            product.SupplierID = SupplierID;
            product.MaterialId = MaterialId;
            product.ProductID = Guid.NewGuid();
            return repos.AddItem(product);
        }

        [HttpPut("edit-product")]
        public bool EditProduct(Guid id, string productCode, string name, int status, DateTime dateCreated, Guid supplierID, Guid materialId)
        {
            var product = repos.GetAll().FirstOrDefault(c => c.ProductID == id);
            product.ProductCode = productCode;
            product.Name = name;
            product.Status = status;
            product.DateCreated = dateCreated;
            product.SupplierID = supplierID;
            product.MaterialId = materialId;
            return repos.EditItem(product);
        }

        [HttpDelete("delete-product")]
        public bool DeleteProduct(Guid id)
        {
            var product = repos.GetAll().FirstOrDefault(c => c.ProductID == id);
            product.Status = 1;
            return repos.EditItem(product);
        }
    }
}
