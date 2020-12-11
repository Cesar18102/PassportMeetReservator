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
using PassportMeetReservator.Extensions;
using PassportMeetReservator.Strategies.DateCheckerNotifyStrategies;

namespace PassportMeetReservator.Data
{
    public class DateChecker
    {
        public event EventHandler<DateCheckerErrorEventArgs> OnRequestError;
        public event EventHandler<DateCheckerOkEventArgs> OnRequestOK;
        public event EventHandler<EventArgs> OnDatesFound;

        public DateCheckerFlowStrategyBase FlowStrategy { get; set; }

        private RestClient ApiClient { get; set; }

        public Dictionary<DateTime, DateTime[]> Slots { get; private set; } = new Dictionary<DateTime, DateTime[]>();

        public PlatformApiInfo PlatformInfo { get; private set; }
        public CityPlatformInfo CityInfo { get; private set; }
        public OperationInfo OperationInfo { get; private set; }

        public int FollowersCount { get; set; }
        public int PausedFollowersCount { get; set; }

        private DelayInfo DelayInfo { get; set; }
        public BootSchedule Schedule { get; set; }

        public DateChecker(PlatformApiInfo platformInfo, CityPlatformInfo cityInfo, OperationInfo operationInfo, DelayInfo delayInfo)
        {
            PlatformInfo = platformInfo;
            CityInfo = cityInfo;
            OperationInfo = operationInfo;
            DelayInfo = delayInfo;
            
            ApiClient = new RestClient(CityInfo.AltApiUrl ?? PlatformInfo.ApiUrl);

            Init();
        }

        public static Dictionary<string, Dictionary<string, DateChecker[]>> CreateFromPlatformInfos(
            PlatformApiInfo[] platforms, DelayInfo delayInfo,
            EventHandler<DateCheckerErrorEventArgs> onRequestErrorHandler,
            EventHandler<DateCheckerOkEventArgs> onRequestOkHandler
        ) {
            return platforms.ToDictionary(
                platform => platform.Name,
                platform => CreateFromPlatformInfo(
                    platform, delayInfo,
                    onRequestErrorHandler, 
                    onRequestOkHandler
                )
            );
        }

        public static Dictionary<string, DateChecker[]> CreateFromPlatformInfo(
            PlatformApiInfo platform, DelayInfo delayInfo,
            EventHandler<DateCheckerErrorEventArgs> onRequestErrorHandler,
            EventHandler<DateCheckerOkEventArgs> onRequestOkHandler
        ) {
            return platform.CityPlatforms.ToDictionary(
                city => city.Name,
                city => CreateFromCityPlatformInfo(
                    platform, city, delayInfo,
                    onRequestErrorHandler,
                    onRequestOkHandler
                )
            );
        }

        public static DateChecker[] CreateFromCityPlatformInfo(
            PlatformApiInfo apiInfo, CityPlatformInfo cityInfo, DelayInfo delayInfo,
            EventHandler<DateCheckerErrorEventArgs> onRequestErrorHandler,
            EventHandler<DateCheckerOkEventArgs> onRequestOkHandler
        ) {
            DateChecker[] checkers = new DateChecker[cityInfo.Operations.Length];

            for (int i = 0; i < cityInfo.Operations.Length; ++i)
            {
                checkers[i] = new DateChecker(apiInfo, cityInfo, cityInfo.Operations[i], delayInfo);
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

                try { await FlowStrategy.DateCheckFlow(GetDates, GetTimesForDate, Slots, NotifyFound); }
                catch { }
            }
        }

        protected void NotifyFound()
        {
            OnDatesFound?.Invoke(this, new EventArgs());
        }

        protected async Task<DateTime[]> GetDates()
        {
            RestRequest request = new RestRequest($"{PlatformInfo.GetAvailableDatesApiMethod}/{OperationInfo.Number}");
            ConfigureRequest(request);

            IRestResponse dates = await ApiClient.ExecuteAsync(request);

            if (dates.StatusCode != HttpStatusCode.OK)
            {
                OnRequestError?.Invoke(this, new DateCheckerErrorEventArgs((int)dates.StatusCode));
                throw new Exception();
            }

            if (dates.Content.Contains(PlatformInfo.GeneralErrorMessage))
            {
                OnRequestError?.Invoke(this, new DateCheckerErrorEventArgs((int)dates.StatusCode));
                throw new Exception();
            }

            DateTime[] availableDates = null;

            try
            {
                DateCheckDto dateCheckDto = JsonConvert.DeserializeObject<DateCheckDto>(dates.Content);
                availableDates = dateCheckDto.AvailableDates;
            }
            catch
            {
                availableDates = JsonConvert.DeserializeObject<DateTime[]>(dates.Content);
            }

            string datesString = JsonConvert.SerializeObject(availableDates);
            OnRequestOK?.Invoke(this, new DateCheckerOkEventArgs(datesString));

            return availableDates;
        }

        protected async Task<DateTime[]> GetTimesForDate(DateTime date)
        {
            RestRequest request = new RestRequest($"{PlatformInfo.GetAvailableSlotsForDateApiMethod}/{OperationInfo.Number}/{date.GetFormattedDate()}");
            ConfigureRequest(request);

            IRestResponse slots = await ApiClient.ExecuteAsync(request);

            if (slots.StatusCode != HttpStatusCode.OK)
                throw new Exception();

            if (slots.Content.Contains(PlatformInfo.GeneralErrorMessage))
                throw new Exception();

            DateTime[] availableTimes = null;

            try
            {
                TimeCheckDto[] timeCheckDtos = JsonConvert.DeserializeObject<TimeCheckDto[]>(slots.Content);
                availableTimes = timeCheckDtos.Select(timeCheckDto => timeCheckDto.DateTime).ToArray();
            }
            catch
            {
                availableTimes = JsonConvert.DeserializeObject<DateTime[]>(slots.Content);
            }

            string timesString = JsonConvert.SerializeObject(availableTimes);
            OnRequestOK?.Invoke(this, new DateCheckerOkEventArgs(timesString));

            return availableTimes;
        }

        protected void ConfigureRequest(RestRequest request)
        {
            request.Method = Method.GET;
            request.Timeout = 300;

            request.AddHeader("authority", CityInfo.Authority);
            request.AddHeader("accept", "application/json, text/plain, */*");
            request.AddHeader("authorization", PlatformInfo.Token);
            request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36");
            request.AddHeader("sec-fetch-site", "same-origin");
            request.AddHeader("sec-fetch-mode", "cors");
            request.AddHeader("sec-fetch-dest", "empty");
            request.AddHeader("referer", CityInfo.Referer);
            request.AddHeader("accept-language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
        }
    }
}
