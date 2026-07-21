using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task_Managment.DAL.Presisitence.Models;

namespace Task_Managment.DAL.Presisitence.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.Property(x => x.Name)
             .HasMaxLength(100)
             .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.HasIndex(x => new
            {
                x.OwnerId,
                x.Name
            }).IsUnique();

            builder.HasOne(x => x.Owner)
                .WithMany(x => x.Projects)
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Tasks)
                .WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
