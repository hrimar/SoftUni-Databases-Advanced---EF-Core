namespace TeamBuilder.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    internal class TeamConfig : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasIndex(t => t.Name).IsUnique();

            builder.Property(t => t.Name)
                .HasMaxLength(25)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasMaxLength(32);

            builder.Property(t => t.Acronym).IsRequired();

            builder.HasOne(t => t.Creator)
                .WithMany(u => u.CreatedTeams)
                .HasForeignKey(t => t.CreatorId)
                .OnDelete(DeleteBehavior.Restrict); // Май винаги е добре така

            builder.HasMany(t => t.Invitations)
                .WithOne(i => i.Team)
                .HasForeignKey(i => i.TeamId)
                .OnDelete(DeleteBehavior.Restrict); //
        }
    }
}
