using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class LoginCommand
    {
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(2, inputArgs);

            string username = inputArgs[0];
            string password = inputArgs[1];

            if(AuthenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LogoutFirst);
            }

            User user = this.GetUserByCredentials(username, password);

            if(user == null)
            {
                throw new ArgumentException(Constants.ErrorMessages.UserOrPasswordIsInvalid);
            }

            AuthenticationManager.Login(user);

            return $"User {user.Username} successfuly logged in!";
        }

        private User GetUserByCredentials(string username, string password)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                var currentUser = context.Users
                    .Where(u => u.Username == username && u.Password == password && !u.IsDeleted)
                    .FirstOrDefault();

                return currentUser;
            }
        }
    }
}
