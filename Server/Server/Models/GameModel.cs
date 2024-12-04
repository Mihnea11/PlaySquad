using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        public string Type { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        [ForeignKey("Arena")]
        public int ArenaId { get; set; }
        public Arena Arena { get; set; }

        public int GoalsTeamA { get; set; }
        public int GoalsTeamB { get; set; }
    }
}
