using AppData.Models;
using AppView.IServices;

namespace AppView.Services
{
	public class StyleService : IStyleService
	{
		ShopDBContext _dbContext;
		public StyleService()
		{
			_dbContext = new ShopDBContext();
		}
		public bool AddStyle(Style style)
		{
			try
			{
				_dbContext.Add(style);
				_dbContext.SaveChanges();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public List<Style> GetAllStyles()
		{
			return _dbContext.Styles.ToList();
		}

		public Style GetStyleById(Guid id)
		{
			return _dbContext.Styles.First(x=>x.StyleID==id);
		}

		public bool RemoveStyle(Style style)
		{
			try
			{
				_dbContext.Add(style);
				_dbContext.SaveChanges();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool UpdateStyle(Style style)
		{
			try
			{
				_dbContext.Add(style);
				_dbContext.SaveChanges();
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
