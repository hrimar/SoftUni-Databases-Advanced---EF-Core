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
    public class CreateEventCommand
    {
        // CreateEvent <name> <description> <startDate> <endDate>
        public string Execute(string[] inputArgs)
        {
            if (inputArgs.Length < 6)
            {
                throw new FormatException(Constants.ErrorMessages.InvalidArgumentsCount);
            }

            int inputLenght = inputArgs.Length;

            var currentUser = AuthenticationManager.GetCurrentUser();

            string eventName = inputArgs[0];
            if (eventName.Length > 25)
            {
                throw new ArgumentException("Event name should be no longer than 25 symbols");
            }

            string description = string.Join(" ", inputArgs.Skip(1).Take(inputLenght - 5));
            if (description.Length > 32)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.InvalidDescription, 32));
            }

            DateTime startDate;
            startDate = ValidateDate($"{inputArgs[inputLenght - 4]} {inputArgs[inputLenght - 3]}");

            DateTime endDate = ValidateDate($"{inputArgs[inputLenght - 2]} {inputArgs[inputLenght - 1]}");
            if (endDate <= startDate)
            {
                throw new ArgumentException(Constants.ErrorMessages.InvalidEndDate);
            }

            CreateCurrentEvent(currentUser, eventName, description, startDate, endDate);

            return $"Event {eventName} was created successfully!";
        }

        private static DateTime ValidateDate(string date)
        {
            try
            {
                DateTime startDate = DateTime.ParseExact(date, Constants.DateTimeFormat, CultureInfo.InvariantCulture);

                return startDate;
            }
            catch (Exception)
            {
                throw new FormatException(Constants.ErrorMessages.InvalidDateFormat);
            }
        }

        private static void CreateCurrentEvent(User currentUser, string eventName, string description, DateTime startDate, DateTime endDate)
        {
            var currentEvent = new Event()
            {
                Name = eventName,
                Description = description,
                StartDate = startDate,
                EndDate = endDate,
                CreatorId = currentUser.Id
            };

            using (var context = new TeamBuilderContext())
            {
                var isEventExisting = context.Events
                .Any(e => e.Name == currentEvent.Name && e.StartDate == currentEvent.StartDate);

                if (isEventExisting)
                {
                    throw new InvalidOperationException(Constants.ErrorMessages.EventAlreadyExist);
                }

                context.Events.Add(currentEvent);
                context.SaveChanges();
            }
        }
    }
}
