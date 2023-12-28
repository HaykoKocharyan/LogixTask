namespace Logix.Repo.Entities
{
    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<UserClass> UserClasses { get; set; } = null!;
    }
}
