using HospitalApi.Hubs;
using HospitalApi.Models.DTOs;
using HospitalApi.Models.Entities;
using HospitalApi.Models.Validators;
using HospitalApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HospitalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalasController(SalasRepository salasRepos, Repository<Paciente> pacientesRepos,
        UsuariosRepository usuariosRepository, IHubContext<NotificacionHub> _hubContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetSalas()
        {
            var salas = await salasRepos.GetSalas();
            return salas != null ? Ok(salas) : NotFound("No hay salas disponibles");
        }
        [HttpGet("{numerosala}")]
        public async Task<IActionResult> GetSala(string numerosala)
        {
            var sala = await salasRepos.GetSala(numerosala);
            return sala != null ? Ok(sala) : NotFound("No existe la sala");
        }

        [HttpGet("{iddoctor:int}")]
        public async Task<IActionResult> GetSala(int iddoctor)
        {
            var salabydoctor = await salasRepos.GetSalaByDoctor(iddoctor);
            var sala = new SalaDTO()
            {
                Doctor = salabydoctor.Doctor ?? 0,
                Id = salabydoctor.Id,
                NumeroSala = salabydoctor.NumeroSala
            };
            return sala != null ? Ok(sala) : NotFound("No existe la sala");
        }

        [HttpPost("Agregar")]
        public async Task<IActionResult> PostSala(SalaDTO dto)
        {
            SalaDTOValidator validador = new();
            var result = validador.Validate(dto);
            if (result.IsValid)
            {
                var anterior = await salasRepos.GetSala(dto.NumeroSala);
                if (anterior != null)
                {
                    return Conflict("Ya hay una sala con el mismo numero de sala");
                }
                Sala newSala = new()
                {
                    Id = 0,
                    NumeroSala = dto.NumeroSala,
                    Doctor = dto.Doctor
                };
                await salasRepos.Insert(newSala);
                return Ok("Se ha agregado la sala");
            }
            return BadRequest("La sala no es valida");
        }
        [HttpPut("Editar")]
        public async Task<IActionResult> PutSala(SalaDTO dto)
        {
            SalaDTOValidator validador = new();
            var result = validador.Validate(dto);
            if (result.IsValid)
            {
                var sala = await salasRepos.Get(dto.Id);
                if (sala != null)
                {
                    var doc = await usuariosRepository.Get(dto.Doctor);
                    if (doc != null && doc.Rol == 1)
                    {
                        return BadRequest("No puedes asignar administradores a las salas");
                    }
                    sala.NumeroSala = dto.NumeroSala;
                    sala.Doctor = dto.Doctor;
                    await salasRepos.Update(sala);
                    return Ok("Sala actualizada");
                }
            }
            return BadRequest("Ingresa el doctor a la sala");
        }
        [HttpPut("UtilizarSala/{id}")]
        public async Task<IActionResult> UtilizarSala(int id)
        {
            var sala = await salasRepos.Get(id);
            if (sala != null)
            {
                if (sala.Estado == 0)//Inactiva
                {
                    sala.Estado++;//Activa
                    await salasRepos.Update(sala);
                    return Ok("La sala se esta utilizando correctamente");
                }
                return BadRequest("La sala esta siendo utilizada");
            }
            return NotFound("No se ah encontrado la sala");
        }
        [HttpPut("InutilizarSala/{id}")]
        public async Task<IActionResult> InutilizarSala(int id)
        {
            var sala = await salasRepos.Get(id);
            if (sala != null)
            {
                if (sala.Estado == 1)//Activa
                {
                    sala.Estado--;//Inactiva
                    await salasRepos.Update(sala);
                    return Ok("La sala se esta disponible");
                }
                return BadRequest("La sala esta siendo utilizada");
            }
            return NotFound("No se ah encontrado la sala");
        }
        [HttpPut("{idSala:int}/AsignarDoctor/{doctor:int}")]
        public async Task<IActionResult> AsignarDoctor(int idSala, int doctor)
        {
            var sala = await salasRepos.Get(idSala);
            if (sala != null)
            {
                if (sala.Doctor != null)
                {
                    if (sala.Estado == 0)
                    {
                        var doc = await usuariosRepository.Get(doctor);
                        if (doc != null && doc.Rol == 2)
                        {
                            sala.Doctor = doctor;
                            await salasRepos.Update(sala);
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
        public async Task<IActionResult> QuitarDoctor(int id)
        {
            var sala = await salasRepos.Get(id);
            if (sala != null)
            {
                if (sala.Doctor != null)
                {
                    sala.Doctor = null!;
                    await InutilizarSala(sala.Id);
                    await salasRepos.Update(sala);
                    return Ok("Se ha sacado al doctor de la sala");
                }
            }
            return NotFound("No hay doctor en la sala");
        }
        [HttpPut("{idsala:int}/AsignarPaciente/{idpaciente:int}")]
        public async Task<IActionResult> AsignarPaciente(int idsala, int idpaciente)
        {
            var sala = await salasRepos.Get(idsala);
            if (sala == null)
            {
                return NotFound("No se ah encontrado la sala");
            }
            var paciente = await pacientesRepos.Get(idpaciente);
            if (paciente == null)
            {
                return NotFound("No se ah encontrado al paciente");
            }
            if (sala.Estado == 0)//Inactiva
            {
                if (sala.Paciente == null)
                {
                    sala.Paciente = idpaciente;
                    await salasRepos.Update(sala);
                    //Enviar notificación al cliente
                    await _hubContext.Clients.User(paciente.Id.ToString()).
                        SendAsync("RecibirNotificacion", $"Has sido asignado a la sala {sala.NumeroSala}");
                    return Ok("El paciente ah sido asignado correctamente.");
                }
                return sala.Paciente == idpaciente ? Conflict("El paciente ya esta asignado a la sala")
                    : Conflict("Ya hay otro paciente en la sala");
            }
            return Conflict("La sala esta en uso");
        }
        [HttpPut("QuitarPaciente/{idsala:int}")]
        public async Task<IActionResult> QuitarPaciente(int idsala)
        {
            var sala = await salasRepos.Get(idsala);
            if (sala == null)
            {
                return NotFound("No se ah encontrado la sala");
            }
            sala.Paciente = null!;
            await salasRepos.Update(sala);
            return Ok("Se ah quitado al paciente de la sala.");
        }
        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> DeleteSala(int id)
        {
            var sala = await salasRepos.Get(id);
            //si la sala esta activa, quitar al doctor y al paciente, despues eliminarla
            if (sala != null && sala.Estado == 0)
            {
                if (sala.Doctor != null)
                {
                    await QuitarDoctor(sala.Id);
                }
                if (sala.Paciente != null)
                {
                    await QuitarPaciente(sala.Paciente ?? 0);
                }
                await salasRepos.Delete(sala);
                return Ok("Se ha eliminado la sala");
            }
            return NotFound("No se ha eliminado la sala");
        }
    }
}
