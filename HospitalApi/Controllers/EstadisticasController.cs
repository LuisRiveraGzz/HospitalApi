using HospitalApi.Hubs;
using HospitalApi.Models.DTOs;
using HospitalApi.Models.Entities;
using HospitalApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HospitalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadisticasController : ControllerBase
    {
        private readonly IHubContext<EstadisticasHub> _hubContext;
        private Dictionary<int, EstadisticaDTO> Estadisticas { get; set; } = [];
        private Queue<Paciente> Pacientes { get; set; } = [];

        private readonly UsuariosRepository UsuariosRepos;
        private readonly PacientesRepository PacientesRepos;
        private readonly SalasRepository SalasRepos;
        public EstadisticasController(
             IHubContext<EstadisticasHub> hubContext,
             UsuariosRepository usuariosRepository,
             PacientesRepository pacientesRepository,
             SalasRepository salasRepos)
        {
            _hubContext = hubContext;
            UsuariosRepos = usuariosRepository;
            PacientesRepos = pacientesRepository;
            SalasRepos = salasRepos;
            ActualizarUsuarios();
            ActualizarPacientes();
        }

        private void ActualizarUsuarios()
        {
            Estadisticas.Clear();
            foreach (var user in UsuariosRepos.GetAll().Where(x => x.Rol == 2))
            {
                Estadisticas.Add(user.Id, new());
            }
        }
        public void ActualizarPacientes()
        {
            Pacientes.Clear();
            foreach (var paciente in PacientesRepos.GetAll())
            {
                Pacientes.Enqueue(paciente);
            }
        }

        public Dictionary<int, EstadisticaDTO> ObtenerEstadisticas()
        {
            return Estadisticas;
        }
        public EstadisticaDTO ObtenerEstadistica(int doctorId)
        {
            if (Estadisticas.TryGetValue(doctorId, out var estadisticas))
            {
                return estadisticas;
            }
            throw new KeyNotFoundException("Doctor no encontrado.");
        }
        public async Task<ActionResult> AtenderPaciente(int doctorId)
        {
            if (!Estadisticas.ContainsKey(doctorId))
            {
                return BadRequest("Doctor no encontrado.");
            }
            if (Pacientes.Count == 0)
            {
                return BadRequest("No hay pacientes en la cola.");
            }
            var paciente = Pacientes.Dequeue();
            EstadisticaDTO estadisticas = Estadisticas[doctorId];
            var tiempoAtencion = SimularAtencion();
            estadisticas.PacientesAtendidos++;
            //Promedio de tiempo de atencion al cliente
            estadisticas.TiempoAtencion = (estadisticas.TiempoAtencion + tiempoAtencion) / estadisticas.PacientesAtendidos;
            // Enviar actualización a todos los clientes conectados
            var est = new Dictionary<int, EstadisticaDTO>
            {
                { doctorId, estadisticas }
            };
            await _hubContext.Clients.All.SendAsync("ActualizarEstadisticas", est);

            return Ok($"se ah atendido al cliente {paciente.Nombre} en {tiempoAtencion}");
        }
        private static TimeSpan SimularAtencion()
        {
            // Simula el tiempo de atención, por ejemplo, entre 10 y 30 minutos
            var random = new Random();
            int minutos = random.Next(3, 10);
            return TimeSpan.FromMinutes(minutos);
        }
    }
}