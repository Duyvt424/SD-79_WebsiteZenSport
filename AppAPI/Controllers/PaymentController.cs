using AppAPI.cofig;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    public class PaymentController : Controller
    {/*
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("api/payment")]
        public string paymentOrder()
        {
            //Get Config Info
            
            string vnp_Returnurl = "https://localhost:7120/";//URL nhan ket qua tra ve 
            string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = "N36LJNQF"; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = "YNDXCJPFDQAQVRENXIJSRVUGPYGVTYAN"; //Secret Key

            //Get payment input
            //OrderInfo order = new OrderInfo();
            //order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
            //order.Amount = 100000; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
            //order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending" khởi tạo giao dịch chưa có IPN
            //order.CreatedDate = DateTime.Now;
            //Save order to db

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            Utils utils = new Utils();
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (100000 * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMdd"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_BankCode", "NCB");
            vnpay.AddRequestData("vnp_IpAddr", utils.GetIpAddress(_httpContextAccessor));
            // Lấy thời điểm hiện tại
            DateTime currentTime = DateTime.Now;

            // Đặt thời gian sống là 30 phút từ thời điểm hiện tại
            DateTime expireDate = currentTime.AddMinutes(30);

            // Chuyển đổi expireDate thành chuỗi theo định dạng bạn cần (ví dụ: "yyyy-MM-dd HH:mm:ss")
            string formattedExpireDate = expireDate.ToString("yyyyMMdd");

            // Thêm dữ liệu vào vnpay
            vnpay.AddRequestData("vnp_ExpireDate", formattedExpireDate);

            vnpay.AddRequestData("vnp_Locale", "vn");
            
            
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + DateTime.Now.ToString("yyyyMMdd")*//*Ticks*//*);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", DateTime.Now.ToString("yyyyMMdd")*//*.Ticks.ToString()*//*); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

            //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            //Response.Redirect(paymentUrl);
            return paymentUrl;*/
        //  }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("api/payment")]
        public string PaymentOrder([FromQuery(Name = "totalPayment")] string totalPayment)
        {
            try
            {   
                Double total = Double.Parse(totalPayment);
                // Get Config Info
                string vnp_Returnurl = "https://localhost:7120/";
                string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
                string vnp_TmnCode = "FCDTXZ7R";
                string vnp_HashSecret = "CFXSPSWTFKNDMMSUBRALHRJAKEPAKBUE";

                // Build URL for VNPAY
                VnPayLibrary vnpay = new VnPayLibrary();
                Utils utils = new Utils();

                // Add payment request data
                vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
                vnpay.AddRequestData("vnp_Command", "pay");
                vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                vnpay.AddRequestData("vnp_Amount", (total * 100).ToString());
                vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CurrCode", "VND");
                vnpay.AddRequestData("vnp_BankCode", "NCB");
                vnpay.AddRequestData("vnp_IpAddr", utils.GetIpAddress(_httpContextAccessor));

                DateTime currentTime = DateTime.Now;
                DateTime expireDate = currentTime.AddMinutes(30);
                string formattedExpireDate = expireDate.ToString("yyyyMMddHHmmss");
                vnpay.AddRequestData("vnp_ExpireDate", formattedExpireDate);

                vnpay.AddRequestData("vnp_Locale", "vn");
                vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_OrderType", "other");
                vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
                vnpay.AddRequestData("vnp_TxnRef", DateTime.Now.ToString("yyyyMMddHHmmss"));

                string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

                // Log the payment URL for debugging
                // log.InfoFormat("VNPAY URL: {0}", paymentUrl);

                return paymentUrl;
            }
            catch (Exception ex)
            {
                // Log any exceptions for debugging
                // log.Error("An error occurred while generating VNPAY URL", ex);
                return "Error: " + ex.Message; // Or handle the error as needed
            }
        }
    }
}
