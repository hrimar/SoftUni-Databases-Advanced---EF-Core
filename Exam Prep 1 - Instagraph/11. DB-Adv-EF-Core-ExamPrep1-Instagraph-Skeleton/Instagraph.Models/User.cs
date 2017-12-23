using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Instagraph.Models
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(30)] // Unique
        public string Username { get; set; }

        [MaxLength(20)] 
        public string Password { get; set; }

        public int ProfilePictureId { get; set; }
        public Picture ProfilePicture { get; set; } // YES 1., но не е стринг при проверната на инсерта!!

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<Post> Posts { get; set; } = new List<Post>();


        public ICollection<UserFollower> Followers { get; set; } = //YES! 2.
            new List<UserFollower>();

        public ICollection<UserFollower> UsersFollowing { get; set; } = //YES! 2.
          new List<UserFollower>();
    }
}