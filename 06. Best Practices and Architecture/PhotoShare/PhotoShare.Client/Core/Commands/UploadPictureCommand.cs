namespace PhotoShare.Client.Core.Commands
{
    using PhotoShare.Data;
    using PhotoShare.Models;
    using System;
    using System.Linq;

    public class UploadPictureCommand
    {
        // UploadPicture <albumName> <pictureTitle> <pictureFilePath>
        public static string Execute(string[] data)
        {
            string albumName = data[1];
            string pictureTitle = data[2];
            string pictureFilePath = data[3];
           

            
            using (var db = new PhotoShareContext())
            {                
                if (db.Albums.Any(u => u.Name != albumName))
                {
                    throw new InvalidOperationException($"Album {albumName} not found!");
                }

            }

            Picture picture = new Picture
            {
                Title = pictureTitle,
                Path = pictureFilePath,
                AlbumId = 1
              
            };

            using (PhotoShareContext context = new PhotoShareContext())
            {
                context.Pictures.Add(picture);
                context.SaveChanges();
            }

            return $"Picture {pictureTitle} added to {albumName}!";
        }
    }
}
