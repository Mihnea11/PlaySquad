namespace Server.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
    }

    public class UserCreateDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
    }
    public class RoleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class RoleCreateDTO
    {
        public string Name { get; set; }
    }
    public class ArenaDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
    }

    public class ArenaCreateDTO
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
    }
    public class GameDTO
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ArenaId { get; set; }
    }

    public class GameCreateDTO
    {
        public string Type { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ArenaId { get; set; }
    }
}
