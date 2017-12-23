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
    public class ShowEventCommand
    {
        // ShowEvent <eventName>
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(1, inputArgs);

            string eventName = inputArgs[0];
            if (!CommandHelper.IsEventExisting(eventName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.EventNotFound, eventName));
            }

            string result = GetEventDetails(eventName);

            return result;
        }


        private static string GetEventDetails(string eventName)
        {
            var result = new StringBuilder();

            using (var context = new TeamBuilderContext())
            {
                
                var eventsDetails = context
                    .Events
                    .Where(e => e.Name == eventName)
                    .Select(e => new
                    {
                        EventName = e.Name,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                        Description = e.Description,
                        Teams = e.ParticipatingTeams
                            .Select(te => new
                            {
                                TeamName = te.Team.Name
                            })
                            .ToArray()
                    })
                    .ToArray();

                foreach (var currentEvent in eventsDetails)
                {
                    string startDate = string.Empty;
                    string endDate = string.Empty;

                    if (currentEvent.StartDate != null)
                    {
                        startDate = " " + currentEvent.StartDate.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    }
                    if (currentEvent.EndDate != null)
                    {
                        endDate = " " + currentEvent.EndDate.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    }

                    result.AppendLine($"{currentEvent.EventName}{startDate}{endDate}");

                    if (currentEvent.Description != null)
                    {
                        result.AppendLine(currentEvent.Description);
                    }

                    if (currentEvent.Teams.Length > 0)
                    {
                        result.AppendLine("Teams:");

                        foreach (var team in currentEvent.Teams)
                        {
                            result.AppendLine($"-{team.TeamName}");
                        }
                    }
                }
            }

            return result.ToString();
        }
    }
}
