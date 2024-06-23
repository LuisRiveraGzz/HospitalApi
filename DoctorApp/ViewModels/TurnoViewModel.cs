using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoctorApp.Models.DTOs;
using DoctorApp.Properties;
using DoctorApp.Services;
using DoctorApp.Views;
using DoctorApp.Views.Doctor;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Windows;
using System.Windows.Media;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;


namespace DoctorApp.ViewModels
{
    public partial class TurnoViewModel : ObservableObject
    {
        #region Propiedades
        string nombre = "";
        string sala = "";
        string turno = "";
        string paciente = "";
        string estadoSala = "";
        public string Sala { get => sala; set { sala = value; OnPropertyChanged(nameof(Sala)); } }
        public string Turno { get => turno; set { turno = value; OnPropertyChanged(nameof(Turno)); } }
        public string Paciente { get => paciente; set { paciente = value; OnPropertyChanged(nameof(Paciente)); } }
        
        public string EstadoSala
        {
            get => estadoSala; set
            {
                estadoSala = value;
                OnPropertyChanged(nameof(EstadoSala));
                OnPropertyChanged(nameof(BotonSalaText));
                OnPropertyChanged(nameof(BotonSalaBackground));
            }
        }
        #endregion
        private readonly SalasService salasService = new();
        private readonly PacienteService pacienteService = new();
        HubConnection EstadisticasHub { get; set; } = null!;
        public TurnoViewModel()
        {
            EstadisticasHub = new HubConnectionBuilder().WithUrl("https://hospitalapi.websitos256.com/EstadisticasHub").Build();
            EstadisticasHub.On<int>("RecibirEstadistica", (turno) =>
            {
                Turno = turno.ToString();
                OnPropertyChanged(nameof(Turno));
            }); 
        }
        public string Nombre
        {
            get => nombre; set
            {
                nombre = value;
                OnPropertyChanged(nameof(Nombre));
            }
        }



        public string BotonSalaText => EstadoSala == "Activa" ? "Desactivar Sala" : "Activar Sala";
        public Brush? BotonSalaBackground => (EstadoSala == "Activa") ? (Brushes.Red) : (Brushes.Green);

       
        public async Task<SalaDTO> ObtenerSala()
        {
            var token = Settings.Default.Token;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            string nombreClaim = jsonToken?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? "";
            Nombre = nombreClaim.ToString();
            int iduser = int.Parse(jsonToken?.Claims.FirstOrDefault(x => x.Type == "id")?.Value ?? "0");
            var salabydoc = await salasService.GetSalaByDoctor(iduser);
            return salabydoc;
        }


        public async Task ObtenerUsuario()
        {
            var salabyDoct = await ObtenerSala();
            EstadoSala = salabyDoct.Estado == 0 ? "Inactiva" : "Activa";
            Sala = salabyDoct.NumeroSala;
            if (string.IsNullOrWhiteSpace(Sala))
            {
                Sala = "El doctor no tiene ninguna sala asignada";
            }

        }
        [RelayCommand]
        public async Task Siguiente()
        {
            var salabyDoct = await ObtenerSala();
            if (salabyDoct != null)
            {
                //Obtener pacientes
                var pacientes = await pacienteService.GetPacientes();
                if (pacientes != null)
                {
                    var pacienteAsignar = pacientes.FirstOrDefault();
                    if (salabyDoct.Paciente == 0)
                    {
                        if (pacienteAsignar != null)
                        {
                            await salasService.AsignarPaciente(salabyDoct.Id, pacienteAsignar.Id);
                            Paciente = pacienteAsignar.Nombre;
                        }
                    }
                    else
                    {
                        pacienteAsignar = pacientes.FirstOrDefault();
                        await salasService.QuitarPaciente(salabyDoct.Id);
                        if (pacienteAsignar != null)
                        {
                            await pacienteService.Eliminar(pacienteAsignar);
                            await salasService.AsignarPaciente(salabyDoct.Id, pacienteAsignar.Id);
                            Paciente = pacienteAsignar.Nombre;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Sin pacientes en la cola");
                }
            }
            //Agregar id sala id paciente
        }
        [RelayCommand]
        public async Task CambiarEstado()
        {
            var salabyDoct = await ObtenerSala();
            Sala = salabyDoct.NumeroSala;

            if (salabyDoct.Estado == 0)
            {
                await salasService.ActivarSala(salabyDoct.Id);
                EstadoSala = "Activa";
            }
            else if (salabyDoct.Estado == 1)
            {
                await salasService.DesactivarSala(salabyDoct.Id);
                EstadoSala = "Inactiva";
            }


        }

        [RelayCommand]
        public async Task CerrarSesion()
        {
            Settings.Default.Token = "";
            Settings.Default.Save();
            var salabyDoct = ObtenerSala();
            await salasService.DesactivarSala(salabyDoct.Id);

            var LoginViews = new LoginView();
            LoginViews.Show();
            var TurnosWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is TurnosView);
            TurnosWindow?.Close();
        }
    }
}
