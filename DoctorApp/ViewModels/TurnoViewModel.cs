using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoctorApp.Properties;
using DoctorApp.Services;
using DoctorApp.Views.Doctor;
using DoctorApp.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DoctorApp.ViewModels
{
    public partial class TurnoViewModel:ObservableObject
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
            ObtenerUsuario();
        }
        
        public string Nombre { get => nombre; set
            {
                nombre = value;
                OnPropertyChanged(nameof(Nombre));
            } 
        }
        public string Sala { get => sala; set { sala = value; OnPropertyChanged(nameof(Sala)); } }
        public string Turno { get => turno; set { turno = value; OnPropertyChanged(nameof(Turno)); } }  
        public string Paciente { get => paciente; set { paciente = value; OnPropertyChanged(nameof(Paciente)); }}
        public async Task ObtenerUsuario()
        {
            var token = Settings.Default.Token;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            string nombreClaim = jsonToken?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value ;
            Nombre = nombreClaim.ToString();
            var doctores = await usuariosService.GetUsuarios();
            var nombre = doctores.FirstOrDefault(x=>x.Nombre == Nombre);
            var salas = await _salaservice.GetSalas();
            if (salas != null)
            {
                var salausuario = salas.Where(x => x.Doctor == nombre.Id).Select(x => x.Doctor).ToString();
                if (salausuario != null)
                {
                    Sala = salausuario;
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
