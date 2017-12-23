using P1.EmployeesMapping.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeesMapping.DtoModel
{
    public class ManagerDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<EmployeeDto> ManagedEmployees { get; set; } = new List<EmployeeDto>();

        public int ManagedEmployeesCount
        {
            get { return this.ManagedEmployees.Count; }
        }       
    }
}
