namespace Logix.Repo.Entities
{
    public class UserClass
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int ClassId { get; set; }
        public Class Class { get; set; } = null!;
    }
}
