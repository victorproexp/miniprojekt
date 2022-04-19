global using Data;
global using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Json;
using Service;

var builder = WebApplication.CreateBuilder(
    new WebApplicationOptions() 
    {
        WebRootPath = "wwwroot"
    }
);

// Swagger-halløj der tilføjer nogle udviklingsværktøjer direkte i app'en.
// Se mere her: https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS skal slåes til i app'en. Ellers kan man ikke hente data fra den
// fra et andet domæne.
// Se mere her: https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-6.0
var AllowSomeStuff = "_AllowSomeStuff";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSomeStuff, builder => {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

Console.WriteLine($"Application Name: {builder.Environment.ApplicationName}");
Console.WriteLine($"Environment Name: {builder.Environment.EnvironmentName}");
Console.WriteLine($"ContentRoot Path: {builder.Environment.ContentRootPath}");
Console.WriteLine($"WebRootPath: {builder.Environment.WebRootPath}");

// Tilføj DbContext factory som service.
// Det gør at man kan få TodoContext ind via dependecy injection - fx 
// i DataService (smart!)
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration["dbcs"]));

// Kan vise flotte fejlbeskeder i browseren hvis der kommer fejl fra databasen
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Tilføj DataService så den kan bruges i endpoints
builder.Services.AddScoped<DataService>();

// Her kan man styrer hvordan den laver JSON.
builder.Services.Configure<JsonOptions>(options =>
{
    // Super vigtig option! Den gør, at programmet ikke smider fejl
    // når man returnerer JSON med objekter, der refererer til hinanden.
    // (altså dobbelrettede associeringer)
    options.SerializerOptions.ReferenceHandler = 
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

// Byg app'ens objekt
var app = builder.Build();

// Sørg for at HTML mv. også kan serveres
var options = new DefaultFilesOptions();
options.DefaultFileNames.Clear();
options.DefaultFileNames.Add("index.html");
app.UseDefaultFiles(options);
app.UseStaticFiles(new StaticFileOptions()
    {
        ServeUnknownFileTypes = true
    });

// Sæt Swagger og alt det andet halløj op
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();
app.UseCors(AllowSomeStuff);

// Middlware der kører før hver request. Alle svar skal have ContentType: JSON.
app.Use(async (context, next) =>
{
    context.Response.ContentType = "application/json; charset=utf-8";
    await next(context);
});

// Herunder alle endpoints i API'en
app.MapGet("/api/topics", (DataService service) =>
{
    return service.GetTopics();
});

app.MapGet("/api/topics/{id}", (DataService service, int id) =>
{
    return service.GetTopicById(id);
});

app.MapGet("/api/posts", (DataService service) =>
{
    return service.GetPosts();
});

app.MapGet("/api/posts/{id}", (DataService service, int id) =>
{
    return service.GetPostById(id);
});

app.MapGet("/api/users", (DataService service) =>
{
    return service.GetUsers();
});

app.MapGet("/api/users/{id}", (DataService service, int id) =>
{
    return service.GetUserById(id);
});

app.MapGet("/api/comments", (DataService service) =>
{
    return service.GetComments();
});

app.MapPost("/api/posts", (DataService service, PostData data) =>
{
    service.CreatePost(data.heading, data.text, data.topicId, data.userId);
});

app.MapPost("/api/posts/{id}/comments", (DataService service, CommentData data) =>
{
    service.CreateComment(data.text, data.postId, data.userId);
});

app.MapPut("/api/posts/{id}/votes", (DataService service, int id) =>
{
    service.UpvotePost(id);
});

app.MapPut("/api/comments/{id}/votes", (DataService service, int id) =>
{
    service.UpvoteComment(id);
});

app.Run();

record PostData(string heading, string text, int topicId, int userId);
record CommentData(string text, int postId, int userId);
