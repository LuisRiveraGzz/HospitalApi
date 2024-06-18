using HospitalApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalApi.Repositories
{
    public class PacientesRepository(WebsitosHospitalbdContext context) : Repository<Paciente>(context)
    {
        WebsitosHospitalbdContext Context = context;
        public async Task<List<Paciente>> GetPacientes()
        {
            return await Context.Paciente.ToListAsync();
        }
    }
}
