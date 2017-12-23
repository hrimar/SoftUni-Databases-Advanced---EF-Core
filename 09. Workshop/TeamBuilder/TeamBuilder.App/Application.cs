using System;
using TeamBuilder.App.Core;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App
{
    class Application
    {
        static void Main()
        {
            // ResetDatabase();

            CommandDispatcher commandDispatcher = new CommandDispatcher();
            Engine engine = new Engine(commandDispatcher);
            engine.Run();
        }

        private static void ResetDatabase()
        {
            using (var db = new TeamBuilderContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                Console.WriteLine("Database successfully created!");
            }
        }
    }
}
