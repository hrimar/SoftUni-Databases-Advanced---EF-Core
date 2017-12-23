using System;
using System.Collections.Generic;
using System.Text;

namespace P1.EmployeesMapping.App.Command.Contracts
{
    internal interface ICommand
    {
        string Execute(params string[] args);
    }
}
