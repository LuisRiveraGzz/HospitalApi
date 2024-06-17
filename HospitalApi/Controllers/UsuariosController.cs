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
        [HttpGet("{id:int}")]
        public IActionResult GetUsuario(int id) => Ok(usuariosRepos.Get(id));

        [HttpPost("/Agregar")]
        public IActionResult Post(UsuarioDTO dto)
        {
            UsuarioDTOValidator validador = new();
            var result = validador.Validate(dto);
            if (result.IsValid)
            {
                var anterior = usuariosRepos.GetUsuario(dto.Nombre);
                if (anterior != null)
                {
                    //verificar que el rol sea diferente, si es el mismo rol
                    //entonces mostrar un mensaje que diga que hay un usuario con ese rol registrado
                    if (anterior.Rol == dto.Rol)
                    {

                        return anterior.Rol == 1 ?
                            Conflict("Ya hay un administrador registrado con ese nombre") :
                            Conflict("Ya hay un doctor registrado con ese nombre");
                    }
                }
                Usuario user = new()
                {
                    Id = 0,
                    Nombre = dto.Nombre,
                    Contraseña = Encriptacion.StringToSHA512(dto.Contraseña),
                    Rol = dto.Rol
                };
                usuariosRepos.Insert(user);
                return user.Rol == 1 ? Ok("Administrador agregado") : Ok("Doctor agregado");
            }
            return BadRequest("Ingresa un Usuario Valido");
        }
        [HttpPut("/Editar")]
        public IActionResult Put(UsuarioDTO dto)
        {
            UsuarioDTOValidator validador = new();
            var result = validador.Validate(dto);
            if (result.IsValid)
            {
                var anterior = usuariosRepos.GetUsuario(dto.Nombre);
                if (anterior != null)
                {
                    anterior.Nombre = dto.Nombre;
                    anterior.Contraseña = Encriptacion.StringToSHA512(dto.Contraseña);
                    usuariosRepos.Insert(anterior);
                    return Ok("Usuario agregado");
                }
            }
            return BadRequest("Ingresa un Usuario Valido");
        }

        [HttpDelete("Eliminar/{id:int}")]
        public IActionResult Delete(int id)
        {
            var user = usuariosRepos.Get(id);
            if (user != null)
            {
                usuariosRepos.Delete(user);
                return Ok("Usuario eliminado.");
            }
            return NotFound("Usuario no eliminado");
        }
    }
}
