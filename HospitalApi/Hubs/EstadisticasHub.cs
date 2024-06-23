using Microsoft.AspNetCore.SignalR;

namespace HospitalApi.Hubs
{
    public class EstadisticasHub : Hub
    {
        static Dictionary<IClientProxy, TimeSpan> EstadisticaPaciente { get; set; } = new Dictionary<IClientProxy, TimeSpan>();
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
        public void EnviarNumeroPaciente(TimeSpan tiempo)
        {
            var lista = EstadisticaPaciente.Where(x => x.Value > tiempo).ToList();
            Clients.All.SendAsync("RecibirTiempoEspera", lista.Count);
        }

        // Envía las estadísticas actualizadas a todos los clientes conectados
        public void EnviarEstadisticas()
        {
            foreach (var cliente in EstadisticaPaciente.ToList())
            {
                // Añade 1 segundo al tiempo de espera de cada cliente
                EstadisticaPaciente[cliente.Key] = cliente.Value.Add(TimeSpan.FromSeconds(1));
                cliente.Key.SendAsync("RecibirEstadistica", cliente.Value);
            }
        }

        // Conecta un paciente con un ID específico
        public void Conectar(int id)
        {
            EstadisticaPaciente.Add(Clients.User(id.ToString()), TimeSpan.Zero);
            ++turno;
        }

        // Desconecta un paciente con un ID específico
        public void Desconectar(int id)
        {
            var client = EstadisticaPaciente.FirstOrDefault(x => x.Key == Clients.User(id.ToString()));
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