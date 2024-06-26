﻿using HospitalApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalApi.Repositories
{
    public class UsuariosRepository(WebsitosHospitalbdContext context) : Repository<Usuario>(context)
    {
        private readonly WebsitosHospitalbdContext Context = context;
        public IAsyncEnumerable<Usuario> GetUsuarios()
        {
            return Context.Usuario.Include(x => x.Sala).AsAsyncEnumerable();
        }
        public Usuario? GetUsuario(int id)
        {
            return Context.Usuario.Include(x => x.Sala).FirstOrDefault(x => x.Id == id);
        }
        public Task<Usuario?> GetUsuarioByName(string nombre)
        {
            return Context.Usuario.Include(x => x.Sala).FirstOrDefaultAsync(x => x.Nombre == nombre);
        }

    }
}
