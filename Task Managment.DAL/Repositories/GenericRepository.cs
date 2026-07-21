using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Task_Managment.BLL.Specification;
using Task_Managment.DAL.Presisitence.Context;
using Task_Managment.DAL.Repositories.Interfaces;
using Task_Managment.DAL.Specifications;

namespace Task_Managment.DAL.Repositories
{
    public class GenericRepository<T> :IGenericRepository<T> where T : BaseEntity
    {
        
        private readonly TaskManagemntDbContext _context;

        public GenericRepository(TaskManagemntDbContext context)
        {
            _context = context;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T?> GetEntityWithSpecAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).CountAsync();
        }

        public async Task<bool> AnyAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).AnyAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            entity.SoftDelete();
            _context.Set<T>().Update(entity);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification);
        }
    }
}
