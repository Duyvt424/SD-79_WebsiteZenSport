using AppView.Models;
using AppData.Models;
using Newtonsoft.Json;

namespace AppView.Services
{
    public static class HistoryChatSessionService
    {
        public static List<HistoryChat> GetObjFromSession(ISession session, string key)
        {
            // Lấy string json từ Session
            var jsonData = session.GetString(key);
            if (jsonData == null) return new List<HistoryChat>();//Nếu null trả về một list rỗng
            //Chuyển đổi dl vừa lấy đc sang dạng mong muốn
            var ShoesDetails = JsonConvert.DeserializeObject<List<HistoryChat>>(jsonData);
            return ShoesDetails;
        }
        public static void SetObjToSession(ISession session, string key, object values)
        {
            var jsonData = JsonConvert.SerializeObject(values);
            session.SetString(key, jsonData);
        }
    }
}
