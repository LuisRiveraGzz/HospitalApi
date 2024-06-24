using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoctorApp.Models.DTOs;
using DoctorApp.Models.Validators;
using DoctorApp.Services;
using DoctorApp.Views.Admin.Doctores;
using DoctorApp.Views.Admin.Salas;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DoctorApp.ViewModels
{
    public partial class SalasViewModel : ObservableObject
    {
        public ObservableCollection<SalaDTO> Salas { get; set; } = new();
        public ObservableCollection<UsuarioDTO> Doctores { get; set; } = new();
        public SalaDTO Sala { get; set; } = new();
        public SalaDTO SalaSeleccionada { get; set; } = new();  
        string error = "";
        public string Error
        {
            get => error; set
            {
                error = value; OnPropertyChanged(nameof(Error));
            }
        }
        UsuariosService usuariosService { get; set; } = new();
        SalasService salasService { get; set; } = new();
        public SalasViewModel()
        {
            Iniciar();
        }

        private async void Iniciar()
        {
            await ObtenerSalas();
            await ObtenerDoctores();
        }
        [RelayCommand]
        public async Task VerActividades()
        {
            //Limpia errores
            Error = "";
            SalasView salasView = new();
            salasView.Show();
            DoctoresView view = new();
            view.Show();
            //Cierra la antigua
            var doctoresWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault();
            doctoresWindow?.Close();
            await Task.CompletedTask;
        }
        [RelayCommand]
        public async Task VerSalas()
        {
            //Crea una sala
            Sala = new();
            //Limpia errores
            Error = "";
            SalasView view = new();
            view.Show();
            //Cierra la antigua
            var Salawindow = Application.Current.Windows.OfType<Window>().FirstOrDefault();
            Salawindow?.Close();
            await ObtenerSalas();
            await Task.CompletedTask;
        }
        public async Task ObtenerDoctores()
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
            await Task.CompletedTask;
        }
        [RelayCommand]
        public async Task VerAgregar()
        {
            Sala = new();
            Error = "";
            Views.Admin.Salas.AgregarView agregarView = new();
            agregarView.Show();
            //Cierra la antigua
            var agregarsala = Application.Current.Windows.OfType<Window>().FirstOrDefault();
            agregarsala?.Close();
            await Task.CompletedTask;
        }
        [RelayCommand]
        public async Task VerEditar()
        {
            Error = "";
            //Muestra la nueva ventana
            Views.Admin.Salas.EditarView editarView = new();
            editarView.Show();
            //Cierra la antigua
            var doctoresWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault();
            doctoresWindow?.Close();
            await Task.CompletedTask;
        }
        [RelayCommand]
        public async Task VerEliminar()
        {
            Error = "";
        }
        [RelayCommand]
        public async Task Cancelar()
        {
            SalasView dv = new();
            dv.Show();
            var salasWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault();
            salasWindow?.Close();
            await Task.CompletedTask;
        }
        [RelayCommand]
        public async Task Agregar()
        {
            try
            {
                var result = SalaValidator.Validate(Sala);
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
            catch (Exception)
            {

                throw;
            }
        }
        [RelayCommand]
        public async Task Editar()
        {
            try
            {
                var results = SalaValidator.Validate(SalaSeleccionada);
                if (results.IsValid)
                {
                    await salasService.Editar(SalaSeleccionada);
                    await VerSalas();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
