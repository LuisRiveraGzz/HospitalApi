using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoctorApp.Properties;
using DoctorApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
            var nombreClaim = jsonToken?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            Nombre = nombreClaim.ToString();
            var doctores = await usuariosService.GetUsuarios();
            var nombre = doctores.FirstOrDefault(x=>x.Nombre == Nombre);
            var salas = await _salaservice.GetSalas();
            if (salas!= null)
            {
                Sala = salas.FirstOrDefault(x => x.Doctor == nombre.Id).ToString();
            }
        }
        [RelayCommand]
        public async Task Siguiente()
        {

        }
       
    }
}
