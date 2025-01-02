namespace Server.Models
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int RoleId { get; set; }

        public RegisterRequest() 
        {
            Email = string.Empty;
            Password = string.Empty;
            Name = string.Empty;
            RoleId = 0;
        }
    }
}
