namespace AppView.Models
{
    public class ProductViewModel
    {
        public Guid ProductID { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public int Status { get; set; }
        public string SupplierName { get; set; }
        public string MaterialName { get; set; }
    }
}
