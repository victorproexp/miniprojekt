namespace Model
{
    public class Topic
    {
        public int TopicId { get; set; }
        public string Text { get; set; } = string.Empty;

        public List<Post> Posts { get; set; } = new();
    }
}