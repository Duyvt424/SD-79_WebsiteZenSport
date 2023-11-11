namespace AppView.Models
{
    public class CartItemViewModel
    {
        public Guid ShoesDetailsID { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal? TotalPrice { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public string Size { get; set; }
        public string? ProductImage { get; set; }
        public string? MaHD { get; set; }
    }
}