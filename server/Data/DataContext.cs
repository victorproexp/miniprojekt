using Model;

namespace Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Topic>().HasData(
                new Topic { TopicId = 1, Text = "Generelt"},
                new Topic { TopicId = 2, Text = "Forretningsdesign"},
                new Topic { TopicId = 3, Text = "Systemudvikling"},
                new Topic { TopicId = 4, Text = "Cloud Computing"},
                new Topic { TopicId = 5, Text = "Applikationsudvikling"}
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Username = "victorp"
                },
                new User
                {
                    UserId = 2,
                    Username = "jesperm"
                }
            );

            modelBuilder.Entity<Post>().HasData(
                new Post 
                { 
                    PostId = 1, 
                    Heading = "Hvorfor har Kristians fag større fremmøde?",
                    TopicId = 1,
                    UserId = 1
                },
                new Post 
                { 
                    PostId = 2,
                    Heading = "Hvem skal med til Grundfos?",
                    TopicId = 2,
                    UserId = 1
                }
            );

            modelBuilder.Entity<Comment>().HasData(
                new Comment
                {
                    CommentId = 1,
                    Text = "Programmering er sjovt",
                    PostId = 1,
                    UserId = 1
                },
                new Comment
                {
                    CommentId = 2,
                    Text = "Mig :)",
                    PostId = 2,
                    UserId = 1
                }
            );
        }
        
        public DbSet<Topic> Topics => Set<Topic>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<User> Users => Set<User>();
    }
}
