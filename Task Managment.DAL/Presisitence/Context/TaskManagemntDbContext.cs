using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task_Managment.DAL.Presisitence.Models;

namespace Task_Managment.DAL.Presisitence.Context
{
    public class TaskManagemntDbContext:IdentityDbContext<ApplicationUser>
    {
        public TaskManagemntDbContext(DbContextOptions<TaskManagemntDbContext> dbContext):base()
        {
            
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


    }
}
