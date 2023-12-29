using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AppView.Controllers
{
    public class StylesController : Controller
    {
        private readonly IAllRepositories<Style> repos;
        private ShopDBContext context = new ShopDBContext();
        private DbSet<Style> style;

        public StylesController()
        {
            style = context.Styles;
            AllRepositories<Style> all = new AllRepositories<Style>(context, style);
            repos = all;
        }
        private string GenerateStyleCode()
        {
            var lastStyle = context.Styles.OrderByDescending(c => c.StyleCode).FirstOrDefault();
            if (lastStyle != null)
            {
                var lastNumber = int.Parse(lastStyle.StyleCode.Substring(2)); // Lấy phần số cuối cùng từ ColorCode
                var nextNumber = lastNumber + 1; // Tăng giá trị cuối cùng
                var newStyleCode = "ST" + nextNumber.ToString("D3")  ;
                return newStyleCode;
            }
            return "ST001"; // Trường hợp không có ColorCode trong cơ sở dữ liệu, trả về giá trị mặc định "CL001"
        }

        public async Task<IActionResult> GetAllStyles()
        {
            string apiUrl = "https://localhost:7036/api/Style/get-style"; ;
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var styles = JsonConvert.DeserializeObject<List<Style>>(apiData);
            return View(styles);
        }

        public async Task<IActionResult> CreateStyle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateStyle(Style style)
        {
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Style/create-style?StyleCode={GenerateStyleCode()}&name={style.Name}&status={style.Status}&DateCreated={style.DateCreated.ToString("yyyy-MM-ddTHH:mm:ss")  }";
            var response = await httpClient.PostAsync(apiUrl, null);
            return RedirectToAction("GetAllStyles")  ;
        }

        [HttpGet]
        public async Task<IActionResult> EditStyle(Guid id) // Khi ấn vào Create thì hiển thị View
        {
            // Lấy Product từ database dựa theo id truyền vào từ route
            Style style = repos.GetAll().FirstOrDefault(c => c.StyleID == id);
            return View(style);
        }

        public async Task<IActionResult> EditStyle(Style style) // Thực hiện việc Tạo mới
        {
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Style/update-style?id={style.StyleID}&StyleCode={style.StyleCode}&name={style.Name}&status={style.Status}&DateCreated={style.DateCreated.ToString("yyyy-MM-ddTHH:mm:ss")  }";
            var response = await httpClient.PutAsync(apiUrl, null);
            return RedirectToAction("GetAllStyles")  ;
        }

        public async Task<IActionResult> DeleteStyle(Guid id)
        {
            var style = repos.GetAll().First(c => c.StyleID == id);
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Style/delete-style?id={id}";
            var response = await httpClient.DeleteAsync(apiUrl);
            return RedirectToAction("GetAllStyles")  ;
        }
        public async Task<IActionResult> FindStyle(string searchQuery)
        {
            var style = repos.GetAll().Where(c => c.StyleCode.ToLower().Contains(searchQuery.ToLower()) || c.Name.ToLower().Contains(searchQuery.ToLower()));
            return View(style);
        }
      

    }
}
