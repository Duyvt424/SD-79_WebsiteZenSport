using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RankController : ControllerBase
	{
		private readonly IAllRepositories<Rank> _repos;
		private ShopDBContext _dbContext = new ShopDBContext();
		private DbSet<Rank> _rank;
		// GET: api/<RankController>
		public RankController()
		{
			_rank = _dbContext.Ranks;
			AllRepositories<Rank> all = new AllRepositories<Rank>(_dbContext, _rank);
			_repos = all;
		}
		[HttpGet("get-rank")]
		public IEnumerable<Rank> GetAll()
		{
			return _repos.GetAll();
		}

		// GET api/<RankController>/5
		[HttpGet("find-rank")]
		public IEnumerable<Rank> GetAll(string name)
		{
			return _repos.GetAll().Where(c=>c.Name.ToLower().Contains(name.ToLower())).ToList();

		}

		// POST api/<RankController>
		[HttpPost("create-rank")]
		public bool CreateRank(string RankCode, string Name, string Desciption, decimal ThresholdAmount, decimal ReducedValue, int Status, DateTime DateCreated)
		{
			Rank rank = new Rank();
			rank.RankCode = RankCode;
			rank.Name = Name;
			rank.Desciption = Desciption;
			rank.ThresholdAmount = ThresholdAmount;
			rank.ReducedValue = ReducedValue;
			rank.Status = Status;
			rank.DateCreated = DateCreated;
			rank.RankID = Guid.NewGuid();
			return _repos.AddItem(rank);
		}

		// PUT api/<RankController>/5
		[HttpPut("update-rank")]
		public bool Put(string RankCode, string Name, string Desciption, decimal ThresholdAmount, decimal ReducedValue, int Status, DateTime DateCreated, Guid RankID)
		{
			var rak = _repos.GetAll().FirstOrDefault(c=>c.RankID == RankID);
			rak.RankCode = RankCode;
			rak.Name = Name;
			rak.Desciption= Desciption;
			rak.ThresholdAmount = ThresholdAmount;
			rak.ReducedValue = ReducedValue;
			rak.Status = Status;
			rak.DateCreated = DateCreated;
			return _repos.EditItem(rak);
		}

		// DELETE api/<RankController>/5
		[HttpDelete("delete-rank")]
		public bool Delete(Guid id)
		{
			var rak = _repos.GetAll().First(c=>c.RankID==id);
			rak.Status = 1;
			return _repos.EditItem(rak);
		}
	}
	//abcdf
}
