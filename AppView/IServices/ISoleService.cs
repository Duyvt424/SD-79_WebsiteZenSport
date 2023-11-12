using AppData.Models;

namespace AppView.IServices
{
    public interface ISoleService
    {
        public bool AddSole(Sole sole);
        public bool RemoveSole(Sole sole);
        public bool UpdateSole(Sole sole);
        public Sole GetSoleById(Guid id);
        public List<Sole> GetAllSole();
    }
}
