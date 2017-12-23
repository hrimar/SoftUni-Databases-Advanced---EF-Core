using PhotoShare.Data;
using PhotoShare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoShare.Client.Core.Commands
{
    public class LoginCommand
    {
        public static string Execute(string[] data, PhotoShareContext context)
        {
            //Login < username > < password >
            string username = data[1];
            string password = data[2];

           User user = context.Users.Where(u => u.Username == username).SingleOrDefault();
                       

                if (user == null)
                {                   
                    throw new ArgumentException($"Invalid username or password!");
                }
                if ( user.Password != password)
                {
                    throw new ArgumentException($"Invalid username or password!");
                }

                if (Session.User != null)  //user.Username == username && user.Password == password)
                {
                    throw new ArgumentException($"You should logout first!");
                }

            
            user.LastTimeLoggedIn = DateTime.Now;
            context.SaveChanges();
            Session.User = user;

            return $"User {username} successfully logged in!";
        }
    }
}
