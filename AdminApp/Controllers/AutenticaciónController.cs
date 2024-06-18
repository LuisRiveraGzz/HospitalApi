﻿using AdminApp.Models.ViewModels;
using AdminApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdminApp.Controllers
{
    public class AutenticaciónController(AutenticaciónService autenticaciónService) : Controller
    {

        // GET: /api/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /api/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel dto)
        {
            if (!ModelState.IsValid)
            {
                // Volver a mostrar el formulario con errores de validación
                return View(dto);
            }

            var token = await autenticaciónService.Login(dto);
            if (!string.IsNullOrWhiteSpace(token))
            {
                // Guardar el token en la sesión
                HttpContext.Session.SetString("Token", token);
                //Trabajo en proceso
                // Redirigir a la acción Index del DoctoresController
                //return RedirectToAction("Index", "Doctores");
            }
            ModelState.AddModelError("", "Error en la autenticación.");
            return View(dto);
        }
    }
}
