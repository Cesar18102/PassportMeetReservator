using Newtonsoft.Json;

namespace Common.Data.Dto
{
    public class DateBlockDto
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
