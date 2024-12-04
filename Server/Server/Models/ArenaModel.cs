using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Arena
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public int OwnerId { get; set; }

        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public float Price { get; set; }
    }
}
