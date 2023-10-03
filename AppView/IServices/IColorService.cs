using AppData.Models;

namespace AppView.IServices
{
	public interface IColorService
	{
		public bool AddColor(Color color);
		public bool RemoveColor(Color color);
		public bool UpdateColor(Color color);
		public List<Color> GetAllColors();
		public Color GetColorById(Guid id);
	}
}
