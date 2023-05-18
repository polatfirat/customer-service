using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Repository.Abstract
{
    public interface IRepository<T>
    {
        Task<T> InsertAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(int entityId);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> GetAllQueryable();
    }
}
