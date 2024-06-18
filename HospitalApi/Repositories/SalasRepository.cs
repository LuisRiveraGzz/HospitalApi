using HospitalApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalApi.Repositories
{
    public class SalasRepository(WebsitosHospitalbdContext context) : Repository<Sala>(context)
    {
        private readonly WebsitosHospitalbdContext Context = context;
        public async Task<List<Sala>> GetSalas()
        {
            return await Context.Sala.ToListAsync();
        }
        public async Task<Sala?> GetSala(string numero)
        {
            return await Context.Sala.Include(x => x.DoctorNavigation).FirstOrDefaultAsync(x => x.NumeroSala == numero);
        }
    }
}