using FlipCardApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace FlipCardApp.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Deck> Decks { get; set; } = null!;
        public DbSet<Card> Cards { get; set; } = null!;
        public DbSet<Pile> Piles { get; set; } = null!;
        public DbSet<ReviewHistory> ReviewHistories { get; set; } = null!;
        public DbSet<Goal> Goals { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
            });

            // Configure Deck entity
            modelBuilder.Entity<Deck>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
                
                entity.HasOne(d => d.User)
                    .WithMany(u => u.Decks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Card entity
            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Question).IsRequired();
                entity.Property(e => e.Answer).IsRequired();
                
                entity.HasOne(c => c.Deck)
                    .WithMany(d => d.Cards)
                    .HasForeignKey(c => c.DeckId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Pile entity
            modelBuilder.Entity<Pile>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                
                entity.HasOne(p => p.Deck)
                    .WithMany(d => d.Piles)
                    .HasForeignKey(p => p.DeckId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure ReviewHistory entity
            modelBuilder.Entity<ReviewHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasOne(r => r.Card)
                    .WithMany(c => c.ReviewHistories)
                    .HasForeignKey(r => r.CardId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(r => r.User)
                    .WithMany()
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.NoAction); // Prevent circular cascade delete
            });

            // Configure Goal entity
            modelBuilder.Entity<Goal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
                
                entity.HasOne(g => g.User)
                    .WithMany(u => u.Goals)
                    .HasForeignKey(g => g.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
