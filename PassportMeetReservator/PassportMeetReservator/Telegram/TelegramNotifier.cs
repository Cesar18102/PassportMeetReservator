using System;
using System.Threading.Tasks;

using Newtonsoft.Json;

using RestSharp;

namespace PassportMeetReservator.Telegram
{
    public class TelegramNotifier
    {
        private const string BOT_ENDPOINT = "https://api.telegram.org/bot";

        private const int MAX_MESSAGE_LENGTH = 4096;

        private const string SEND_MESSAGE_ENDPOINT = "sendMessage";

        private const string TOKEN = "1337342163:AAHvTWbCp0TuWLB047w2MMckAyDvRR4lZBI";

        public virtual async Task NotifyMessage(string message, string chatId)
        {
            await NotifySplitMessage(message, chatId);
        }

        protected async Task NotifySplitMessage(string message, string chatId)
        {
            int messagePartsCount = (int)Math.Ceiling((float)message.Length / MAX_MESSAGE_LENGTH);

            for (int i = 0; i < messagePartsCount; ++i)
            {
                int subMessageLength = Math.Min(MAX_MESSAGE_LENGTH, message.Length - i * MAX_MESSAGE_LENGTH);
                string subMessage = message.Substring(i * MAX_MESSAGE_LENGTH, subMessageLength);
                await SendMessage(subMessage, chatId);
            }
        }

        protected async Task SendMessage(string message, string chatId)
        {
            RestClient client = new RestClient($"{BOT_ENDPOINT}{TOKEN}/{SEND_MESSAGE_ENDPOINT}") { Timeout = -1 };
            RestRequest request = new RestRequest(Method.POST);

            TelegramMessageDto dto = new TelegramMessageDto(
                chatId, message
            );

            string json = JsonConvert.SerializeObject(dto);
            request.AddJsonBody(json);

            IRestResponse response = await client.ExecuteAsync(request);
        }

        protected class TelegramMessageDto
        {
            [JsonProperty("chat_id")]
            public string ChatId { get; private set; }

            [JsonProperty("text")]
            public string Text { get; private set; }

            public TelegramMessageDto(string chatId, string text)
            {
                ChatId = chatId;
                Text = text;
            }
        }
    }
}
