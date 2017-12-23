﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeesMapping.DtoModel
{
    public class EmployeePersonalDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Salary { get; set; }
        public DateTime? Birthday { get; set; }
        public string Address { get; set; }
    }
}
