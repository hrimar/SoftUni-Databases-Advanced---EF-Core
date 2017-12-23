using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class CreateTeamCommand
    {
        // CreateTeam <name> <acronym> <description>
        public string Execute(string[] commandArgs)
        {
            if (commandArgs.Length < 2)
            {
                throw new FormatException(Constants.ErrorMessages.InvalidArgumentsCount);
            }

            AuthenticationManager.Authorize();

            string teamName = commandArgs[0];
            if (teamName.Length > 25)
            {
                throw new ArgumentException(Constants.ErrorMessages.InvalidTeamName);
            }

            if (CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamExists, teamName));
            }

            string acronym = commandArgs[1];
            if (acronym.Length != 3)
            {
                throw new ArgumentException(Constants.ErrorMessages.InvalidAcronym);
            }

            string description = null;
            if (commandArgs.Length > 2)
            {
                description = string.Join(" ", commandArgs.Skip(2));
                if (description.Length > 32)
                {
                    throw new ArgumentException(string.Format(Constants.ErrorMessages.InvalidDescription, 32));
                }
            }

            CreateTeam(teamName, acronym, description);

            return $"Team {teamName} successfully created!";
        }

        private static void CreateTeam(string teamName, string acronym, string description)
        {
            var currentTeam = new Team()
            {
                Name = teamName,
                Acronym = acronym,
                Description = description,
                CreatorId = AuthenticationManager.GetCurrentUser().Id
            };

            using (var db = new TeamBuilderContext())
            {
                db.Teams.Add(currentTeam);
                db.SaveChanges();

                var newTeamId = db
                    .Teams
                    .First(t => t.Name == teamName)
                    .Id;

                db.UserTeams.Add(new UserTeam()
                {
                    UserId = AuthenticationManager.GetCurrentUser().Id,
                    TeamId = newTeamId
                });
                db.SaveChanges();
            }

        }
    }
}
