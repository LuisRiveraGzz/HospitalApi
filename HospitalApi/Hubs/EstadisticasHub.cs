using Microsoft.AspNetCore.SignalR;

namespace HospitalApi.Hubs
{
    public class EstadisticasHub : Hub
    {
        static Dictionary<IClientProxy, TimeSpan> EstadisticaPaciente { get; set; } = [];
        public EstadisticasHub()
        {
            while (true)
            {
                Task.Delay(1000);
                EnviarEstadisticas();
            }
        }
        public void EnviarEstadisticas()
        {
            foreach (var cliente in EstadisticaPaciente)
            {
                cliente.Value.Add(new(1000));
                cliente.Key.SendAsync("RecibirEstadistica", cliente.Value);
            }
        }
        public void Conectar(int id)
        {
            EstadisticaPaciente.Add(Clients.User(id.ToString()), new());
        }

    }
}
