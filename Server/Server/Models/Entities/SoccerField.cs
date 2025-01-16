using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Server.Models.Entities
{
    public class SoccerField
    {
        [Key] // Definește această proprietate ca fiind cheia primară
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Configurare pentru increment automat
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(10)] // Minim 10 caractere
        public string Name { get; set; }

        [MaxLength(500)]
        [MinLength(10)] // Minim 10 caractere
        public string? Description { get; set; }

        [Required]
        public string PictureUrl { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(6, int.MaxValue)] // Capacitatea minimă trebuie să fie >= 6
        public int MinCapacity { get; set; }

        [Range(6, 50)] // Capacitatea maximă trebuie să fie <= 50
        public int MaxCapacity { get; set; }

        [Required]
        public bool Indoor { get; set; }

        public int OwnerId { get; set; }

        [Required]
        public User Owner { get; set; }

        public ICollection<Booking>? Bookings { get; set; }

        public SoccerField()
        {
            Name = string.Empty;
            Description = string.Empty;
            PictureUrl = string.Empty;
            Price = 0;
            MinCapacity = 6; // Valoare implicită minimă
            MaxCapacity = 50; // Valoare implicită maximă
            Indoor = false;
            Owner = new User();
        }
    }
}