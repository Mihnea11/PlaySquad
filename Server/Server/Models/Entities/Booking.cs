﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Models.Entities
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int FieldId { get; set; }
        public SoccerField Field { get; set; }

        public int CreatorId { get; set; }

        [Required]
        public User Creator { get; set; }

        [JsonIgnore]
        public ICollection<User>? WaitingList { get; set; } // Users awaiting approval

        [JsonIgnore]
        public ICollection<User>? ApprovedParticipants { get; set; } // Approved participants

        [Range(1, int.MaxValue)]
        public int MaxParticipants { get; set; }

        public Booking()
        {
            Id = 0;
            Field = new SoccerField();
            Creator = new User();
            WaitingList = new List<User>();
            ApprovedParticipants = new List<User>();
            MaxParticipants = 1;
        }
    }
}
