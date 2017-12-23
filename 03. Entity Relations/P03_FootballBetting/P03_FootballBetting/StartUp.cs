namespace P03_FootballBetting
{
    using System;
    using P03_FootballBetting.Data;
    using P03_FootballBetting.Data.Models;


    public class StartUp
    {
        static void Main()
        {
            using (var db = new FootballBettingContext())
            {
                db.Database.EnsureCreated();
            }

        }
    }
}
