using AppData.IRepositories;
using AppData.IServices;
using AppData.Models;
using AppData.Repositories;
using AppData.Services;
using AppView.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AppView.Controllers
{
    public class ProductController : Controller
    {
        private readonly IAllRepositories<Product> _repos;
        private readonly IAllRepositories<Material> materialRepos;
        private readonly IAllRepositories<Supplier> supplierRepos;
        ShopDBContext _dbContext = new ShopDBContext();
        DbSet<Product> _product;
        DbSet<Material> _material;
        DbSet<Supplier> _supplier;
      
        public ProductController()
        {
            _product = _dbContext.Products;
            AllRepositories<Product> all = new AllRepositories<Product>(_dbContext, _product);
            _repos = all;

            _material = _dbContext.Materials;
            AllRepositories<Material> materialAll = new AllRepositories<Material>(_dbContext, _material);
            materialRepos = materialAll;

            _supplier = _dbContext.Suppliers;
            AllRepositories<Supplier> supplierAll = new AllRepositories<Supplier>(_dbContext, _supplier);
            supplierRepos = supplierAll;
        }
        private string GenerateProductCode()
        {
            var lastProduct = _dbContext.Products.OrderByDescending(c => c.ProductCode).FirstOrDefault();
            if (lastProduct != null)
            {
                var lastNumber = int.Parse(lastProduct.ProductCode.Substring(2)); // Lấy phần số cuối cùng từ ColorCode
                var nextNumber = lastNumber + 1; // Tăng giá trị cuối cùng
                var newProductCode = "PD" + nextNumber.ToString("D3");
                return newProductCode;
            }
            return "PD001"; // Trường hợp không có ColorCode trong cơ sở dữ liệu, trả về giá trị mặc định "CL001"
        }
        public async Task<IActionResult> GetAllProduct()
        {
            string apiUrl = "https://localhost:7036/api/Product/get-product";
            var httpClient = new HttpClient(); // tạo ra để callApi
            var response = await httpClient.GetAsync(apiUrl);// Lấy dữ liệu ra
            string apiData = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<Product>>(apiData);
            var suppliers = supplierRepos.GetAll();
            var materials = materialRepos.GetAll();

            // Tạo danh sách ProductViewModel với thông tin Supplier và Material
            var productViewModels = products.Select(product => new ProductViewModel
            {
                ProductID = product.ProductID,
                ProductCode = product.ProductCode,
                Name = product.Name,
                DateCreated = product.DateCreated,
                Status = product.Status,
                SupplierName = suppliers.FirstOrDefault(s => s.SupplierID == product.SupplierID)?.Name,
                MaterialName = materials.FirstOrDefault(m => m.MaterialId == product.MaterialId)?.Name
            }).ToList();

            return View(productViewModels);
        }
        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            using (ShopDBContext dBContext = new ShopDBContext())
            {
                var supplier = dBContext.Suppliers.Where(c => c.Status == 0).ToList();
                SelectList selectListSupplier = new SelectList(supplier, "SupplierID", "Name");
                ViewBag.SupplierList = selectListSupplier;

                var material = dBContext.Materials.Where(c => c.Status == 0).ToList();
                SelectList selectListMaterial = new SelectList(material, "MaterialId", "Name");
                ViewBag.MaterialList = selectListMaterial;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Product/create-product?productCode={GenerateProductCode()}&name={product.Name}&status={product.Status}&DateCreated={product.DateCreated}&SupplierID={product.SupplierID}&MaterialId={product.MaterialId}";
            var response = await httpClient.PostAsync(apiUrl, null);
            return RedirectToAction("GetAllProduct");
        }
        [HttpGet]
        public async Task<IActionResult> EditProduct(Guid id)
        {
            Product product = _repos.GetAll().FirstOrDefault( c => c.ProductID == id);
            using (ShopDBContext dBContext = new ShopDBContext())
            {
                var supplier = dBContext.Suppliers.Where(c => c.Status == 0).ToList();
                SelectList selectListSupplier = new SelectList(supplier, "SupplierID", "Name");
                ViewBag.SupplierList = selectListSupplier;

                var material = dBContext.Materials.Where(c => c.Status == 0).ToList();
                SelectList selectListMaterial = new SelectList(material, "MaterialId", "Name");
                ViewBag.MaterialList = selectListMaterial;
            }
            return View(product);
        }
        public async Task<IActionResult> EditProduct(Product product)
        {
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Product/edit-product?id={product.ProductID}&productCode={product.ProductCode}&name={product.Name}&status={product.Status}&dateCreated={product.DateCreated}&supplierID={product.SupplierID}&materialId={product.MaterialId}";
            var response = await httpClient.PutAsync(apiUrl, null);
            return RedirectToAction("GetAllProduct");
        }
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = _repos.GetAll().FirstOrDefault( c => c.ProductID == id);
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Product/delete-product?id={id}";
            var response = await httpClient.DeleteAsync(apiUrl);
            return RedirectToAction("GetAllProduct");
        }
        public async Task<IActionResult> FindProduct(string searchQuery)
        {
            var product = _repos.GetAll().Where(c => c.ProductCode.ToLower().Contains(searchQuery.ToLower()) || c.Name.ToLower().Contains(searchQuery.ToLower()));
            return View(product);
        }
    }
}
