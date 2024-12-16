using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Users.Any() || context.Roles.Any() || context.Games.Any() || context.Arenas.Any())
                {
                    return;
                }

                var roles = new[]
                {
                    new Role { Name = "Owner" },
                    new Role { Name = "Player" }
                };
                context.Roles.AddRange(roles);

                var arenas = new[]
                {
                    new Arena { Name = "Soccer Arena 1", Address = "123 Main St", MinPlayers = 5, MaxPlayers = 10, Type = "Soccer", Price = 100 },
                    new Arena { Name = "Soccer Arena 2", Address = "456 Elm St", MinPlayers = 6, MaxPlayers = 12, Type = "Soccer", Price = 150 },
                    new Arena { Name = "Soccer Arena 3", Address = "789 Oak St", MinPlayers = 7, MaxPlayers = 14, Type = "Soccer", Price = 200 }
                };
                context.Arenas.AddRange(arenas);

                var random = new Random();
                var users = Enumerable.Range(1, 20).Select(i => new User
                {
                    Name = $"User{i}",
                    Email = $"user{i}@example.com",
                    Password = $"Password{i}",
                    Age = random.Next(18, 50),
                    City = $"City{i % 3}",
                    Roles = new List<Role>
                    {
                        roles[random.Next(roles.Length)]
                    }
                }).ToList();
                context.Users.AddRange(users);

                var games = Enumerable.Range(1, 10).Select(i => new Game
                {
                    Type = "Soccer",
                    StartTime = DateTime.UtcNow.AddDays(-i),
                    EndTime = DateTime.UtcNow.AddDays(-i).AddHours(2),
                    Arena = arenas[random.Next(arenas.Length)],
                    GoalsTeamA = random.Next(0, 5),
                    GoalsTeamB = random.Next(0, 5),
                }).ToList();
                context.Games.AddRange(games);

                foreach (var user in users)
                {
                    var gamesForUser = games.OrderBy(_ => random.Next()).Take(random.Next(0, 4)).ToList();
                    foreach (var game in gamesForUser)
                    {
                        user.Games.Add(game);
                        game.Users.Add(user);
                    }
                }

                context.SaveChanges();
            }
        }
    }
}