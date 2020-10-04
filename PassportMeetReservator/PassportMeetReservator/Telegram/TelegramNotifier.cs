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
        private const string SEND_PHOTO_ENDPOINT = "sendPhoto";

        private const string CHAT_ID_PARAM_NAME = "chat_id";
        private const string CAPTION_PARAM_NAME = "caption";
        private const string PHOTO_PARAM_NAME = "photo";

        private const string TOKEN = "1337342163:AAHvTWbCp0TuWLB047w2MMckAyDvRR4lZBI";

        private string[] GetChatIds(string chatId)
        {
            return chatId.Split(',', '.', ' ');
        }

        public virtual async Task NotifyPhoto(string path, string caption, string chatId)
        {
            foreach (string chat in GetChatIds(chatId))
                await notifyPhoto(path, caption, chat);
        }

        protected virtual async Task notifyPhoto(string path, string caption, string chatId)
        {
            RestClient client = new RestClient($"{BOT_ENDPOINT}{TOKEN}/{SEND_PHOTO_ENDPOINT}") { Timeout = -1 };
            RestRequest request = new RestRequest(Method.POST);

            request.AddParameter(CHAT_ID_PARAM_NAME, chatId);
            request.AddParameter(CAPTION_PARAM_NAME, caption);
            request.AddFile(PHOTO_PARAM_NAME, path);

            IRestResponse response = await client.ExecuteAsync(request);
        }

        public virtual async Task NotifyMessage(string message, string chatId)
        {
            foreach (string chat in GetChatIds(chatId))
                await notifyMessage(message, chat);
        }

        protected virtual async Task notifyMessage(string message, string chatId)
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
            [JsonProperty(CHAT_ID_PARAM_NAME)]
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
