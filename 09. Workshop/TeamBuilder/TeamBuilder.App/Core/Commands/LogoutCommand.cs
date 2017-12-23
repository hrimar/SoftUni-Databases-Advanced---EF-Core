using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class LogoutCommand
    {
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(0, inputArgs);
            AuthenticationManager.Authorize(); //
            User currentUser = AuthenticationManager.GetCurrentUser();

            AuthenticationManager.Logout();

            return $"User {currentUser.Username} successfully logged out!";

        }
    }
}
