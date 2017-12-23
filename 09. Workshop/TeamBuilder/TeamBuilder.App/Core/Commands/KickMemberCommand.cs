namespace TeamBuilder.App.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Data;
    using TeamBuilder.Models;
    using Microsoft.EntityFrameworkCore;

    public class KickMemberCommand
    {
        // KickMember<teamName> <username>
        public string Execute(string[] commandArgs)
        {
            Check.CheckLength(2, commandArgs);
            AuthenticationManager.Authorize();

            string teamName = commandArgs[0];
            string userName = commandArgs[1];
            ValidateCommand(teamName, userName);


            using (var db = new TeamBuilderContext())
            {
                var team = db
                    .Teams
                    .First(t => t.Name == teamName);

                var userTobeKicked = db
                    .Users
                    .First(u => u.Username == userName);

                var membership = db
                    .UserTeams
                    .First(t => t.TeamId == team.Id && t.UserId == userTobeKicked.Id);

                db.UserTeams.Remove(membership);
                db.SaveChanges();
            }

            return $"User {userName} was kicked from {teamName}!";
        }
                

        private static void ValidateCommand(string teamName, string userName)
        {
            var currentUser = AuthenticationManager.GetCurrentUser();

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            if (!CommandHelper.IsUserExisting(userName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.UserNotFound, userName));
            }

            if (!CommandHelper.IsMemberOfTeam(teamName, userName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.NotPartOfTeam, userName, teamName));
            }

            if (!CommandHelper.IsUserCreatorOfTeam(teamName, currentUser))
            {
                throw new InvalidOperationException(string.Format(Constants.ErrorMessages.NotAllowed));
            }

            using (var db = new TeamBuilderContext())
            {
                var userToBeKicked = db
                    .Users
                    .First(u => u.Username == userName);

                if (CommandHelper.IsUserCreatorOfTeam(teamName, userToBeKicked))
                {
                    throw new InvalidOperationException("Command not allowed. Use DisbandTeam instead.");
                }
            }
        }
    }
}
