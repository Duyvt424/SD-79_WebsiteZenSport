using AppData.Models;
using AppView.IServices;

namespace AppView.Services
{
    public class SoleService : ISoleService
    {
        ShopDBContext _dbContext;
        public SoleService()
        {
            _dbContext = new ShopDBContext();
        }
        public bool AddSole(Sole sole)
        {
            try
            {
                _dbContext.Add(sole);
                _dbContext.SaveChanges();
                return true;
            }

            catch
            {
                return false;
            }
        }

        public List<Sole> GetAllSole()
        {

            return _dbContext.Soles.ToList();
        }

        public Sole GetSoleById(Guid id)
        {
            return _dbContext.Soles.First(x => x.SoleID == id);
        }

        public bool RemoveSole(Sole sole)
        {
            try
            {
                _dbContext.Remove(sole);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateSole(Sole sole)
        {
            try
            {
                _dbContext.Update(sole);
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
