using AppAPI.DTO;
using AppAPI.Interfaces;
using AppAPI.Services;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoesDetailsController : ControllerBase
    {
        private readonly IAllRepositories<ShoesDetails> repos;
        ShopDBContext context = new ShopDBContext();
        DbSet<ShoesDetails> shoesdt;
        private readonly IShoesDetailsService _shoesDT;

        private readonly IProduct _product;
        public ShoesDetailsController(IProduct product)
        {
            shoesdt = context.ShoesDetails;
            AllRepositories<ShoesDetails> all = new AllRepositories<ShoesDetails>(context, shoesdt);
            repos = all;
            _product = product;
            _shoesDT = new ShoesDetailsService();
        }

        [HttpGet("get-shoesdetails")]
        public IEnumerable<shoesDetailsDTO> GetAllShoesDetails()
        {
            return _shoesDT.GetallShoedetailDtO().ToList();
        }

        [HttpGet("find-shoesdetails")]
        public IEnumerable<ShoesDetails> FindShoesDetails(string shoesDetailsCode)
        {
            return repos.GetAll().Where(p => p.ShoesDetailsCode == shoesDetailsCode);
        }

        // POST api/<ShoesDetailsController>
        [HttpPost("create-shoesdetail")]
        public bool CreateShoesDetail(string shoesdetailsCode, DateTime dateCreated, decimal price, decimal importprice, string description, int status, Guid colorId, Guid productId, Guid soleId, Guid styleId, Guid SexID)
        {
            ShoesDetails shoesdt = new ShoesDetails();
            shoesdt.ShoesDetailsId = Guid.NewGuid();
            shoesdt.ShoesDetailsCode = shoesdetailsCode;
            shoesdt.DateCreated = dateCreated;
            shoesdt.Price = price;
            shoesdt.ImportPrice = importprice;
            shoesdt.Description = description;
            shoesdt.Status = status;
            shoesdt.ColorID = colorId;
            shoesdt.ProductID = productId;
            shoesdt.SoleID = soleId;
            shoesdt.StyleID = styleId;
            shoesdt.SexID = SexID;
            return repos.AddItem(shoesdt);
        }

        // PUT api/<ShoesDetailsController>/5
        [HttpPut("edit-shoesdetail")]
        public bool Put(Guid id, string shoesdetailsCode, DateTime dateCreated, decimal price, decimal importprice, string description, int status, Guid colorId, Guid productId, Guid soleId, Guid styleId, Guid SexID)
        {
            var shoesdt = repos.GetAll().First(p => p.ShoesDetailsId == id);
            shoesdt.ShoesDetailsCode = shoesdetailsCode;
            shoesdt.DateCreated = dateCreated;
            shoesdt.Price = price;
            shoesdt.ImportPrice = importprice;
            shoesdt.Description = description;
            shoesdt.Status = status;
            shoesdt.ColorID = colorId;
            shoesdt.ProductID = productId;
            shoesdt.SoleID = soleId;
            shoesdt.StyleID = styleId;
            shoesdt.SexID = SexID;
            return repos.EditItem(shoesdt);
        }
        [HttpDelete("delete-shoesdetail")]
        public bool Delete(Guid id)
        {
            var shoesdt = repos.GetAll().First(p => p.ShoesDetailsId == id);
            shoesdt.Status = 1;
            return repos.EditItem(shoesdt);
        }


        [HttpPost("filter-shoesdetailbyPrice")]
        public async Task<IActionResult> FilterByPrice([FromBody] List<RangeItemcs> rangeItems)
        {
            try
            {
                var listShoesByrangMoney = await _product.FilterShoeDetail(rangeItems);
                return Ok(listShoesByrangMoney);
            }
            catch (Exception ex)
            {
                ProductDTO product = new ProductDTO();
                product.status = 403;
                product.Message = ex.Message;
                return NotFound(product);
            }
        }
        [HttpPost("filter-shoesdetailbyBranch")]
        public async Task<IActionResult> FilterByBranch([FromQuery] List<string> rangeItems)
        {
            try
            {
                var listShoesByrangMoney = await _product.FilterShoeByBranch(rangeItems);
                return Ok(listShoesByrangMoney);
            }
            catch (Exception ex)
            {
                ProductDTO product = new ProductDTO();
                product.status = 403;
                product.Message = ex.Message;
                return NotFound(product);
            }
        }

        [HttpPost("filter-shoesdetailbyColor")]
        public async Task<IActionResult> FilterByColor([FromQuery] List<string> rangeItems)
        {
            try
            {
                var listShoesByrangMoney = await _product.FilterByColor(rangeItems);
                return Ok(listShoesByrangMoney);
            }
            catch (Exception ex)
            {
                ProductDTO product = new ProductDTO();
                product.status = 403;
                product.Message = ex.Message;
                return NotFound(product);
            }
        }


        [HttpPost("filter-shoesdetailbyStyle")]
        public async Task<IActionResult> FilterByStyle([FromQuery] List<string> rangeItems)
        {
            try
            {
                var listShoesByrangMoney = await _product.FilterByStyle(rangeItems);
                return Ok(listShoesByrangMoney);
            }
            catch (Exception ex)
            {
                ProductDTO product = new ProductDTO();
                product.status = 403;
                product.Message = ex.Message;
                return NotFound(product);
            }
        }

        [HttpPost("filter-shoesdetailbySex")]
        public async Task<IActionResult> FilterBySex([FromQuery] List<string> rangeItems)
        {
            try
            {
                var listShoesByrangMoney = await _product.FilterBySex(rangeItems);
                return Ok(listShoesByrangMoney);
            }
            catch (Exception ex)
            {
                ProductDTO product = new ProductDTO();
                product.status = 403;
                product.Message = ex.Message;
                return NotFound(product);
            }
        }


        [HttpPost("filter-allProduct")]
        public async Task<IActionResult> FilterAll([FromBody] List<RangeItemcs> instancePrice, [FromQuery] List<string> instacneBranch, [FromQuery] List<string> instanceStyle, [FromQuery] List<string> instanceSex, [FromQuery] List<string> instanceColor)
        {
            try
            {
                var listAllProdcut = await _product.FilterShoe(instancePrice, instacneBranch, instanceStyle, instanceSex, instanceColor);
                return Ok(listAllProdcut);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
