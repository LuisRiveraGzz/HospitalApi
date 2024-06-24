
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
        HubConnection EstadisticasHub { get; set; } = null!;
        public TimeSpan TiempoEspera { get; set; } = new();
        public int Turno { get; set; } = 0;
        public int NumPacientes { get; set; } = 0;

        private readonly NotificacionesService notificaciones = new();
        public PacienteViewModel()
        {
            #region Estadisticas Hub
            //EstadisticasHub = new HubConnectionBuilder().WithUrl("https://localhost:7095/EstadisticasHub").Build();
            EstadisticasHub = new HubConnectionBuilder().WithUrl("https://hospitalapi.websitos256.com/EstadisticasHub").Build();
            EstadisticasHub.On<int>("RecibirNumPacientes", (num) =>
            {
                NumPacientes = num;
                OnPropertyChanged(nameof(NumPacientes));
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
            EstadisticasHub.StartAsync();
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
                    var anterior = await service.BuscarPaciente(Paciente.Nombre);
                    if (anterior.Id == 0)
                    {
                        await service.AgregarPaciente(Paciente);
                        var actual = await service.BuscarPaciente(Paciente.Nombre);
                        await Shell.Current.Navigation.PushAsync(new Views.TunoView()
                        {
                            BindingContext = this
                        });
                        await EstadisticasHub.InvokeAsync("Conectar", actual.Id);
                        Thread hilo = new(new ThreadStart(ObtenerNum))
                        {
                            IsBackground = true
                        };
                    }
                    else
                    {
                        Error = "Ya se ah registrado un usuario con el mismo nombre";
                    }
                }
                OnPropertyChanged(nameof(Paciente));
            }
            catch { }
        }

        private void ObtenerNum()
        {
            while (true)
            {
                Task.Delay(1000);
                EstadisticasHub.InvokeAsync("EnviarNumeroPaciente", Paciente.Id);
            }
        }
    }
}
