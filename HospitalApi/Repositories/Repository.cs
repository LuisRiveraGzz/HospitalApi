
using HospitalApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalApi.Repositories

{
    public class Repository<T>(WebsitosHospitalbdContext context) : IRepository<T> where T : class
    {
        public virtual DbSet<T> GetAll()
        {
            return context.Set<T>();
        }
        public async Task<T?> Get(int id)
        {
            return await context.FindAsync<T>(id);
        }
        public async Task Insert(T entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }
        public async Task Update(T entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
        public async Task Delete(T entity)
        {
            context.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
