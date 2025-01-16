using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Server.Models.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required] 
        [EmailAddress] 
        [MaxLength(255)]
        public string Email { get; set; }

        [Required] 
        [MinLength(8)]
        public string PasswordHash { get; set; }

        [MaxLength(255)] 
        public string? GoogleId { get; set; }

        [Required] 
        [MaxLength(255)] 
        [MinLength(2)] 
        public string Name { get; set; }

        [Url]
        public string? PictureUrl { get; set; }

        [Required]
        public int RoleId { get; set; }

        [JsonIgnore]
        public Role? Role { get; set; }

        [JsonIgnore]
        public ICollection<SoccerField>? OwnedFields { get; set; } 

        [JsonIgnore]
        public ICollection<Booking>? OwnedBookings { get; set; } 

        [JsonIgnore]
        public ICollection<Booking>? RequestedBookings { get; set; } 

        [JsonIgnore]
        public ICollection<Booking>? ApprovedBookings { get; set; } 

        public User()
        {
            Email = string.Empty;
            PasswordHash = string.Empty;
            GoogleId = string.Empty;
            Name = string.Empty;
            PictureUrl = string.Empty;
            RoleId = 0;
            OwnedFields = new List<SoccerField>();
            OwnedBookings = new List<Booking>();
            RequestedBookings = new List<Booking>();
            ApprovedBookings = new List<Booking>();
        }
    }
}