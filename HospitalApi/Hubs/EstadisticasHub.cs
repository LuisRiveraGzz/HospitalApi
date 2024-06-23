using Microsoft.AspNetCore.SignalR;

namespace HospitalApi.Hubs
{
    public class EstadisticasHub : Hub
    {
                cliente.Key.SendAsync("RecibirEstadistica", cliente.Value);
    }
}
