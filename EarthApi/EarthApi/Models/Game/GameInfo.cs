namespace EarthApi.Models.Game
{
    public class GameInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string IconUrl { get; set; } = "";
        public bool IsEnabled { get; set; }
        public bool IsUnderMaintenance { get; set; }
        public string Route { get; set; } = "";
    }
}
