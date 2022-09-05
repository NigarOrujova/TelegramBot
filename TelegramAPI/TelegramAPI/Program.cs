using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramAPI.DAL;
using TelegramAPI.Models;
using Message = TelegramAPI.Models.Message;

namespace TelegramAPI
{
    public struct BotUpdate
    {
        public string text;
        public long id;
        public string? username;
        public string? firstname;
        public string? lastname;
    }
    public class Program
    {
        static TelegramBotClient Bot = new TelegramBotClient("//token");

        static string fileName = "updates.json";
        static List<BotUpdate> botUpdates = new List<BotUpdate>();

        static void Main(string[] args)
        {

            //Read all saved updates
            try
            {
                var botUpdatesString = System.IO.File.ReadAllText(fileName);

                botUpdates = JsonConvert.DeserializeObject<List<BotUpdate>>(botUpdatesString) ?? botUpdates;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading or deserializing {ex}");
            }

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new UpdateType[]
                {
                    UpdateType.Message,
                    UpdateType.EditedMessage,
                }
            };

            Bot.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions);

            Console.ReadLine();
        }

        private static Task ErrorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }

        private static async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken arg3)
        {
            using (var db = new AppDbContext())
            {
                if (update.Type == UpdateType.Message)
                {
                    if (update.Message.Type == MessageType.Text)
                    {
                        //write an update
                        var _botUpdate = new Message
                        {
                            Text = update.Message.Text,
                            Username = update.Message.From.Username,
                            FisrtName = update.Message.From.FirstName,
                            LastName = update.Message.From.LastName
                        };
                        await bot.SendTextMessageAsync(update.Message.Chat.Id, _botUpdate.Text);
                        await db.AddAsync(_botUpdate);
                        await db.SaveChangesAsync();
                    }
                }
            }
        }
    }
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
            => services.AddDbContext<AppDbContext>();
    }
}

