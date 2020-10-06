using System;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;

using RestSharp;

using PassportMeetReservator.Data.Platforms;

namespace PassportMeetReservator.Data
{
    public class DateChecker
    {
        private const string GENERAL_ERROR_RESPONSE = "General error.";
        private const string CHECK_DATE_API_ENDPOINT = "Slot/GetAvailableDaysForOperation/";
        private RestClient ApiClient { get; set; } = new RestClient("https://rejestracjapoznan.poznan.uw.gov.pl/api");

        public DateTime[] Dates { get; private set; } = new DateTime[] { };

        public string Token { get; private set; }
        public string CityUrl { get; private set; }

        public string Operation { get; private set; }
        public int OperationNumber { get; private set; }

        public int FollowersCount { get; set; }
        public int UpdateInterval { get; set; } = 300;

        public DateChecker(string cityUrl, string operation, int operationNumber, string token)
        {
            CityUrl = cityUrl;
            Operation = operation;
            OperationNumber = operationNumber;
            Token = token;

            Init();
        }

        public static Dictionary<string, DateChecker[]> CreateFromPlatformInfos(CityPlatformInfo[] platforms, string token)
        {
            return platforms.ToDictionary(
                platform => platform.Name,
                platform => CreateFromPlatformInfo(platform, token)
            );
        }

        public static DateChecker[] CreateFromPlatformInfo(CityPlatformInfo platform, string token)
        {
            return platform.Operations.Select(
                (operation, operationNumber) => new DateChecker(platform.BaseUrl, operation, operationNumber + 1, token)
            ).ToArray();
        }

        public async void Init()
        {
            while(true)
            {
                await Task.Delay(UpdateInterval);

                if (FollowersCount == 0)
                    continue;

                DateTime[] temp = await GetDates();

                lock (this)
                    Dates = temp;
            }
        }

        public async Task<DateTime[]> GetDates()
        {
            RestRequest request = new RestRequest(CHECK_DATE_API_ENDPOINT + OperationNumber);
            request.Method = Method.GET;

            request.AddHeader("authority", CityUrl.Replace("https://", "").Trim('/'));
            request.AddHeader("accept", "application/json, text/plain, */*");
            request.AddHeader("authorization", Token);
            request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36");
            request.AddHeader("sec-fetch-site", "same-origin");
            request.AddHeader("sec-fetch-mode", "cors");
            request.AddHeader("sec-fetch-dest", "empty");
            request.AddHeader("referer", CityUrl);
            request.AddHeader("accept-language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");

            IRestResponse dates = await ApiClient.ExecuteAsync(request);

            if (dates.StatusCode != HttpStatusCode.OK || dates.Content.Contains(GENERAL_ERROR_RESPONSE))
                return new DateTime[] { };

            return JsonConvert.DeserializeObject<DateTime[]>(dates.Content);
        }
    }
}
