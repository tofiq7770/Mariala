using Mariala.Models;

namespace Mariala.Areas.Admin.ViewModels.Teams
{
    public class UpdateTeamVM
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public IFormFile? Photo { get; set; }
        public int PositionId { get; set; }
        public string? TwitLink { get; set; }
        public string? FbLink { get; set; }
        public string? LinkedLink { get; set; }
        public List<Position>? Positions { get; set; }
    }
}
