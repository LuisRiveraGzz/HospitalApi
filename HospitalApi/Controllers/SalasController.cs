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
        [HttpGet("{numerosala}")]
        public IActionResult GetSala(string numerosala)
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
                var anterior = salasRepos.GetSala(dto.NumeroSala);
                if (anterior != null)
                {
                    return Conflict("Ya hay una sala con el mismo numero de sala");
                }
                Sala newSala = new()
                {
                    Id = 0,
                    NumeroSala = dto.NumeroSala
                };
                salasRepos.Insert(newSala);
                return Ok("Se ha agregado la sala");
            }
            return BadRequest("La sala no es valida");
        }
        [HttpPut("Editar")]
        public IActionResult PutSala(SalaDTO dto)
        {
            SalaDTOValidator validador = new();
            var result = validador.Validate(dto);
            if (result.IsValid)
            {
                var sala = salasRepos.Get(dto.Id);
                if (sala != null)
                {
                    if (dto.Doctor == 1)
                    {
                        return BadRequest("No puedes asignar administradores a las salas");
                    }
                    sala.NumeroSala = dto.NumeroSala;
                    sala.Doctor = dto.Doctor;
                    salasRepos.Update(sala);
                    return Ok("Sala actualizada");
                }
            }
            return BadRequest("Ingresa el doctor a la sala");
        }
        [HttpPut("QuitarDoctor/{id:int}")]
        public IActionResult QuitarDoctor(int id)
        {
            var sala = salasRepos.Get(id);
            if (sala != null)
            {
                if (sala.Doctor != null)
                {
                    sala.Doctor = null!;
                    salasRepos.Update(sala);
                    return Ok("Se ha sacado al doctor de la sala");
                }
            }
            return NotFound("No hay doctor en la sala");
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
                    if (doctor != null)
                    {
                        QuitarDoctor(sala.Id);
                    }
                }
                salasRepos.Delete(sala);
                return Ok("Se ha eliminado la sala");
            }
            return NotFound("No se ha eliminado la sala");
        }
    }
}
