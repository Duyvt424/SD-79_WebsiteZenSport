# Zensport Sneakers

# Giới thiệu
Zensport Sneakers là một website thương mại điện tử bán giày thể thao, được thiết kế và phát triển bởi nhóm 4 thành viên và là một đồ án tốt nghiệp. Ứng dụng giúp quản lý sản phẩm, đơn hàng, khách hàng, mã giảm giá, thanh toán trực tuyến thông qua VNPay, phân hạng khách hàng và chatbot AI. Tất cả đều dựa trên nhu cầu thực tiễn và hơn hết là hướng tới một trải nghiệm tốt nhất cho người dùng.

# Công nghệ sử dụng
* ASP.NET Core MVC
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* jQuery, Bootstrap

# Công cụ sử dụng
* Visual Studio 2022
* SQL Server Management Studio (SSMS)
* Postman, Swagger
* GitHub
* Sourcetree

# Chức năng
Zensport cung cấp các chức năng chính như:

* Quản lý sản phẩm, khách hàng, hóa đơn, coupon
* Đăng nhập bằng Google
* Tính phí vận chuyển bằng API Giao Hàng Nhanh
* Thanh toán trực tuyến qua VNPAY
* Giỏ hàng & đặt hàng
* Theo dõi trạng thái đơn hàng
* Phân quyền người dùng
* Phân hạng khách hàng
* Chatbot AI tư vấn sản phẩm
* Dashboard thống kê cho Admin

#  Hướng dẫn sử dụng
## 1. Clone dự án từ GitHub
```bash
git clone https://github.com/Duyvt424/SD-79_WebsiteZenSport.git
```
Mở solution bằng Visual Studio
## 2. Cấu hình cơ sở dữ liệu SQL Server 
* Cài đặt SQL Server
* Chạy file DATN_A.sql trong thư mục Database để tạo CSDL
## 3. Cập nhật chuỗi kết nối (Connection String)
*  Tại Program.cs trong project AppAPI hãy thay thế bằng chuỗi kết nối của bạn:
```bash
builder.Services.AddDbContext<ShopDBContext>(options =>
{
    options.UseSqlServer("Your_SQL_Connection_String");
});
```
* Làm tương tự với chuỗi kết nối trong ShopDBContext.cs thuộc AppData\Models
## 4. Cấu hình Google OAuth
* Tạo project trên Google Cloud Console
* Kích hoạt OAuth consent screen
* Lấy ClientId và ClientSecret, thêm vào appsettings.json:
```bash
"GoogleKeys": {
  "ClientId": "your-client-id",
  "ClientSecret": "your-client-secret"
}
```
## 4. Run project
* Build cả AppAPI và AppView
* Chạy project và truy cập qua localhost của bạn 

# Nhóm thực hiện dự án

| STT | Họ và tên         | MSSV     | Chức vụ | Github                        |
|-----|-------------------|----------|---------|-------------------------------|
| 1   | Vũ Tường Duy      | PH24890  | **Leader** | [Duyvt424](https://github.com/Duyvt424) |
| 2   | Đoàn Thanh Huyền| PH24484   | Member  | [DoanHuyen250894](https://github.com/DoanHuyen250894) |
| 3   |  Hà Kiều Anh| PH24928   | Member  | [anhhkph24928](https://github.com/anhhkph24928) |
| 4   |  Lương Hoàng Nam| PH24250   | Member  | [HNamtq](https://github.com/HNamtq) |
