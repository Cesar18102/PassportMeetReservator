using Newtonsoft.Json;

namespace Common.Data.Forms
{
    public class LogInForm
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
