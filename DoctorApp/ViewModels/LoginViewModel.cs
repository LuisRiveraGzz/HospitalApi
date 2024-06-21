using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoctorApp.Models.DTOs;
using DoctorApp.Models.Validators;
using DoctorApp.Properties;
using DoctorApp.Services;
using DoctorApp.Views;
using DoctorApp.Views.Admin.Doctores;
using DoctorApp.Views.Doctor;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Windows;

namespace DoctorApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private string usuario = "";
        private string contraseña = "";
        private string error = "";
        private readonly ApiService api;

        public LoginViewModel()
        {
            api = new ApiService();

        }
        public string Usuario
        {
            get => usuario;
            set
            {
                usuario = value;
                OnPropertyChanged(nameof(Usuario));
            }
        }
        public string Contraseña
        {
            get => contraseña;
            set
            {
                contraseña = value;
                OnPropertyChanged(nameof(Contraseña));

            }
        }
        public string Error
        {
            get => error;
            set
            {
                error = value;
                OnPropertyChanged(nameof(Error));
            }
        }

        [RelayCommand]
        private async Task Confirmar()
        {
            Error = "";
            var dto = new LoginDTO
            {
                Usuario = usuario,
                Contraseña = contraseña
            };

            var result = LoginValidator.Validate(dto);
            if (result.IsValid)
            {

                var token = await api.Login(dto);
                if (!string.IsNullOrWhiteSpace(token))
                {
                    token = token.Replace("\"", "");
                    Settings.Default.Token = token;
                    Settings.Default.Save();

                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                    var rolClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                    //Administrador
                    if (rolClaim?.Value == "Administrador")
                    {
                        var doctoresView = new DoctoresView();
                        doctoresView.Show();
                    }
                    //Doctores
                    else
                    {
                        var turnosViews = new TurnosView();
                        turnosViews.Show();

                    }
                    var loginWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is LoginView);
                    loginWindow?.Close();
                }
                else
                {
                    Error = "Contraseña o Usuario incorrecto/a";
                }
            }
            else
            {

                Error = string.Join("\n", result.Errors.Select(x => x.ErrorMessage));
            }
        }
    }
}
