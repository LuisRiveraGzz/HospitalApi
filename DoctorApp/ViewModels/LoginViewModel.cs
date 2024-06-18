using CommunityToolkit.Mvvm.Input;
using DoctorApp.Models.DTOs;
using DoctorApp.Models.Validators;
using DoctorApp.Properties;
using DoctorApp.Services;
using DoctorApp.Views;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace DoctorApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string usuario = "";
        private string contraseña = "";
        private string error = "";
        private readonly ApiService api;

        public event PropertyChangedEventHandler? PropertyChanged;
        public LoginViewModel()
        {
            api = new ApiService();
            ConfirmarCommand = new RelayCommand(Confirmar);

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
        private async void Confirmar()
        {
            Error = "";
            var dto = new LoginDTO
            {
                Usuario = Usuario,
                Contraseña = Contraseña
            };
            var result = LoginValidator.Validate(dto);
            if (result.IsValid)
            {
                var token = await api.Login(dto);
                if (token != null)
                {
                    Settings.Default.Token = token;
                    Settings.Default.Save();
                    var turnosViews = new TurnosView();
                    turnosViews.Show();
                    var loginWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is LoginView);
                    if (loginWindow != null)
                    {
                        loginWindow.Close();
                    }
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

        public ICommand ConfirmarCommand { get; }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
