namespace PhotoShare.Client.Core.Commands
{
    using PhotoShare.Client.Utilities;
    using PhotoShare.Data;
    using PhotoShare.Models;
    using System;
    using System.Linq;

    public class AddTagToCommand 
    {
        // AddTagTo <albumName> <tag>
        public static string Execute(string[] data)
        {
            string album = data[1].ValidateOrTransform();
            string tag = data[2].ValidateOrTransform();                    

            using (PhotoShareContext db = new PhotoShareContext())
            {
                if (db.Albums.Any(t => t.Name != album)
                    || db.Tags.Any(t => t.Name != tag))
                {
                    throw new ArgumentException($"Either tag or album do not exist!");
                }

                db.Tags.Add(new Tag
                {
                    Name = tag
                });

                db.SaveChanges();
            }

            return tag + $" was added to {album}!";
        }
    }
}
