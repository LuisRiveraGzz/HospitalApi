using Microsoft.AspNetCore.SignalR;

namespace HospitalApi.Hubs
{
    public class NotificacionHub : Hub
    {
        public async Task EnviarMensaje(int idpaciente, string NumeroSala)
        {
            await Clients.User(idpaciente.ToString()).SendAsync("RecibirNotificacion", $"Has sido asignado a la sala {NumeroSala}");
        }
    }
}