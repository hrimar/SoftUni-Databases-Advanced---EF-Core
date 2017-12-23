namespace P1.EmployeesMapping.App
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using AutoMapper;
    using EmployeesMapping.Model;
   // using EmployeesMapping.DtoModel;
    using global::EmployeesMapping.DtoModel;

    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, Employee>();

            CreateMap<Employee, EmployeePersonalDto>();

            CreateMap<Employee, ManagerDto>();

            CreateMap<Employee, EmployeeBirthdayDto>();
        }
    }
}
