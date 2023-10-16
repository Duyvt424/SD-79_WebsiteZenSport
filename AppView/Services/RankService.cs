using AppData.Models;
using AppView.IServices;

namespace AppView.Services
{
	public class RankService : IRankService
	{
		ShopDBContext _dbContext;
		public RankService()
		{
			_dbContext = new ShopDBContext();
		}
		public bool AddRank(Rank rank)
		{
			try
			{
				_dbContext.Add(rank);
				_dbContext.SaveChanges();
				return true;
			}
			catch
			{

				return false;
			}
		}

		public List<Rank> GetAllRanks()
		{
			return _dbContext.Ranks.ToList();
		}

		public Rank GetRankById(Guid id)
		{
			return _dbContext.Ranks.First(x=>x.RankID == id);
		}

		public bool RemoveRank(Rank rank)
		{
			try
			{
				_dbContext.Remove(rank);
				_dbContext.SaveChanges();
				return true;
			}
			catch
			{

				return false;
			}
		}

		public bool UpdateRank(Rank rank)
		{
			try
			{
				_dbContext.Update(rank);
				_dbContext.SaveChanges();
				return true;
			}
			catch
			{

				return false;
			}
		}
	}
	//abcd
}
