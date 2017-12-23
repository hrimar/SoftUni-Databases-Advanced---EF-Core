namespace TeamBuilder.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    internal class InvitationConfig : IEntityTypeConfiguration<Invitation>
    {
        public void Configure(EntityTypeBuilder<Invitation> builder)
        {
            builder.HasKey(i => i.Id);

            builder.HasOne(i => i.InvitedUser)
                .WithMany(u => u.ReceivedInvitations)
                .HasForeignKey(i => i.InvitedUserId)
                .OnDelete(DeleteBehavior.Restrict); //

            builder.HasOne(i => i.Team)
                .WithMany(t => t.Invitations)
                .HasForeignKey(i => i.TeamId)
                .OnDelete(DeleteBehavior.Restrict);   // 
        }
    }
}
