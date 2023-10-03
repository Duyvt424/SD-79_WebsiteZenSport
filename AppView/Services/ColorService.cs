using AppData.Models;
using AppView.IServices;
using Microsoft.EntityFrameworkCore;

namespace AppView.Services
{
	public class ColorService : IColorService
	{
		ShopDBContext _dbContext;
		public ColorService()
		{
			_dbContext = new ShopDBContext();
		}
		public bool AddColor(Color color)
		{
			try
			{
				_dbContext.Add(color);
				_dbContext.SaveChanges();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public List<Color> GetAllColors()
		{
			return _dbContext.Colors.ToList();
		}

		public Color GetColorById(Guid id)
		{
			return _dbContext.Colors.First(x => x.ColorID == id);
		}

		public bool RemoveColor(Color color)
		{
			try
			{
				_dbContext.Add(color);
				_dbContext.SaveChanges();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool UpdateColor(Color color)
		{
			try
			{
				_dbContext.Add(color);
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
