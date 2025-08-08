using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Social_Network.Core.Domain.Entities.Battleship.Ships;

namespace Social_Network.Infraestructure.Persistence.Configurations.Battleship.Ships
{
    public class ShipEntityConfig : IEntityTypeConfiguration<Ship>
    {
        public void Configure(EntityTypeBuilder<Ship> builder)
        {
            builder.ToTable("Ships");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.ShipSize)
                .IsRequired();
            builder.Property(s => s.IsSunk)
                .IsRequired()
                .HasDefaultValue(false);
            builder.HasOne(s => s.BattleShipGame)
                .WithMany(g => g.Ships)
                .HasForeignKey(s => s.BattleShipGameId)
                .OnDelete(DeleteBehavior.NoAction);
            /*  builder.HasOne(s => s.Player)
                  .WithMany()
                  .HasForeignKey(s => s.PlayerId)
                  .OnDelete(DeleteBehavior.Cascade);*/
            builder.Property(s => s.PlayerId)
                .IsRequired();
        }
    }
}
