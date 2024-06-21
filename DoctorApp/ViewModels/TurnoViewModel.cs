using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoctorApp.Properties;
using DoctorApp.Services;
using DoctorApp.Views;
using DoctorApp.Views.Doctor;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Windows;
using System.Windows.Media;

namespace DoctorApp.ViewModels
{
    public partial class TurnoViewModel : ObservableObject
    {
        string nombre = "";
        string sala = "";
        string turno = "";
        string paciente = "";
        string estadosala = "";
        private readonly UsuariosService usuariosService = new();
        private readonly SalasService salasService = new();
        public TurnoViewModel()
        {
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
        public string EstadoSala
        {
            get => estadosala; set
            {
                estadosala = value;
                OnPropertyChanged(nameof(EstadoSala));
                OnPropertyChanged(nameof(BotonSalaText));
                OnPropertyChanged(nameof(BotonSalaBackground));
            }
        }

        public string BotonSalaText => EstadoSala == "Activa" ? "Desactivar Sala" : "Activar Sala";
        public Brush BotonSalaBackground => EstadoSala == "Activa" ? Brushes.Red : (Brush)new BrushConverter().ConvertFromString("#0BDC54");

        public async Task ObtenerUsuario()
        {
            var token = Settings.Default.Token;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            string nombreClaim = jsonToken?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? "";
            Nombre = nombreClaim.ToString();
            int iduser = int.Parse(jsonToken?.Claims.FirstOrDefault(x => x.Type == "id")?.Value ?? "0");

            var salabydoc = await salasService.GetSalaByDoctor(iduser);
            Sala = salabydoc.numeroSala;
            if (string.IsNullOrWhiteSpace(Sala))
            {
                Sala = "El doctor no tiene ninguna sala asignada";
            }

        }
        [RelayCommand]
        public async Task Siguiente()
        {

        }
        [RelayCommand]
        public async Task CambiarEstado()
        {
            var token = Settings.Default.Token;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            int iduser = int.Parse(jsonToken?.Claims.FirstOrDefault(x => x.Type == "id")?.Value ?? "0");
            var salabydoc = await salasService.GetSalaByDoctor(iduser);

            if (salabydoc.estado == 0)
            {
                await salasService.ActivarSala(salabydoc.id);
                EstadoSala = "Activa";
            }
            else if (salabydoc.estado == 1)
            {
                await salasService.DesactivarSala(salabydoc.id);
                EstadoSala = "Inactiva";
            }
           
           
        }

        [RelayCommand]
        public void CerrarSesion()
        {
            Settings.Default.Token = "";
            Settings.Default.Save();
            var LoginViews = new LoginView();
            LoginViews.Show();
            var TurnosWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is TurnosView);
            TurnosWindow?.Close();
        }

    }
}
