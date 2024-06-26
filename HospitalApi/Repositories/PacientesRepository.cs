﻿using HospitalApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalApi.Repositories
{
    public class PacientesRepository(WebsitosHospitalbdContext context) : Repository<Paciente>(context)
    {
        private readonly WebsitosHospitalbdContext Context = context;
        public async Task<List<Paciente>> GetPacientes()
        {
            return await Context.Paciente.ToListAsync();
        }

        public async Task<Paciente?> GetPaciente(string paciente)
        {
            return await Context.Paciente.Include(x => x.Sala).FirstOrDefaultAsync(x => x.Nombre == paciente);
        }
    }
}
