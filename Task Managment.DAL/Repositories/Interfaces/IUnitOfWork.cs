using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Task_Managment.DAL.Presisitence.Models;
using Task = Task_Managment.DAL.Presisitence.Models.Task;

namespace Task_Managment.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork:IAsyncDisposable 
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> CompleteAsync();
    }
}
