
namespace EarthApi.Models.Player
{
    public class GetPlayerInfoRequest
    {
        public string Username { get; set; }

        internal void ValidateRequest()
        {
            if(string.IsNullOrWhiteSpace(Username))
                throw new ArgumentException("Username is required");
        }
    }
}
