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
		private ShopDBContext _dbContext = new ShopDBContext();
		private DbSet<Role> _role;
		public RoleController()
		{
			_role = _dbContext.Roles;
			AllRepositories<Role> all = new AllRepositories<Role>(_dbContext, _role);
			repos = all;
		}
		private string GenerateRoleCode()
		{
			var last = _dbContext.Roles.OrderByDescending(c => c.RoleCode).FirstOrDefault();
			if (last != null)
			{
				var lastNumber = int.Parse(last.RoleCode.Substring(2)); // Lấy phần số cuối cùng từ ColorCode
				var nextNumber = lastNumber + 1; // Tăng giá trị cuối cùng
				var newCode = "RO" + nextNumber.ToString("D3"); // Tạo ColorCode mới
				return newCode;
			}
			return "RO001"; // Trường hợp không có ColorCode trong cơ sở dữ liệu, trả về giá trị mặc định "CL001"
		}
		
		public async Task<IActionResult> GetAllRole()
		{

			string apiUrl = "https://localhost:7036/api/Role/get-role";
			var httpClient = new HttpClient();
			var response = await httpClient.GetAsync(apiUrl);
			string apiData = await response.Content.ReadAsStringAsync();
			// Lấy kqua trả về từ API
			// Đọc từ string Json vừa thu được sang List<T>
			var color = JsonConvert.DeserializeObject<List<Role>>(apiData);
			return View(color);
		}
      
		[HttpGet]
		public async Task<IActionResult> CreateRole()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> CreateRole(Role role)
		{
			var httpClient = new HttpClient();
			string apiUrl = $"https://localhost:7036/api/Role/create-role?RoleCode={GenerateRoleCode()}&RoleName={role.RoleName}&Status={role.Status}&DateCreated={role.DateCreated}";
			var response = await httpClient.PostAsync(apiUrl, null);
			return RedirectToAction("GetAllRole");
			
		}

		[HttpGet]
		public async Task<IActionResult> EditRole(Guid id) // Khi ấn vào Create thì hiển thị View
		{
			// Lấy Product từ database dựa theo id truyền vào từ route
			Role role = repos.GetAll().FirstOrDefault(c => c.RoleID == id);
			return View(role);
		}
		public async Task<IActionResult> EditRole(Role role) // Thực hiện việc Tạo mới
		{
			/*var httpClient = new HttpClient();
			string apiUrl = $"https://localhost:7036/api/Role/update-role?RoleCode={role.RoleCode}&RoleName={role.RoleName}&Status={role.Status}&DateCreated={role.DateCreated}&ColorID={role.RoleID}";
			var response = await httpClient.PutAsync(apiUrl, null);
			return RedirectToAction("GetAllRole");*/
			if (repos.EditItem(role))
			{
				return RedirectToAction("GetAllRole");
			}
			else return BadRequest();
		} 
      public async Task<IActionResult> DeleteRole(Guid id)
      {
          var cus = repos.GetAll().First(c => c.RoleID == id);
          var httpClient = new HttpClient();
          string apiUrl = $"https://localhost:7036/api/Role/delete-role?id={id}";
          var response = await httpClient.DeleteAsync(apiUrl);
          return RedirectToAction("GetAllRole");
        }
      public async Task<IActionResult> FindRole(string searchQuery)
      {
          var color = repos.GetAll().Where(c => c.RoleCode.ToLower().Contains(searchQuery.ToLower()) || c.RoleName.ToLower().Contains(searchQuery.ToLower()));
          return View(color);
      }
      public IActionResult Details(Guid id)
      {
          ShopDBContext dBContext = new ShopDBContext();
          var rol = repos.GetAll().FirstOrDefault(c => c.RoleID == id);
          return View(rol);
      } 
    }
}
