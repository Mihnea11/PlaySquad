namespace Server.DTO
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public string RoleName { get; set; }

        public UserResponse()
        {
            Id = 0;
            Email = string.Empty;
            Name = string.Empty;
            PictureUrl = string.Empty;
            RoleName = string.Empty;
        }
    }
}
