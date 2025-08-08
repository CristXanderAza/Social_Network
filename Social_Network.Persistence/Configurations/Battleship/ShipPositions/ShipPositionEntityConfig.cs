using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Social_Network.Core.Domain.Entities.Battleship.ShipPositions;

namespace Social_Network.Infraestructure.Persistence.Configurations.Battleship.ShipPositions
{
    public class ShipPositionEntityConfig : IEntityTypeConfiguration<ShipPosition>
    {
        public void Configure(EntityTypeBuilder<ShipPosition> builder)
        {
            builder.ToTable("ShipPositions");
            builder.HasKey(sp => sp.Id);
            builder.Property(sp => sp.X)
                .IsRequired();
            builder.Property(sp => sp.Y)
                .IsRequired();
            builder.HasOne(sp => sp.Ship)
                .WithMany(s => s.ShipPositions)
                .HasForeignKey(sp => sp.ShipId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
