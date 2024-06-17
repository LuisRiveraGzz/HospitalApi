using HospitalApi.Models.DTOs;
using HospitalApi.Models.Entities;
using HospitalApi.Models.Validators;
using HospitalApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HospitalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalasController(SalasRepository salasRepos, UsuariosRepository usuariosRepos) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetSalas()
        {
            var salas = salasRepos.GetSalas();
            return salas != null ? Ok(salas) : NotFound("No hay salas disponibles");
        }
        [HttpGet("{numerosala:string}")]
        public IActionResult GetSalas(string numerosala)
        {
            var sala = salasRepos.GetSala(numerosala);
            return sala != null ? Ok(sala) : NotFound("No existe la sala");
        }
        [HttpPost("Agregar")]
        public IActionResult PostSala(SalaDTO dto)
        {
            SalaDTOValidator validador = new();
            var result = validador.Validate(dto);
            if (result.IsValid)
            {
                Sala newSala = new()
                {
                    Id = 0,
                    NumeroSala = dto.NumeroSala
                };
                salasRepos.Insert(newSala);
                return Ok("Se ah agregado la sala");
            }
            return NotFound("La sala no es valida");
        }

        [HttpPut("Editar")]
        public IActionResult PutSala(SalaDTO dto)
        {
            if (dto.Doctor != null)
            {
                SalaDTOValidator validador = new();
                var result = validador.Validate(dto);
                if (result.IsValid)
                {
                    var sala = salasRepos.GetSala(dto.NumeroSala);
                    if (sala != null)
                    {
                        salasRepos.Update(sala);
                        return Ok("Se ah agregado la sala");
                    }
                }
            }
            return BadRequest("Ingresa el doctor a la sala");
        }
        [HttpDelete("Eliminar/{id:int}")]
        public IActionResult DeleteSala(int id)
        {
            var sala = salasRepos.Get(id);
            if (sala != null)
            {
                if (sala.Doctor != null)
                {
                    var doctor = usuariosRepos.Get(sala.Doctor ?? 0);
                    //Eliminar el doctor
                    if (doctor != null)
                    {
                        usuariosRepos.Delete(doctor);
                        salasRepos.Delete(sala);
                        return Ok("Se ah eliminado la sala");
                    }
                }
            }
            return NotFound("No se ah eliminado la sala");
        }

    }
}
