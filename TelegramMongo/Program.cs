using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using TelegramMongo.Services;

namespace TelegramMongo
{
    internal class Program
    {
        private static readonly ITelegramBotClient Client;
        private static readonly UserManagementService UserManagementService;
        private static readonly BookManagementService BookManagementService;
        private static readonly Random Random;

        static Program()
        {
            Client = new TelegramBotClient(PrivateTokens.TelegramBot);
            UserManagementService = new UserManagementService();
            Random = new Random();
            BookManagementService = new BookManagementService(Random);
        }

        private static void Main(string[] args)
        {
            Console.WriteLine($"Program started at {DateTime.Now}");
            Client.OnMessage += Bot_OnMessage;
            Client.StartReceiving();

            Console.ReadLine();
        }

        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            switch (e.Message.Text ?? string.Empty)
            {
                case "/register":
                {
                    var chat = e.Message.Chat;

                    var loadedUser =
                        await UserManagementService.GetUserAsync(chat.Id) ??
                        await UserManagementService.SignUpUserAsync(new UserRegistrationModel(chat));

                    await Client.SendTextMessageAsync(e.Message.Chat, Messages.Hello(loadedUser.FirstName));
                }
                    break;

                case "/getBook":
                {
                    var chat = e.Message.Chat;
                    var loadedUser = await UserManagementService.GetUserAsync(chat.Id);

                    if (loadedUser == null)
                    {
                        await Client.SendTextMessageAsync(e.Message.Chat, Messages.RegisterFirst);
                        break;
                    }

                    if (loadedUser.BooksGotDate.Date == DateTime.Now.Date && loadedUser.BooksGot == 3)
                    {
                        await Client.SendTextMessageAsync(e.Message.Chat, Messages.BookLimitReached);
                        break;
                    }

                    await Client.SendTextMessageAsync(e.Message.Chat, Messages.Wait);

                    var doc = await BookManagementService.GetBook();
                    await Client.SendDocumentAsync(e.Message.Chat, doc);

                    loadedUser.BooksGotDate = DateTime.Now;
                    loadedUser.BooksGot++;
                    await UserManagementService.UpdateUserAsync(loadedUser);
                }
                    break;
                default:
                {
                    await Client.SendTextMessageAsync(e.Message.Chat, Messages.Usage);
                }
                    break;
            }
        }
    }
}