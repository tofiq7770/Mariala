namespace Mariala.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? TwitLink { get; set; }
        public string? FbLink { get; set; }
        public string? LinkedLink { get; set; }
        public int  PositionId { get; set; }
        public Position? Positions { get; set; }

    }
}
