using CommunityToolkit.Mvvm.Input;
using DoctorApp.Models.DTOs;
using DoctorApp.Services;
using DoctorApp.Views.Admin.Doctores;
using System.Collections.ObjectModel;
using System.Windows;

namespace DoctorApp.ViewModels
{
    public partial class DoctoresViewModel
    {
        #region Properties
        public ObservableCollection<UsuarioDTO> Usuarios { get; set; } = [];
        public UsuarioDTO Usuario { get; set; } = new();
        public string Error { get; set; } = "";
        #endregion
        UsuariosService UsuariosService { get; set; } = new();
        public DoctoresViewModel()
        {
            Iniciar();
        }
        private async void Iniciar()
        {
            await ActualizarLista();
        }
        private async Task ActualizarLista()
        {
            Usuarios.Clear();
            foreach (var user in await UsuariosService.GetUsuarios())
            {
                Usuarios.Add(user);
            }
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
            AgregarView agregarView = new();
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
            EditarView editarView = new();
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
    }
}
