using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AppView.Controllers
{
    public class MaterialsController : Controller
    {
        // GET: MaterialsController
        private readonly IAllRepositories<Material> repos;
        private ShopDBContext context = new ShopDBContext();
        private DbSet<Material> materials;
         public MaterialsController()
        {
            materials = context.Materials;
            AllRepositories<Material> all = new AllRepositories<Material>(context, materials);
            repos = all;
        }

        private bool CheckUserRole()
        {
            var CustomerRole = HttpContext.Session.GetString("UserId");
            var EmployeeNameSession = HttpContext.Session.GetString("RoleName");
            var EmployeeName = EmployeeNameSession != null ? EmployeeNameSession.Replace("\"", "") : null;
            if (CustomerRole != null || EmployeeName != "Quản lý")
            {
                return false;
            }
            return true;
        }
        private string GenerateMaterialCode()
        {
            var lastMaterial = context.Materials.OrderByDescending(c => c.MaterialCode).FirstOrDefault();
            if (lastMaterial != null)
            {
                var lastNumber = int.Parse(lastMaterial.MaterialCode.Substring(2)); // Lấy phần số cuối cùng từ ColorCode
                var nextNumber = lastNumber + 1; // Tăng giá trị cuối cùng
                var newMaterialCode = "MT" + nextNumber.ToString("D3");
                return newMaterialCode;
            }
            return "MT001";
        }
       
        public async Task<IActionResult> GetAllMaterials()
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            string apiUrl = "https://localhost:7036/api/Material/get-material";
            var httpClient = new HttpClient(); // tạo ra để callApi
            var response = await httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var materials = JsonConvert.DeserializeObject<List<Material>>(apiData);
            return View(materials);
        }

        public async Task<IActionResult> CreateMaterial()
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMaterial(Material material)
        {
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Material/create-material?materialCode={GenerateMaterialCode()}&name={material.Name}&status={material.Status}&dateCreated={material.DateCreated}";
            var response = await httpClient.PostAsync(apiUrl, null);
            return RedirectToAction("GetAllMaterials");
        }

        [HttpGet]
        public async Task<IActionResult> EditMaterial(Guid id) // Khi ấn vào Create thì hiển thị View
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            // Lấy Product từ database dựa theo id truyền vào từ route
            Material material = repos.GetAll().FirstOrDefault(c => c.MaterialId == id);
            return View(material);
        }

        public async Task<IActionResult> EditMaterial(Material material) // Thực hiện việc Tạo mới
        {
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Material/update-material?id={material.MaterialId}&materialCode={material.MaterialCode}&name={material.Name}&status={material.Status}&dateCreated={material.DateCreated}";
            var response = await httpClient.PutAsync(apiUrl, null);
            return RedirectToAction("GetAllMaterials");
        }

        public async Task<IActionResult> DeleteMaterial(Guid id)
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            var material = repos.GetAll().First(c => c.MaterialId == id);
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Material/delete-material?id={id}";
            var response = await httpClient.DeleteAsync(apiUrl);
            return RedirectToAction("GetAllMaterials");
        }
        public async Task<IActionResult> FindMaterial(string searchQuery)
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            var material = repos.GetAll().Where(c => c.MaterialCode.ToLower().Contains(searchQuery.ToLower()) || c.Name.ToLower().Contains(searchQuery.ToLower()));
            return View(material);
        }
    }
}
