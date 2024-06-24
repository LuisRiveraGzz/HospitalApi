using Microsoft.AspNetCore.SignalR;

namespace HospitalApi.Hubs
{
    public class EstadisticasHub : Hub
    {
        static Dictionary<int, TimeSpan> EstadisticaPaciente { get; set; } = [];
        int turno = 0;
        public EstadisticasHub()
        {
            Thread hilo = new(new ThreadStart(ActualizarEstadisticas))
            {
                IsBackground = true
            };
            hilo.Start();
        }
        public void ActualizarEstadisticas()
        {
            while (EstadisticaPaciente.Count > 0)
            {
                foreach (var cliente in EstadisticaPaciente)
                {
                    // Añade 1 segundo al tiempo de espera de cada cliente
                    EstadisticaPaciente[cliente.Key] = cliente.Value.Add(TimeSpan.FromSeconds(1));
                }
            }

        }
        public async Task EnviarNumeroPaciente(int id)
        {
            var lista = EstadisticaPaciente.Keys.Where(x => x < id);
            var num = lista.Count();
            await Clients.User(id.ToString()).SendAsync("RecibirNumeroPacientes", num);
        }
        public async Task EnviarEstadisticas()
        {
            foreach (var paciente in EstadisticaPaciente)
            {
                int idpaciente = paciente.Key;
                var usuario = Clients.User(idpaciente.ToString());
                if (usuario != null)
                {
                    int numpacientes = EstadisticaPaciente.Where(x => x.Key < idpaciente).Count();
                    await usuario.SendAsync("RecibirNumeroPacientes", numpacientes);
                }
            }
        }
        // Conecta un paciente con un ID específico
        public async Task Conectar(int id)
        {
            EstadisticaPaciente.Add(id, TimeSpan.Zero);
            turno++;
            await Task.CompletedTask;
        }
        // Desconecta un paciente con un ID específico
        public async Task Desconectar(int id)
        {
            var client = EstadisticaPaciente.FirstOrDefault(x => x.Key == id);
            EstadisticaPaciente.Remove(client.Key);
            await Task.CompletedTask;
        }
        // Reinicia el contador de turnos
        public async Task ReiniciarTurnos()
        {
            turno = 0;
            await Task.CompletedTask;
        }

        // Envía el número de turno a todos los clientes
        public async Task EnviarTurno()
        {
            await Clients.All.SendAsync("RecibirTurno", turno);
        }
    }
}