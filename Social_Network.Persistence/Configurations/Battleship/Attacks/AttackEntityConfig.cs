using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Social_Network.Core.Domain.Entities.Battleship.Attacks;

namespace Social_Network.Infraestructure.Persistence.Configurations.Battleship.Attacks
{
    public class AttackEntityConfig : IEntityTypeConfiguration<Attack>
    {
        public void Configure(EntityTypeBuilder<Attack> builder)
        {
            builder.ToTable("Attacks");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.X)
                .IsRequired();
            builder.Property(a => a.Y)
                .IsRequired();
            builder.HasOne(a => a.Ship)
                .WithMany(s => s.Attacks)
                .HasForeignKey(a => a.ShipId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(a => a.BattleShipGame)
                .WithMany(g => g.Attacks)
                .HasForeignKey(a => a.BattleShipGameId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Property(A => A.PlayerId)
                .IsRequired();
            /*   builder.HasOne(a => a.Player)
                   .WithMany()
                   .HasForeignKey(a => a.PlayerId)
                   .OnDelete(DeleteBehavior.Cascade);*/
            builder.Property(a => a.Result)
                .IsRequired();
        }
    }
}
