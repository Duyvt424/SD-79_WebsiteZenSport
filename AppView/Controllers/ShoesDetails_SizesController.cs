using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppView.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
namespace AppView.Controllers
{
    public class ShoesDetails_SizesController : Controller
    {
        private readonly IAllRepositories<ShoesDetails_Size> repos;
        private readonly IAllRepositories<ShoesDetails> shoesDTRepos;
        private readonly IAllRepositories<Size> sizeRepos;
        private ShopDBContext context = new ShopDBContext();
        private DbSet<ShoesDetails_Size> shoesDetails_Size;
        private DbSet<ShoesDetails> shoesDetails;
        private DbSet<Size> size;
        public ShoesDetails_SizesController()
        {
            shoesDetails_Size = context.ShoesDetails_Sizes;
            AllRepositories<ShoesDetails_Size> all = new AllRepositories<ShoesDetails_Size>(context, shoesDetails_Size);
            repos = all;

            shoesDetails = context.ShoesDetails;
            AllRepositories<ShoesDetails> shoesDTAll = new AllRepositories<ShoesDetails>(context, shoesDetails);
            shoesDTRepos = shoesDTAll;

            size = context.Sizes;
            AllRepositories<Size> sizeAll = new AllRepositories<Size>(context, size);
            sizeRepos = sizeAll;
        }

        public async Task<IActionResult> GetAllShoesDetails_Size()
        {
            string apiUrl = "https://localhost:7036/api/ShoesDetails_Size/get-shoesdetails_size";
            var httpClient = new HttpClient(); // tạo ra để callApi
            var response = await httpClient.GetAsync(apiUrl);// Lấy dữ liệu ra
            string apiData = await response.Content.ReadAsStringAsync();
            var shoesDT_Size = JsonConvert.DeserializeObject<List<ShoesDetails_Size>>(apiData);
            var shoesDT = shoesDTRepos.GetAll();
            var size = sizeRepos.GetAll();
            // Tạo danh sách ProductViewModel với thông tin Supplier và Material
            var shoesDetailsSizeViewModels = shoesDT_Size.Select(shoesDT_Size => new ShoesDetails_SizeViewModel
            {
                ID = shoesDT_Size.ID,
                Quantity = shoesDT_Size.Quantity,
                ShoesDetailsCode = shoesDT.FirstOrDefault(s => s.ShoesDetailsId == shoesDT_Size.ShoesDetailsId)?.ShoesDetailsCode,
                SizeName = size.FirstOrDefault(m => m.SizeID == shoesDT_Size.SizeID)?.Name
            }).ToList();
            return View(shoesDetailsSizeViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> CreateShoesDetails_Size()
        {
            using (ShopDBContext dBContext = new ShopDBContext())
            {
                var shoesDT = dBContext.ShoesDetails.Where(c => c.Status == 0).ToList();
                SelectList selectListShoesDT = new SelectList(shoesDT, "ShoesDetailsId", "ShoesDetailsCode");
                ViewBag.ShoesDTList = selectListShoesDT;

                var size = dBContext.Sizes.Where(c => c.Status == 0).ToList();
                SelectList selectListSize = new SelectList(size, "SizeID", "Name");
                ViewBag.SizeList = selectListSize;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateShoesDetails_Size(ShoesDetails_Size shoesDetails_Size)
        {
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/ShoesDetails_Size/add-shoesdetails_size?quantity={shoesDetails_Size.Quantity}&ShoesDetailsId={shoesDetails_Size.ShoesDetailsId}&SizeID={shoesDetails_Size.SizeID}";
            var response = await httpClient.PostAsync(apiUrl, null);
            return RedirectToAction("GetAllShoesDetails_Size");
        }

        [HttpGet]
        public async Task<IActionResult> EditShoesDetails_Size(Guid id)
        {
            ShoesDetails_Size shoesDetails_Size = repos.GetAll().FirstOrDefault(c => c.ID == id);
            using (ShopDBContext dBContext = new ShopDBContext())
            {
                var shoesDT = dBContext.ShoesDetails.Where(c => c.Status == 0).ToList();
                SelectList selectListShoesDT = new SelectList(shoesDT, "ShoesDetailsId", "ShoesDetailsCode");
                ViewBag.ShoesDTList = selectListShoesDT;

                var size = dBContext.Sizes.Where(c => c.Status == 0).ToList();
                SelectList selectListSize = new SelectList(size, "SizeID", "Name");
                ViewBag.SizeList = selectListSize;
            }
            return View(shoesDetails_Size);
        }
        public async Task<IActionResult> EditShoesDetails_Size(ShoesDetails_Size shoesDetails_Size)
        {
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/ShoesDetails_Size/edit-shoesdetails_size?id={shoesDetails_Size.ID}&quantity={shoesDetails_Size.Quantity}&ShoesDetailsId={shoesDetails_Size.ShoesDetailsId}&SizeID={shoesDetails_Size.SizeID}";
            var response = await httpClient.PutAsync(apiUrl, null);
            return RedirectToAction("GetAllShoesDetails_Size");
        }
    }
}
