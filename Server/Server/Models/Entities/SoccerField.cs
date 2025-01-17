﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Server.Models.Entities
{
    public class SoccerField
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }

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
            MinCapacity = 6; 
            MaxCapacity = 50; 
            Indoor = false;
            Owner = new User();
        }
    }
}