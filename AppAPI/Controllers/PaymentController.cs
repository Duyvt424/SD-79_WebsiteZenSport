using AppAPI.cofig;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PaymentController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("api/payment")]
        public string PaymentOrder([FromQuery(Name = "totalPayment")] string totalPayment, [FromQuery(Name = "billID")] string billID, [FromQuery(Name = "customerID")] string customerID)
        {
            try
            {   
                Double total = Double.Parse(totalPayment);
                // Get Config Info
                string vnp_Returnurl = $"https://localhost:7120/Bill/DetailsBill?billID={billID}&customerID={customerID}";
                string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
                string vnp_TmnCode = "P1QG69PV";
                string vnp_HashSecret = "M16XD5ML4KJV0GKQD4Z207OHJXICU4ZT";

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
