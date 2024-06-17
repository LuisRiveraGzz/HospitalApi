using HospitalApi.Models.DTOs;
using HospitalApi.Models.Entities;
using HospitalApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HospitalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController(Repository<Paciente> pacientesRepos) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetPacientes()
        {
            var pacientes = pacientesRepos.GetAll();
            return pacientes != null ? Ok(pacientes) : NotFound("No hay pacientes");
        }

        [HttpGet("Paciente/{nombre}")]
        public IActionResult GetPacientebyName(string nombre)
        {
            var paciente = pacientesRepos.GetAll()
                .FirstOrDefault(x => x.Nombre == nombre.ToUpper());
            return paciente != null ? Ok(paciente) : NotFound("No se encontro el paciente");
        }
        [HttpPost("Agregar")]
        public IActionResult Post(PacienteDTO dto)
        {

            if (!string.IsNullOrWhiteSpace(dto.Nombre))
            {
                if (dto != null)
                {
                    Paciente paciente = new()
                    {
                        Id = 0,
                        Nombre = dto.Nombre.ToUpper()
                    };
                    //se le asigna un is automaticamente
                    pacientesRepos.Insert(paciente);
                    return Ok("Paciente Agregado");
                }
            }
            return BadRequest("Ingrese su nombre");
        }
        [HttpPut("Editar")]
        public IActionResult Put(PacienteDTO dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Nombre))
            {
                if (dto != null)
                {
                    var paciente = pacientesRepos.Get(dto.Id);
                    if (paciente != null)
                    {
                        paciente.Nombre = dto.Nombre.ToUpper();
                        pacientesRepos.Update(paciente);
                        return Ok("Paciente Actualizado");
                    }
                }
            }
            return BadRequest("Ingrese su nombre");
        }

        [HttpDelete("Eliminar/{id:int}")]
        public IActionResult Delete(int id)
        {
            var paciente = pacientesRepos.Get(id);
            if (paciente != null)
            {
                pacientesRepos.Delete(paciente);
                return Ok("Paciente eliminado");
            }
            return BadRequest("Ingrese su nombre");
        }
    }
}
