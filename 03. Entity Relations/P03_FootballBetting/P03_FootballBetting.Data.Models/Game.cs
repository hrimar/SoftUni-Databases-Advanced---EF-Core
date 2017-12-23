using System;
using System.Collections.Generic;

namespace P03_FootballBetting.Data.Models
{
    public class Game
    {
        public int GameId { get; set; }
        public DateTime DateTime { get; set; }

       
        public int HomeTeamId { get; set; }
        public Team HomeTeam { get; set; }
        public byte HomeTeamGoals { get; set; }
        public float HomeTeamBetRate { get; set; }

        public int AwayTeamId { get; set; }
        public Team AwayTeam { get; set; }
        public byte AwayTeamGoals { get; set; }
        public float AwayTeamBetRate { get; set; }

        public float DrawBetRate { get; set; }

        public GameResult Result { get; set; }

        public ICollection<Bet> Bets { get; set; } = new HashSet<Bet>();

        public ICollection<PlayerStatistic> PlayerStatistics { get; set; } = new HashSet<PlayerStatistic>();
    }
}