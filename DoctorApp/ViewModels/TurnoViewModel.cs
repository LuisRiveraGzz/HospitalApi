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

namespace DoctorApp.ViewModels
{
    public partial class TurnoViewModel : ObservableObject
    {
        string nombre = "";
        string sala = "";
        string turno = "";
        string paciente = "";
        private readonly SalasService _salaservice;
        private readonly UsuariosService usuariosService;
        public TurnoViewModel()
        {
            _salaservice = new();
            usuariosService = new();
            _ = ObtenerUsuario();
        }

        public string Nombre
        {
            get => nombre; set
            {
                nombre = value;
                OnPropertyChanged(nameof(Nombre));
            }
        }
        public string Sala { get => sala; set { sala = value; OnPropertyChanged(nameof(Sala)); } }
        public string Turno { get => turno; set { turno = value; OnPropertyChanged(nameof(Turno)); } }
        public string Paciente { get => paciente; set { paciente = value; OnPropertyChanged(nameof(Paciente)); } }
        public async Task ObtenerUsuario()
        {
            var token = Settings.Default.Token;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            string nombreClaim = jsonToken?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? "";
            Nombre = nombreClaim.ToString();
            int iduser = int.Parse(jsonToken?.Claims.FirstOrDefault(x => x.Type == "id")?.Value ?? "0");

            var doctor = await usuariosService.GetUsuario(iduser);
            var salas = doctor.sala;
            if (salas != null)
            {
                if (salas != null)
                {
                    SalaDTO sala = salas.FirstOrDefault() ?? new();
                    Sala = sala.NumeroSala;
                }
                else
                {
                    Sala = "El doctor no tiene ninguna sala asignada";
                }
            }
        }
        [RelayCommand]
        public async Task Siguiente()
        {

        }
        [RelayCommand]
        public void CerrarSesion()
        {
            Settings.Default.Token = "";
            Settings.Default.Save();
            var LoginViews = new LoginView();
            LoginViews.Show();
            var TurnosWIndow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is TurnosView);
            TurnosWIndow?.Close();
        }

    }
}
