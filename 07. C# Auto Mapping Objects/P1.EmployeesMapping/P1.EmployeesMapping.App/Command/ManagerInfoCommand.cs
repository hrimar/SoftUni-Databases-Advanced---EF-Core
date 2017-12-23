using EmployeesMapping.Services;
using P1.EmployeesMapping.App.Command.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace P1.EmployeesMapping.App.Command
{
    public class ManagerInfoCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public ManagerInfoCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] args)
        {
            int employeeId = int.Parse(args[0]);

            var employee = employeeService.ManagerInfo(employeeId);
                              

            return employee;
        }
    }
}
