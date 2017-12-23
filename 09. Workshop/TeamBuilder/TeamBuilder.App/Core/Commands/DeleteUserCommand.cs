using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class DeleteUserCommand
    {
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(0, inputArgs);
            AuthenticationManager.Authorize();

            User currentUser = AuthenticationManager.GetCurrentUser();

            DeleteUser(currentUser.Id);

            
            return $"User {currentUser.Username} was deleted seccesfully!";
        }

        private void DeleteUser(int id)
        {
            using (var context = new TeamBuilderContext())
            {
                var user = context.Users.First(u => u.Id == id);

                user.IsDeleted = true;
                context.SaveChanges();
                                
            }

            AuthenticationManager.Logout();
        }
    }
}
