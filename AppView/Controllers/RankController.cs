using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AppView.Controllers
{
	public class RankController : Controller
	{
		private readonly IAllRepositories<Rank> repos;
		private ShopDBContext context = new ShopDBContext();
		private DbSet<Rank> rank;
		public RankController()
		{
			rank = context.Ranks;
			AllRepositories<Rank> all = new AllRepositories<Rank>(context, rank);
			repos = all;


		}
		private string GenerateRankCode()
		{
			var lastRank = context.Ranks.OrderByDescending(c => c.RankCode).FirstOrDefault();
			if (lastRank != null)
			{
				var lastNumber = int.Parse(lastRank.RankCode.Substring(2));
				var nextNumber = lastNumber + 1;
				var newRankCode = "R" + nextNumber.ToString("D3");
				return newRankCode;
			}
			return "R001";
		}
		public async Task<IActionResult> GetAllRanks()
		{
			string apiUrl = "https://localhost:7036/api/Rank/get-rank";
			var httpClient = new HttpClient();
			var reponse = await httpClient.GetAsync(apiUrl);
			string apiData = await reponse.Content.ReadAsStringAsync();
			var ranks = JsonConvert.DeserializeObject<List<Rank>>(apiData);
			return View(ranks);
		}
		public async Task<IActionResult> CreateRank()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> CreateRank(Rank rank)
		{
			var httpClient = new HttpClient();
			string apiUrl = $"https://localhost:7036/api/Rank/create-rank?RankCode={GenerateRankCode()}&name={rank.Name}&desciption={rank.Desciption}&thresholdAmount={rank.ThresholdAmount}&reducedValue={rank.ReducedValue}&status={rank.Status}&DateCreated={rank.DateCreated}";
			var reponse = await httpClient.PostAsync(apiUrl, null);
			return RedirectToAction("GetAllRanks");
		}
		[HttpGet]
		public async Task<IActionResult> EditRank(Guid id)
		{
			Rank rank = repos.GetAll().FirstOrDefault(c => c.RankID == id);
			return View(rank);
		}
		public async Task<IActionResult> EditRank(Rank rank)
		{
			var httpClient = new HttpClient();
			string apiUrl = $"https://localhost:7036/api/Rank/update-rank?RankCode={rank.RankCode}&name={rank.Name}&desciption={rank.Desciption}&thresholdAmount={rank.ThresholdAmount}&reducedValue={rank.ReducedValue}&status={rank.Status}&DateCreated={rank.DateCreated}&RankID={rank.RankID}";
			var reponse = await httpClient.PutAsync(apiUrl, null);
			return RedirectToAction("GetAllRanks");
		}
		public async Task<IActionResult> DeleteRank(Guid id)
		{
			var rank = repos.GetAll().First(c=>c.RankID == id);
			var httpClient = new HttpClient();
			string apiUrl = $"https://localhost:7036/api/Rank/delete-rank?id={id}";
			var reponse = await httpClient.DeleteAsync(apiUrl);
			return RedirectToAction("GetAllRanks");
		}
		public async Task<IActionResult> FindRank(string searchQuery)
		{
			var rank = repos.GetAll().Where(c=>c.RankCode.ToLower().Contains(searchQuery.ToLower()) || c.Name.ToLower().Contains(searchQuery.ToLower()));
			return View(rank);
		}
	}
}
/*fefeofjk*/
