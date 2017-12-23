using System;
using System.Collections.Generic;
using System.Text;

namespace P03_FootballBetting.Data
{
    public class Confoguration
    {
        internal const string ConnectionString = @"Server=.;Database=FootballBetting;Integrated Security=True";

        // internal, за да може само нашия DbContext да го чете!!!
    }
}
