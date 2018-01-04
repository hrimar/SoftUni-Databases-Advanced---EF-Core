using System;

using Instagraph.Data;

using Newtonsoft.Json;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Xml.Linq;
using Instagraph.Data.Dto.Export;
using System.Collections.Generic;

namespace Instagraph.DataProcessor
{
    public class Serializer
    {
        public static string ExportUncommentedPosts(InstagraphContext context)
        {
            var posts = context.Posts
                .Where(c => c.Comments.Count() == 0)
                .Select(c => new {
                    Id = c.Id,
                    Picture = c.Picture.Path,
                    User = c.User.Username
                })
                .OrderBy(c=>c.Id)
                .ToArray();

            var json = JsonConvert.SerializeObject(posts, Formatting.Indented);
            return json;
        }

        public static string ExportPopularUsers(InstagraphContext context)
        {
            // Extract all users who have posts, commented by their followers
            var users = context.Users
                .Include(u=>u.Followers)
                .Include(u=>u.Posts)
                .ThenInclude(p=>p.Comments)
                .Where(u=>u.Posts.Any(p=>p.Comments.Any(c=> u.Followers.Any(f=>f.FollowerId == c.UserId))))
                 .Select(u => new 
                 {
                     Username = u.Username,
                     Followers = u.Followers.Count
                 })
                .ToArray();

            var json = JsonConvert.SerializeObject(users, Formatting.Indented);
            return json;
        }

        public static string ExportCommentsOnPosts(InstagraphContext context)
        {
            var users = context.Users
                .Include(u=>u.Posts)
                .ThenInclude(p=>p.Comments)
                .Select(u => new
                {
                    Username = u.Username,
                    MostComments = u.Posts
                            .Select(p => p.Comments.Count)
                            .ToArray()
                })
                 .ToArray();
                       
            var commentsOnPosts = new List<CommentsOnPostDto>();

            foreach (var u in users)
            {
                var username = u.Username;
                var countOfPosts = 0;

                if (u.MostComments.Any()) // if there has any Comments
                {
                    countOfPosts = u.MostComments.OrderByDescending(c => c).FirstOrDefault();
                }

                var dto = new CommentsOnPostDto()
                {
                    Username = username,
                    MostComments = countOfPosts
                };

               commentsOnPosts.Add(dto);
            }
            var orderedCommentsOnPosts = commentsOnPosts
                .OrderByDescending(cp => cp.MostComments)
                .ThenBy(cp => cp.Username);


            var xmlDoc = new XDocument(new XElement("user"));

            foreach (var cOnPost in orderedCommentsOnPosts)
            {
                xmlDoc.Root.Add(new XElement("user",
                    new XElement("Username", cOnPost.Username),
                    new XElement("MostComments", cOnPost.MostComments)));

                //// or
                //var user = new XElement("user");
                //user.Add(new XElement("Username", cOnPost.Username));
                //user.Add(new XElement("MostComments", cOnPost.MostComments));
                //xmlDoc.Root.Add(user);
            }

            string xmlString = xmlDoc.ToString();
            return xmlString;
        }
    }
}
