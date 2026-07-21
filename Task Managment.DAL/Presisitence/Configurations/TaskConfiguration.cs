using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task_Managment.DAL.Presisitence.Models;
using Task = Task_Managment.DAL.Presisitence.Models.Task;

namespace Task_Managment.DAL.Presisitence.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.Property(x => x.Title)
           .HasMaxLength(150)
           .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.Status)
                .HasConversion<string>();

            builder.Property(x => x.Priority)
                .HasConversion<string>();

            builder.HasIndex(x => x.ProjectId);

            builder.HasIndex(x => x.Status);

            builder.HasIndex(x => x.Priority);

            builder.HasIndex(x => x.DueDate);
        }
    }
}
