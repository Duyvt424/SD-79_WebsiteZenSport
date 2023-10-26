using AppData.Models;

namespace AppView.IServices
{
	public interface ICustomerService
	{
		public bool AddCustomer(Customer customer);
		public bool UpdateCustomer(Customer customer);
		public bool RemoveCustomer(Customer customer);
		public List<Customer> GetAllCustomers();
		public Customer GetCustomerById(Guid id);
	}
}
