using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data;       // !
using P02_DatabaseFirst.Data.Models;    // !
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;  // !
using System.Text.RegularExpressions;

namespace P02_DatabaseFirst
{
    class StartUp
    {
        static void Main()
        {
            using (var db = new SoftUniContext())
            {
                //// 03. Employees Full Information
                //var employees = db.Employees
                //        .Select(e => new
                //        {
                //            e.FirstName,
                //            e.LastName,
                //            e.MiddleName,
                //            e.JobTitle,
                //            e.Salary
                //        })
                //            .OrderBy(a => a)
                //            .ToList();  // само за принт с foreach може и без ToList()

                //foreach (var em in employees)
                //{
                //    Console.WriteLine($"{em.FirstName} {em.LastName} {em.MiddleName} {em.JobTitle} { em.Salary:f2}");

                //}

                //// 4.	Employees with Salary Over 50 000
                //var employees = db.Employees
                //        //.Where(s => s.Salary > 50000)
                //        .Select(e => new
                //        {
                //            e.FirstName,
                //             
                //            e.Salary // Where da e 1-vo и после Select само на FirtName!!!
                //        })
                //        .Where(s => s.Salary > 50000)
                //            .OrderBy(a => a.FirstName)
                //            .ToList();

                //foreach (var em in employees)
                //{
                //    Console.WriteLine($"{em.FirstName}");
                //}

                //// 5. Employees from Research and Development
                ////Да се принтнат само хора от департамент "Research and Development":
                //var employees = db.Employees
                //    .Where(d => d.Department.Name == "Research and Development")
                //    .OrderBy(a => a.Salary)
                //    .ThenByDescending(a => a.FirstName)
                //     .Select(e => new
                //     {
                //         Name = $"{e.FirstName} {e.LastName}", // вместо всяко име по отделно
                //         DepartmentName = e.Department.Name,
                //         e.Salary
                //     })
                //      .ToList();

                //foreach (var em in employees)
                //{
                //    Console.WriteLine($"{em.Name} from {em.DepartmentName} - ${em.Salary:f2}");
                //}

                ////6. На Town с Id 4  да се добави адрес "..." и да се зададе на емплой Наков: - ПАК!
                //var addAddress = new Address()
                //{
                //    AddressText = "Vitoshka 15",
                //    TownId = 4
                //};

                //db.Addresses.Add(addAddress);         //- запомняме адреса
                //var nakov = db.Employees.Where(e => e.LastName == "Nakov").FirstOrDefault();
                //nakov.Address = addAddress; // намираме само един човек с това име и му даваме ст-ст
                //db.SaveChanges(); // ако искаме на всички хора по дадено условие да дадем ст-сти, 
                //// то махаме FirstOrDefault() и добавяме адреса във foreach

                //// или това решение:
                ////var surchedMen = db.Employees.SingleOrDefault(e => e.LastName == "Nakov");
                ////if (surchedMen != null)
                ////{
                ////    surchedMen.Address = addAddress;
                ////    db.SaveChanges();
                ////}

                //var result = db.Employees
                //        .OrderByDescending(a => a.AddressId)
                //        .Take(10)
                //        .Select(e => new { e.Address.AddressText });
                //        //.ToList();

                //foreach (var r in result)
                //{
                //    Console.WriteLine($"{r.AddressText}");
                //}

                // 7. Employees and Projec - ПАК !!!
                //var employees = db.Employees
                //    .Where
                //    (
                //    e => e.EmployeeProjects.Any(ep =>
                //                              ep.Project.StartDate.Year >= 2001 &&
                //                              ep.Project.StartDate.Year <= 2003)
                //    )
                //    .Take(30)
                //    .Select(e => new
                //    {
                //        Name = $"{e.FirstName} {e.LastName}",
                //        ManagerName = $"{e.Manager.FirstName} {e.Manager.LastName}",
                //        Projects = e.EmployeeProjects.Select(ep => new
                //                           {
                //                               ep.Project.Name,
                //                               ep.Project.StartDate,
                //                               ep.Project.EndDate
                //                           })                        
                //    });
                //foreach (var e in employees)
                //{
                //    Console.WriteLine($"{e.Name} - Manager: {e.ManagerName}");
                //    foreach (var i in e.Projects)
                //    {
                //        Console.Write($"--{i.Name} - " +
                //    $"{i.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - ");
                //        if (i.EndDate == null)
                //            Console.WriteLine("not finished");
                //        else
                //        Console.WriteLine($"{i.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");
                //    }
                //}

                //// 8.Addresses by Town  
                //// С Include не става Count, a triabva da e taka, no с Select stava - OK
                ////var emloyees = db.Addresses.Include(t => t.Town)
                ////        .OrderByDescending(e => e.Employees.Count)
                ////        .ThenBy(t => t.Town.Name)
                ////        .ThenBy(a => a.AddressText)
                ////        .Take(10)
                ////        .ToList();

                //var emloyees = db.Addresses
                //        .Select(e => new
                //        {
                //            EmpCount = e.Employees.Count,
                //            TownsSort =e.Town.Name,
                //            AddText = e.AddressText
                //        })
                //        .OrderByDescending(e => e.EmpCount)
                //        .ThenBy(t => t.TownsSort)
                //        .ThenBy(t => t.AddText)
                //        .Take(10);
                //foreach (var e in emloyees)
                //{
                //    Console.WriteLine($"{e.AddText}, {e.TownsSort} - {e.EmpCount} employees");
                //}

                ////9.Employee 147 - ПАК !!! - ОК
                //var person = db.Employees                  
                //    .Select(e => new
                //     {
                //        EmpId = e.EmployeeId,
                //         Name = $"{e.FirstName} {e.LastName}",
                //         Job = $"{e.JobTitle}",
                //         Proj = e.EmployeeProjects.Select(p => new 
                //         {
                //            p.Project.Name 
                //        })
                //     })
                //     .FirstOrDefault(e => e.EmpId == 147)
                //    ;

                //Console.WriteLine($"{person.Name} - {person.Job}");
                //foreach (var p in person.Proj.OrderBy(pr=>pr.Name))
                //{
                //    Console.WriteLine(p.Name);
                //}



                ////10.Departments with More Than 5 Employees - ПАК!-ок
                //var departments = db.Departments//.Include(e => e.Employees)
                //    .Where(e => e.Employees.Count() > 5)
                //    .OrderBy(e => e.Employees.Count())
                //    .ThenBy(d => d.Name)                // това не е по условие а трябва !?!
                //    .Select(e => new 			// това е анонимен обект !
                //    {
                //        e.Employees,
                //        e.Manager,
                //        e.Name
                //    })
                //    ;

                //foreach (var d in departments)
                //{
                //    Console.WriteLine($"{d.Name} - {d.Manager.FirstName} {d.Manager.LastName}");
                //    foreach (var e in d.Employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
                //    {
                //        Console.WriteLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                //    }
                //    Console.WriteLine("----------");
                //}

                ////11.Find Latest 10 Projects - Пример за формат на дата и Ордер в foreach-a: -OK
                //var projects = db.Projects
                //    .OrderByDescending(a => a.StartDate)
                //    .Take(10);

                //foreach (var p in projects.OrderBy(p => p.Name))
                //{
                //    Console.WriteLine($"{p.Name}");
                //    Console.WriteLine($"{p.Description}");
                //    Console.WriteLine($"{p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");
                //}

                ////12.	Increase Salaries
                ////var selectedDepartments = db.Employees
                ////.Select(e=>new
                ////{
                ////    EmpF = e.FirstName,
                ////    EmpL = e.LastName,
                ////    e.Salary, // но след селект не може да слагаме нова ст-ст на това проп-ти
                ////    Deps = e.Department
                ////})
                ////.Where(d => d.Deps.Name == "Engineering" ||
                ////d.Deps.Name == "Tool Design" || d.Deps.Name == "Marketing" ||
                ////d.Deps.Name == "Information Services")
                ////.OrderBy(e => e.EmpF)
                ////.ThenBy(e => e.EmpL);

                //var selectedDepartments = db.Employees // виж и https://github.com/gaydov/Softuni-DB-Advanced/blob/master/5IntroToEFCore/IncreaseSalaries/Launcher.cs
                //.Where(d => d.Department.Name == "Engineering" ||
                //d.Department.Name == "Tool Design" || d.Department.Name == "Marketing" ||
                //d.Department.Name == "Information Services")
                //.OrderBy(e => e.FirstName)
                //.ThenBy(e => e.LastName);

                //foreach (var d in selectedDepartments)
                //{
                //    d.Salary = d.Salary * 1.12m;
                //    Console.WriteLine($"{d.FirstName} {d.LastName} (${d.Salary:f2})");                   
                //}
                //db.SaveChanges();


                //// 13.	Find Employees by First Name Starting With "Sa" - OK
                //var surchedEmp = db.Employees
                //     .Where(e => e.FirstName.StartsWith("Sa"))
                //     .Select(e => new
                //     {
                //         e.FirstName,
                //         e.LastName,
                //         e.JobTitle,
                //         e.Salary
                //     })
                //     .OrderBy(e=>e.FirstName)
                //     .ThenBy(e=>e.LastName);
                //foreach (var e in surchedEmp)
                //{
                //    Console.WriteLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");

                //}

                //// 14.	Delete Project by Id -  да изтрие проект с Id 2, а за целта и всички свързани с него от мапинг табл-та:
                //// При такива задачи се мисли къде има връзки нещото, което искаме да изтрием и може да гръмне
                //// a данните вързани с тока, което ще трием ги слагаме на NULL 
                //var empProject = db.EmployeesProjects.Where(p => p.ProjectId == 2);

                //foreach (var ep in empProject)  // за да се изтрие не само един проект а всички такива
                //{
                //    db.EmployeesProjects.Remove(ep);
                //}

                //var project = db.Projects.Find(2);
                //db.Projects.Remove(project);
                //db.SaveChanges();

                //var projectNames = db.Projects
                //    .Take(10)
                //    .Select(p => p.Name);

                //foreach (var p in projectNames)
                //{
                //    Console.WriteLine(p);
                //}                
            }

            //15.Remove Towns- да се изтрие подаден град, а следователно и адресите в него.
            // Да се принтират адресите, които са изтрити. За целта AdressId на хората които живеят там да се сложи на NULL
            var towN = Console.ReadLine();
            using (var db = new SoftUniContext())
            {
                var selectedAddress = db.Addresses.Where(t => t.Town.Name == towN).ToList()  ;
                
                foreach (var employee in db.Employees)
                {
                    if(selectedAddress.Contains(employee.Address))
                    {
                        employee.AddressId = null;
                    }
                }
                db.Addresses.RemoveRange(selectedAddress);
                Town townToBeDeleted = db.Towns.SingleOrDefault(t => t.Name.Equals(towN));
                db.Towns.Remove(townToBeDeleted);

                int countOdRemovedAdresses = selectedAddress.Count;

                string addressAddresses;
                string wasWere;
                if (countOdRemovedAdresses > 1)
                {
                    addressAddresses = "addresses";
                    wasWere = "were";
                }
                else
                {
                    addressAddresses = "address";
                    wasWere = "was";
                }
                Console.WriteLine($"{countOdRemovedAdresses} {addressAddresses} in {towN} {wasWere} deleted");

                db.SaveChanges();
            }

        }
    }
}
