using Microsoft.AspNetCore.Identity;
using MvcMovie.Models;

namespace MvcMovie.Data
{
    public static class DbInitializer
    {
        public static void Initialize(MvcMovieContext context)
        {
            Console.WriteLine(">>> DbInitializer started <<<");

            if (context.User.Any())
            {
                Console.WriteLine(">>> Users already exist. Skipping seeding.");
                return;
            }

            var roles = new List<Role>
            {
                new Role { Name = "Admin" },
                new Role { Name = "Moderator" },
                new Role { Name = "User" }
            };
            context.Role.AddRange(roles);
            context.SaveChanges();

            var hasher = new PasswordHasher<User>();
            var users = new List<User>();

            for (int i = 1; i <= 20; i++)
            {
                var user = new User
                {
                    Username = $"user{i}",
                    Mail = $"user{i}@mail.com",
                    RoleId = 3,
                    IsActive = 1
                };
                user.Password = hasher.HashPassword(user, "pass");
                users.Add(user);
            }

            var admin = new User
            {
                Username = "admin",
                Mail = "admin@mail.com",
                RoleId = 1,
                IsActive = 1
            };
            admin.Password = hasher.HashPassword(admin, "admin");
            users.Add(admin);

            context.User.AddRange(users);
            context.SaveChanges();

            var random = new Random();
            var statuses = Enum.GetValues(typeof(MovieStatus));
            var priorities = Enum.GetValues(typeof(Priorities));
            var tasks = new List<MvcMovie.Models.Task>();

            foreach (var user in users)
            {
                for (int i = 0; i < random.Next(2, 6); i++)
                {
                    tasks.Add(new MvcMovie.Models.Task
                    {
                        Title = $"Task {i + 1} for {user.Username}",
                        DueDate = DateTime.Today.AddDays(random.Next(1, 30)),
                        Priority = (Priorities)priorities.GetValue(random.Next(priorities.Length)),
                        Status = (MovieStatus)statuses.GetValue(random.Next(statuses.Length)),
                        UserId = user.UserId
                    });
                }
            }

            context.TodoTasks.AddRange(tasks);
            context.SaveChanges();
        }
    }
}
