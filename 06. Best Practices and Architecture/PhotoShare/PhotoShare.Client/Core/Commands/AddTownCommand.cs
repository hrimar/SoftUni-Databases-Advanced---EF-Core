namespace PhotoShare.Client.Core.Commands
{
    using Models;
    using Data;
    using System.Linq;
    using System;

    public class AddTownCommand
    {
        // AddTown <townName> <countryName>
        public static string Execute(string[] data)
        {
            string townName = data[1];
            string country = data[2];

            using (PhotoShareContext context = new PhotoShareContext())
            {
                if(context.Towns.Any(t=>t.Name == townName))
                {
                    throw new ArgumentException($"Town {townName} was already added!");
                }

                //var townCheck = context.Towns
                //    .AsNoTracking() // не искаме да я тракваме, защото само проверяваме
                //    .Where(t => t.Name == townName)
                //    .FirstOrDefault();

                Town town = new Town
                {
                    Name = townName,
                    Country = country
                };

                context.Towns.Add(town);
                context.SaveChanges();

                return townName + " was added to database!";
            }
        }
    }
}
