using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class AcceptInviteCommand
    {
        public string Execute(string[] commandArgs)
        {
            Check.CheckLength(1, commandArgs);
            AuthenticationManager.Authorize();

            string teamName = commandArgs[0];

            if ( !CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            var currentUser = AuthenticationManager.GetCurrentUser();
           

            if ( !CommandHelper.IsInviteExisting(teamName, currentUser) )
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.InviteNotFound, teamName));
            }

            AcceptInvitation(teamName, currentUser);

            return $"User {currentUser.Username} joined team {teamName}!";
        }

        private static void AcceptInvitation(string teamName, User currentUser)
        {
            using (var db = new TeamBuilderContext())
            {
                var teamId = db
                    .Teams
                    .First(t => t.Name == teamName)
                    .Id;

                var newMembership = new UserTeam()
                {
                    TeamId = teamId,
                    UserId = currentUser.Id
                };

                db.UserTeams.Add(newMembership);
                db.SaveChanges();

                var invitationToMarkAsInactive = db
                    .Invitations
                    .First(i => i.TeamId == teamId && i.InvitedUserId == currentUser.Id);

                invitationToMarkAsInactive.IsActive = false;
                db.SaveChanges();
            }
        }
    }
}
