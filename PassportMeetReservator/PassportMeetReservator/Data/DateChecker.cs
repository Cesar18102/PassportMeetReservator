using System;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;

using RestSharp;

using PassportMeetReservator.Data.Platforms;
using PassportMeetReservator.Data.CustomEventArgs;
using PassportMeetReservator.Data.Dto;

namespace PassportMeetReservator.Data
{
    public class DateChecker
    {
        public event EventHandler<DateCheckerErrorEventArgs> OnRequestError;
        public event EventHandler<DateCheckerOkEventArgs> OnRequestOK;

        private const string GENERAL_ERROR_RESPONSE = "General error.";
        private const string CHECK_DATE_API_ENDPOINT = "Slot/GetAvailableDaysForOperation/";
        private RestClient ApiClient { get; set; } = new RestClient("https://rejestracjapoznan.poznan.uw.gov.pl/api");

        public DateTime[] Dates { get; private set; } = new DateTime[] { };

        public string Token { get; private set; }

        public string City { get; private set; }
        public string CityUrl { get; private set; }

        public OperationInfo Operation { get; private set; }

        public int FollowersCount { get; set; }
        public int PausedFollowersCount { get; set; }

        private DelayInfo DelayInfo { get; set; }
        public BootSchedule Schedule { get; set; }

        public DateChecker(string city, string cityUrl, OperationInfo operation, string token, DelayInfo delayInfo)
        {
            City = city;
            CityUrl = cityUrl;
            Operation = operation;
            Token = token;
            DelayInfo = delayInfo;

            Init();
        }

        public static Dictionary<string, DateChecker[]> CreateFromPlatformInfos(
            CityPlatformInfo[] platforms, string token, 
            EventHandler<DateCheckerErrorEventArgs> onRequestErrorHandler,
            EventHandler<DateCheckerOkEventArgs> onRequestOkHandler,
            DelayInfo delayInfo
        ) {
            return platforms.ToDictionary(
                platform => platform.Name,
                platform => CreateFromPlatformInfo(platform, token, onRequestErrorHandler, onRequestOkHandler, delayInfo)
            );
        }

        public static DateChecker[] CreateFromPlatformInfo(
            CityPlatformInfo platform, string token, 
            EventHandler<DateCheckerErrorEventArgs> onRequestErrorHandler,
            EventHandler<DateCheckerOkEventArgs> onRequestOkHandler,
            DelayInfo delayInfo
        ) {
            DateChecker[] checkers = new DateChecker[platform.Operations.Length];

            for(int i = 0; i < platform.Operations.Length; ++i)
            {
                checkers[i] = new DateChecker(platform.Name, platform.BaseUrl, platform.Operations[i], token, delayInfo);
                checkers[i].OnRequestError += onRequestErrorHandler;
                checkers[i].OnRequestOK += onRequestOkHandler;
            }

            return checkers;
        }

        public async void Init()
        {
            while(true)
            {
                await Task.Delay(DelayInfo.DateCheckDelay);

                if (FollowersCount == 0)
                    continue;

                if (PausedFollowersCount == FollowersCount)
                    continue;

                if (Schedule != null && !Schedule.IsInside(DateTime.Now.TimeOfDay))
                    continue;

                Dates = await GetDates();
            }
        }

        public async Task<DateTime[]> GetDates()
        {
            RestRequest request = new RestRequest(CHECK_DATE_API_ENDPOINT + Operation.Number);
            request.Method = Method.GET;
            request.Timeout = 1000;

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

            if (dates.StatusCode != HttpStatusCode.OK)
            {
                OnRequestError?.Invoke(this, new DateCheckerErrorEventArgs((int)dates.StatusCode));
                return new DateTime[] { };
            }

            if (dates.Content.Contains(GENERAL_ERROR_RESPONSE))
            {
                OnRequestError?.Invoke(this, new DateCheckerErrorEventArgs((int)dates.StatusCode));
                return new DateTime[] { };
            }

            DateCheckDto dateCheckDto = JsonConvert.DeserializeObject<DateCheckDto>(dates.Content);
            string datesString = JsonConvert.SerializeObject(dateCheckDto.AvailableDates);

            OnRequestOK?.Invoke(this, new DateCheckerOkEventArgs(datesString));
            return dateCheckDto.AvailableDates;
        }
    }
}
