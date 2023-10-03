using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AppView.Controllers
{
    public class SoleController : Controller
    {
        private readonly IAllRepositories<Sole> _repos;
        private ShopDBContext _dbContext = new ShopDBContext();
        private DbSet<Sole> _sole;
        public SoleController()
        {
            _sole = _dbContext.Soles;
            AllRepositories<Sole> all = new AllRepositories<Sole>(_dbContext, _sole);
            _repos = all;
        }
        public async Task<IActionResult> GetAllSole()
        {
            string apiUrl = "https://localhost:7036/api/Sole/Get-Sole";
            var httpClient = new HttpClient(); // tạo ra để callApi
            var response = await httpClient.GetAsync(apiUrl);// Lấy dữ liệu ra
                                                             // Lấy dữ liệu Json trả về từ Api được call dạng string
            string apiData = await response.Content.ReadAsStringAsync();
            // Lấy kqua trả về từ API
            // Đọc từ string Json vừa thu được sang List<T>
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
            string apiUrl = $"https://localhost:7036/api/Sole/Create-Sole?name={sole.Name}&fabric={sole.Fabric}&status={sole.Status}&height={sole.Height}";
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(apiUrl,null);
            string apiData = await response.Content.ReadAsStringAsync();
            //// Cập nhật thông tin từ apiData vào đối tượng customer
            //var newSole = JsonConvert.DeserializeObject<Sole>(apiData);
            //_repos.AddItem(sole);
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
            string apiUrl = $"https://localhost:7036/api/Sole/Update-Sole?soleID={sole.SoleID}&name={sole.Name}&fabric={sole.Fabric}&height={sole.Height}&status={sole.Status}";
            var response = await httpClient.PutAsync(apiUrl, null);
            return RedirectToAction("GetAllSole");
        }
        public async Task<IActionResult> DeleteSole(Guid id)
        {
            var sl = _repos.GetAll().First(c => c.SoleID == id);
            
            var httpClien = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Sole/Delete-Sole?soleID={id}";
            var response = await httpClien.DeleteAsync(apiUrl);
            return RedirectToAction("GetAllSole");
        }
    }
}
