using AppView.DTO;
using AppView.IRepositories;
using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppView.Repository
{
    public class ShoesDetailsService : IShoesDetailsService
    {
        ShopDBContext dBContext;
        public ShoesDetailsService()
        {
            dBContext = new ShopDBContext();
        }
        public bool AddShoesDetails(ShoesDetails shoesDetails)
        {
            try
            {
                dBContext.ShoesDetails.Add(shoesDetails);
                dBContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteShoesDetails(Guid id)
        {
            try
            {
                var ShoesDT = dBContext.ShoesDetails.Find(id);
                dBContext.ShoesDetails.Remove(ShoesDT);
                dBContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<shoesDetailsDTO> GetallShoedetailDtO()
        {
            var querry = from shoe in dBContext.ShoesDetails.AsQueryable()

                         join product in dBContext.Products.AsQueryable()
                         on shoe.ProductID equals product.ProductID into Product
                         from pro in Product.DefaultIfEmpty()


                         select new shoesDetailsDTO
                         {
                             ShoesDetailsId = shoe.ShoesDetailsId,
                             ShoesDetailsCode = shoe.ShoesDetailsCode,
                             DateCreated = shoe.DateCreated,
                             Price = shoe.Price,
                             ImportPrice = shoe.ImportPrice,
                             Description = shoe.Description,
                             Status = shoe.Status,
                             ColorID = shoe.ColorID,
                             ProductID = pro.ProductID,
                             SoleID = shoe.SoleID,
                             StyleID = shoe.StyleID,
                             SexID = shoe.SexID,
                             ImageUrl = shoe.ImageUrl,
                             Name = pro.Name,
                             SupplierID = pro.SupplierID

                             };
            return querry.ToList();
        }

        public List<ShoesDetails> GetAllShoesDetails()
        {
            return dBContext.ShoesDetails.ToList();
        }

        public ShoesDetails GetShoesDetailsById(Guid id)
        {
            return dBContext.ShoesDetails.First(c => c.ShoesDetailsId == id);
        }

        public bool UpdateShoesDetails(ShoesDetails shoesDetails)
        {
            try
            {
                var ShoesDT = dBContext.ShoesDetails.Find(shoesDetails.ShoesDetailsId);
                dBContext.ShoesDetails.Update(ShoesDT);
                dBContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
