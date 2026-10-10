using Libraries.Orm.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexoVerse.Contracts.Entity.Models;
using NexoVerse.Contracts.Space.Models;

namespace NexoVerse.Infrastructure.Mapping;

public sealed class UserMapping : IPersistable<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.Tokens).WithOne(x => x.User).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Spaces).WithMany(x => x.Members)
            .UsingEntity<Dictionary<string, object>>("SpaceMembers",
                join => join.HasOne<Space>().WithMany().HasForeignKey("SpaceId").OnDelete(DeleteBehavior.Cascade),
                join => join.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.NoAction),
                join => join.HasKey("SpaceId", "UserId"));
    }
}
