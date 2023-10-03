using AppData.IServices;
using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Services
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
