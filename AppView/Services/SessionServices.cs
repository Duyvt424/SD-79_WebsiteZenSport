using AppData.Models;
using AppView.Models;
using Newtonsoft.Json;
namespace AppView.Services
{
    public static class SessionServices
    {
        public static List<CartItemViewModel> GetObjFromSession(ISession session, string key)
        {
            // Lấy string json từ Session
            var jsonData = session.GetString(key);
            if (jsonData == null) return new List<CartItemViewModel>();//Nếu null trả về một list rỗng
            //Chuyển đổi dl vừa lấy đc sang dạng mong muốn
            var ShoesDetails = JsonConvert.DeserializeObject<List<CartItemViewModel>>(jsonData);
            //Nếu null trả về một list rỗng
            return ShoesDetails;
        }
        public static void SetObjToSession(ISession session, string key, object values)
        {
            var jsonData = JsonConvert.SerializeObject(values);
            session.SetString(key, jsonData);
        }
    }
}
