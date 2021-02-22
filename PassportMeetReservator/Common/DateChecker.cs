using System;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;
using RestSharp;
using Autofac;

using Common.Data;
using Common.Data.Dto;
using Common.Data.Platforms;
using Common.Data.CustomEventArgs;

using Common.Services;
using Common.Extensions;
using Common.Strategies.DateCheckerNotifyStrategies;

namespace Common
{
    public class DateChecker
    {
        public event EventHandler<DateCheckerErrorEventArgs> OnRequestError;
        public event EventHandler<DateCheckerOkEventArgs> OnRequestOK;
        public event EventHandler<ProxyChangedEventArgs> OnProxyChanged;
        public event EventHandler<EventArgs> OnDatesFound;

        public DateCheckerFlowStrategyBase FlowStrategy { get; set; }

        protected ProxyProvider ProxyProvider = CommonDependencyHolder.ServiceDependencies.Resolve<ProxyProvider>();
        protected RestClient ApiClient { get; set; }

        public TimeCheckDto[] TimeCheckDtos { get; private set; }
        public Dictionary<DateTime, DateTime[]> Slots { get; private set; } = new Dictionary<DateTime, DateTime[]>();

        public PlatformApiInfo PlatformInfo { get; private set; }
        public CityPlatformInfo CityInfo { get; private set; }
        public OperationInfo OperationInfo { get; private set; }

        public event EventHandler<EventArgs> FollowersCountChanged;
        public event EventHandler<EventArgs> PausedFollowersCountChanged;
        public event EventHandler<EventArgs> BlockedFollowersCountCountChanged;

        private int followersCount = 0;
        private int pausedFollowersCount = 0;
        private int blockedFollowersCount = 0;

        public int FollowersCount
        {
            get => followersCount;
            set
            {
                followersCount = value;
                FollowersCountChanged?.Invoke(this, new EventArgs());
            }
        }

        public int PausedFollowersCount
        {
            get => pausedFollowersCount;
            set
            {
                pausedFollowersCount = value;
                PausedFollowersCountChanged?.Invoke(this, new EventArgs());
            }
        }

        public int BlockedFollowersCount
        {
            get => blockedFollowersCount;
            set
            {
                blockedFollowersCount = value;
                BlockedFollowersCountCountChanged?.Invoke(this, new EventArgs());
            }
        }

        private DelayInfo DelayInfo { get; set; }
        public BootSchedule Schedule { get; set; }

        public void InitDateChecker(PlatformApiInfo platformInfo, CityPlatformInfo cityInfo, OperationInfo operationInfo, DelayInfo delayInfo)
        {
            PlatformInfo = platformInfo;
            CityInfo = cityInfo;
            OperationInfo = operationInfo;
            DelayInfo = delayInfo;
            
            ApiClient = new RestClient(CityInfo.AltApiUrl ?? PlatformInfo.ApiUrl);

            Init();
        }

        public static Dictionary<string, Dictionary<string, T[]>> CreateFromPlatformInfos<T>(
            PlatformApiInfo[] platforms, DelayInfo delayInfo,
            EventHandler<DateCheckerErrorEventArgs> onRequestErrorHandler,
            EventHandler<DateCheckerOkEventArgs> onRequestOkHandler
        ) where T : DateChecker, new() {
            return platforms.ToDictionary(
                platform => platform.Name,
                platform => CreateFromPlatformInfo<T>(
                    platform, delayInfo,
                    onRequestErrorHandler, 
                    onRequestOkHandler
                )
            );
        }

        public static Dictionary<string, T[]> CreateFromPlatformInfo<T>(
            PlatformApiInfo platform, DelayInfo delayInfo,
            EventHandler<DateCheckerErrorEventArgs> onRequestErrorHandler,
            EventHandler<DateCheckerOkEventArgs> onRequestOkHandler
        ) where T : DateChecker, new() {
            return platform.CityPlatforms.ToDictionary(
                city => city.Name,
                city => CreateFromCityPlatformInfo<T>(
                    platform, city, delayInfo,
                    onRequestErrorHandler,
                    onRequestOkHandler
                )
            );
        }

        public static T[] CreateFromCityPlatformInfo<T>(
            PlatformApiInfo apiInfo, CityPlatformInfo cityInfo, DelayInfo delayInfo,
            EventHandler<DateCheckerErrorEventArgs> onRequestErrorHandler,
            EventHandler<DateCheckerOkEventArgs> onRequestOkHandler
        ) where T : DateChecker, new() {
            T[] checkers = new T[cityInfo.Operations.Length];

            for (int i = 0; i < cityInfo.Operations.Length; ++i)
            {
                checkers[i] = new T();
                checkers[i].InitDateChecker(apiInfo, cityInfo, cityInfo.Operations[i], delayInfo);

                checkers[i].OnRequestError += onRequestErrorHandler;
                checkers[i].OnRequestOK += onRequestOkHandler;
            }

            return checkers;
        }

        public void AddFollower(bool paused)
        {
            FollowersCount++;

            if (paused)
                PausedFollowersCount++;
        }

        public void RemoveFollower(bool paused)
        {
            FollowersCount--;

            if (paused)
                PausedFollowersCount--;
        }

        public async void Init()
        {
            while(true)
            {
                await Task.Delay(DelayInfo.DateCheckDelay);

                if (FollowersCount == 0)
                    continue;

                if (PausedFollowersCount + BlockedFollowersCount == FollowersCount)
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
                TimeCheckDtos = JsonConvert.DeserializeObject<TimeCheckDto[]>(slots.Content);
                availableTimes = TimeCheckDtos.Select(timeCheckDto => timeCheckDto.DateTime).ToArray();
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
            UpdateProxy();

            request.Method = Method.GET;
            request.Timeout = DelayInfo.RequestTimeout;

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

        private void UpdateProxy()
        {
            string proxy = ProxyProvider.GetNextProxy();

            if (proxy != null)
            {
                ApiClient.Proxy = new WebProxy(proxy);
                OnProxyChanged?.Invoke(this, new ProxyChangedEventArgs(proxy));
            }
        }
    }
}
