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
    public class PurchaseMethodController : ControllerBase
    {
        private readonly IAllRepositories<PurchaseMethod> repos;
        ShopDBContext context = new ShopDBContext();
        DbSet<PurchaseMethod> product;
        // GET: api/<ImageController>
        public PurchaseMethodController()
        {
            product = context.PurchaseMethods;
            AllRepositories<PurchaseMethod> all = new AllRepositories<PurchaseMethod>(context, product);
            repos = all;
        }
        [HttpGet("get-pu")]
        public IEnumerable<PurchaseMethod> GetPurchaseMethod()
        {
            return repos.GetAll();
        }

        [HttpGet("find-pu")]
        public IEnumerable<PurchaseMethod> FindPurchaseMethod(string name) // Tìm theo tên
        {
            return repos.GetAll().Where(p => p.MethodName.ToLower().Contains(name.ToLower()));
        }
        // POST api/<ImageController>
        [HttpPost("create-pu")]
        public bool CreatePurchaseMethod(string MethodName, int status, DateTime DateCreated)
        {
            PurchaseMethod pu = new PurchaseMethod();
            pu.MethodName = MethodName;
            pu.Status = status;
            pu.DateCreated = DateCreated;
            pu.PurchaseMethodID= Guid.NewGuid();
            return repos.AddItem(pu);
        }

        [HttpPut("edit-pu")]
        public bool EditPurchaseMethod(Guid id, string MethodName, int status, DateTime DateCreated)
        {
            var product = repos.GetAll().FirstOrDefault(c => c.PurchaseMethodID == id);
            product.MethodName = MethodName;
           
            product.Status = status;
            product.DateCreated = DateCreated;
         
            return repos.EditItem(product);
        }

        [HttpDelete("delete-pu")]
        public bool DeletePurchaseMethod(Guid id)
        {
            var product = repos.GetAll().FirstOrDefault(c => c.PurchaseMethodID == id);
            product.Status = 1;
            return repos.EditItem(product);
        }
    }
}
