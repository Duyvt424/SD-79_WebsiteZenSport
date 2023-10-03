using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.IServices;
using AppData.Models;

namespace AppData.Services
{
    public class ImageService : IImageService
    {
        ShopDBContext _dBContext;
        public ImageService()
        {
            _dBContext = new ShopDBContext();
        }
        public bool CreateImage(Image i)
        {
            try
            {
                _dBContext.Images.Add(i);
                _dBContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool DeleteImage(Guid Id)
        {
            try
            {
                var image = _dBContext.Images.Find(Id);
                _dBContext.Images.Remove(image);
                _dBContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public List<Image> GetAllImages()
        {
            return _dBContext.Images.ToList();
        }

        public Image GetImageById(Guid Id)
        {
            return _dBContext.Images.FirstOrDefault(p => p.ImageID == Id);
        }

        public List<Image> GetImageByName(string Name)
        {
            return _dBContext.Images.Where(p => p.Name.Contains(Name)).ToList();
        }

        public bool UpdateImage(Image i)
        {
            try
            {
                var image = _dBContext.Images.Find(i.ImageID);
                var SPCT = _dBContext.ShoesDetails.FirstOrDefault(s => s.ShoesDetailsId == i.ShoesDetailsID);
                image.ShoesDetailsID = SPCT.ShoesDetailsId;
                image.Name = i.Name;
                image.Image1 = i.Image1;
                image.Image2 = i.Image2;    
                image.Image3 = i.Image3;
                image.Image4 = i.Image4;
                image.Status= i.Status;
                _dBContext.Images.Update(image);
                _dBContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
