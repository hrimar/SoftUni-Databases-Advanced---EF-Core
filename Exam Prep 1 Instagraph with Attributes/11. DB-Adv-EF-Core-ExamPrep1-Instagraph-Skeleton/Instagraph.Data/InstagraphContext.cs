using Instagraph.Models;
using Microsoft.EntityFrameworkCore;

namespace Instagraph.Data
{
    public class InstagraphContext : DbContext
    {
        public InstagraphContext() { }

        public InstagraphContext(DbContextOptions options)
            :base(options) { }

        public DbSet<Picture> Pictures { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
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
            modelBuilder.Entity<User>()
                .HasAlternateKey(s => s.Username); // for Unique

            modelBuilder.Entity<UserFollower>()
         .HasKey(s => new { s.UserId, s.FollowerId }); // for mapping tabl - FK
                        
            modelBuilder.Entity<UserFollower>()
                .HasOne(o => o.User)
                .WithMany(oi => oi.Followers)
                .HasForeignKey(o => o.UserId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserFollower>()
             .HasOne(o => o.Follower)
             .WithMany(oi => oi.UsersFollowing)
             .HasForeignKey(o => o.FollowerId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(o => o.ProfilePicture)
                .WithMany(oi => oi.Users)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(o => o.Post)
                .WithMany(oi => oi.Comments)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
