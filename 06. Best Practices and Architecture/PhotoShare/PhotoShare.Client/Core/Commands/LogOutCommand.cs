using PhotoShare.Data;
using PhotoShare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoShare.Client.Core.Commands
{
    class LogOutCommand
    {
        public static string Execute(string[] data, PhotoShareContext context)
        {
            User currentlyLoggedUser = Session.User;

            if (currentlyLoggedUser == null)
            {
                throw new InvalidOperationException("You should log in first in order to logout.");
            }

            Session.User = null;
            return $"User {currentlyLoggedUser.Username} successfully logged out!";
        }
    }
}
