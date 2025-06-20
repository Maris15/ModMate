using Discord;
using Discord.WebSocket;
using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ModMateManager
{
    public class Program
    {
        private static DiscordSocketClient _client;
        private static Commands _commands;
        private static Core _core;

        public static async Task Main()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages
            });

            const string token = "MTM4MDk2MDg0NTY5MzA1OTE3Mw.G3G2ds.Bi1uDbLaXrOCsd8TNITxQM_Si4OHruEwOtmvH4";

            _core = new Core();
            _commands = new Commands(_client, _core);

            _client.Log += LogAsync;
            _client.Ready += ReadyAsync;
            _client.SlashCommandExecuted += SlashCommandHandler;

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Token is invalid");
            }

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            Console.WriteLine("Initilizing has complete\n running...");
            await Task.Delay(-1);
        }
        private static Task LogAsync(LogMessage msg)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {msg.ToString()}");
            return Task.CompletedTask;
        }

        private static async Task ReadyAsync()
        {
            await _commands.RegisterCommandsAsync();
            Console.WriteLine($"The client has successfully logged in as {_client.CurrentUser.Username}");
        }

        private static async Task SlashCommandHandler(SocketSlashCommand command)
        {
            try
            {
                await _commands.HandleCommandAsync(command);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred handling command: {ex.Message}");

                if (!command.HasResponded)
                {
                    await command.RespondAsync("An error occurred while processing the command.", ephemeral: true);
                }
            }
        }
    }
}