using System;
using System.Collections.Generic;
using System.Text;

namespace P1.EmployeesMapping.Data
{
    public class Configuration
    {
        public static string ConnectionString => 
            $"Server=.;Database=EmployeesMapping;Integrated Security=True";

        // internal, за да може само нашия DbContext да го чете, НО ТУК ЩЕ НИ ТРЯБВА И ОТ ДР. МЕСТА ЗАТОВА PUBLIC!!!
        // static - за д асе вика от всякъде и не const!
        // => означава, че е само с get
    }
}
