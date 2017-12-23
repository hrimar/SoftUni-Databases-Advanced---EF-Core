using EmployeesMapping.Services;
using P1.EmployeesMapping.App.Command.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace P1.EmployeesMapping.App.Command
{
    public class SetManagerCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public SetManagerCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] args)
        {
            int employeeId = int.Parse(args[0]);
            int managerId = int.Parse(args[1]);
            
           employeeService.SetManager(employeeId, managerId);

            return $"Manager ID: {managerId} is set to employee ID: {employeeId}";
        }
    }
}
