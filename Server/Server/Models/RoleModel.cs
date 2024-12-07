using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Server.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Navigation property for many-to-many relationship with Users
        public ICollection<User> Users { get; set; }
    }
}
