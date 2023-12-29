using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AppView.Controllers
{
    public class SolesController : Controller
    {
        private readonly IAllRepositories<Sole> _repos;
        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<Sole> _sole;
        public SolesController()
        {
            _sole = _dbContext.Soles;
            AllRepositories<Sole> all = new AllRepositories<Sole>(_dbContext, _sole);
            _repos = all;
        }
        private string GenerateSoleCode()
        {
            var lastSole = _dbContext.Soles.OrderByDescending(c => c.SoleCode).FirstOrDefault();
            if (lastSole != null)
            {
                var lastNumber = int.Parse(lastSole.SoleCode.Substring(2)); // Lấy phần số cuối cùng từ ColorCode
                var nextNumber = lastNumber + 1; // Tăng giá trị cuối cùng
                var newSoleCode = "SL" + nextNumber.ToString("D3"); // Tạo ColorCode mới
                return newSoleCode;
            }
            return "SL001"; // Trường hợp không có ColorCode trong cơ sở dữ liệu, trả về giá trị mặc định "CL001"
        }
        public async Task<IActionResult> GetAllSole()
        {
            string apiUrl = "https://localhost:7036/api/Sole/get-sole";
            var httpClient = new HttpClient(); // tạo ra để callApi
            var response = await httpClient.GetAsync(apiUrl);// Lấy dữ liệu ra
            string apiData = await response.Content.ReadAsStringAsync();
            var sole = JsonConvert.DeserializeObject<List<Sole>>(apiData);
            return View(sole);
        }
        [HttpGet]
        public async Task<IActionResult> CreateSole()
        {
            return View();
        }
        [HttpPost]


        public async Task<IActionResult> CreateSole(Sole sole)
        {
            string apiUrl = $"https://localhost:7036/api/Sole/create-sole?SoleCode={GenerateSoleCode()}&Name={sole.Name}&Status={sole.Status}&Height={sole.Height}&DateCreated={sole.DateCreated.ToString("yyyy-MM-ddTHH:mm:ss")}";
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(apiUrl, null);
            string apiData = await response.Content.ReadAsStringAsync();
            return RedirectToAction("GetAllSole");
        }


        [HttpGet]
        public async Task<IActionResult> EditSole(Guid id) // Khi ấn vào Create thì hiển thị View
        {
            // Lấy Product từ database dựa theo id truyền vào từ route
            Sole sole = _repos.GetAll().FirstOrDefault(c => c.SoleID == id);
            return View(sole);
        }
        public async Task<IActionResult> EditSole(Sole sole) // Thực hiện việc Tạo mới
        {
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Sole/update-sole?SoleID={sole.SoleID}&SoleCode={sole.SoleCode}&Name={sole.Name}&Height={sole.Height}&Status={sole.Status}&DateCreated={sole.DateCreated.ToString("yyyy-MM-ddTHH:mm:ss")}";
            var response = await httpClient.PutAsync(apiUrl, null);
            return RedirectToAction("GetAllSole");
        }
        public async Task<IActionResult> DeleteSole(Guid id)
        {
            var sl = _repos.GetAll().First(c => c.SoleID == id);

            var httpClien = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Sole/delete-sole?soleID={id}";
            var response = await httpClien.DeleteAsync(apiUrl);
            return RedirectToAction("GetAllSole");
        }
        public async Task<IActionResult> FindSole(string searchQuery)
        {
            var sole = _repos.GetAll().Where(c => c.SoleCode.ToLower().Contains(searchQuery.ToLower()) || c.Name.ToLower().Contains(searchQuery.ToLower()));
            return View(sole);
        }
    }
}
