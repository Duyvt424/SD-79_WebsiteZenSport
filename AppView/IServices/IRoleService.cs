using AppData.Models;

namespace AppView.IServices
{
    public interface IRoleService
    {
        public bool CreateRole(Role c);
        public bool UpdateRole(Role c);
        public bool DeleteRole(Guid Id);
        public List<Role> GetAll();
        public Role GetUserById(Guid Id);
    }
}
