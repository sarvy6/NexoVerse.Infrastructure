using System;
using System.Collections.Generic;
using System.Text;
using Libraries.Orm.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexoVerse.Contracts.Entity.Models;

namespace NexoVerse.Infrastructure.Mapping {
    public class TokenMapping : IPersistable<Token> {
        public void Configure(EntityTypeBuilder<Token> builder) {
            builder.ToTable("Tokens");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.User)
                   .WithMany(x => x.Tokens)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
