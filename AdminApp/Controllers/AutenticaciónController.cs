using AdminApp.Models.ViewModels;
using AdminApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdminApp.Controllers
{
    public class AutenticaciónController(AutenticaciónService autenticaciónService) : Controller
    {

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var token = await autenticaciónService.Login(dto);
            if (!string.IsNullOrWhiteSpace(token))
            {
                // Guardar el token en la sesión                                    
                HttpContext.Session.SetString("Token", token);
                //Trabajo en proceso
                // Redirigir a la acción Index del DoctoresController
                return RedirectToAction("Index", "Doctores");
            }
            ModelState.AddModelError("", "Error en la autenticación.");
            return View(dto);
        }
    }
}
