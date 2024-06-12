using HospitalApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalApi.Repositories
{
    public class UsuariosRepository(WebsitosHospitalbdContext context) : Repository<Usuario>(context)
    {
        private readonly WebsitosHospitalbdContext Context = context;
        public IEnumerable<Usuario> GetUsuarios()
        {
            return Context.Usuario.Include(x => x.Sala);
        }
        public Usuario? GetUsuario(string nombre)
        {
            return Context.Usuario.Include(x => x.Sala).FirstOrDefault(x => x.Nombre == nombre);
        }
    }
}
