namespace TeamBuilder.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TeamBuilder.Models;

    public class EventTeamConfig : IEntityTypeConfiguration<EventTeam>
    {
        public void Configure(EntityTypeBuilder<EventTeam> builder)
        {
            builder.HasKey(e => new { e.TeamId, e.EventId });

            builder.HasOne(e => e.Team)
                .WithMany(t => t.ParticipatedEvents)
                .HasForeignKey(e => e.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Event)
                .WithMany(e => e.ParticipatingTeams)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
