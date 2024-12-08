using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace Server.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public int Age { get; set; }

        [MaxLength(255)]
        public string City { get; set; }

        // Navigation property for many-to-many relationship with Roles
        [JsonIgnore]
        public ICollection<Role> Roles { get; set; }

        // Navigation property for many-to-many relationship with Games
        [JsonIgnore]
        public ICollection<Game> Games { get; set; }
    }

    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Navigation property for many-to-many relationship with Users
        [JsonIgnore]
        public ICollection<User> Users { get; set; }
    }
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Type { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        // Foreign Key for Arena
        public int ArenaId { get; set; }
        [JsonIgnore]
        public Arena Arena { get; set; }

        public int GoalsTeamA { get; set; }

        public int GoalsTeamB { get; set; }

        // Navigation property for many-to-many relationship with Users
        [JsonIgnore]
        public ICollection<User> Users { get; set; }
    }
    public class Arena
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public string Address { get; set; }

        public int MinPlayers { get; set; }

        public int MaxPlayers { get; set; }

        public string Type { get; set; }

        public decimal Price { get; set; }

        // Navigation property to the Games in this Arena
        [JsonIgnore]
        public ICollection<Game> Games { get; set; }
    }
}
