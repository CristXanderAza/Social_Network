using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Social_Network.Core.Domain.Entities.Reactions;

namespace Social_Network.Infraestructure.Persistence.Configurations.Reactions
{
    public class ReactonEntityConfig : IEntityTypeConfiguration<Reaction>
    {
        public void Configure(EntityTypeBuilder<Reaction> builder)
        {
            builder.ToTable("Reactions");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Type)
                .IsRequired();
            /*builder.HasOne(r => r.User)
                .WithMany(u => u.Reactions)
                .HasForeignKey(r => r.UserId);*/
            builder.Property(r => r.UserId)
                .IsRequired();
            builder.HasOne(r => r.Post)
                .WithMany(p => p.Reactions)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

