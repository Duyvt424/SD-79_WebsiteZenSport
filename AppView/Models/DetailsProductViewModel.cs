using AppData.Models;

namespace AppView.Models
{
    public class DetailsProductViewModel
    {
        public Guid ShoesDT { get; set; }
        public Guid NameProduct { get; set; }
        public Guid StyleProduct { get; set; }
        public Guid ImageGoldens { get; set; }
        public decimal Price { get; set; }
        public Guid ShoesDetailsId { get; set; }
        public string Description { get; set; }
        public List<HistoryChat> ChatHistory { get; set; }
    }
}
