using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Task_Managment.DAL.Presisitence.Models;
using Task = Task_Managment.DAL.Presisitence.Models.Task;

namespace Task_Managment.DAL.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplySoftDeleteQueryFilter(this ModelBuilder builder)
        {
            builder.Entity<Project>()
                .HasQueryFilter(p => !p.IsDeleted);

            builder.Entity<Task>()
                .HasQueryFilter(t => !t.IsDeleted);
        }
    }
}
