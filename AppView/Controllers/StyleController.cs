using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AppView.Controllers
{
	public class StyleController : Controller
	{
		private readonly IAllRepositories<Style> repos;
		private ShopDBContext context = new ShopDBContext();
		private DbSet<Style> style;

		public StyleController()
		{
			style = context.Styles;
			AllRepositories<Style> all = new AllRepositories<Style>(context, style);
			repos = all;
		}

		public async Task<IActionResult> GetAllStyles()
		{
			string apiUrl = "https://localhost:7036/api/Style/get-style";
			var httpClient = new HttpClient(); // tạo ra để callApi
			var response = await httpClient.GetAsync(apiUrl);// Lấy dữ liệu ra
															 // Lấy dữ liệu Json trả về từ Api được call dạng string
			string apiData = await response.Content.ReadAsStringAsync();
			// Lấy kqua trả về từ API
			// Đọc từ string Json vừa thu được sang List<T>
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
			string apiUrl = $"https://localhost:7036/api/Customer/create-style?name={style.Name}&status={style.Status}";
			var httpClient = new HttpClient();
			var response = await httpClient.GetAsync(apiUrl);
			string apiData = await response.Content.ReadAsStringAsync();
			// Cập nhật thông tin từ apiData vào đối tượng customer
			var newStyle = JsonConvert.DeserializeObject<Style>(apiData);
			repos.AddItem(style);
			return RedirectToAction("GetAllStyles");
		}

		[HttpGet]
		public IActionResult EditStyle(Guid id) // Khi ấn vào Create thì hiển thị View
		{
			// Lấy Product từ database dựa theo id truyền vào từ route
			Style style = repos.GetAll().FirstOrDefault(c => c.StyleID == id);
			return View(style);
		}

		public IActionResult EditStyle(Style style) // Thực hiện việc Tạo mới
		{
			if (repos.EditItem(style))
			{
				return RedirectToAction("GetAllStyles");
			}
			else return BadRequest();
		}

		public IActionResult DeleteStyle(Guid id)
		{
			var style = repos.GetAll().First(c => c.StyleID == id);
			if (repos.RemoveItem(style))
			{
				return RedirectToAction("GetAllStyles");
			}
			else return Content("Error");
		}
	}
}