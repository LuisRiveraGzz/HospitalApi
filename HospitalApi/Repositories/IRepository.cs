using Microsoft.EntityFrameworkCore;

namespace HospitalApi.Repositories
{
    public interface IRepository<T> where T : class
    {

        Task<T?> Get(int id);
        DbSet<T> GetAll();
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}