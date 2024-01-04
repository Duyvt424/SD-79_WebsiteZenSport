using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppView.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AppView.Controllers
{ 
    public class ImageController : Controller
    {
        private readonly IAllRepositories<Image> _repos;
        private readonly IAllRepositories<ShoesDetails> _shoesDTRepos;
        ShopDBContext _dbContext = new ShopDBContext();
        DbSet<Image> _images;
        DbSet<ShoesDetails> _shoesDT;

        public ImageController()
        {
            _images = _dbContext.Images;
            AllRepositories<Image> all = new AllRepositories<Image>(_dbContext, _images);
            _repos = all;

            _shoesDT = _dbContext.ShoesDetails;
            AllRepositories<ShoesDetails> shoesDTAll = new AllRepositories<ShoesDetails>(_dbContext, _shoesDT);
            _shoesDTRepos = shoesDTAll;
        }

        private bool CheckUserRole()
        {
            var CustomerRole = HttpContext.Session.GetString("UserId");
            var EmployeeNameSession = HttpContext.Session.GetString("RoleName");
            var EmployeeName = EmployeeNameSession != null ? EmployeeNameSession.Replace("\"", "") : null;
            if (CustomerRole != null || EmployeeName != "Quản lý")
            {
                return false;
            }
            return true;
        }
        private string GenerateImageCode()
        {
            var lastImage = _dbContext.Images.OrderByDescending(c => c.ImageCode).FirstOrDefault();
            if (lastImage != null)
            {
                var lastNumber = int.Parse(lastImage.ImageCode.Substring(2));
                var nextNumber = lastNumber + 1;
                var newImageCode = "IG" + nextNumber.ToString("D3");
                return newImageCode;
            }
            return "IG001";
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImge()
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            string apiUrl = "https://localhost:7036/api/Image/get-image";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var images = JsonConvert.DeserializeObject<List<Image>>(apiData);
            var shoesDT = _shoesDTRepos.GetAll();
            // Tạo danh sách ViewModel với thông tin Supplier và Material
            var imageViewModels = images.Select(images => new ImageViewModel
            {
                ImageID = images.ImageID,
                ImageCode = images.ImageCode,
                Name = images.Name,
                Image1 = images.Image1,
                Image2 = images.Image2,
                Image3 = images.Image3,
                Image4 = images.Image4,
                DateCreated = images.DateCreated,
                Status = images.Status,
                ShoesDetailsCode = shoesDT.FirstOrDefault(s => s.ShoesDetailsId == images.ShoesDetailsID)?.ShoesDetailsCode,
            }).ToList();
            return View(imageViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            using (ShopDBContext shopDBContext = new ShopDBContext())
            {
                var shoes = shopDBContext.ShoesDetails.Where(c => c.Status == 0).ToList();
                SelectList selectListShoesDT = new SelectList(shoes, "ShoesDetailsId", "ShoesDetailsCode");
                ViewBag.ShoesDTList = selectListShoesDT;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Image image, [Bind(Prefix = "imageFile1")] IFormFile imageFile1, [Bind(Prefix = "imageFile2")] IFormFile imageFile2, [Bind(Prefix = "imageFile3")] IFormFile imageFile3, [Bind(Prefix = "imageFile4")] IFormFile imageFile4)
        {
            if (imageFile1 != null && imageFile1.Length > 0) // Kiểm tra tệp tin ảnh 1
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", imageFile1.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile1.CopyToAsync(stream);
                }
                image.Image1 = imageFile1.FileName;
            }

            if (imageFile2 != null && imageFile2.Length > 0) // Kiểm tra tệp tin ảnh 2
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", imageFile2.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile2.CopyToAsync(stream);
                }
                image.Image2 = imageFile2.FileName;
            }

            if (imageFile3 != null && imageFile3.Length > 0) // Kiểm tra tệp tin ảnh 3
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", imageFile3.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile3.CopyToAsync(stream);
                }
                image.Image3 = imageFile3.FileName;
            }

            if (imageFile4 != null && imageFile4.Length > 0) // Kiểm tra tệp tin ảnh 4
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", imageFile4.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile4.CopyToAsync(stream);
                }
                image.Image4 = imageFile4.FileName;
            }
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Image/create-image?imageCode={GenerateImageCode()}&name={image.Name}&image1={imageFile1.FileName}&image2={imageFile2.FileName}&image3={imageFile3.FileName}&image4={imageFile4.FileName}&status={image.Status}&DateCreated={image.DateCreated.ToString("yyyy-MM-ddTHH:mm:ss")}&shoesDetailsId={image.ShoesDetailsID}";
            var response = await httpClient.PostAsync(apiUrl, null);
            return RedirectToAction("GetAllImge");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            Image image = _repos.GetAll().FirstOrDefault(c => c.ImageID == id);
            using (ShopDBContext shopDBContext = new ShopDBContext())
            {
                var shoes = shopDBContext.ShoesDetails.Where(c => c.Status == 0).ToList();
                SelectList selectListShoesDT = new SelectList(shoes, "ShoesDetailsId", "ShoesDetailsCode");
                ViewBag.ShoesDTList = selectListShoesDT;
            }
            return View(image);
        }
        public async Task<IActionResult> Edit(Image image,  [Bind(Prefix = "imageFile1")] IFormFile imageFile1, [Bind(Prefix = "imageFile2")] IFormFile imageFile2, [Bind(Prefix = "imageFile3")] IFormFile imageFile3, [Bind(Prefix = "imageFile4")] IFormFile imageFile4)
        {
            if (imageFile1 != null && imageFile1.Length > 0) // Kiểm tra tệp tin ảnh 1
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", imageFile1.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile1.CopyToAsync(stream);
                }
                image.Image1 = imageFile1.FileName;
            }

            if (imageFile2 != null && imageFile2.Length > 0) // Kiểm tra tệp tin ảnh 2
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", imageFile2.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile2.CopyToAsync(stream);
                }
                image.Image2 = imageFile2.FileName;
            }

            if (imageFile3 != null && imageFile3.Length > 0) // Kiểm tra tệp tin ảnh 3
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", imageFile3.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile3.CopyToAsync(stream);
                }
                image.Image3 = imageFile3.FileName;
            }

            if (imageFile4 != null && imageFile4.Length > 0) // Kiểm tra tệp tin ảnh 4
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", imageFile4.FileName);
                using (var stream = new FileStream(path, FileMode.Create))//
                {
                    await imageFile4.CopyToAsync(stream);
                }
                image.Image4 = imageFile4.FileName;
            }
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Image/update-image?id={image.ImageID}&imageCode={image.ImageCode}&name={image.Name}&image1={imageFile1.FileName}&image2={imageFile2.FileName}&image3={imageFile3.FileName}&image4={imageFile4.FileName}&status={image.Status}&DateCreated={image.DateCreated.ToString("yyyy-MM-ddTHH:mm:ss")}&shoesDetailsId={image.ShoesDetailsID}";
            var response = await httpClient.PutAsync(apiUrl, null);
            return RedirectToAction("GetAllImge");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            var image = _repos.GetAll().FirstOrDefault(c => c.ImageID == id);
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Image/delete-image?id={id}";
            var response = await httpClient.DeleteAsync(apiUrl);
            return RedirectToAction("GetAllImge");
        }
        public async Task<IActionResult> FindImage(string searchQuery)
        {
            if (CheckUserRole() == false)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            var images = _repos.GetAll().Where(c => c.ImageCode.ToLower().Contains(searchQuery.ToLower()) || c.Name.ToLower().Contains(searchQuery.ToLower()));
            var shoesDT = _shoesDTRepos.GetAll();
            // Tạo danh sách ViewModel với thông tin Supplier và Material
            var imageViewModels = images.Select(images => new ImageViewModel
            {
                ImageID = images.ImageID,
                ImageCode = images.ImageCode,
                Name = images.Name,
                Image1 = images.Image1,
                Image2 = images.Image2,
                Image3 = images.Image3,
                Image4 = images.Image4,
                DateCreated = images.DateCreated,
                Status = images.Status,
                ShoesDetailsCode = shoesDT.FirstOrDefault(s => s.ShoesDetailsId == images.ShoesDetailsID)?.ShoesDetailsCode,
            }).ToList();
            return View(imageViewModels);
        }
    }
}
