using AdminApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AdminApp.Controllers
{
    public class DoctoresController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Agregar()
        {
            return View();
        }
        public IActionResult Agregar(UsuarioViewModel vm)
        {
            return View(vm);
        }
    }
}
