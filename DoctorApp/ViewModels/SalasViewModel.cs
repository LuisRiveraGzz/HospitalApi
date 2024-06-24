using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoctorApp.Models.DTOs;
using DoctorApp.Services;

using DoctorApp.Views.Admin.Salas;
using System.Collections.ObjectModel;
using System.Windows;

namespace DoctorApp.ViewModels
{
    public partial class SalasViewModel : ObservableObject
    {
        public ObservableCollection<SalaDTO> Salas { get; set; } = new();
        public ObservableCollection<UsuarioDTO> Doctores { get; set; } = new();
        public SalaDTO Sala { get; set; } = new();
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
            var agregarsala = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is SalasView);
            agregarsala?.Close();
            await Task.CompletedTask;
        }
    }
}
