using AppData.Models;
using AppView.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace AppView.Controllers
{
    public class CheckOutController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> SubmitInfo()
        {
            return View();
        }
        public async Task<IActionResult> SubmitInfo(int ServiceId, int FromDistrictId, int ToDistrictId, int ToWardCode, int InsuranceValue, int Weight, int Length, int Width, int Height)
        {
            string token = "6092d580-fde7-11ed-a967-deea53ba3605";
            int shopId = 4185066;
            var client = new HttpClient();
            string apiUrl = $"https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee?service_id={ServiceId}&insurance_value={InsuranceValue}&coupon&to_ward_code={ToWardCode}&to_district_id={ToDistrictId}&from_district_id={FromDistrictId}&weight={Weight}&length={Length}&width={Width}&height={Height}";
            client.DefaultRequestHeaders.Add("token", token);
            client.DefaultRequestHeaders.Add("shop_id", shopId.ToString());
            var response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<JObject>(responseData);

                var result = new ProvinceViewModels
                {
                    Total = (int)apiResponse["data"]["total"],
                    ServiceFee = (int)apiResponse["data"]["service_fee"],
                    InsuranceFee = (int)apiResponse["data"]["insurance_fee"],
                    // Cập nhật các thuộc tính khác tương ứng...
                };
                return View(result);
            }
            else
            {
                return Content("Error");
            }
        }
        //[HttpGet]
        //public async Task<List<ProvinceViewModels>> GetProvince(string token)
        //{
        //    string apiUrl = "https://online-gateway.ghn.vn/shiip/public-api/master-data/province";
        //    var httpClient = new HttpClient();
        //    httpClient.DefaultRequestHeaders.Add("token", token);
        //    var response = await httpClient.GetAsync(apiUrl);
        //    var responseData = await response.Content.ReadAsStringAsync();
        //    // Parse JSON object directly to a dynamic object
        //    dynamic apiResponse = JsonConvert.DeserializeObject(responseData);
        //    // Access the "data" property of the dynamic object
        //    var data = apiResponse.data;
        //    // Deserialize the "data" property to a list of ProvinceViewModels
        //    var provinces = JsonConvert.DeserializeObject<List<ProvinceViewModels>>(data.ToString());
        //    return provinces;
        //}
    }
}
