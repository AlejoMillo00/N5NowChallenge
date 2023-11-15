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

        builder
            .HasMany(x => x.Permissions)
            .WithOne(x => x.PermissionTypeEntity)
            .HasForeignKey(x => x.PermissionType)
            .HasConstraintName("FK_Permission_PermissionType")
            .OnDelete(DeleteBehavior.NoAction);

        AddBaseData(builder);
    }

    private static void AddBaseData(EntityTypeBuilder<PermissionType> builder)
    {
        builder.HasData(new List<PermissionType>
        {
            new(){ Id = 1, Description = "Read files." },
            new(){ Id = 2, Description = "Write files." },
            new(){ Id = 3, Description = "Read and write files." },
        });
    }
}
