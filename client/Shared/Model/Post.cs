using System.Text.Json.Serialization;

namespace Model 
{
    public class Post
    {
        public int PostId { get; set; }
        public string Heading { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public int Votes { get; set; }

        //[JsonIgnore]
        public Topic? Topic { get; set; }
        public int TopicId { get; set; }

        //[JsonIgnore]
        public User? User { get; set; }
        public int UserId { get; set; }

        public List<Comment> Comments { get; set; } = new();
    }
}
