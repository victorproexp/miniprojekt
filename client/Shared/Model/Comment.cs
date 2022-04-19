using System.Text.Json.Serialization;

namespace Model
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public int Votes { get; set; }

        //[JsonIgnore]
        public Post? Post { get; set; }
        public int PostId { get; set; }

        //[JsonIgnore]
        public User? User { get; set; }
        public int UserId { get; set; }
    }
}