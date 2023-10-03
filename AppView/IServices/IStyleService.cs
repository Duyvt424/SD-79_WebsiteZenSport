using AppData.Models;

namespace AppView.IServices
{
	public interface IStyleService
	{
		public bool AddStyle(Style style);
		public bool RemoveStyle(Style style);
		public bool UpdateStyle(Style style);
		public Style GetStyleById(Guid id);
		public List<Style> GetAllStyles();

	}
}
