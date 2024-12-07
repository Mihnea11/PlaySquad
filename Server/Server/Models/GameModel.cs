using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace Server.Models
{
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
        public Arena Arena { get; set; }

        public int GoalsTeamA { get; set; }

        public int GoalsTeamB { get; set; }

        // Navigation property for many-to-many relationship with Users
        public ICollection<User> Users { get; set; }
    }
}
