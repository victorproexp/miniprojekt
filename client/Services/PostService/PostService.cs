using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Text.Json;


namespace TodoListBlazor.Services;

public class PostService : IPostService
{
    private readonly HttpClient _http;
    private readonly NavigationManager _navigationManager;
    private readonly IConfiguration _configuration;
    private readonly string _baseAPI = string.Empty;

    public PostService(HttpClient http, IConfiguration configuration, NavigationManager navigationManager) {
        _http = http;
        _navigationManager = navigationManager;
        _configuration = configuration;
        _baseAPI = configuration["base_api"];
    }

    public List<Post> Posts { get; set; } = new();
    public List<Topic> Topics { get; set; } = new();
    public List<User> Users { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();

    public async Task GetTopics()
    {
        string url = $"{_baseAPI}topics/";
        var result = await _http.GetFromJsonAsync<List<Topic>>(url);
        if (result != null)
        {
            Topics = result;
        }
    }

    public async Task GetUsers()
    {
        string url = $"{_baseAPI}users/";
        var result = await _http.GetFromJsonAsync<List<User>>(url);
        if (result != null)
        {
            Users = result;
        }
    }

    public async Task GetPosts()
    {
        string url = $"{_baseAPI}posts/";
        var result = await _http.GetFromJsonAsync<List<Post>>(url);
        if (result != null)
        {
            Posts = result;
        }
    }

    public async Task<Post> GetPost(int id)
    {
        string url = $"{_baseAPI}posts/{id}";
        var result = await _http.GetFromJsonAsync<Post>(url);
        if (result != null)
            return result;
        throw new Exception("Post was not found!");
    }

    public async Task GetComments()
    {
        string url = $"{_baseAPI}comments/";
        var result = await _http.GetFromJsonAsync<List<Comment>>(url);
        if (result != null)
        {
            Comments = result;
        }
    }

    public async Task CreatePost(Post data)
    {
        Console.WriteLine("Post Post Data!");
        Console.WriteLine(data);
        PostData newData = new(data.Heading, data.Text, data.TopicId, data.UserId);
        string url = $"{_baseAPI}posts/";
        var res = await _http.PostAsJsonAsync(url, newData);
        Console.WriteLine(res);
        _navigationManager.NavigateTo("/");
    }

    public async Task CreateComment(int id, Comment data)
    {
        Console.WriteLine("Put Comment Data!");
        Console.WriteLine(data);
        Console.WriteLine(id);
        CommentData newData = new(data.Text, data.PostId, data.UserId);
        string url = $"{_baseAPI}posts/{id}/comments/";
        var res = await _http.PostAsJsonAsync(url, newData);
        Console.WriteLine(res);
        _navigationManager.NavigateTo("/");
    }

    public async Task PutPostData(Post data)
    {
        Console.WriteLine("Put Post Data!");
        Console.WriteLine(data);
        PostData newData = new PostData(data.Heading, data.Text, data.TopicId, data.UserId);
        string url = $"{_baseAPI}tasks/{data.PostId}";
        var res = await _http.PutAsJsonAsync(url, newData);
        Console.WriteLine(res);
    }

    public async Task UpvotePost(int id)
    {
        string url = $"{_baseAPI}posts/{id}/votes";
        var res = await _http.PutAsJsonAsync(url, id);
        Console.WriteLine(res);
    }

    public async Task UpvoteComment(int id)
    {
        string url = $"{_baseAPI}comments/{id}/votes";
        var res = await _http.PutAsJsonAsync(url, id);
        Console.WriteLine(res);
    }

    record PostData(string heading, string text, int topicId, int userId);
    record CommentData(string text, int postId, int userId);
    
}
