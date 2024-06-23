using Microsoft.AspNetCore.SignalR;

namespace HospitalApi.Hubs
{
    public class NotificacionHub : Hub
    {
        public void EnviarNotificacion(int idpaciente, string numerosala)
        {
            Clients.User(idpaciente.ToString()).SendAsync("RecibirNotificacion", $"Has sido asignado a la sala {numerosala}");
        }
    }
}
