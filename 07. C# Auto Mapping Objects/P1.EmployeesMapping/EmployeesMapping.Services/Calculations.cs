using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeesMapping.Services
{
    public class Calculations
    {
        public static int CalcCurrentAge(DateTime birthday)
        {
            DateTime currentDate = DateTime.Now;

            int age = currentDate.Year - birthday.Year;

            if (birthday > currentDate.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}
