namespace TodoListBlazor.Services
{
    public interface IPostService
    {
        List<Post> Posts { get; set; }
        List<Topic> Topics { get; set; }
        List<User> Users { get; set; }
        List<Comment> Comments { get; set; }
        Task GetTopics();
        Task GetUsers();
        Task GetPosts();
        Task<Post> GetPost(int id);
        Task GetComments();
        Task CreatePost(Post data);
        Task CreateComment(int id, Comment data);
        Task PutPostData(Post data);
        Task UpvotePost(int id);
        Task UpvoteComment(int id);
    }
}