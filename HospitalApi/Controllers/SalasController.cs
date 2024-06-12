using HospitalApi.Models.DTOs.Sala;
using HospitalApi.Models.Entities;
using HospitalApi.Models.Validators;
using HospitalApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HospitalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalasController(SalasRepository salasRepository) : ControllerBase
    {
        [HttpGet("/GetSalas")]
        public IActionResult GetSalas()
        {
            var salas = salasRepository.GetSalas();
            return Ok(salas);
        }
        [HttpPost("PostSala")]
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
                salasRepository.Insert(newSala);
                return Ok("Se ah agregado la sala");
            }
            return NotFound("La sala no es valida");
        }

        [HttpPut("PutSala")]
        public IActionResult PutSala(SalaDTO dto)
        {
            SalaDTOValidator validador = new();
            if (dto.Doctor != null)
            {
                var result = validador.Validate(dto);
                if (result.IsValid)
                {
                    var sala = salasRepository.GetSala(dto.NumeroSala);
                    if (sala != null)
                    {
                        salasRepository.Update(sala);
                        return Ok("Se ah agregado la sala");
                    }
                }
            }
            return BadRequest("Ingresa el doctor a la sala");
        }
        [HttpDelete("DeleteSala/{id:int}")]
        public IActionResult DeleteSala(int id)
        {
            var sala = salasRepository.Get(id);
            if (sala != null)
            {
                if (sala.Doctor != null)
                {
                    return Conflict("La sala esta en uso");
                }
                salasRepository.Delete(sala);
                return Ok("Se ah eliminado la sala");
            }
            return NotFound("No se ah eliminado la sala");
        }
    }
}
