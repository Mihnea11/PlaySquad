using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Server.Models
{
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
        public ICollection<Game> Games { get; set; }
    }
}
