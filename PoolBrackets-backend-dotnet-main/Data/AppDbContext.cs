using Microsoft.EntityFrameworkCore;
using PoolBrackets_backend_dotnet.Models;

namespace PoolBrackets_backend_dotnet.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<PlayerInEvent> PlayerInEvents { get; set; }
        public DbSet<PlayerHistory> PlayerHistories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== INDEXES FOR PERFORMANCE =====

            // Unique index for User.Email (used in login)
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Index for Player.Name (used in search)
            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Name);

            // Unique index for Player.Email (used in registration)
            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Email)
                .IsUnique();

            // Composite unique index for PlayerInEvent to prevent duplicates
            modelBuilder.Entity<PlayerInEvent>()
                .HasIndex(pie => new { pie.PlayerId, pie.EventId })
                .IsUnique();

            // ===== RELATIONSHIPS =====

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Event)
                .WithMany()
                .HasForeignKey(m => m.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.FirstPlayer)
                .WithMany()
                .HasForeignKey(m => m.FirstPlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.SecondPlayer)
                .WithMany()
                .HasForeignKey(m => m.SecondPlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PlayerHistory>()
                .HasOne(ph => ph.Player)
                .WithMany()
                .HasForeignKey(ph => ph.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlayerHistory>()
                .HasOne(ph => ph.Event)
                .WithMany()
                .HasForeignKey(ph => ph.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
