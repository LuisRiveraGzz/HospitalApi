
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using PacienteApp.Models.DTOs;
using PacienteApp.Services;
using System.ComponentModel;

namespace PacienteApp.ViewModels
{
    public partial class PacienteViewModel : INotifyPropertyChanged
    {
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
        public int Turno { get; set; } = 0;
        public int NumPacientes { get; set; } = 0;
        public PacienteViewModel()
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
                Task.Delay(2000);
                Paciente.Id = 0;
                Paciente.Nombre = "";
                //Cambiar vista
                Shell.Current.Navigation.PushAsync(new Views.RegistroView());
            });
            #endregion
            #region Estadisticas Hub
            EstadisticasHub = new HubConnectionBuilder().WithUrl("https://hospitalapi.websitos256.com/EstadisticasHub").Build();
            EstadisticasHub.On<TimeSpan>("RecibirEstadistica", (tiempo) =>
            {
                TiempoEspera = tiempo;
                OnPropertyChanged(nameof(TiempoEspera));
            });
            EstadisticasHub.On<int>("RecibirEstadistica", (turno) =>
            {
                Turno = turno;
                OnPropertyChanged(nameof(TiempoEspera));
            });
            EstadisticasHub.On<int>("RecibirNumeroPacientes", (num) =>
            {
                //num es el numero de pacientes antes que esperaron mas que el paciente actual
                if (NumPacientes != num)
                    NumPacientes = num;
            });
            #endregion
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
                    Thread hilo = new(new ThreadStart(ObtenerNumeroUsuarios))
                    {
                        IsBackground = true
                    };
                }
                OnPropertyChanged(nameof(Paciente));
            }
            catch { }
        }

        void ObtenerNumeroUsuarios()
        {
            while (true)
            {
                //Enviar cada 3 segundos
                Task.Delay(3000);
                EstadisticasHub.InvokeAsync("EnviarNumeroPaciente", TiempoEspera);
            }
        }
    }
}