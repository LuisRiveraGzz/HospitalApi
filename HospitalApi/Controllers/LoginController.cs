using HospitalApi.Helpers;
using HospitalApi.Models.DTOs;
using HospitalApi.Models.Validators;
using HospitalApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HospitalApi.Controllers
{
    public class LoginController(UsuariosRepository usuariosRepository, JwtHelper helper) : Controller
    {
        public UsuariosRepository UsuariosRepos { get; } = usuariosRepository;

        [HttpPost("Login")]
        public IActionResult Login(LoginDTO login)
        {
            LoginDTOValidator validador = new();
            var result = validador.Validate(login);
            if (result.IsValid)
            {
                var user = UsuariosRepos.GetUsuario(login.Usuario);

                if (user != null)
                {
                    var token = helper.GetToken(user.Nombre, (user.Rol == 1 ? "Administrador" : "Doctor"), user.Id);
                    return Ok(token);
                }
            }
            return BadRequest("Usuario o contraseña incorrecta");
        }
    }
}
