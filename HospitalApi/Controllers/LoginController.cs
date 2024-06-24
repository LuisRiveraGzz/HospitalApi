using HospitalApi.Helpers;
using HospitalApi.Models.DTOs;
using HospitalApi.Models.Validators;
using HospitalApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HospitalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(UsuariosRepository usuariosRepository, JwtHelper helper) : Controller
    {
        public UsuariosRepository UsuariosRepos { get; } = usuariosRepository;
        [HttpPost]
        public IActionResult Login(LoginDTO login)
        {
            LoginDTOValidator validador = new();
            var result = validador.Validate(login);
            if (result.IsValid)
            {
                var user = UsuariosRepos.GetAll().FirstOrDefault(x => x.Nombre == login.Usuario
                            && x.Contraseña == Encriptacion.StringToSHA512(login.Contraseña));
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