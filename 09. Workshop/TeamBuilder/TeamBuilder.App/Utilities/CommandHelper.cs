using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Utilities
{
    public class CommandHelper
    {
        public static bool IsTeamExisting(string teamName)
        {
            using (TeamBuilderContext db = new TeamBuilderContext())
            {
                return db.Teams.Any(t => t.Name == teamName);
            }
        }

        public static bool IsUserExisting(string username)
        {
            using (TeamBuilderContext db = new TeamBuilderContext())
            {
                return db.Users.Any(t => t.Username == username && t.IsDeleted == false);
            }
        }

        public static bool IsInviteExisting(string teamName, User user)
        {
            using (TeamBuilderContext db = new TeamBuilderContext())
            {
                return db.Invitations.Any(i => i.Team.Name == teamName && i.InvitedUserId == user.Id && i.IsActive);
            }
        }

        public static bool IsEventExisting(string eventName)
        {
            using (TeamBuilderContext db = new TeamBuilderContext())
            {
                return db.Events.Any(e => e.Name == eventName);
            }
        }


        public static bool IsMemberOfTeam(string teamName, string username)
        {
            using (TeamBuilderContext db = new TeamBuilderContext())
            {
                return db.Teams
                    .Single(t => t.Name == teamName)
                    .Members.Any(m => m.User.Username == username);

            }
        }

        public static bool IsUserCreatorOfTeam(string teamName, User user)
        {
            using (TeamBuilderContext db = new TeamBuilderContext())
            {
                return db.Teams
                    .Any(t => t.Name == teamName && t.Creator.Id == user.Id);
            }
        }

        public static bool IsUserCreatorOfEvent(string eventName, User user)
        {
            using (TeamBuilderContext db = new TeamBuilderContext())
            {
                return db.Events.Any(createdEvent =>
                    createdEvent.Name == eventName &&
                    createdEvent.CreatorId == user.Id);
            }
        }

        public static bool IsTeamAlreadyAddedToEvent(string teamName, string eventName)
        {
            var result = false;

            using (var db = new TeamBuilderContext())
            {
                if (db.EventTeams.Any(te => te.Event.Name == eventName && te.Team.Name == teamName))
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
