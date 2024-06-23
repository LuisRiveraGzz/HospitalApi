using Microsoft.AspNetCore.SignalR;

namespace HospitalApi.Hubs
{
    public class EstadisticasHub : Hub
    {
        static Dictionary<IClientProxy, TimeSpan> EstadisticaPaciente { get; set; } = [];
        public EstadisticasHub()
        {
            //Cada 1 segundo envia estadisticas
            while (true)
            {
                Task.Delay(1000);
                EnviarEstadisticas();
            }
        }
        //Envia las estadisticas actualizadas
        public void EnviarEstadisticas()
        {
            foreach (var cliente in EstadisticaPaciente)
            {
                //Se le agrega 1 segundo a cada cliente cada segundo
                cliente.Value.Add(new(1000));
                cliente.Key.SendAsync("RecibirEstadistica", cliente.Value);
            }
        }
        //Agrega el cliente a la lista
        public void Conectar(int id)
        {
            EstadisticaPaciente.Add(Clients.User(id.ToString()), new());
        }
        //Elimina el cliente de la lista
        public void Desconectar(int id)
        {
            EstadisticaPaciente.Remove(Clients.User(id.ToString()));
        }

    }
}
