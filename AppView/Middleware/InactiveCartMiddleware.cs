using AppData.Models;
using AppView.Models;
using AppView.Services;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AppView.Middleware
{
    public class InactiveCartMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ShopDBContext _dbContext;

        public InactiveCartMiddleware(RequestDelegate next)
        {
            _next = next;
            _dbContext = new ShopDBContext();
        }

        public async Task Invoke(HttpContext context)
        {
            // Lấy giá trị SessionCreationTime từ session (nếu có)
            var sessionCreationTimeString = context.Session.GetString("SessionCreationTime");

            // Nếu giá trị không tồn tại, hoặc không thể chuyển đổi thành DateTime, thì đặt giá trị mới
            if (string.IsNullOrEmpty(sessionCreationTimeString) || !DateTime.TryParse(sessionCreationTimeString, out DateTime sessionCreationTime))
            {
                sessionCreationTime = DateTime.Now;
                context.Session.SetString("SessionCreationTime", sessionCreationTime.ToString());
            }

            // Kiểm tra và xóa giỏ hàng nếu cần
            if (CheckAndCleanInactiveCart(context, sessionCreationTime))
            {
                // Nếu giỏ hàng được xóa, đặt lại giá trị mới cho SessionCreationTime
                sessionCreationTime = DateTime.Now;
                context.Session.SetString("SessionCreationTime", sessionCreationTime.ToString());
            }

            // Chuyển giao cho middleware tiếp theo trong pipeline
            await _next(context);
        }

        private bool CheckAndCleanInactiveCart(HttpContext context, DateTime sessionCreationTime)
        {
            var currentTime = DateTime.Now;

            if ((currentTime - sessionCreationTime).TotalDays > 30)
            {
                // Xóa giỏ hàng
                var cartItems = SessionServices.GetObjFromSession(context.Session, "Cart") as List<CartItemViewModel>;
                if (cartItems != null && cartItems.Any())
                {
                    // Cộng lại số lượng tồn cho sản phẩm
                    foreach (var cartItem in cartItems)
                    {
                        var size = _dbContext.Sizes.First(c => c.Name == cartItem.Size).SizeID;
                        var shoesDT_Size = _dbContext.ShoesDetails_Sizes.FirstOrDefault(c => c.ShoesDetailsId == cartItem.ShoesDetailsID && c.SizeID == size);
                        if (shoesDT_Size != null)
                        {
                            shoesDT_Size.Quantity += cartItem.Quantity;
                            _dbContext.Update(shoesDT_Size);
                        }
                    }
                    _dbContext.SaveChanges();
                    context.Session.Remove("Cart");

                    // Giỏ hàng đã được xóa
                    return true;
                }
            }

            // Giỏ hàng không được xóa
            return false;
        }
    }
}
