using Microsoft.AspNetCore.Mvc;

namespace AdminApp.Services
{
    public class DoctoresController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
