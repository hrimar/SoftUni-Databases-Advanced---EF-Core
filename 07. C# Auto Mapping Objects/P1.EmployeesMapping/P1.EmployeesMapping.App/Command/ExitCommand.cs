

namespace P1.EmployeesMapping.App.Command
{
    using P1.EmployeesMapping.App.Command.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Text;

    class ExitCommand : ICommand
    {
        public string Execute(params string[] args)
        {
            Console.WriteLine("Goodbye!");
            Environment.Exit(0);

            return String.Empty;
        }
    }
}
