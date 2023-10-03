using AppData.Models;
using System.Drawing;
using Size = AppData.Models.Size;

namespace AppView.IServices
{
	public interface ISizeService
	{
		public bool AddSize(Size shoeSize);
		public bool RemoveSize(Size shoeSize);
		public bool UpdateSize(Size shoeSize);
		public List<Size> GetAllSizes();
		public Size GetSizeByID(Guid id);
		
	}
}
