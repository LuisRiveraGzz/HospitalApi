using Microsoft.AspNetCore.Mvc;

namespace AdminApp.Controllers
{
    public class DoctoresController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
