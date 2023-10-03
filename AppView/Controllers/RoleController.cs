using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AppView.Controllers
{
    public class RoleController : Controller
    {
        private readonly IAllRepositories<Role> repos;
        private ShopDBContext context = new ShopDBContext();
        private DbSet<Role> role;

        public RoleController()
        {
            role = context.Roles;
            AllRepositories<Role> all = new AllRepositories<Role>(context, role);
            repos = all;
        }
        
        
       public async Task<IActionResult> GetAllRole()
        {
            string apiUrl = "https://localhost:7036/api/Role";
            var httpClient = new HttpClient(); // tạo ra để callApi
           var response = await httpClient.GetAsync(apiUrl);// Lấy dữ liệu ra
                                                             // Lấy dữ liệu Json trả về từ Api được call dạng string
            string apiData = await response.Content.ReadAsStringAsync();
            // Lấy kqua trả về từ API
            // Đọc từ string Json vừa thu được sang List<T>
            var role = JsonConvert.DeserializeObject<List<Role>>(apiData);
            return View(role);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            
            return View();
        }     
        [HttpPost]
        public async Task<IActionResult> Create(Role role)     
        {
            HttpClient httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Role/create-role?name={role.RoleName}&status={role.Status}";
            
            var response = await httpClient.PostAsync(apiUrl, null);
            //string apiData = await response.Content.ReadAsStringAsync();
            // Cập nhật thông tin từ apiData vào đối tượng customer
            //var role = JsonConvert.DeserializeObject<Role>(apiData);
           // repos.AddItem(role);
            return RedirectToAction("GetAllRole");
        }
        [HttpGet]
        public IActionResult Edit(Guid id) // Khi ấn vào Create thì hiển thị View
        {
            // Lấy Product từ database dựa theo id truyền vào từ route
            Role role = repos.GetAll().FirstOrDefault(c => c.RoleID == id);
           
            return View(role);
        }
        public async Task<IActionResult> Edit(Role role) // Thực hiện việc Tạo mới
        {
            //Role role = repos.GetAll().FirstOrDefault(c => c.RoleID == id);
            HttpClient httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Role/update-role?id={role.RoleID}&name={role.RoleName}&status={role.Status}";
            var response = await httpClient.PutAsync(apiUrl, null);
            
                return RedirectToAction("GetAllRole");
            
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            var role = repos.GetAll().First(c => c.RoleID == id);
            HttpClient httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Role/delete-role?id={id}";    
            var response = await httpClient.DeleteAsync(apiUrl);
                return RedirectToAction("GetAllRole");
            
        }
        public IActionResult Details(Guid id)
        {
            ShopDBContext dBContext = new ShopDBContext();
            var rol = repos.GetAll().FirstOrDefault(c => c.RoleID == id);
            return View(rol);
        }
    }
}
