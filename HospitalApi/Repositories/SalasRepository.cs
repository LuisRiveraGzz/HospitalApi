using HospitalApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalApi.Repositories
{
    public class SalasRepository(WebsitosHospitalbdContext context) : Repository<Sala>(context)
    {
        private readonly WebsitosHospitalbdContext Context = context;
        public IEnumerable<Sala> GetSalas()
        {
            return Context.Sala.Include(x => x.DoctorNavigation).OrderBy(x => x.NumeroSala);
        }
        public Sala? GetSala(string numero)
        {
            return Context.Sala.Include(x => x.DoctorNavigation).FirstOrDefault(x => x.NumeroSala == numero);
        }
    }
}
