﻿using Newtonsoft.Json;

namespace PassportMeetReservator.Data
{
    public class LogInDto
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password_salted")]
        public string PasswordSalted { get; set; }

        [JsonProperty("salt")]
        public string Salt { get; set; }
    }
}
