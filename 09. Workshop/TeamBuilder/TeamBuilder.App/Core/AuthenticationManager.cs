namespace TeamBuilder.App.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Data;
    using TeamBuilder.Models;

    public class AuthenticationManager
    {
        private static User currentUser;


        public static void Login(User user)
        {
           if(currentUser != null)   
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LogoutFirst);
            }

            currentUser = user;
        }

        public static void Logout()
        {
            Authorize();

            currentUser = null;           
        }

        public static bool IsAuthenticated()
        {
            if (currentUser == null)
            {
                return false;
            }

            return true;
        }

        public static User GetCurrentUser()
        {
            Authorize();

            return currentUser;
        }


        public static void Authorize()
        {
            if(currentUser == null)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LoginFirst);
            }
        }
        
    }
}
