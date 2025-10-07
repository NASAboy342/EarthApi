namespace EarthApi.Models.Player
{
    public class Player
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string IPAddress { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime LastLogout { get; set; }
        public bool IsOnline { get; set; }
        public string GameSessionInJson { get; set; }
    }
}
