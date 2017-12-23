using EmployeesMapping.DtoModel;
using EmployeesMapping.Services;
using P1.EmployeesMapping.App.Command.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace P1.EmployeesMapping.App.Command
{
    class ListEmployeesOlderThanCommand : ICommand
    {
        private readonly EmployeeService employeeService;              

        public ListEmployeesOlderThanCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] args)
        {
            int age = int.Parse(args[0]);

            var employees = this.employeeService.ListEmployeesOlderThan(age);

            if (employees.Count == 0)
            {
                return "No such employees found.";
            }

            StringBuilder sb = new StringBuilder();

            foreach (var employeeBDto in employees)
            {
                string managerName = "[no manager]";

                if (employeeBDto.Manager != null)
                {
                    managerName = employeeBDto.Manager.LastName;
                }

                sb.AppendLine(
                    $"{employeeBDto.FirstName} {employeeBDto.LastName} - " +
                    $"${employeeBDto.Salary:F2} - Manager: {managerName}");
            }

            return sb.ToString().Trim();
        }

    }
}
