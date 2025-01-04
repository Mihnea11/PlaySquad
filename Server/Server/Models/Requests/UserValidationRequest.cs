namespace Server.Models.Requests
{
    public class UserValidationRequest
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public UserValidationRequest()
        {
            Id = 0;
            Email = string.Empty;
            Name = string.Empty;
        }
    }
}
