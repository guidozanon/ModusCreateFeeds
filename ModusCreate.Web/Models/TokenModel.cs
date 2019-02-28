using Newtonsoft.Json;

namespace ModusCreate.Web.Models
{
    public class TokenModel : RefreshTokenModel
    {
        [JsonProperty("jwt")]
        public string Jwt { get; set; }
    }

    public class RefreshTokenModel
    {
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
