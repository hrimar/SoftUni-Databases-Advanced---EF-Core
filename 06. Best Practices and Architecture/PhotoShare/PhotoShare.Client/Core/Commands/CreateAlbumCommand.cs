namespace PhotoShare.Client.Core.Commands
{
    using PhotoShare.Client.Utilities;
    using PhotoShare.Data;
    using PhotoShare.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CreateAlbumCommand
    {
        // CreateAlbum <username> <albumTitle> <BgColor> <tag1> <tag2>...<tagN>
        public static string Execute(string[] data)
        {
            string username = data[1];
            string albumTitle = data[2];
            string bgColor = (data[3]).ToLower();

            //for (int i = 4; i < data.Length; i++)
            //{
            //    string tag = data[i];
            //}
            string[] tags = data.Skip(4).Select(t => t.ValidateOrTransform()).ToArray();// ?

     
            using (var db = new PhotoShareContext())
            {
                var checkUser = db.Users
                    .Where(u => u.Username == username)
                    .FirstOrDefault();

                if (checkUser == null)
                {
                    throw new ArgumentException($"User {username} not found!");
                }

                var checkAlbum = db.Albums
                    .Where(u => u.Name == albumTitle)
                    .FirstOrDefault();

                if (checkAlbum != null)
                {
                    throw new ArgumentException($"Album {albumTitle} exists!");
                }

               Color acceptedColour = new Color();

                string[] colours = new[] { "White", "Green", "Blue", "Pink", "Yellow", "Black", "Cyan", "Magenta", "Red", "Purple", "WhiteBlackGradient" };

                switch (bgColor)
                {
                    case "white": acceptedColour = Color.White; break;
                    case "green": acceptedColour = Color.Green; break;
                    case "blue": acceptedColour = Color.Blue; break;
                    case "pink": acceptedColour = Color.Pink; break;
                    case "yellow": acceptedColour = Color.Yellow; break;
                    case "black": acceptedColour = Color.Black; break;
                    case "cyan": acceptedColour = Color.Cyan; break;
                    case "magenta": acceptedColour = Color.Magenta; break;
                    case "red": acceptedColour = Color.Red; break;
                    case "purple": acceptedColour = Color.Purple; break;
                    case "whiteblackgradient": acceptedColour = Color.WhiteBlackGradient; break;
                    default:
                        throw new ArgumentException($"Color {bgColor} not found!" + Environment.NewLine +
              "You may use one of this: " + string.Join(", ", colours));
                }

                
                if (!tags.All(t => db.Tags.Any(ct => ct.Name == t)))
                {
                    throw new ArgumentException($"Invalid tags!");
                }


                Album album = new Album
                {
                    Name = albumTitle,               
                    AlbumRoles = new List<AlbumRole>()
                    {
                        new AlbumRole()
                        {
                            User = checkUser,
                            Role = Role.Owner
                        }
                    },
                    AlbumTags = tags.Select(t => new AlbumTag()
                    {
                        Tag = db.Tags
                            .FirstOrDefault(ct => ct.Name == t)
                    })
                    .ToArray()
                };

                
                    db.Albums.Add(album);
                    db.SaveChanges();
                
            
            return $"Album {album} successfully created!";
            }
        }
    }
}
