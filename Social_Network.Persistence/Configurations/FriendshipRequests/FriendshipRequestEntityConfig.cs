using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Social_Network.Core.Domain.Entities.FriendshipRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Infraestructure.Persistence.Configurations.FriendshipRequests
{
    public class FriendshipRequestEntityConfig : IEntityTypeConfiguration<FriendshipRequest>
    {
        public void Configure(EntityTypeBuilder<FriendshipRequest> builder)
        {
            builder.ToTable("FriendshipRequests");
            builder.HasKey(fr => fr.Id);
            builder.Property(fr => fr.Status)
                .IsRequired();
            builder.Property(fr => fr.ResponseDate)
                .IsRequired(false);
            builder.Ignore(fr => fr.CommonFriendsCount);
            /*  builder.HasOne(fr => fr.FromUser)
                  .WithMany(u => u.SentFriendshipRequests)
                  .HasForeignKey(fr => fr.FromUserId)
                  .OnDelete(DeleteBehavior.Cascade);
              builder.HasOne(fr => fr.ToUser)
                  .WithMany(u => u.ReceivedFriendshipRequests)
                  .HasForeignKey(fr => fr.ToUserId)
                  .OnDelete(DeleteBehavior.Cascade);*/
            builder.Property(fr => fr.FromUserId)
                .IsRequired();
            builder.Property(fr => fr.ToUserId)
                .IsRequired();
        }
    }
}
