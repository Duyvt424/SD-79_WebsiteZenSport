using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;

namespace AppData.IServices
{
    public interface IImageService
    {
        public bool CreateImage(Image i);
        public bool UpdateImage(Image i);
        public bool DeleteImage(Guid Id);
        public List<Image> GetAllImages();
        public Image GetImageById(Guid Id);
        public List<Image> GetImageByName(string Name);
    }
}
