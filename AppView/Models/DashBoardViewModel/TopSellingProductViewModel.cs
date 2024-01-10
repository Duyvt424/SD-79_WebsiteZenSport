namespace AppView.Models.DashBoardViewModel
{
    public class TopSellingProductViewModel
    {
        public Guid ShoesDetailsId { get; set; }
        public string ShoesDetailsCode { get; set; }
        public int TotalQuantitySold { get; set; }
        public decimal Price { get; set; }
        public decimal ImportPrice { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; } /// <summary>
        /// 
        /// </summary>
        public string? ColorName { get; set; }
        public string? ProductName { get; set; }
        public string? SoleName { get; set; }
        public string? StyleName { get; set; }
      
        public string? SexName { get; set; }
        public string? Image { get; set; }
    }
}

