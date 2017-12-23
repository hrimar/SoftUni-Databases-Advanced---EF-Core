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

    public class AddTeamToCommand
    {
        // AddTeamTo <eventName> <teamName>
        public  string Execute(string[] commandArgs)
        {
            Check.CheckLength(2, commandArgs);
            AuthenticationManager.Authorize();

            string eventName = commandArgs[0];
            string teamName = commandArgs[1];
            ValidateCommand(eventName, teamName);

            AddTeamTo(eventName, teamName);

            return $"Team {teamName} added for {eventName}!";
        }

        private static void AddTeamTo(string eventName, string teamName)
        {
            using (var db = new TeamBuilderContext())
            {
                var events = db
                    .Events
                    .Where(e => e.Name == eventName)
                    .ToList();

                var eventId = default(int);

                if (events.Count > 1)
                {
                    DateTime? latestDate = events
                        .Max(e => e.StartDate);                       

                    eventId = events
                        .Single(e => e.StartDate == latestDate)
                        .Id;
                }
                if (events.Count == 1)
                {
                    eventId = events.First().Id;
                }

                var teamId = db
                    .Teams
                    .First(t => t.Name == teamName)
                    .Id;

                var currentTeamEvent = new EventTeam()
                {
                    EventId = eventId,
                    TeamId = teamId
                };

                db.EventTeams.Add(currentTeamEvent);
                db.SaveChanges();
            }
        }

        private static void ValidateCommand(string eventName, string teamName)
        {
            var currentUser = AuthenticationManager.GetCurrentUser();

            if (!CommandHelper.IsEventExisting(eventName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.EventNotFound, eventName));
            }

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            if (!CommandHelper.IsUserCreatorOfEvent(eventName, currentUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            if (CommandHelper.IsTeamAlreadyAddedToEvent(teamName, eventName))
            {
                throw new InvalidOperationException("Cannot add same team twice!");
            }
        }
    }
}
