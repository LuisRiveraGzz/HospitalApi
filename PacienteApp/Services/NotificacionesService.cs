using Microsoft.AspNetCore.SignalR.Client;

namespace PacienteApp.Services
{
    public class NotificacionesService
    {
        HubConnection NotificacionesHub { get; set; } = null!;
        public NotificacionesService()
        {
            #region Notificaciones Hub
            //Conexion al hub de la api
            NotificacionesHub = new HubConnectionBuilder()
                .WithUrl("https://hospitalapi.websitos256.com/NotificacionHub")
                .Build();

            NotificacionesHub.On<string>("RecibirNotificacion", (message) =>
            {
                //Mostrar el mensaje
                Shell.Current.DisplayAlert("Notificacion", message, "Ok");
                //Cambiar vista
                Shell.Current.Navigation.PushAsync(new Views.RegistroView());
            });
            #endregion
        }
    }
}
