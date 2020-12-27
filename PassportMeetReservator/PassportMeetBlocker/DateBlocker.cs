using System;
using System.Net;
using System.Threading.Tasks;

using RestSharp;

using Newtonsoft.Json;

using Common;
using Common.Data.CustomEventArgs;
using Common.Data.Dto;

namespace PassportMeetBlocker
{
    public class DateBlocker : DateChecker
    {
        public event EventHandler<DateCheckerErrorEventArgs> OnBlockRequestError;
        public event EventHandler<SlotBlockedEventArgs> OnSlotBlocked;

        public async Task<DateBlockDto> BlockSlot(TimeCheckDto time)
        {
            string blockMethod = CityInfo.AltBlockSlotApiMethod ?? PlatformInfo.BlockSlotApiMethod;

            RestRequest request = new RestRequest($"{blockMethod}/{time.Id}");
            ConfigureRequest(request);

            IRestResponse dates = await ApiClient.ExecuteAsync(request);

            if (dates.StatusCode != HttpStatusCode.OK)
            {
                OnBlockRequestError?.Invoke(this, new DateCheckerErrorEventArgs((int)dates.StatusCode));
                return null;
            }

            if (dates.Content.Contains(PlatformInfo.GeneralErrorMessage))
            {
                OnBlockRequestError?.Invoke(this, new DateCheckerErrorEventArgs((int)dates.StatusCode));
                return null;
            }

            try
            {
                DateBlockDto block = JsonConvert.DeserializeObject<DateBlockDto>(dates.Content);
                OnSlotBlocked?.Invoke(this, new SlotBlockedEventArgs() { Slot = time, Block = block });
                return block;
            }
            catch { return null; }
        }
    }
}
