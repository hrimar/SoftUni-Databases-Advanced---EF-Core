using Instagraph.Models;
using Microsoft.EntityFrameworkCore;

namespace Instagraph.Data
{
    public class InstagraphContext : DbContext
    {
        public InstagraphContext() { }

        public InstagraphContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserFollower> UsersFollowers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Picture>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Path)
                .IsRequired();
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Caption)
                .IsRequired();

                entity.HasOne(p => p.Picture)
                .WithMany(pic => pic.Posts)
                .HasForeignKey(p => p.PictureId)
                .OnDelete(DeleteBehavior.Restrict);//

                entity.HasOne(p => p.User)
               .WithMany(pic => pic.Posts)
               .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Restrict); 
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Content)
                  .IsRequired()
                .HasMaxLength(250);

                entity.HasOne(p => p.Post)
                .WithMany(c => c.Comments)
                .HasForeignKey(p => p.PostId);

                entity.HasOne(p => p.User)
               .WithMany(c => c.Comments)
               .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Restrict); // YES! навсякъде  4.
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasAlternateKey(e => e.Password); // !!!

                entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(30);

                entity.Property(e => e.Password)
                  .IsRequired()
                .HasMaxLength(20);

                entity.HasOne(p => p.ProfilePicture)
               .WithMany(pic => pic.Users)
               .HasForeignKey(p => p.ProfilePictureId)
                     .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<UserFollower>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.FollowerId });

                entity.HasOne(p => p.User)
               .WithMany(f => f.Followers)
               .HasForeignKey(p => p.UserId);

                entity.HasOne(p => p.Follower)
              .WithMany(f => f.UsersFollowing)
              .HasForeignKey(p => p.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
