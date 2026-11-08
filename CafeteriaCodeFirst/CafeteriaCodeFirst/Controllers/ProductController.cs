using Microsoft.AspNetCore.Mvc;

namespace CafeteriaCodeFirst.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
