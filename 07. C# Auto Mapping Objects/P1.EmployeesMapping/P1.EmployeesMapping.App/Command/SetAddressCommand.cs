using EmployeesMapping.Services;
using P1.EmployeesMapping.App.Command.Contracts;
using System;
using System.Linq;

namespace P1.EmployeesMapping.App.Command
{
    public class SetAddressCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public SetAddressCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        //<employeeId> <dd-MM-yyyy> 
        public string Execute(params string[] args)
        {
            int employeeId = int.Parse(args[0]);
            string address = String.Join(" ", args.Skip(1)); // махаме Id-то и join-ваме останалите суми за адрес 

            var employeeName = employeeService.SetAddress(employeeId, address);

            return $"{employeeName}'s address was set to {address}";
        }
    }
}
