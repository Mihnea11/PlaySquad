namespace Server.Models.Responses
{
    public class SoccerFieldResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int MinCapacity { get; set; }
        public int MaxCapacity { get; set; }
        public bool Indoor { get; set; }
        public UserResponse Owner { get; set; }

        public SoccerFieldResponse()
        {
            Id = 0;
            Name = string.Empty;
            Description = string.Empty;
            PictureUrl = string.Empty;
            Price = 0;
            MinCapacity = 6;
            MaxCapacity = 50;
            Indoor = false;
            Owner = new UserResponse();
        }
    }
}
