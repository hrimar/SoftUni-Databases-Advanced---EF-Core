namespace Instagraph.Models
{
    public class UserFollower
    {
        public int UserId { get; set; }     // PK
        public User User { get; set; }

        public int FollowerId { get; set; } // PK
        public User Follower { get; set; }          // YES! 3.
    }
}