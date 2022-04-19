namespace Model
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;

        public List<Post> Posts { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
    }
}