namespace EarthApi.Models.Game
{
    public class GetAllGamesResponse : EarthApiResponseBase
    {
        public List<GameInfo> Games { get; set; } = new List<GameInfo>();
    }
}
