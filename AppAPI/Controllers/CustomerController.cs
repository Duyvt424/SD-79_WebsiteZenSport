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
    public class CustomerController : ControllerBase
    {
        private readonly IAllRepositories<Customer> _repos;
        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<Customer> _customer;
        public CustomerController()
        {
            _customer = _dbContext.Customers;
            AllRepositories<Customer> all = new AllRepositories<Customer>(_dbContext, _customer);
            _repos = all;
        }
        // GET: api/<ValuesController>
        [HttpGet("get-customer")]
        public IEnumerable<Customer> GetAll()
        {
           return _repos.GetAll();
        }

        // GET api/<ValuesController>/5
        [HttpGet("find-customer")]
        public IEnumerable<Customer> GetAll(string name)
        {
            return _repos.GetAll().Where(c => c.UserName.ToLower().Contains(name.ToLower())).ToList();
        }

        // POST api/<ValuesController>
        [HttpPost("create-customer")]
        public bool CreateCustomer(string FullName, string UserName, string Password, string Email, int Sex, string ResetPassword, string PhoneNumber, int Status, Guid RankID, DateTime DateCreated)
        {
            Customer customer = new Customer();
            customer.FullName = FullName;
            customer.UserName = UserName;
            customer.Password = Password;
            customer.Email = Email;
            customer.Sex = Sex;
            customer.PhoneNumber = PhoneNumber;
            customer.Status = Status;
            customer.RankID = RankID;
            customer.ResetPassword = ResetPassword;
            customer.DateCreated = DateCreated;
            return _repos.AddItem(customer);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("update-customer")]
        public bool UpdateCustomer(string FullName, string Password, string Email, int Sex, string PhoneNumber, int Status, Guid CumstomerID, string Res)
        {
            var cus = _repos.GetAll().First(c => c.CumstomerID == CumstomerID);
            cus.FullName = FullName;
            cus.Password = Password;
            cus.Email = Email;
            cus.Sex = Sex;
            cus.PhoneNumber = PhoneNumber;
            cus.Status = Status;
            cus.ResetPassword = Res;
            return _repos.EditItem(cus);
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("delete-customer")]
        public bool Delete(Guid id)
        {
            var cus = _repos.GetAll().First(c => c.CumstomerID == id);
            return _repos.RemoveItem(cus);
        }
    }
    //aff
    //ikrfjkie
}
