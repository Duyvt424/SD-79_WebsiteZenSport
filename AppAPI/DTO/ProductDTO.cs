using AppData.Models;

namespace AppAPI.DTO
{
    public class ProductDTO
    {
        public string Message { get; set; }
        public int status {  get; set; }
        public List<shoesDetailsDTO> Shoe_Details { get; set; }
    }
}
