
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
        public virtual T? Get(int id)
        {
            return context.Find<T>(id);
        }
        public void Insert(T entity)
        {
            context.Add(entity);
            context.SaveChanges();
        }
        public void Update(T entity)
        {
            context.Update(entity);
            context.SaveChanges();
        }
        public void Delete(T entity)
        {
            context.Remove(entity);
            context.SaveChanges();
        }
    }
}
