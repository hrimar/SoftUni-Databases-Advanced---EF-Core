namespace P1.EmployeesMapping.App
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using System.Linq;
    using P1.EmployeesMapping.App.Command.Contracts;
    using Microsoft.Extensions.DependencyInjection;

    internal class CommandParser
    {
        public static ICommand Parse(IServiceProvider serviceProvider, string commandName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var commandTypes = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(ICommand)));

            var commandType = commandTypes.SingleOrDefault(t => t.Name.ToLower() == 
            $"{commandName.ToLower()}command");

            if(commandType == null) // commandType държи цялата инф-я за един клас
            {
                throw new InvalidOperationException("Invalid command.");
            }

            var constructor = commandType.GetConstructors().First();

            var constructorParams = constructor
                .GetParameters()
                .Select(pi => pi.ParameterType)
                .ToArray();

            var constructorArgs = constructorParams
                .Select(p => serviceProvider.GetService(p))
                .ToArray();

            var command = (ICommand)constructor.Invoke(constructorArgs);

            return command;
        }
    }
}
