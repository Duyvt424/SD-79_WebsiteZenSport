namespace AppView.Models
{
    public class ImageViewModel
    {
        public Guid ImageID { get; set; }
        public string ImageCode { get; set; }
        public string? Name { get; set; }
        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
        public string? Image4 { get; set; }
        public int Status { get; set; }
        public DateTime DateCreated { get; set; }
        public string? ShoesDetailsCode { get; set; }
    }
}
