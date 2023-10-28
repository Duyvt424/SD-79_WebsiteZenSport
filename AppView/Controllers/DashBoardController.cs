using Microsoft.AspNetCore.Mvc;

namespace AppView.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult index()
        {
            return View();
        }
        public IActionResult tables()
        {
            return View();
        }
    }
}
