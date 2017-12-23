namespace EmployeesMapping.Services
{
    using System;
    using P1.EmployeesMapping.Data;
    using EmployeesMapping.DtoModel;
    using AutoMapper;
    using P1.EmployeesMapping.Model;
    using System.Text;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
  using AutoMapper.QueryableExtensions;

    public class EmployeeService
    {
        private readonly EmployeesMappingContext context;

        public EmployeeService(EmployeesMappingContext context)
        {
            this.context = context;
        }

        public EmployeeDto ById(int employeeId)
        {
            var employee = context.Employees.Find(employeeId);


            var employeeDto = Mapper.Map<EmployeeDto>(employee);

            return employeeDto;
        }

        public void AddEmployee(EmployeeDto dto)
        {
            var employee = Mapper.Map<Employee>(dto);  //

            context.Employees.Add(employee);

            context.SaveChanges(); 
        }

        public string SetBirthday(int employeeId, DateTime date)
        {
            var employee = context.Employees.Find(employeeId);

            employee.Birthday = date;

            context.SaveChanges();

            return $"{employee.FirstName} {employee.LastName}";
        }

        public string SetAddress(int employeeId, string address)
        {
            var employee = context.Employees.Find(employeeId);

            employee.Address = address;

            context.SaveChanges();

            return $"{employee.FirstName} {employee.LastName}";
        }

        public EmployeePersonalDto PersonalById(int employeeId)
        {
            var employee = context.Employees.Find(employeeId);
            
            var employeePersonalDto = Mapper.Map<EmployeePersonalDto>(employee);

            return employeePersonalDto;
        }

        public void SetManager(int employeeId, int managerId)
        {
            var employee = context.Employees.Find(employeeId);
            var manager = context.Employees.Find(managerId);
            
            employee.Manager = manager;  // 

            context.SaveChanges();          
        }

        public string ManagerInfo(int employeeId)
        {
            var employee = context.Employees.Find(employeeId);

            var managerDto = Mapper.Map<ManagerDto>(employee);

            var sb = new StringBuilder();
            sb.AppendLine($"{managerDto.FirstName} {managerDto.LastName} | Employees: " +
                $"{managerDto.ManagedEmployees.Count}");
            foreach (var me in managerDto.ManagedEmployees)
            {
                sb.AppendLine($"- {me.FirstName} {me.LastName} - ${me.Salary:f2}");
            }
            return sb.ToString();
        }

        public IList<EmployeeBirthdayDto>ListEmployeesOlderThan(int age)
        {
          List<EmployeeBirthdayDto> employees = this.context
            .Employees
            .Include(e => e.Manager)
            .Where(e => e.Birthday != null && Calculations.CalcCurrentAge(e.Birthday.Value) > age)
            .OrderByDescending(e => e.Salary)
            .ProjectTo<EmployeeBirthdayDto>()
            .ToList();

            return employees;
        }
    }
}
