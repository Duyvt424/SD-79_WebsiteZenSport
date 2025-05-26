using AppView.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AppView.IRepositories
{
    public interface IProduct
    {
        Task<ProductDTO> FilterShoeDetail(List<RangeItemcs> instancePrice);
        Task<ProductDTO> FilterShoeByBranch(List<string> instacneBranch);
        Task<ProductDTO> FilterByStyle(List<string> instanceStyle);
        Task<ProductDTO> FilterBySex(List<string> instanceSex);
        Task<ProductDTO> FilterByColor(List<string> instanceColor);
        Task<ProductDTO> FilterShoe(List<RangeItemcs> instancePrice, List<string> instacneBranch, List<string> instanceStyle, List<string> instanceSex, List<string> instanceColor);
    }
}
