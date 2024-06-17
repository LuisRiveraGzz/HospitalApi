using HospitalApi.Models.DTOs;
using HospitalApi.Models.Entities;
using HospitalApi.Models.Validators;
using HospitalApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HospitalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalasController(SalasRepository salasRepos, Repository<Paciente> pacientesRepos, UsuariosRepository usuariosRepository) : ControllerBase
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
        [HttpPut("UtilizarSala/{id}")]
        public IActionResult UtilizarSala(int id)
        {
            var sala = salasRepos.Get(id);
            if (sala != null)
            {
                if (sala.Estado == 0)//Inactiva
                {
                    sala.Estado++;//Activa
                    salasRepos.Update(sala);
                    return Ok("La sala se esta utilizando correctamente");
                }
                return BadRequest("La sala esta siendo utilizada");
            }
            return NotFound("No se ah encontrado la sala");
        }
        [HttpPut("InutilizarSala/{id}")]
        public IActionResult InutilizarSala(int id)
        {
            var sala = salasRepos.Get(id);
            if (sala != null)
            {
                if (sala.Estado == 1)//Activa
                {
                    sala.Estado--;//Inactiva
                    salasRepos.Update(sala);
                    return Ok("La sala se esta disponible");
                }
                return BadRequest("La sala esta siendo utilizada");
            }
            return NotFound("No se ah encontrado la sala");
        }
        [HttpPut("Sala/{idSala:int}/AsignarDoctor/{doctor:int}")]
        public IActionResult AsignarDoctor(int idSala, int doctor)
        {
            var sala = salasRepos.Get(idSala);
            if (sala != null)
            {
                if (sala.Doctor != null)
                {
                    if (sala.Estado == 0)
                    {
                        var doc = usuariosRepository.Get(doctor);
                        if (doc != null && doc.Rol == 2)
                        {

                            sala.Doctor = doctor;
                            salasRepos.Update(sala);
                            return Ok("Se ha asignado el doctor de la sala");
                        }
                    }
                    return Conflict("La sala esta en uso");
                }
                return NotFound("No se ah encontrado al doctor");
            }
            return NotFound("No se ah asignado el doctor a la sala");
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
        [HttpPut("AsignarPaciente")]
        public IActionResult AsignarPaciente(int idsala, int idpaciente)
        {
            var sala = salasRepos.Get(idsala);
            if (sala == null)
            {
                return NotFound("No se ah encontrado la sala");
            }
            var paciente = pacientesRepos.Get(idpaciente);
            if (paciente == null)
            {
                return NotFound("No se ah encontrado al paciente");
            }

            if (sala.Estado == 0)//Inactiva
            {
                sala.Paciente = idpaciente;
                salasRepos.Update(sala);
                return Ok("");
            }
            return Conflict("La sala esta en uso");
        }
        [HttpPut("QuitarPaciente/{idsala:int}")]
        public IActionResult QuitarPaciente(int idsala)
        {
            var sala = salasRepos.Get(idsala);
            if (sala == null)
            {
                return NotFound("No se ah encontrado la sala");
            }
            sala.Paciente = null!;
            salasRepos.Update(sala);
            return Ok("");
        }
        [HttpDelete("Eliminar/{id:int}")]
        public IActionResult DeleteSala(int id)
        {
            var sala = salasRepos.Get(id);
            if (sala != null)
            {
                if (sala.Doctor != null)
                {
                    QuitarDoctor(sala.Id);
                }
                salasRepos.Delete(sala);
                return Ok("Se ha eliminado la sala");
            }
            return NotFound("No se ha eliminado la sala");
        }
    }
}
