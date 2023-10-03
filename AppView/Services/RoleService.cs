using AppData.Models;
using AppView.IServices;

namespace AppView.Services
{
    public class RoleService : IRoleService
    {
        ShopDBContext dbContext = new ShopDBContext();
        public RoleService()
        {
            dbContext = new ShopDBContext();
        }
        public bool CreateRole(Role c)
        {
            try
            {
                dbContext.Roles.Add(c);
                dbContext.SaveChanges();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool DeleteRole(Guid Id)
        {
            try
            {
                var p = dbContext.Roles.Find(Id);
                dbContext.Roles.Remove(p);
                dbContext.SaveChanges();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public List<Role> GetAll()
        {
            return dbContext.Roles.ToList();
        }

        public Role GetUserById(Guid Id)
        {
            return dbContext.Roles.FirstOrDefault(c => c.RoleID == Id);
        }

        public bool UpdateRole(Role c)
        {
            try
            {
                var p = dbContext.Roles.Find(c.RoleID);
                p.RoleName = c.RoleName;

                p.Status = c.Status;
                dbContext.Roles.Update(p);
                dbContext.SaveChanges();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
