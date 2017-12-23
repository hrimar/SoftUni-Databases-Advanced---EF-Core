namespace PhotoShare.Client.Core.Commands
{
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Data;
    using System;
    using System.Linq;
    using PhotoShare.Models;
    using System.Text;

    public class PrintFriendsListCommand 
    {
        // PrintFriendsList <username>
        public static string Execute(string[] data)
        {
            string username = data[1];
                      
            var sb = new StringBuilder();

            using (var db = new PhotoShareContext())
            {
                var checkUser = db.Users
                    .Where(u => u.Username == username)
                    .FirstOrDefault();

                if (checkUser == null)
                {
                    throw new ArgumentException($"Username {username} not found!");
                }


                var userId = db.Users.Where(u => u.Username == username)
                    .Select(i => i.Id)
                    .FirstOrDefault();

                var friends = db.Friendships.GroupBy(i => i.UserId);


                Console.WriteLine("Friends:");
                foreach (var friend in friends)
                {
                    foreach (var f in friend)
                    {
                        var fren = db.Users.Where(u=>u.Id == f.FriendId).FirstOrDefault();

                        sb.AppendLine($"-{fren.Username}");
                    }
                }
            }
            return sb.ToString();
        }
    }
}
