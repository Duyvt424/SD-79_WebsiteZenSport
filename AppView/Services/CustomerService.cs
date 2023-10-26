using AppData.Models;
using AppView.IServices;

namespace AppView.Services
{
	public class CustomerService : ICustomerService
	{
		ShopDBContext _dbContext;
		public CustomerService()
		{
			_dbContext = new ShopDBContext();
		}

		public bool AddCustomer(Customer customer)
		{
			try
			{
				_dbContext.Add(customer);
				_dbContext.SaveChanges();
				return true;
			}
			catch 
			{

				return false;
			}
		}

		public List<Customer> GetAllCustomers()
		{
			return _dbContext.Customers.ToList();
		}

		public Customer GetCustomerById(Guid id)
		{
			return _dbContext.Customers.First(c=>c.CumstomerID == id);
		}

		public bool RemoveCustomer(Customer customer)
		{
			try
			{
				_dbContext.Remove(customer);
				_dbContext.SaveChanges();
				return true;
			}
			catch 
			{

				return false;
			}
		}

		public bool UpdateCustomer(Customer customer)
		{
			try
			{
				_dbContext.Update(customer);
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
