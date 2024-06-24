using Microsoft.AspNetCore.SignalR;

namespace HospitalApi.Hubs
{
    public class EstadisticasHub : Hub
    {
        static Dictionary<int, TimeSpan> EstadisticaPaciente { get; set; } = [];
        int turno = 0;
        public EstadisticasHub()
        {
            // Cada 1 segundo envía estadísticas
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    EnviarEstadisticas();
                }
            });
        }

        // Envía el número de pacientes que han esperado más que un tiempo específico
        public void EnviarNumeroPaciente(int id)
        {
            var lista = EstadisticaPaciente.Keys.Where(x => x < id);
            Clients.All.SendAsync("RecibirTiempoEspera", lista.Count());
        }

        // Envía las estadísticas actualizadas a todos los clientes conectados
        public void EnviarEstadisticas()
        {
            foreach (var cliente in EstadisticaPaciente.ToList())
            {
                // Añade 1 segundo al tiempo de espera de cada cliente
                EstadisticaPaciente[cliente.Key] = cliente.Value.Add(TimeSpan.FromSeconds(1));
                var user = Clients.User(cliente.Key.ToString());
                user.SendAsync("RecibirEstadistica", cliente.Value);
            }
        }
        // Conecta un paciente con un ID específico
        public void Conectar(int id)
        {
            EstadisticaPaciente.Add(id, TimeSpan.Zero);
            turno++;
        }

        // Desconecta un paciente con un ID específico
        public void Desconectar(int id)
        {
            var client = EstadisticaPaciente.FirstOrDefault(x => x.Key == id);
            EstadisticaPaciente.Remove(client.Key);
        }

        // Reinicia el contador de turnos
        public void ReiniciarTurnos()
        {
            turno = 0;
        }

        // Envía el número de turno a todos los clientes
        public void EnviarTurno()
        {
            Clients.All.SendAsync("RecibirTurno", turno);
        }
    }
}