using AppData.Models;

namespace AppAPI.DTO

{
    public class shoesDetailsDTO
    {
        public Guid ShoesDetailsId { get; set; }
        public string? ShoesDetailsCode { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal Price { get; set; }
        public decimal ImportPrice { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public Guid? ColorID { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? SoleID { get; set; }
        public Guid? StyleID { get; set; }
        public Guid? SexID { get; set; }
        public string? ImageUrl { get; set; }
        public string Name { set; get; }
        public Guid? SupplierID { set; get; }
    }
}
