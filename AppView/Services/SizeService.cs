using AppData.Models;
using AppView.IServices;

namespace AppView.Services
{
	public class SizeService : ISizeService
	{
		ShopDBContext _dBContext;
		public SizeService()
		{
			_dBContext = new ShopDBContext();
		}
		public bool AddSize(Size shoeSize)
		{
			try
			{
				_dBContext.Add(shoeSize);
				_dBContext.SaveChanges();
				return true;
			}
			catch {  return false; }
		}

		public List<Size> GetAllSizes()
		{
			return _dBContext.Sizes.ToList();
		}

		public Size GetSizeByID(Guid id)
		{
			return _dBContext.Sizes.First(x => x.SizeID == id);
		}

		public bool RemoveSize(Size shoeSize)
		{
			try
			{
				_dBContext.Remove(shoeSize);
				_dBContext.SaveChanges();
				return true;
			}catch { return false; }
		}

		public bool UpdateSize(Size shoeSize)
		{
			try
			{
				_dBContext.Update(shoeSize);
				_dBContext.SaveChanges();
				return true;
			}
			catch
			{
				return false;
			}
			
		}
	}
}
