

namespace HospitalStartUp
{
    using System;
    using P01_HospitalDatabase;
    using P01_HospitalDatabase.Data;
    using P01_HospitalDatabase.Data.Models;
    using P01_HospitalDatabase.Initializer;
    using Microsoft.EntityFrameworkCore;

    class StartUp

    {
        static void Main(string[] args)
        {
            DatabaseInitializer.ResetDatabase(); //това трие базата, прави я с миграциите и я пълни с данните

            using (var db = new HospitalContext())
            {
                //db.Database.EnsureCreated();
                db.Database.Migrate();
                // DatabaseInitializer.SeedPatients(db, 100); // за добавяне на нови 100 човека
            }
        }
    }
}
