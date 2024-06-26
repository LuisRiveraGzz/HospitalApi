﻿using DoctorApp.Models.DTOs;
using DoctorApp.Properties;
using DoctorApp.Views;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Windows;

namespace DoctorApp.Services
{
    public class UsuariosService
    {
        private readonly HttpClient Client;
        public UsuariosService()
        {
            Client = new()
            {
                BaseAddress = new Uri("https://hospitalapi.websitos256.com/api/Usuarios/")
            };
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Settings.Default.Token);
        }
        public async Task CerrarSesion()
        {
            //Quitar token el token a la configuración
            Settings.Default.Reset();
            //Quitar token al cliente http
            Client.DefaultRequestHeaders.Authorization = null;
            //Mostrar Login
            LoginView login = new();
            login.Show();
            //Cierra la antigua
            var doctoresWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is not LoginView);
            doctoresWindow?.Close();
            await Task.CompletedTask;
        }
        public async Task<IEnumerable<UsuarioDTO>> GetUsuarios()
        {
            try
            {
                var response = await Client.GetAsync("Doctores");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var usuarios = JsonConvert.DeserializeObject<IEnumerable<UsuarioDTO>>(json);
                return usuarios ?? [];
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Credenciales Expiradas", "Han expirado sus credenciales, inicia sesion nuevamente", MessageBoxButton.OK);
                    await CerrarSesion();
                }
            }
            return [];
        }
        public async Task<UsuarioGet> GetUsuario(int id)
        {
            try
            {
                var response = await Client.GetAsync($"{id}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var usuario = JsonConvert.DeserializeObject<UsuarioGet>(json);
                return usuario ?? new();
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Credenciales Expiradas", "Han expirado sus credenciales, inicia sesion nuevamente", MessageBoxButton.OK);
                    await CerrarSesion();
                }
            }
            return new();
        }
        public async Task Agregar(UsuarioDTO dto)
        {
            try
            {
                var response = await Client.PostAsJsonAsync("Agregar", dto);
                response.EnsureSuccessStatusCode();
            }
            catch { }
        }
        public async Task Editar(UsuarioDTO dto)
        {
            try
            {
                var response = await Client.PutAsJsonAsync("Editar", dto);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    MessageBox.Show("Credenciales Expiradas", "Han expirado sus credenciales, inicia sesion nuevamente", MessageBoxButton.OK);
            }
        }
        public async Task Eliminar(UsuarioDTO dto)
        {
            try
            {
                var response = await Client.DeleteAsync($"Eliminar/{dto.Id}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Credenciales Expiradas", "Han expirado sus credenciales, inicia sesion nuevamente", MessageBoxButton.OK);
                    await CerrarSesion();
                }
            }
        }
    }
}
