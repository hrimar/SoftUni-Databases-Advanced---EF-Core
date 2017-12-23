using System;

using Instagraph.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using System.Collections.Generic;
using Instagraph.DataProcessor.DtoModels;
using Newtonsoft.Json;
using AutoMapper.QueryableExtensions;

namespace Instagraph.DataProcessor
{
    public class Serializer
    {
        public static string ExportUncommentedPosts(InstagraphContext context)
        {
            var posts = context.Posts
                .Where(p => p.Comments.Count() == 0)
                .Select(p => new
                {
                    Id = p.Id,
                    Picture = p.Picture.Path,
                    User = p.User.Username
                })
                .OrderBy(p => p.Id)
                .ToArray();


            var jsonString = JsonConvert.SerializeObject(posts, Formatting.Indented,
                     new JsonSerializerSettings()
       
              {
                         DefaultValueHandling = DefaultValueHandling.Ignore
                     });
            return jsonString;
        }

        public static string ExportPopularUsers(InstagraphContext context)
        {
            var users = context.Users
                .Where(u => u.Posts
                    .Any(p => p.Comments
                        .Any(c => u.Followers
                            .Any(f => f.FollowerId == c.UserId))))
                  .OrderBy(u => u.Id)
                  .Select(u => new
                  {
                      Username = u.Username,
                      Followers = u.Followers.Count()
                  })
                .ToArray();
            string jsonString = JsonConvert.SerializeObject(users, Formatting.Indented);
            //var jsonString = JsonConvert.SerializeObject(users, Formatting.Indented,
            //        new JsonSerializerSettings()
            //        {
            //            DefaultValueHandling = DefaultValueHandling.Ignore
            //        });
            return jsonString;

            //// Негово гърмящо решение:
            //var users = context.Users
            //    .Where(u => u.Posts
            //        .Any(p => p.Comments
            //            .Any(c => u.Followers
            //                .Any(f => f.FollowerId == c.UserId))))
            //    .OrderBy(u => u.Id)
            //    .ProjectTo<PopularUserDto>()
            //    .ToArray();

            //string jsonString = JsonConvert.SerializeObject(users, Formatting.Indented);

            //return jsonString;
        }

        public static string ExportCommentsOnPosts(InstagraphContext context)
        {
            var users = context.Users
            .Select(u => new
            {
                Username = u.Username,
                PostsCommentsCount = u.Posts.Select(p => p.Comments.Count()).ToArray()
            });

            // при селекцията гърми ако взимаме 1 ст-ст от базата, която е int, но ако взимаме масив той може да е празен и не гърми


            var userDtos = new List<UserTopPostDto>();

            var xmlDoc = new XDocument();
            xmlDoc.Add(new XElement("users")); // това е root-a

            foreach (var user in users)
            {
                int mostComents = 0;
                if (user.PostsCommentsCount.Any())
                {
                    mostComents = user.PostsCommentsCount.OrderByDescending(c => c).First();
                }

                // правим ДТО (string, int) за ст-те които да запомни и подаде на XML-a
                var userDto = new UserTopPostDto()
                {
                    Username = user.Username,
                    MostComments = mostComents
                };

                userDtos.Add(userDto);
            }

            userDtos = userDtos.OrderByDescending(u => u.MostComments).ThenBy(u => u.Username).ToList();


            foreach (var u in userDtos)
            {
                xmlDoc.Root.Add(new XElement("user",
                new XElement("Username", u.Username),
                new XElement("MostComments", u.MostComments)));
            }

            //foreach (var u in userDtos)
            //{
            //    var userNames = new XElement("user");
            //    userNames.Add(new XElement("Username", u.Username));
            //    userNames.Add(new XElement("MostComments", u.MostComments));
            //    xmlDoc.Root.Add(userNames);
            //}


            string xmlString = xmlDoc.ToString();          
            return xmlString;

        }
    }
}
