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
    public class DeclineInviteCommand
    {
        // DeclineInvite <teamName>
        public string Execute(string[] commandArgs)
        {
            Check.CheckLength(1, commandArgs);
            AuthenticationManager.Authorize();

            string teamName = commandArgs[0];
            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            var currentUser = AuthenticationManager.GetCurrentUser();

            DeclineInvitation(teamName, currentUser);

            return $"Invite from {teamName} declined.";
        }

        private static void DeclineInvitation(string teamName, User currentUser)
        {
            using (var db = new TeamBuilderContext())
            {
                var teamId = db
                    .Teams
                    .First(t => t.Name == teamName)
                    .Id;

                var invitationToMarkAsInactive = db
                   .Invitations
                   .First(i => i.TeamId == teamId && i.InvitedUserId == currentUser.Id);

                invitationToMarkAsInactive.IsActive = false;
                db.SaveChanges();
            }
        }

    }
}
