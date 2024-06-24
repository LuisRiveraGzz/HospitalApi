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
    public partial class DoctoresViewModel : INotifyPropertyChanged
    {
        #region Properties
        public ObservableCollection<UsuarioDTO> Usuarios { get; set; } = [];
        public ObservableCollection<SalaDTO> Salas { get; set; } = [];
        public SalaDTO SalaSeleccionada { get; set; } = new();
        public Dictionary<int, int> PacientesAtendidos { get; set; } = [];
        public UsuarioDTO Usuario { get; set; } = new();
        public UsuarioDTO UsuarioSeleccionado { get; set; } = new();
        public string Error { get; set; } = "";
        #endregion

        private readonly UsuarioDTOValidator validador = new();
        public event PropertyChangedEventHandler? PropertyChanged;
        UsuariosService UsuariosService { get; set; } = new();
        public SalasService SalasService { get; set; } = new();
        public DoctoresViewModel()
        {
            Iniciar();
        }
        private async void Iniciar()
        {
            await ObtenerPacientes();
        }
        private async Task ObtenerPacientes()
        {
            Usuarios.Clear();
            PacientesAtendidos.Clear();
            foreach (var user in await UsuariosService.GetUsuarios())
            {
                Usuarios.Add(user);
                //Agrega como llave el id del usuario y
                //asigna la cantidad de pacientes que atendió
                //como 0, si este no estaba en el diccionario 
                if (PacientesAtendidos.ContainsKey(user.Id))
                {
                    //Saltar doctor si ya estaba en el diccionario
                    continue;
                }
                PacientesAtendidos.Add(user.Id, 0);
            }
            OnPropertyChanged(nameof(Usuarios));
            OnPropertyChanged(nameof(PacientesAtendidos));
        }
        private async Task ObtenerSalas()
        {
            Salas.Clear();
            foreach (var sala in await SalasService.GetSalas())
            {
                Salas.Add(sala);
            }
            OnPropertyChanged(nameof(Salas));
        }
        private async void OnPropertyChanged(string PropertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            await Task.CompletedTask;
        }

        #region Vistas
        [RelayCommand]
        public async Task VerSalas()
        {
            //Crea un usuario
            Usuario = new();
            //Limpia errores
            Error = "";
            SalasView view = new();
            view.Show();
            //Cierra la antigua
            var doctoresWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault();
            doctoresWindow?.Close();
            await ObtenerPacientes();
            await Task.CompletedTask;
        }
        [RelayCommand]
        public async Task VerUsuarios()
        {
            //Crea un usuario
            Usuario = new();
            //Limpia errores
            Error = "";
            DoctoresView view = new();
            view.Show();
            //Cierra la antigua
            var doctoresWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault();
            doctoresWindow?.Close();
            await ObtenerPacientes();
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
        public async Task VerEditar(UsuarioDTO user)
        {
            Usuario = user;
            //Quitar la contraseña
            Usuario.Contraseña = "";
            Error = "";
            //Muestra la nueva ventana
            Views.Admin.Doctores.EditarView editarView = new()
            {
                DataContext = this
            };
            editarView.Show();
            //Cierra la antigua
            var doctoresWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault();
            doctoresWindow?.Close();
            await Task.CompletedTask;
        }
        [RelayCommand]
        public async Task VerEliminar(UsuarioDTO user)
        {
            Usuario = user;
            Error = "";
            //Muestra la nueva ventana
            EliminarView View = new()
            {
                DataContext = this
            };
            View.Show();
            //Cierra la antigua
            var doctoresWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is DoctoresView);
            doctoresWindow?.Close();
            await Task.CompletedTask;
        }
        [RelayCommand]
        public static async Task Cancelar()
        {
            DoctoresView dv = new();
            dv.Show();
            var doctoresWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault();
            doctoresWindow?.Close();
            await Task.CompletedTask;
        }
        #endregion

        #region CRUD
        #region Create
        [RelayCommand]
        public async Task Agregar()
        {
            try
            {
                var result = validador.Validate(Usuario);
                if (result.IsValid)
                {
                    //Agregar doctores únicamente
                    Usuario.Rol = 2;
                    await UsuariosService.Agregar(Usuario);
                    await VerUsuarios();
                }
            }
            catch { }
        }
        #endregion
        #region Update
        public async Task Editar(UsuarioDTO user)
        {
            try
            {
                var result = validador.Validate(user);
                if (result.IsValid)
                {
                    if (user != null)
                    {
                        Usuario.Id = user.Id;
                        Usuario.Nombre = user.Nombre;
                        Usuario.Contraseña = user.Contraseña;
                        if (string.IsNullOrWhiteSpace(Usuario.Nombre) || string.IsNullOrWhiteSpace(Usuario.Contraseña))
                        {
                            await UsuariosService.Editar(Usuario);
                            await VerUsuarios();
                        }
                        Error = "Ingrese el usuario y la contraseña";
                    }
                    else
                    {
                        Error = "Seleccione una sala";
                    }
                }
            }
            catch
            {
                Error = "Ingresa los datos solicitados";
            }
            OnPropertyChanged(nameof(Error));
        }
        #endregion
        #region Delete
        public async Task Eliminar()
        {
            try
            {
                await UsuariosService.Eliminar(UsuarioSeleccionado);
                await VerUsuarios();
            }
            catch { }
        }
        #endregion
        #endregion
    }
}
