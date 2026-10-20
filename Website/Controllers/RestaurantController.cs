using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
    public class RestaurantController : Controller
    {
        public IActionResult Index(int id)
        {
            return View(id);
        }
    }
}