namespace EarthApi.Models.Player
{
    public class GetPlayerInfoResponse : EarthApiResponseBase
    {
        public bool IsOnline { get; set; }
        public string Username { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
    }
}
