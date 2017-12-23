namespace P03_FootballBetting.Data
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using P03_FootballBetting.Data.Models;

    public class FootballBettingContext : DbContext
    {

        public FootballBettingContext()
        {
        }
        public FootballBettingContext(DbContextOptions options)
            :base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }
        public DbSet<Position> Positions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            //base.OnConfiguring(optionBuilder); ???

            if (!optionBuilder.IsConfigured)
            {
                optionBuilder.UseSqlServer(Confoguration.ConnectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.TeamId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(e => e.Initials)
                    .IsRequired()
                    .HasColumnType("NCHAR(3)");

                entity.Property(e => e.LogoUrl)
                    .IsUnicode(false);

                entity.HasOne(e => e.PrimaryKitColor)
                .WithMany(c => c.PrimaryKitTeams)
                .HasForeignKey(e => e.PrimaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(e => e.SecondaryKitColor)
                .WithMany(c => c.SecondaryKitTeams)
                .HasForeignKey(e => e.SecondaryKitColorId);


                entity.HasOne(e => e.Town)
                .WithMany(c => c.Teams)
                .HasForeignKey(e => e.TownId);
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.HasKey(e => e.ColorId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40);                
            });

            modelBuilder.Entity<Town>(entity =>
            {
                entity.HasKey(e => e.TownId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.HasOne(e => e.Country)
                .WithMany(c => c.Towns)
                .HasForeignKey(e => e.CountryId);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CountryId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(80);                
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(e => e.PlayerId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(e => e.IsInjured)
                    .HasDefaultValue(false);

                entity.HasOne(e => e.Team)
                .WithMany(c => c.Players)
                .HasForeignKey(e => e.TeamId);

                entity.HasOne(e => e.Position)
                .WithMany(c => c.Players)
                .HasForeignKey(e => e.PositionId);
            });


            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(e => e.PositionId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);                   
            });


            modelBuilder.Entity<PlayerStatistic>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.GameId });
                                
                entity.HasOne(e => e.Game)
                .WithMany(c => c.PlayerStatistics)
                .HasForeignKey(e => e.GameId);

                entity.HasOne(e => e.Player)
                .WithMany(c => c.PlayerStatistics)
                .HasForeignKey(e => e.PlayerId);
            });


            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(e => e.GameId);

                entity.HasOne(e => e.HomeTeam)
                .WithMany(c => c.HomeGames)
                .HasForeignKey(e => e.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.AwayTeam)
                .WithMany(c => c.AwayGames)
                .HasForeignKey(e => e.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Bet>(entity =>
            {
                entity.HasKey(e => e.BetId);

                entity.HasOne(e => e.Game)
                .WithMany(c => c.Bets)
                .HasForeignKey(e => e.GameId);

                entity.HasOne(e => e.User)
                .WithMany(c => c.Bets)
                .HasForeignKey(e => e.UserId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.Name)
                    .HasMaxLength(100);
                
                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Password)
                   .IsRequired()
                   .HasMaxLength(30);

                entity.Property(e => e.Email)
                   .IsRequired()
                   .HasMaxLength(60);                
            });
        }
    }
}
