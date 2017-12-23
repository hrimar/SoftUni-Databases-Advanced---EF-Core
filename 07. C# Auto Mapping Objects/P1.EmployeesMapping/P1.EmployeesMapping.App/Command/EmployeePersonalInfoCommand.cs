namespace P1.EmployeesMapping.App.Command
{
    using global::EmployeesMapping.DtoModel;
    using global::EmployeesMapping.Services;
    using P1.EmployeesMapping.App.Command.Contracts;
    using System;
    using System.Globalization;

    class EmployeePersonalInfoCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public EmployeePersonalInfoCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        // <employeeId>
        public string Execute(params string[] args)
        {
            int employeeId = int.Parse(args[0]);

            var e = employeeService.PersonalById(employeeId);

            // тъй като датата e nullable то ако са null трябва да гo заменим с дуг стринг:
            string birthday = "[no birthday specified]";
            if(e.Birthday != null)
            {
                birthday = e.Birthday.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }

           // string birthday = e.Birthday.Value.ToString("dd-MM-yyyy") ?? "[no birthday specified]";

            string address = e.Address ?? "[no address specified]";

            string result= $"ID: {employeeId} - {e.FirstName} {e.LastName} - ${e.Salary:f2}"+Environment.NewLine+
                            $"Birthday: {birthday}"+ Environment.NewLine +
                            $"Address: {address}";

            return result;
        }
    }
}

