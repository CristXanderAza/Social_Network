using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Social_Network.Core.Domain.Entities.Battleship.BattleshipGames;

namespace Social_Network.Infraestructure.Persistence.Configurations.Battleship.BattleshipGames
{
    public class BattleShipGameEntityConfig : IEntityTypeConfiguration<BattleShipGame>
    {
        public void Configure(EntityTypeBuilder<BattleShipGame> builder)
        {
            builder.ToTable("BattleShipGames");
            builder.HasKey(g => g.Id);
            builder.Property(g => g.Status)
                .IsRequired();
            builder.Property(b => b.LastMovementAt)
                .IsRequired(false);
           /* builder.HasOne(g => g.Player1)
                .WithMany(u => u.BattleShipGamesAsPlayer1)
                .HasForeignKey(g => g.Player1Id)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(g => g.Player2)
                .WithMany(u => u.BattleShipGamesAsPlayer2)
                .HasForeignKey(g => g.Player2Id)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(g => g.Winner)
                .WithMany(u => u.BattleShipGamesAsWinner)
                .HasForeignKey(g => g.WinnerId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(g => g.CurrentTurnPlayer)
                .WithMany()
                .HasForeignKey(g => g.CurrentTurnPlayerId)
                .OnDelete(DeleteBehavior.SetNull);*/
           builder.Property(builder => builder.Player1Id)
                .IsRequired();
            builder.Property(builder => builder.Player2Id)
                .IsRequired();
            builder.Property(builder => builder.WinnerId)
                .IsRequired(false);
            builder.Property(builder => builder.CurrentTurnPlayerId)
                .IsRequired();

            builder.Property(g => g.StartedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(g => g.FinishedAt)
                .IsRequired(false);
        }
    }
}
