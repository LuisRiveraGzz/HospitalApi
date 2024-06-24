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
            while (true)
            {
                foreach (var cliente in EstadisticaPaciente.ToList())
                {
                    // Añade 1 segundo al tiempo de espera de cada cliente
                    EstadisticaPaciente[cliente.Key] = cliente.Value.Add(TimeSpan.FromSeconds(1));
                }
            }
        }
        public void EnviarNumeroPaciente(int id)
        {
            var lista = EstadisticaPaciente.Keys.Where(x => x < id);
            var num = lista.Count();
            Clients.User(id.ToString()).SendAsync("RecibirNumeroPacientes", num);
        }
        public void EnviarEstadisticas()
        {
            foreach (var paciente in EstadisticaPaciente)
            {
                int idpaciente = paciente.Key;
                var usuario = Clients.User(idpaciente.ToString());
                if (usuario != null)
                {
                    int numpacientes = EstadisticaPaciente.Where(x => x.Key < idpaciente).Count();
                    usuario.SendAsync("RecibirNumeroPacientes", numpacientes);
                }
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