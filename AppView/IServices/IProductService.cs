using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;

namespace AppData.IServices
{
    public interface IProductService
    {
        public bool CreateProduct(Product i);
        public bool UpdateProduct(Product i);
        public bool DeleteProduct(Guid Id);
        public List<Product> GetAllProducts();
        public Product GetProductById(Guid Id);
        public List<Product> GetProductByName(string Name);
    }
}
