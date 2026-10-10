using Libraries.Orm.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexoVerse.Contracts.Entity.Models;
using NexoVerse.Contracts.Space.Models;

namespace NexoVerse.Infrastructure.Mapping;

public sealed class SpaceInvitationMapping : IPersistable<SpaceInvitation>
{
    public void Configure(EntityTypeBuilder<SpaceInvitation> builder)
    {
        builder.ToTable("SpaceInvitations");
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Space).WithMany(x => x.Invitations)
            .HasForeignKey(x => x.SpaceId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.InvitedUser).WithMany(x => x.Invitations)
            .HasForeignKey(x => x.InvitedUserId).IsRequired().OnDelete(DeleteBehavior.NoAction);
    }
}
