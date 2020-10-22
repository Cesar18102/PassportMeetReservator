using Newtonsoft.Json;

namespace PassportMeetReservator.Data
{
    public class LogInForm
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
