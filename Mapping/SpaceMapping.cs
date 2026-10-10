using Libraries.Orm.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexoVerse.Contracts.Entity.Models;
using NexoVerse.Contracts.Space.Models;

namespace NexoVerse.Infrastructure.Mapping;

public sealed class SpaceMapping : IPersistable<Space>
{
    public void Configure(EntityTypeBuilder<Space> builder)
    {
        builder.ToTable("Spaces");
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.CreatedBy).WithMany(x => x.CreatedSpaces)
            .HasForeignKey(x => x.CreatedById).IsRequired().OnDelete(DeleteBehavior.NoAction);
    }
}
