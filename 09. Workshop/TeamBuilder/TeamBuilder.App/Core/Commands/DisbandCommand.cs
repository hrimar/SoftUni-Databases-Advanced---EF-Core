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

    public class DisbandCommand
    {
        // DisbandCommand <temName>     
        public  string Execute(string[] commandArgs)
        {
            Check.CheckLength(1, commandArgs);
            AuthenticationManager.Authorize();

            string teamName = commandArgs[0];

            var currentUser = AuthenticationManager.GetCurrentUser();

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            if (!CommandHelper.IsUserCreatorOfTeam(teamName, currentUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            using (var db = new TeamBuilderContext())
            {
                var teamToBeDeleted = db
                    .Teams
                    .First(t => t.Name == teamName);

                var membershipsToBeDeleted = db
                    .UserTeams
                    .Where(ut => ut.TeamId == teamToBeDeleted.Id)
                    .ToList();

                var eventsToBeDeleted = db
                    .EventTeams
                    .Where(te => te.TeamId == teamToBeDeleted.Id)
                    .ToList();

                var invitationsToBeDeleted = db
                    .Invitations
                    .Where(i => i.TeamId == teamToBeDeleted.Id)
                    .ToList();

                db.UserTeams.RemoveRange(membershipsToBeDeleted);
                db.EventTeams.RemoveRange(eventsToBeDeleted);
                db.Invitations.RemoveRange(invitationsToBeDeleted);
                db.Teams.Remove(teamToBeDeleted);

                db.SaveChanges();
            }

            return $"{teamName} has disbanded!";
        }

       
    }
}
