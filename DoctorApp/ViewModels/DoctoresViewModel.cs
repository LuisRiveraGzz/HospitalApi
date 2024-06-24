using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoctorApp.Models.DTOs;
using DoctorApp.Services;
using DoctorApp.Views.Admin.Doctores;
using DoctorApp.Views.Admin.Salas;
using System.Collections.ObjectModel;
using System.Windows;

namespace DoctorApp.ViewModels
{
    public partial class DoctoresViewModel : ObservableObject
    {
        #region Properties

        [ObservableProperty]
        public ObservableCollection<UsuarioDTO> usuarios = [];
        [ObservableProperty]
        public Dictionary<int, int> pacientesAtendidos = [];
        [ObservableProperty]
        public UsuarioDTO usuario = new();
        [ObservableProperty]
        public string error = "";
        #endregion
        UsuariosService UsuariosService { get; set; } = new();
        public DoctoresViewModel()
        {
            Iniciar();
        }
        private async void Iniciar()
        {
            await ActualizarListas();
        }
        private async Task ActualizarListas()
        {
            Usuarios.Clear();
            foreach (var user in await UsuariosService.GetUsuarios())
            {
                Usuarios.Add(user);
                PacientesAtendidos.Add(user.Id, 0);
            }
            await Task.CompletedTask;
        }
        [RelayCommand]
        public async Task VerUsuarios()
        {
            //Crea un usuario
            Usuario = new();
            //Limpia errores
            Error = "";
            await ActualizarListas();
            await Task.CompletedTask;
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
        public async Task VerAgregar()
        {
            //Crea un usuario
            Usuario = new();
            //Limpia errores
            Error = "";
            //Muestra la nueva ventana
            Views.Admin.Doctores.AgregarView agregarView = new();
            agregarView.Show();
            //Cierra la antigua
            var doctoresWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is DoctoresView);
            doctoresWindow?.Close();
            await Task.CompletedTask;
        }
        [RelayCommand]
        public async Task VerEditar()
        {
            Error = "";
            //Muestra la nueva ventana
            Views.Admin.Doctores.EditarView editarView = new();
            editarView.Show();
            //Cierra la antigua
            var doctoresWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is DoctoresView);
            doctoresWindow?.Close();
            await Task.CompletedTask;
        }
        [RelayCommand]
        public async Task VerEliminar()
        {
            Error = "";
            //Muestra la nueva ventana
            EliminarView agregarView = new();
            agregarView.Show();
            //Cierra la antigua
            var doctoresWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is DoctoresView);
            doctoresWindow?.Close();
            await Task.CompletedTask;
        }
        [RelayCommand]
        public async Task Cancelar()
        {
            DoctoresView dv = new();
            dv.Show();
            var doctoresWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault();
            doctoresWindow?.Close();
            await Task.CompletedTask;
        }

    }
}
