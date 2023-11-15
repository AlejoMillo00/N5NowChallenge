using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

internal sealed class PermissionTypeConfiguration : IEntityTypeConfiguration<PermissionType>
{
    public void Configure(EntityTypeBuilder<PermissionType> builder)
    {
        builder
            .ToTable("PermissionTypes");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.Description)
            .IsRequired();

        //Relations
        builder
            .HasMany(x => x.Permissions)
            .WithOne(x => x.PermissionTypeEntity)
            .HasForeignKey(x => x.PermissionType)
            .HasConstraintName("FK_Permission_PermissionType")
            .OnDelete(DeleteBehavior.NoAction);
    }
}
