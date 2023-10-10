using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IAllRepositories<Material> repos;
        private ShopDBContext context = new ShopDBContext();
        private DbSet<Material> materials;
        public MaterialController()
        {
            materials = context.Materials;
            AllRepositories<Material> all = new AllRepositories<Material>(context, materials);
            repos = all;
        }
        [HttpGet("get-material")]
        public IEnumerable<Material> GetAll()
        {
            return repos.GetAll();
        }

        [HttpGet("find-material")]
        public IEnumerable<Material> GetMaterial(string name)
        {
            return repos.GetAll().Where(x => x.Name == name);
        }

        [HttpPost("create-material")]
        public bool CreateMaterial(string materialCode, string name, int status, DateTime dateCreated)
        {
            Material material = new Material();
            material.MaterialId = Guid.NewGuid();
            material.MaterialCode = materialCode;
            material.Name = name;
            material.Status = status;
            material.DateCreated = dateCreated;
            return repos.AddItem(material);
        }

        [HttpPut("update-material")]
        public bool UpdateMaterial(Guid id, string materialCode, string name, int status, DateTime dateCreated)
        {
            Material material = repos.GetAll().First(x => x.MaterialId == id);
            material.MaterialCode = materialCode;
            material.Name = name;
            material.Status = status;
            material.DateCreated = dateCreated;
            return repos.EditItem(material);
        }

        [HttpDelete("delete-material")]
        public bool DeleteMaterial(Guid id)
        {
            Material material = repos.GetAll().First(x => x.MaterialId == id);
            material.Status = 1;
            return repos.EditItem(material);
        }
    }
}
