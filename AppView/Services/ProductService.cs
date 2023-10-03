using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.IServices;
using AppData.Models;

namespace AppData.Services
{
    public class ProductService : IProductService
    {
        ShopDBContext _dBContext;
        public ProductService()
        {
            _dBContext= new ShopDBContext();
        }
        public bool CreateProduct(Product i)
        {
            try
            {
                _dBContext.Products.Add(i);
                _dBContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool DeleteProduct(Guid Id)
        {

            try
            {
                var pro = _dBContext.Products.Find(Id);
                _dBContext.Products.Remove(pro);
                _dBContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public List<Product> GetAllProducts()
        {
           return  _dBContext.Products.ToList();
        }

        public Product GetProductById(Guid Id)
        {
            return _dBContext.Products.First(p => p.ProductID == Id);
        }

        public List<Product> GetProductByName(string Name)
        {
            return _dBContext.Products.Where( c => c.Name.ToLower().Contains(Name.ToLower())).ToList();
        }

        public bool UpdateProduct(Product i)
        {
            try
            {
                var pro = _dBContext.Products.Find(i.ProductID);
                pro.Name = i.Name;
                pro.Status= i.Status;
                _dBContext.Products.Update(pro);
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
