namespace AppView.Models
{
    public class ShoesDetailsViewModel
    {
        public Guid ShoesDetailsId { get; set; }
        public string? ShoesDetailsCode { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal Price { get; set; }
        public decimal ImportPrice { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public string? ColorName { get; set; }
        public string? ProductName { get; set; }
        public string? SoleName { get; set; }
        public string? StyleName { get; set; }
        public string? SexName { get; set; }
    }
}
