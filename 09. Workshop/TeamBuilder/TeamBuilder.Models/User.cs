namespace TeamBuilder.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class User
    {
        public int Id { get; set; }

        [MinLength(3)]
        [MaxLength(25)]
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [StringLength(30, MinimumLength = 6)]       
        public string Password { get; set; }    

        public Gender Gender { get; set; }

        [Range(0, 150)]
        public int Age { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<Event> CreatedEvents { get; set; } = new List<Event>();

        public ICollection<Invitation> ReceivedInvitations { get; set; } = new List<Invitation>();

        public ICollection<UserTeam> Teams { get; set; } = new List<UserTeam>();

        public ICollection<Team> CreatedTeams { get; set; } = new List<Team>();
    }
}
