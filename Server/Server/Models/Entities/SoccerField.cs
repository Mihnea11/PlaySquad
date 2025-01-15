using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Models.Entities
{
    public class SoccerField
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public string PictureUrl { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int MinCapacity { get; set; }

        [Range(0, int.MaxValue)]
        public int MaxCapacity { get; set; }

        [Required]
        public bool Indoor { get; set; }

        public int OwnerId { get; set; }

        [Required]
        public User Owner { get; set; }

        public ICollection<Booking>? Bookings { get; set; } 

        public SoccerField()
        {
            Id = 0;
            Name = string.Empty;
            Description = string.Empty;
            Price = 0;
            MinCapacity = 0;
            MaxCapacity = 0;
            Indoor = false;
            Owner = new User();
        }
    }
}