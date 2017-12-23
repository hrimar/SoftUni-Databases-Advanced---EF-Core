namespace TeamBuilder.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TeamBuilder.Models;

    internal class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.Username).IsUnique();

            builder.Property(u => u.Username)
                .IsRequired();

            builder.Property(u => u.FirstName)
                .HasMaxLength(25);

            builder.Property(u => u.LastName)
                .HasMaxLength(25);

            builder.HasMany(u => u.ReceivedInvitations)
                .WithOne(i => i.InvitedUser)
                .HasForeignKey(i => i.InvitedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.CreatedTeams)
                .WithOne(t => t.Creator)
                .HasForeignKey(t => t.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.CreatedEvents)
                .WithOne(e => e.Creator)
                .HasForeignKey(e => e.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
