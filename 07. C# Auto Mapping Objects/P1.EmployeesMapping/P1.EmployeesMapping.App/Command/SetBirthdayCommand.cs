using EmployeesMapping.Services;
using P1.EmployeesMapping.App.Command.Contracts;
using System;

namespace P1.EmployeesMapping.App.Command
{
    public class SetBirthdayCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public SetBirthdayCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        //<employeeId> <dd-MM-yyyy> 
        public string Execute(params string[] args)
        {
            int employeeId = int.Parse(args[0]);
            DateTime date = DateTime.ParseExact(args[1], "dd-MM-yyyy", null);

            var employeeName = employeeService.SetBirthday(employeeId, date);

            return $"{employeeName}'s birthday was set to {args[1]}";
        }
    }
}
