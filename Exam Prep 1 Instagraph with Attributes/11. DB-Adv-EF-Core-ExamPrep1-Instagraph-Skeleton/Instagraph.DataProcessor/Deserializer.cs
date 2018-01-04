using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Newtonsoft.Json;

using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

using Instagraph.Data;
using Instagraph.Models;
using Instagraph.Data.Dto.Import;

namespace Instagraph.DataProcessor
{
    public class Deserializer
    {
        private const string FailureMessage = "Error: Invalid data.";
        private const string SuccessMessage = "Successfully imported {0}.";

        public static string ImportPictures(InstagraphContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var deserializedPictures = JsonConvert.DeserializeObject<PictureDto[]>(jsonString);

            var validPictures = new List<Picture>();

            foreach (var pictureDto in deserializedPictures)
            {
                if(!IsValid(pictureDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                var isPictureExists = validPictures.Any(p => p.Path == pictureDto.Path);
                if(isPictureExists)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                if(pictureDto.Size <= 0)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                //if (String.IsNullOrWhiteSpace(pictureDto.Path))
                //{
                //    sb.AppendLine(FailureMessage);
                //    continue;
                //}
                var picture = new Picture()
                {
                    Path = pictureDto.Path,
                    Size = pictureDto.Size
                };
                validPictures.Add(picture);
                sb.AppendLine(String.Format(SuccessMessage, $"Picture {pictureDto.Path}"));
            }
            context.Pictures.AddRange(validPictures);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportUsers(InstagraphContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var deserializedUsers = JsonConvert.DeserializeObject<UserDto[]>(jsonString);

            var validUsers = new List<User>();

            foreach (var userDto in deserializedUsers)
            {
                if (!IsValid(userDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                var picture = context.Pictures.SingleOrDefault(p => p.Path == userDto.ProfilePicture);
                if(picture==null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                var user = new User()
                {
                    Username = userDto.Username,
                    Password = userDto.Password,
                    ProfilePicture = picture
                };
                validUsers.Add(user);
                sb.AppendLine(String.Format(SuccessMessage, $"User {userDto.Username}"));
            }
            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportFollowers(InstagraphContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var deserializedUserFollowers = JsonConvert.DeserializeObject<UserFollowerDto[]>(jsonString);

            var validUserFollowers = new List<UserFollower>();

            foreach (var userFollowerDto in deserializedUserFollowers)
            {
                if (!IsValid(userFollowerDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

               var userExists = context.Users.Any(u => u.Username == userFollowerDto.User);
              var followerExists = context.Users.Any(u => u.Username == userFollowerDto.Follower);

                if (userExists && followerExists)
                {
                    var userId = context.Users.SingleOrDefault(u => u.Username == userFollowerDto.User).Id;
                    var followerId = context.Users.FirstOrDefault(u => u.Username == userFollowerDto.Follower).Id;

                    var isUFAlreadyAdded = validUserFollowers.Any(uf => uf.UserId == userId && uf.FollowerId==followerId);
                    if(isUFAlreadyAdded)
                    {
                        sb.AppendLine(FailureMessage);
                        continue;
                    }

                    var userFollower = new UserFollower()
                    {
                        UserId = userId,
                        FollowerId =followerId

                };

                    validUserFollowers.Add(userFollower);
                    sb.AppendLine(String.Format(SuccessMessage, $"Follower {userFollowerDto.Follower} to User {userFollowerDto.User}"));
                }
                else
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

            }
            context.UsersFollowers.AddRange(validUserFollowers);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportPosts(InstagraphContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(PostDto[]), new XmlRootAttribute("posts"));
            var deserializedPosts = (PostDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));

            var sb = new StringBuilder();

            var validPosts = new List<Post>();

            foreach (var postDto in deserializedPosts)
            {                
                if (!IsValid(postDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                User user = context.Users.SingleOrDefault(u => u.Username == postDto.User);
                Picture picture = context.Pictures.SingleOrDefault(p => p.Path == postDto.Picture);

                if (user == null || picture == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Post currentPost = new Post
                {
                    Caption = postDto.Caption,
                    User = user,
                    Picture = picture
                };

                validPosts.Add(currentPost);
                sb.AppendLine(string.Format(SuccessMessage, $"Post {currentPost.Caption}"));
            }   

            context.Posts.AddRange(validPosts);
            context.SaveChanges();

            var result = sb.ToString();
            return result;
            }

            public static string ImportComments(InstagraphContext context, string xmlString)
            {
            var serializer = new XmlSerializer(typeof(CommentDto[]), new XmlRootAttribute("comments"));
            var deserializedComments = (CommentDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));

            var sb = new StringBuilder();

            var validComments = new List<Comment>();

            foreach (var commentDto in deserializedComments)
            {
                if (!IsValid(commentDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var user = context.Users.SingleOrDefault(u => u.Username == commentDto.User);
                var post = context.Posts.SingleOrDefault(p => p.Id == commentDto.Post.Id);
                
                if (user == null || post==null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                

                Comment comment = new Comment()
                {
                    Content = commentDto.Content,
                    User = user,
                    Post = post
                };
                validComments.Add(comment);
                sb.AppendLine(string.Format(SuccessMessage, $"Comment {comment.Content}"));
               
            }
            context.Comments.AddRange(validComments);
            context.SaveChanges();

            var result = sb.ToString();
            return result;
            }


        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            return isValid;
        }
    }
}
