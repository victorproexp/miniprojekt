using System.Text.Json;
using Model;

namespace Service;

public class DataService
{
    private readonly DataContext _context;

    public DataService(DataContext context)
    {
        _context = context;
    }

    public List<Topic> GetTopics() {
        return _context.Topics.ToList();
    }

    public Topic GetTopicById(int id) {
        return _context.Topics
            .Where(topic => topic.TopicId == id)
            .Include(topic => topic.Posts)
            .First();
    }

    public List<Post> GetPosts() {
        return _context.Posts
            .Include(post => post.User)
            .Include(post => post.Topic)
            .Include(post => post.Comments)
            .ToList();
    }

    public Post GetPostById(int id) {
        return _context.Posts
            .Where(post => post.PostId == id)
            .Include(post => post.User)
            .Include(post => post.Topic)
            .Include(post => post.Comments)
                .ThenInclude(c => c.User)
            .First();
    }

    public List<Comment> GetComments() {
        return _context.Comments
            .Include(comment => comment.User)
            .Include(comment => comment.Post)
            .ToList();
    }

    public List<User> GetUsers() {
        return _context.Users.ToList();
    }
    public User GetUserById(int id) {
        return _context.Users
            .Where(user => user.UserId == id)
            .First();
    }

    public string CreatePost(string heading, string text, int topicId, int userId) {
        //Topic topic = _context.Topics.Where(topic => topic.TopicId == topicId).First();
        //User user = _context.Users.Where(user => user.UserId == userId).First();
        Post post = new Post 
        {
            Heading = heading,
            Text = text,
            TopicId = topicId,
            UserId = userId
        };
        _context.Posts.Add(post);
        _context.SaveChanges();
        return JsonSerializer.Serialize(
            new { msg = "New post created", newPost = post}
        );
    }

    public string CreateComment(string text, int postId, int userId)
    {
        Comment comment = new Comment
        {
            Text = text,
            PostId = postId,
            UserId = userId
        };
        _context.Comments.Add(comment);
        _context.SaveChanges();
        return JsonSerializer.Serialize(
            new { msg = "New comment created", newComment = comment}
        );
    }

    public void UpvotePost(int id)
    {
        Post post = _context.Posts
            .Where(post => post.PostId == id)
            .First();
        post.Votes++;
        _context.SaveChanges();
    }

    public void UpvoteComment(int id)
    {
        Comment comment = _context.Comments
            .Where(post => post.CommentId == id)
            .First();
        comment.Votes++;
        _context.SaveChanges();
    }
}
