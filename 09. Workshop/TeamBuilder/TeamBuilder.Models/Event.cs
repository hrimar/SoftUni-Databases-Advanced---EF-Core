using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeamBuilder.Models
{
    public class Event
    {
        private DateTime endDate;
        private DateTime startDate;

        public Event()  {  }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartDate
        {
            get => this.startDate;
            set => this.startDate = value;
        }              

        public DateTime EndDate
        {
            get => this.endDate;
            set
            {
                if (value < this.startDate)
                {
                    throw new ArgumentException("Start date should be before end date.");
                }
                this.endDate = value;
            }
        }

        public int CreatorId { get; set; }
        public User Creator { get; set; }

        public ICollection<EventTeam> ParticipatingTeams { get; set; } = new
            List<EventTeam>();
    }
}
