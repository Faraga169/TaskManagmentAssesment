using System;
using System.Collections.Generic;
using System.Text;
using Task_Managment.BLL.Specification;

namespace Task_Managment.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T?> GetEntityWithSpecAsync(ISpecification<T> specification);

        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification);

        Task<int> CountAsync(ISpecification<T> specification);

        Task<bool> AnyAsync(ISpecification<T> specification);

        Task AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
