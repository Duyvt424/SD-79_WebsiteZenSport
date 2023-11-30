using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAllRepositories<Address> _repos;
        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<Address> _address;
        public AddressController()
        {
            _address = _dbContext.Addresses;
            AllRepositories<Address> all = new AllRepositories<Address>(_dbContext, _address);
            _repos = all;
        }
        // GET: api/<AddressController>
        [HttpGet("get-address")]
        public IEnumerable<Address> GetAddress()
        {
            return _repos.GetAll();
        }

        [HttpPost("create-address")]
        public string CreateAddress(string Street, string Commune, string District, string Province, bool IsDefaultAddress, decimal ShippingCost, int DistrictId, int WardCode, int ShippingMethodID, int Status, DateTime DateCreated, Guid CumstomerID)
        {
            Address address = new Address();
            address.AddressID = Guid.NewGuid();
            address.Street = Street;
            address.Commune = Commune;
            address.District = District;
            address.Province = Province;
            address.IsDefaultAddress = IsDefaultAddress;
            address.ShippingCost = ShippingCost;
            address.DistrictId = DistrictId;
            address.WardCode = WardCode;
            address.ShippingMethodID = ShippingMethodID;
            address.Status = Status;
            address.DateCreated = DateCreated;
            address.CumstomerID = CumstomerID;
            if (_repos.AddItem(address))
            {
                return "Thêm thành công";
            }
            else
            {
                return "Thêm thất bại";
            }
        }

        // PUT api/<AddressController>/5
        [HttpPut("update-address")]
        public string UpdateAddress(Guid AddressID, string Street, string Commune, string District, string Province, bool IsDefaultAddress, decimal ShippingCost, int DistrictId, int WardCode, int ShippingMethodID, int Status, DateTime DateCreated, Guid CumstomerID)
        {
            var address = _repos.GetAll().First(c => c.AddressID == AddressID);
            address.Street = Street;
            address.Commune = Commune;
            address.District = District;
            address.Province = Province;
            address.IsDefaultAddress = IsDefaultAddress;
            address.ShippingCost = ShippingCost;
            address.DistrictId = DistrictId;
            address.WardCode = WardCode;
            address.ShippingMethodID = ShippingMethodID;
            address.Status = Status;
            address.DateCreated = DateCreated;
            address.CumstomerID = CumstomerID;
            if (_repos.EditItem(address))
            {
                return "Sửa thành công";
            }
            else
            {
                return "Sửa thất bại";
            }
        }

        // DELETE api/<AddressController>/5
        [HttpDelete("delete-address")]
        public bool DeleteAddress(Guid id)
        {
            var address = _repos.GetAll().First(c => c.AddressID == id);
            address.Status = 0;
            return _repos.EditItem(address); ;
        }
    }
}
