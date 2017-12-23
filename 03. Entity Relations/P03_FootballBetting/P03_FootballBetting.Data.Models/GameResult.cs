using System;
using System.Collections.Generic;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    public enum GameResult
    {

        // С enum нямаме нужда да пазим специлано отделна таблиз аза тези ст-сти!
        // пака се записват на позиц по подразбиране:
        //HomeTeamWin,    // poz.0
        //AwayTeamWin,    // poz.1
        //Draw            // poz.2

        // za da im dadem konkretni pozicii:
          HomeTeamWin = 1,    // poz.1
          AwayTeamWin = 2,    // poz.2
          Draw      =  0    // poz.0
    }
}
