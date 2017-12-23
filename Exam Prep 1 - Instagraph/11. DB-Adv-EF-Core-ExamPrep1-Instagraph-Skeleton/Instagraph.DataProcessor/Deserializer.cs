using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

using Newtonsoft.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

using Instagraph.Data;
using Instagraph.Models;
using Instagraph.DataProcessor.DtoModels;
using System.ComponentModel.DataAnnotations;

namespace Instagraph.DataProcessor
{
    public class Deserializer
    {
        private static string errorMsg = "Error: Invalid data.";
        private static string successMsg = "Successfully imported {0}.";


        public static string ImportPictures(InstagraphContext context, string jsonString)
        {
            Picture[] deserializedPictures = JsonConvert.DeserializeObject<Picture[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            var pictures = new List<Picture>();

            foreach (var picture in deserializedPictures)
            {
                bool isValid = (!String.IsNullOrWhiteSpace(picture.Path)) && picture.Size > 0;

                // проверяваме в базата дали я има тая снимка, а също и в колекцията снимки:
                // но на нас назата ни е празна и е достатъчно само да проверим в кол-ята
                bool pictureExists = context.Pictures.Any(p => p.Path == picture.Path) ||
                    pictures.Any(p => p.Path == picture.Path);

                if (!isValid || pictureExists)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                pictures.Add(picture);
                sb.AppendLine(String.Format(successMsg, $"Picture {picture.Path}"));
            }
            context.Pictures.AddRange(pictures);
            context.SaveChanges();

            string result = sb.ToString().TrimEnd();

            return result;
        }

        public static string ImportUsers(InstagraphContext context, string jsonString)
        {
            // A user must have a valid profile picture, username and password.
            UserDto[] deserializedUsers = JsonConvert.DeserializeObject<UserDto[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            var users = new List<User>();

            foreach (var userDto in deserializedUsers)
            {
                // ако всички тези са true то ще имаме валиден вход
                bool isValid = !String.IsNullOrWhiteSpace(userDto.Username) &&
                                     userDto.Username.Length <= 30 &&
                               !String.IsNullOrWhiteSpace(userDto.Password) &&
                                    userDto.Password.Length <= 20 &&
                               !String.IsNullOrWhiteSpace(userDto.ProfilePicture);

                var picture = context.Pictures.FirstOrDefault(p => p.Path == userDto.ProfilePicture);


                bool userExists = context.Users.Any(u => u.Username == userDto.Username);

                if (!isValid || picture == null || userExists)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                var user = Mapper.Map<User>(userDto);
                user.ProfilePicture = picture;


                users.Add(user);
                sb.AppendLine(String.Format(successMsg, $"User {user.Username}"));
            }
            context.Users.AddRange(users);
            context.SaveChanges();

            string result = sb.ToString().TrimEnd();
            return result;

        }

        public static string ImportFollowers(InstagraphContext context, string jsonString)
        {
            UserFollowerDto[] deserializedFollowers = JsonConvert.DeserializeObject<UserFollowerDto[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            var userFollowers = new List<UserFollower>();

            foreach (var dto in deserializedFollowers)
            {
                int? userId = context.Users
                    .FirstOrDefault(u => u.Username == dto.User)?.Id;
                // Така се взима директно Id-то на намерения user и се дори ако е null !!!

                int? followerId = context.Users
                      .FirstOrDefault(u => u.Username == dto.Follower)?.Id;

                if (userId == null || followerId == null)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                // проверка имаме ли такива в кол-та:
                bool alreadyFollowed = userFollowers.Any(f => f.UserId == userId && f.FollowerId == followerId);
                if (alreadyFollowed)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                // Тук няма да ползваме AutoMapper защото не е добре той да бърка в базата, затова на ръка:
                var userFollower = new UserFollower()
                {
                    UserId = userId.Value,
                    FollowerId = followerId.Value
                };

                userFollowers.Add(userFollower);
                sb.AppendLine(String.Format(successMsg, $"Follower {dto.Follower} to User {dto.User}"));
            }
            context.UsersFollowers.AddRange(userFollowers);
            context.SaveChanges();

            string result = sb.ToString().TrimEnd();

            return result;
        }

        public static string ImportPosts(InstagraphContext context, string xmlString)
        {

            var xmlDoc = XDocument.Parse(xmlString);

            var elements = xmlDoc.Root.Elements();

            StringBuilder sb = new StringBuilder();

            var posts = new List<Post>();
            foreach (var element in elements)
            {
                // гледай от заданието колко вложени елемента очакваш и какво име!
                string caption = element.Element("caption")?.Value; // така взимаме ст-ста на caption елемента
                string username = element.Element("user")?.Value;
                string picturePath = element.Element("picture")?.Value;

                bool isValid = !String.IsNullOrWhiteSpace(caption) &&
                                !String.IsNullOrWhiteSpace(username) &&
                                !String.IsNullOrWhiteSpace(picturePath);

                if (!isValid)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                } 

                int? userId = context.Users.FirstOrDefault(u => u.Username == username)?.Id;
                int? pictureId = context.Pictures.FirstOrDefault(u => u.Path == picturePath)?.Id;
                // винаги след взимане на int? после прави проверка за null
                if (userId == null || pictureId == null)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                Post post = new Post()
                {
                    Caption = caption,
                    UserId = userId.Value,
                    PictureId = pictureId.Value
                };
                posts.Add(post);
                sb.AppendLine(String.Format(successMsg, $"Post {caption}"));                
            }

            context.Posts.AddRange(posts);
            context.SaveChanges();

            string result = sb.ToString().TrimEnd();

            return result;
        }

        public static string ImportComments(InstagraphContext context, string xmlString)
        {
            var xmlDoc = XDocument.Parse(xmlString);

            var elements = xmlDoc.Root.Elements();

            StringBuilder sb = new StringBuilder();

            var comments = new List<Comment>();
            foreach (var element in elements)
            {
                // гледай от заданието колко вложени елемента очакваш и какво име!
                string content = element.Element("content")?.Value; // така взимаме ст-ста на  елемента
                string username = element.Element("user")?.Value;
                string postIdString = element.Element("post")?.Attribute("id")?.Value;

                bool isNull = !String.IsNullOrWhiteSpace(content) &&
                                !String.IsNullOrWhiteSpace(username) &&
                                !String.IsNullOrWhiteSpace(postIdString);

                if (!isNull)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                int postId = int.Parse(postIdString);

                int? userId = context.Users.FirstOrDefault(u => u.Username == username)?.Id;
                bool postExists = context.Posts.Any(u => u.Id == postId);
                // винаги след взимане на int? после прави проверка за null
                if (userId == null || !postExists)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                Comment comment = new Comment()
                {
                    Content = content,
                    UserId = userId.Value,
                    PostId = postId
                };
                comments.Add(comment);
                sb.AppendLine(String.Format(successMsg, $"Comment {content}"));
            }

            context.Comments.AddRange(comments);
            context.SaveChanges();

            string result = sb.ToString().TrimEnd();
            return result;
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            return isValid;
        }
    }
}
