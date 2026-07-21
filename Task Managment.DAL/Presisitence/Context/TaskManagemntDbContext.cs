using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task_Managment.DAL.Extensions;
using Task_Managment.DAL.Presisitence.Models;
using Task = Task_Managment.DAL.Presisitence.Models.Task;

namespace Task_Managment.DAL.Presisitence.Context
{
    public class TaskManagemntDbContext:IdentityDbContext<ApplicationUser>
    {
        public TaskManagemntDbContext(DbContextOptions<TaskManagemntDbContext> dbContext):base(dbContext)
        {
            
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyReference).Assembly);
            modelBuilder.ApplySoftDeleteQueryFilter();
        }



        public virtual DbSet<Project> Projects { set; get; }

        public virtual DbSet<Task> Tasks { set; get; }


    }
}
