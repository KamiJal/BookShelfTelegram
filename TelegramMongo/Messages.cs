namespace TelegramMongo
{
    public static class Messages
    {
        public const string Usage = "1. /register - to register yourself in system\n2. /getBook - to get random book";
        public const string RegisterFirst = "You have to register first. Send /register";
        public const string BookLimitReached = "You have reached the book limit. Please try tomorrow.";
        public const string Wait = "Please wait. This operation takes some time to accomplish.";

        public static string Hello(string name)
        {
            return $"Hello {name}, you signed in!";
        }
    }
}