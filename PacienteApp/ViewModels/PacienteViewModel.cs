
using Microsoft.AspNetCore.SignalR.Client;

namespace PacienteApp.ViewModels
{
    public class PacienteViewModel
    {
        private HubConnection _hubConnection;
        public PacienteViewModel()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://hospitalapi.websitos256.com/notificationHub")
                .Build();

            //Aqui se recibe el mensaje
            _hubConnection.On<string>("RecibirNotificacion", (message) =>
            {
                //Mostrar el mensaje
                Shell.Current.DisplayAlert("Notificacion", message, "Ok");
            });

        }
    }
}
