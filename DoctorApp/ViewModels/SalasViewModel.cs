using CommunityToolkit.Mvvm.Input;
using DoctorApp.Models.DTOs;
using DoctorApp.Models.Validators;
using DoctorApp.Services;
using DoctorApp.Views.Admin.Doctores;
using DoctorApp.Views.Admin.Salas;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace DoctorApp.ViewModels
{
    public partial class SalasViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<SalaDTO> Salas { get; set; } = [];
        public ObservableCollection<UsuarioDTO> Doctores { get; set; } = [];
        public SalaDTO Sala { get; set; } = new();
        public SalaDTO SalaSeleccionada { get; set; } = new();
        private string error = "";
        public string Error
        {
            get => error; set
            {
                error = value;
                OnPropertyChanged(nameof(Error));
            }
        }


        private readonly UsuariosService usuariosService = new();
        private readonly SalasService salasService = new();
        private readonly SalaValidator salaValidator = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public SalasViewModel()
        {
            Iniciar();
        }

        private async void Iniciar()
        {
            await ObtenerSalas();
            await ObtenerDoctores();
        }

        private async Task ObtenerDoctores()
        {
            Doctores.Clear();
            foreach (var item in await usuariosService.GetUsuarios())
            {
                Doctores.Add(item);
            }
        }

        private async Task ObtenerSalas()
        {
            Salas.Clear();
            foreach (var item in await salasService.GetSalas())
            {
                Salas.Add(item);
            }
        }

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #region Navigation Methods
        [RelayCommand]
        public async Task VerDoctores()
        {
            Error = "";
            DoctoresView view = new();
            view.Show();
            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault();
            currentWindow?.Close();
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task VerSalas()
        {
            Error = "";
            SalasView view = new();
            view.Show();
            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault();
            currentWindow?.Close();
            await ObtenerSalas();
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task VerAgregar()
        {
            Sala = new();
            Error = "";
            Views.Admin.Salas.AgregarView agregarView = new();
            agregarView.Show();
            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is SalasView);
            currentWindow?.Close();
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task VerEditar()
        {
            Error = "";
            Views.Admin.Salas.EditarView editarView = new()
            {
                DataContext = this
            };
            editarView.Show();
            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is SalasView);
            currentWindow?.Close();
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task VerEliminar()
        {
            Error = "";
            Views.Admin.Salas.EliminarSala eliminarView = new()
            {
                DataContext = this
            };
            eliminarView.Show();
            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is SalasView);
            currentWindow?.Close();
            await Task.CompletedTask;
        }

        [RelayCommand]
        public static async Task Cancelar()
        {
            SalasView salasView = new();
            salasView.Show();
            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault();
            currentWindow?.Close();
            await Task.CompletedTask;
        }
        #endregion

        #region CRUD Methods
        [RelayCommand]
        public async Task Agregar()
        {
            try
            {
                var result = salaValidator.Validate(Sala);
                if (result.IsValid)
                {

                    await salasService.Agregar(Sala);
                    await VerSalas();
                }
                else
                {
                    Error = string.Join("\n", result.Errors.Select(x => x.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }

        [RelayCommand]
        public async Task Editar()
        {
            try
            {
                var result = salaValidator.Validate(SalaSeleccionada);
                if (result.IsValid)
                {
                    await salasService.Editar(SalaSeleccionada);
                    await VerSalas();
                }
                else
                {
                    Error = string.Join("\n", result.Errors.Select(x => x.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }

        [RelayCommand]
        public async Task Eliminar()
        {
            try
            {
                await salasService.Eliminar(SalaSeleccionada);
                await VerSalas();
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }
        #endregion
    }
}
