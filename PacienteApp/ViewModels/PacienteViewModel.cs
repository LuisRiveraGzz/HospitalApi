
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using PacienteApp.Models.DTOs;
using PacienteApp.Services;
using System.ComponentModel;

namespace PacienteApp.ViewModels
{
    public partial class PacienteViewModel : INotifyPropertyChanged
    {
        private readonly HubConnection _hubConnection;
        private readonly ApiService service = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        public PacienteDTO Paciente { get; set; } = new();
        public string Error { get; set; } = "";
        HubConnection NotificacionesHub { get; set; } = null!;
        HubConnection EstadisticasHub { get; set; } = null!;
        public TimeSpan TiempoEspera { get; set; } = new();
        /// <summary>
        /// Estadisticas:
        /// Tiempo de espera promedio //Doctor
        /// Tiempo de espera //Paciente 
        /// Cantidad de pacientes atendidos // Doctor
        /// </summary>
        public PacienteViewModel()
        {
            NotificacionesHub = new HubConnectionBuilder()
                .WithUrl("https://hospitalapi.websitos256.com/NotificacionHub")
                .Build();

            //Aqui se recibe el mensaje del hub cuando el usuario sea llamado,
            //este se identifica por un id de conexion y el hub se encarga de manejar eso
            NotificacionesHub.On<string>("RecibirNotificacion", (message) =>
            {
                //Mostrar el mensaje
                Shell.Current.DisplayAlert("Notificacion", message, "Ok");

                Paciente.Id = 0;
                Paciente.Nombre = "";
                //Cambiar vista
                Shell.Current.Navigation.PushAsync(new Views.RegistroView());
            });
            EstadisticasHub = new HubConnectionBuilder()
                .WithUrl("https://hospitalapi.websitos256.com/EstadisticasHub")
                .Build();

            EstadisticasHub.SendAsync("Conectar", Paciente);

            EstadisticasHub.On<TimeSpan>("RecibirEstadistica", (tiempo) =>
            {
                TiempoEspera = tiempo;
                OnPropertyChanged(nameof(TiempoEspera));
            });

        }
        [RelayCommand]
        public async Task RegistrarUsuario()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Paciente.Nombre))
                {
                    Error = "Ingrese su nombre";
                }
                else
                {
                    await service.AgregarPaciente(Paciente);
                    await Shell.Current.Navigation.PushAsync(new Views.TunoView());
                }
                OnPropertyChanged(nameof(Paciente));
            }
            catch { }
        }

    }
}