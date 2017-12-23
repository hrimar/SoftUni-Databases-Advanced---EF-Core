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
    public class ShowTeamCommand
    {
        public string Execute(string[] commandArgs)
        {
            Check.CheckLength(1, commandArgs);

            string teamName = commandArgs[0];
            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            string result = GetTeamDetails(teamName);

            return result;
        }

        private static string GetTeamDetails(string teamName)
        {
            var result = new StringBuilder();

            using (var db = new TeamBuilderContext())
            {
                var teamDetails = db
                    .Teams
                    .Where(t => t.Name == teamName)
                    .Select(t => new
                    {
                        Name = t.Name,
                        Acronym = t.Acronym,
                        Members = t.Members //
                        .Select(ut => new
                        {
                            UserName = ut.User.Username
                        })
                        .ToArray()
                    })
                    .First();

                result.AppendLine($"{teamDetails.Name} {teamDetails.Acronym}");
                if (teamDetails.Members.Length > 0)
                {
                    result.AppendLine("Members:");

                    foreach (var member in teamDetails.Members)
                    {
                        result.AppendLine($"--{member.UserName}");
                    }
                }
            }

            return result.ToString().TrimEnd();
        }
    }
}
