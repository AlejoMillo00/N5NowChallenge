using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configuration;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder
            .ToTable("Permissions");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.EmployeeForename)
            .IsRequired();


        builder
            .Property(x => x.EmployeeSurname)
            .IsRequired();

        builder
            .Property(x => x.PermissionType)
            .IsRequired();

        builder
            .Property(x => x.PermissionDate)
            .IsRequired();

        //Relations
        builder
            .HasOne(x => x.PermissionTypeEntity)
            .WithMany(x => x.Permissions)
            .HasForeignKey(x => x.PermissionType)
            .HasConstraintName("FK_Permission_PermissionType")
            .OnDelete(DeleteBehavior.NoAction);
    }
}
