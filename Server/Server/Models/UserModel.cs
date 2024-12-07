using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Server.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public int Age { get; set; }

        [MaxLength(255)]
        public string City { get; set; }

        // Navigation property for many-to-many relationship with Roles
        public ICollection<Role> Roles { get; set; }

        // Navigation property for many-to-many relationship with Games
        public ICollection<Game> Games { get; set; }
    }
}
