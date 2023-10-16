using AppData.Models;

namespace AppView.IServices
{
	public interface IRankService
	{
		public bool AddRank(Rank rank);
		public bool RemoveRank(Rank rank);
		public bool UpdateRank(Rank rank);
		public List<Rank> GetAllRanks();
		public Rank GetRankById(Guid id);
	}
	//abcd
}
