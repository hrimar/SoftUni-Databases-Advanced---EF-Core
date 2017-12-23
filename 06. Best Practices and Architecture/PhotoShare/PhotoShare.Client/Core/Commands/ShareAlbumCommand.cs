namespace PhotoShare.Client.Core.Commands
{
    using PhotoShare.Data;
    using PhotoShare.Models;
    using System;
    using System.Linq;

    public class ShareAlbumCommand
    {
        // ShareAlbum <albumId> <username> <permission>
        // For example:
        // ShareAlbum 4 dragon321 Owner
        // ShareAlbum 4 dragon11 Viewer
        public static string Execute(string[] data)
        {
            int albumId = int.Parse(data[1]);
            string username = data[2];
            string permission = data[3];

            string albumName = "";
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
                    .Where(u => u.Id == albumId)
                    .FirstOrDefault();

                albumName = checkAlbum.Name;

                if (checkAlbum == null)
                {
                    throw new ArgumentException($"Album {albumId} not found!");
                }

                if (permission != "Owner" && permission != "Viewer")
                {
                    throw new ArgumentException($"Permission must be either “Owner” or “Viewer”!");
                }

            }
            return $"Username {username} added to album {albumName} ({permission})";
            }
    }
}
