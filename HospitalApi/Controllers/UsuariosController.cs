using HospitalApi.Helpers;
using HospitalApi.Models.DTOs;
using HospitalApi.Models.Entities;
using HospitalApi.Models.Validators;
using HospitalApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HospitalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController(UsuariosRepository usuariosRepos) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUsuarios() => Ok(usuariosRepos.GetUsuarios());
        [HttpGet("Doctores")]
        public IActionResult GetDoctores() => Ok(usuariosRepos.GetAll().Where(x => x.Rol == 2));
        [HttpGet("Administradores")]
        public IActionResult GetAdministradores() => Ok(usuariosRepos.GetAll().Where(x => x.Rol == 1));
        [HttpGet("{id:int}")]
        public IActionResult GetUsuario(int id) => Ok(usuariosRepos.Get(id));
        [HttpPost("Agregar")]
        public async Task<IActionResult> Post(UsuarioDTO dto)
        {
            UsuarioDTOValidator validador = new();
            var result = validador.Validate(dto);
            if (result.IsValid)
            {
                var anterior = await usuariosRepos.GetUsuario(dto.Nombre);
                if (anterior != null)
                {
                    //verificar que el rol sea diferente, si es el mismo rol
                    //entonces mostrar un mensaje que diga que hay un usuario con ese rol registrado
                    if (anterior.Rol == dto.Rol)
                    {
                        //1 = Administrador
                        //2 = Doctor
                        return anterior.Rol == 1 ?
                            Conflict("Ya hay un administrador registrado con ese nombre.") :
                            Conflict("Ya hay un doctor registrado con ese nombre.");
                    }
                }
                Usuario user = new()
                {
                    Id = 0,
                    Nombre = dto.Nombre,
                    Contraseña = Encriptacion.StringToSHA512(dto.Contraseña),
                    Rol = dto.Rol
                };
                await usuariosRepos.Insert(user);
                return user.Rol == 1 ? Ok("Administrador agregado.") : Ok("Doctor agregado.");
            }
            return BadRequest("Ingresa un Usuario Valido.");
        }
        [HttpPut("Editar")]
        public async Task<IActionResult> Put(UsuarioDTO dto)
        {
            UsuarioDTOValidator validador = new();
            var result = validador.Validate(dto);
            if (result.IsValid)
            {
                var anterior = await usuariosRepos.Get(dto.Id);
                if (anterior != null)
                {
                    anterior.Nombre = dto.Nombre;
                    anterior.Contraseña = Encriptacion.StringToSHA512(dto.Contraseña);
                    await usuariosRepos.Update(anterior);
                    return anterior.Rol == 1 ? Ok("Administrador editado.") : Ok("Doctor editado.");
                }
            }
            return BadRequest("Ingresa un Usuario Valido.");
        }
        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await usuariosRepos.Get(id);
            if (user != null)
            {
                sbyte rol = user.Rol;
                await usuariosRepos.Delete(user);
                return rol == 1 ? Ok("Administrador eliminado.") : Ok("Doctor eliminado.");
            }
            return NotFound("Usuario no eliminado.");
        }
    }
}
