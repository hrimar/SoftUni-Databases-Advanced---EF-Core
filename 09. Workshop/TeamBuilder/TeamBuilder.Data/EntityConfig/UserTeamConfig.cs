namespace TeamBuilder.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    internal class UserTeamConfig : IEntityTypeConfiguration<UserTeam>
    {
        public void Configure(EntityTypeBuilder<UserTeam> builder)
        {
            builder.HasKey(ut => new { ut.UserId, ut.TeamId });

            builder.HasOne(e => e.User)
                .WithMany(u => u.Teams)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Team)
                .WithMany(t => t.Members)
                .HasForeignKey(e => e.TeamId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
