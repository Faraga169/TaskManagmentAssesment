using System;
using System.Collections.Generic;
using System.Text;
using Task_Managment.DAL.Presisitence.Context;
using Task_Managment.DAL.Repositories.Interfaces;

namespace Task_Managment.DAL.Repositories
{
    public class UnitOfWork(TaskManagemntDbContext taskManagemntDb) : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repository = new Dictionary<string, object>();

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var Entity = typeof(T).Name;
            if (!_repository.ContainsKey(Entity))
            {
                var repositoryinstance = new GenericRepository<T>(taskManagemntDb);
                _repository[Entity] = repositoryinstance;

            }

            return (IGenericRepository<T>)_repository[Entity];
        }

        public async Task<int> CompleteAsync()
        {
            return await taskManagemntDb.SaveChangesAsync();
        }

        public ValueTask DisposeAsync()
        {
            return taskManagemntDb.DisposeAsync();
        }
    }
}
