namespace P1.EmployeesMapping.App.Command
{
    using global::EmployeesMapping.DtoModel;
    using global::EmployeesMapping.Services;
    using P1.EmployeesMapping.App.Command.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Text;

    class AddEmployeeCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public AddEmployeeCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }
        //<firstName> <lastName> <salary> 
        public string Execute(params string[] args)
        {
            string firstName = args[0];
            string lastName = args[1];
            decimal salary = decimal.Parse(args[2]);

            var employeeDto = new EmployeeDto(firstName, lastName, salary);

            employeeService.AddEmployee(employeeDto);

            return $"{firstName} {lastName} sucessfully added.";
        }
    }
}
