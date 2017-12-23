namespace TeamBuilder.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.ComponentModel.DataAnnotations;

    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [StringLength(3, MinimumLength = 3)]
        public string Acronym { get; set; }

        public int CreatorId { get; set; }
        public User Creator { get; set; }

        public ICollection<UserTeam> Members { get; set; } = new List<UserTeam>();

        public ICollection<EventTeam> ParticipatedEvents { get; set; } = new List<EventTeam>();

        public ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();
    }
}
