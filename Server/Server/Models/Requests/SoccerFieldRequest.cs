using System.ComponentModel.DataAnnotations;

namespace Server.Models.Requests
{
    public class SoccerFieldRequest
    {
        [Required]
        [MaxLength(100)]
        [MinLength(10)]
        public string Name { get; set; }

        [MaxLength(500)]
        [MinLength(10)]
        public string? Description { get; set; }

        [Required]
        public string PictureUrl { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(6, int.MaxValue)]
        public int MinCapacity { get; set; }

        [Range(6, 50)]
        public int MaxCapacity { get; set; }

        [Required]
        public bool Indoor { get; set; }

        [Required]
        public int OwnerId { get; set; }

        public SoccerFieldRequest()
        {
            Name = string.Empty;
            Description = string.Empty;
            PictureUrl = string.Empty;
            Price = 0;
            MinCapacity = 6;
            MaxCapacity = 50;
            Indoor = false;
        }
    }
}
