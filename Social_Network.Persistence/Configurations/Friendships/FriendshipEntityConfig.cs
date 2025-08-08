using Microsoft.EntityFrameworkCore;
using Social_Network.Core.Domain.Entities.Friendships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Infraestructure.Persistence.Configurations.Friendships
{
    public class FriendshipEntityConfig : IEntityTypeConfiguration<Friendship>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Friendship> builder)
        {
            builder.ToTable("Friendships");
            builder.HasKey(f => f.Id);
            /*  builder.HasOne(f => f.FromUser)
                  .WithMany(u => u.FriendshipsFromMe)
                  .HasForeignKey(f => f.FromUserId)
                  .OnDelete(DeleteBehavior.Cascade);
              builder.HasOne(f => f.ToUser)
                  .WithMany(u => u.FriendshipsToMe)
                  .HasForeignKey(f => f.ToUserId)
                  .OnDelete(DeleteBehavior.Cascade);*/
            builder.Property(f => f.FromUserId)
                .IsRequired();
            builder.Property(f => f.ToUserId)
                .IsRequired();

        }
    }
}
