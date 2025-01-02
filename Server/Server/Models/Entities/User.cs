using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Serialization;

namespace Server.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        [MaxLength(255)]
        public string GoogleId { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }
        public string PictureUrl { get; set; }

        public int RoleId { get; set; }

        [JsonIgnore]
        public Role? Role { get; set; }

        public User()
        {
            Id = 0;
            Email = string.Empty;
            PasswordHash = string.Empty;
            GoogleId = string.Empty;
            Name = string.Empty;
            PictureUrl = string.Empty;
            RoleId = 0;
        }
    }
}
