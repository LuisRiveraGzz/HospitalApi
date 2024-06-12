using Microsoft.EntityFrameworkCore;

namespace HospitalApi.Repositories
{
    public interface IRepository<T> where T : class
    {
        void Delete(T entity);
        T? Get(int id);
        DbSet<T> GetAll();
        void Insert(T entity);
        void Update(T entity);
    }
}