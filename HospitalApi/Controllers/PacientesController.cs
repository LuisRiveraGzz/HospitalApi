using HospitalApi.Models.DTOs;
using HospitalApi.Models.Entities;
using HospitalApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HospitalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController(PacientesRepository pacientesRepos) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPacientes()
        {
            var pacientes = await pacientesRepos.GetPacientes();
            return pacientes != null ? Ok(pacientes) : NotFound("No hay pacientes");
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPacientebyName(int id)
        {
            var paciente = await pacientesRepos.Get(id);
            return paciente != null ? Ok(paciente) : NotFound("No se encontró el paciente");
        }
        [HttpPost("Agregar")]
        public async Task<IActionResult> Post(PacienteDTO dto)
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
                    //se le asigna un id automáticamente
                    await pacientesRepos.Insert(paciente);
                    return Ok("Paciente Agregado");
                }
            }
            return BadRequest("Ingrese su nombre");
        }
        [HttpPut("Editar")]
        public async Task<IActionResult> Put(PacienteDTO dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Nombre))
            {
                if (dto != null)
                {
                    var paciente = await pacientesRepos.Get(dto.Id);
                    if (paciente != null)
                    {
                        paciente.Nombre = dto.Nombre.ToUpper();
                        await pacientesRepos.Update(paciente);
                        return Ok("Paciente Actualizado");
                    }
                }
            }
            return BadRequest("Ingrese su nombre");
        }
        [HttpPost("Eliminar")]
        public async Task<IActionResult> Delete(PacienteDTO dto)
        {
            if (dto.Id == 0)
            {
                return BadRequest();
            }
            var paciente = await pacientesRepos.GetPaciente(dto.Id);

            if (paciente != null)
            {
                if (paciente.Sala.Count > 0)
                {
                    return BadRequest("No puedes eliminar un paciente que esta siendo atendido");
                }
                await pacientesRepos.Delete(paciente);

                return Ok("Paciente eliminado");
            }
            return BadRequest("Ingrese su nombre");
        }
    }
}
