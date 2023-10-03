using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AppView.Controllers
{ 
    public class ImageController : Controller
    {
        private readonly IAllRepositories<Image> _repos;
        ShopDBContext _dbContext = new ShopDBContext();
        DbSet<Image> _images;

        public ImageController()
        {

            _images = _dbContext.Images;
            AllRepositories<Image> all = new AllRepositories<Image>(_dbContext, _images);
            _repos = all;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllImge()
        {
            string apiUrl = "https://localhost:7036/api/Image";
            var httpClient = new HttpClient(); // tạo ra để callApi
            var response = await httpClient.GetAsync(apiUrl);// Lấy dữ liệu ra
                                                             // Lấy dữ liệu Json trả về từ Api được call dạng string
            string apiData = await response.Content.ReadAsStringAsync();
            // Lấy kqua trả về từ API
            // Đọc từ string Json vừa thu được sang List<T>
            var images = JsonConvert.DeserializeObject<List<Image>>(apiData);
            return View(images);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            using (ShopDBContext shopDBContext = new ShopDBContext())
            {
                var shoes = shopDBContext.ShoesDetails.ToList();
                SelectList selectListShoesDT = new SelectList(shoes, "ShoesDetailsId", "ShoesDetailsId");
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

            if (_repos.AddItem(image))
            {
                return RedirectToAction("GetAllImge");
            }
            else
            {
                return BadRequest();
            }///
            /*var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7036/api/Image/Create-Image?Name={image.Name}&Image1={image.Image1}&Image2={image.Image2}&Image3={image.Image3}&Image4={image.Image4}&Status={image.Status}&ShoesDetailsID={image.ShoesDetailsID}";
            var response = await httpClient.PostAsync(apiUrl, null);
            return RedirectToAction("GetAllImge");*/
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {

            Image image = _repos.GetAll().FirstOrDefault(c => c.ImageID == id);
            using (ShopDBContext shopDBContext = new ShopDBContext())
            {
                var shoes = shopDBContext.ShoesDetails.ToList();
                SelectList selectListShoesDT = new SelectList(shoes, "ShoesDetailsId", "ShoesDetailsId");
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

            if (_repos.EditItem(image))
            {
                return RedirectToAction("GetAllImge");
            }
            else return BadRequest();
            /*  var httpClient = new HttpClient();
              string apiUrl = $"https://localhost:7036/api/Image/Update-Image?id={image.ImageID}&Name={image.Name}&Image1={image.Image1}&Image2={image.Image2}&Image3={image.Image3}&Image4={image.Image4}&Status={image.Status}&ShoesDetailsID={image.ShoesDetailsID}";
              var response = await httpClient.PutAsync(apiUrl, null);
              return RedirectToAction("GetAllImge");*/
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            Image img = _repos.GetAll().FirstOrDefault(c => c.ImageID == id);
            if (_repos.RemoveItem(img))
            {
                return RedirectToAction("GetAllImge");
            }
            else return BadRequest();
        }
    }
}
