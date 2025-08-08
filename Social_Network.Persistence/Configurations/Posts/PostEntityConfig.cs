using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Social_Network.Core.Domain.Entities.Posts;

namespace Social_Network.Infraestructure.Persistence.Configurations.Posts
{
    public class PostEntityConfig : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Content)
                .IsRequired()
                .HasMaxLength(1000);
            builder.Property(p => p.ImageUrl)
                .HasMaxLength(500)
                .IsRequired(false);
            builder.Property(p => p.VideoUrl)
                .HasMaxLength(500)
                .IsRequired(false);
            builder.Property(p => p.Type)
                .IsRequired();
            /* builder.HasOne(p => p.User)
                 .WithMany(u => u.Posts)
                 .HasForeignKey(p => p.UserId)
                 .OnDelete(DeleteBehavior.Cascade);*/
            builder.Property(p => p.UserId)
                .IsRequired();

            //throw new NotImplementedException();
        }
    }
}
