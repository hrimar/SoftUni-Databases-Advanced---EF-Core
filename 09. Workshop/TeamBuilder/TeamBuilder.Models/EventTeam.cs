using System;
using System.Collections.Generic;
using System.Text;

namespace TeamBuilder.Models
{
    public class EventTeam
    {
        public int TeamId { get; set; }
        public Team Team { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
