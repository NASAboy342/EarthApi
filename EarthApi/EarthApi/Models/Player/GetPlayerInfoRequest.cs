
namespace EarthApi.Models.Player
{
    public class GetPlayerInfoRequest
    {
        public string Username { get; set; }

        internal void ValidateRequest()
        {
        }
    }
}
