using AppData.IRepositories;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Repositories
{
    public class AllRepositories<T> : IAllRepositories<T> where T : class
    {
        public ShopDBContext _dbContext;
        public DbSet<T> dbSet;
        public AllRepositories()
        {

        }
        public AllRepositories(ShopDBContext dbContext, DbSet<T> dbSet)
        {
            _dbContext = dbContext;
            this.dbSet = dbSet;
        }
        public bool AddItem(T item)
        {
            try
            {
                dbSet.Add(item);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool EditItem(T item)
        {
            try
            {
                dbSet.Update(item);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public bool RemoveItem(T item)
        {
            try
            {
                dbSet.Remove(item);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
