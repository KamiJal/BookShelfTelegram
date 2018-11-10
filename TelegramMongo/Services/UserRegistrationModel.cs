using Telegram.Bot.Types;

namespace TelegramMongo.Services
{
    public class UserRegistrationModel
    {
        public UserRegistrationModel()
        {
        }

        public UserRegistrationModel(Chat chat)
        {
            ChatId = chat.Id;
            LastName = chat.LastName;
            FirstName = chat.FirstName;
        }

        public long ChatId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}