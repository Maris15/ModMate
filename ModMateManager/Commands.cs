using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ModMateManager
{
    internal class Commands
    {
        private readonly DiscordSocketClient _client;
        private readonly Core _core;

        // Admins IDs

        private readonly HashSet<ulong> _AdminIds = new HashSet<ulong>
        {
            602811850857644032
        };


        // Channel IDs
        private const ulong UPLOAD_CHANNEL_ID = 1381366774926278769;
        private const ulong REPORT_CHANNEL_ID = 1381387820555305090;
        private const ulong LOGS_CHANNEL_ID = 1381672458649731102; // TDOO: Logs channel

        public Commands(DiscordSocketClient client, Core core)
        {
            _client = client;
            _core = core;
        }

        public async Task RegisterCommandsAsync()
        {
            var commands = new List<SlashCommandBuilder>
            {
                new SlashCommandBuilder()
                .WithName("upload")
                .WithDescription("Submit a mod for review by Admin")
                .AddOption("name", ApplicationCommandOptionType.String, "Name of the mod", isRequired: true)
                .AddOption("author", ApplicationCommandOptionType.String, "author of the mod", isRequired: true)
                .AddOption("description", ApplicationCommandOptionType.String, "Description of the mod", isRequired: true)
                .AddOption("category", ApplicationCommandOptionType.String, "Category of the mod", isRequired: true,
                        choices: new ApplicationCommandOptionChoiceProperties[]
                        {
                            new() { Name = "LSPDFR Core", Value = "LSPDFR Core" },
                            new() { Name = "Plugins", Value = "Plugins" },
                           // new() { Name = "Models", Value = "Models" },
                            new() { Name = "ELS", Value = "ELS" },
                            new() { Name = "Mods Updates", Value = "Mods Updates" },
                            new() { Name = "Scripts", Value = "Scripts" }
                        })
                .AddOption("version", ApplicationCommandOptionType.String, "Version of the mod", isRequired: true)
                .AddOption("download_url", ApplicationCommandOptionType.String, "Download URL or attachment", isRequired: true)
                //.AddOption("install_path", ApplicationCommandOptionType.String, "Installation path", isRequired: false)
                .AddOption("dependencies", ApplicationCommandOptionType.String, "Dependencies that the mod depends on", isRequired: false),

                /*
                .AddOption("install_type", ApplicationCommandOptionType.String, "Installation type", isRequired: false,
                      choices: new ApplicationCommandOptionChoiceProperties[]
                       {
                            new() { Name = "Standard", Value = "standard" },
                           new() { Name = "Car Pack", Value = "carpack" },
                            new() { Name = "ELS", Value = "els" }
                       }),
                */

                new SlashCommandBuilder()
                .WithName("add")
                .WithDescription("[ADMIN] Add a mod to the database")
                .AddOption("name", ApplicationCommandOptionType.String, "Name of the mod", isRequired: true)
                .AddOption("author", ApplicationCommandOptionType.String, "Author of the mod", isRequired: true)
                .AddOption("description", ApplicationCommandOptionType.String, "Description of the mod", isRequired: true)
                .AddOption("category", ApplicationCommandOptionType.String, "Category of the mod", isRequired: true)
                .AddOption("version", ApplicationCommandOptionType.String, "Version of the mod", isRequired: true)
                .AddOption("download_url", ApplicationCommandOptionType.String, "Download URL", isRequired: true)
                .AddOption("install_path", ApplicationCommandOptionType.String, "Installation path", isRequired: true)
                .AddOption("file_size", ApplicationCommandOptionType.Integer, "File size in bytes", isRequired: false)
                .AddOption("is_core", ApplicationCommandOptionType.Boolean, "Is this a core mod?", isRequired: true)
                .AddOption("install_type", ApplicationCommandOptionType.String, "Installation type", isRequired: false)
                .AddOption("ini_path", ApplicationCommandOptionType.String, "ini Path", isRequired: false)
                .AddOption("ini_file_name", ApplicationCommandOptionType.String, "ini File Name", isRequired: false)
                .AddOption("dependencies", ApplicationCommandOptionType.String, "Dependencies (comma-separated)", isRequired: false),

                new SlashCommandBuilder()
                .WithName("edit")
                .WithDescription("[ADMIN] Edit an existing mod")
                .AddOption("mod_name", ApplicationCommandOptionType.String, "Name of the mod to edit", isRequired: true)
                    .AddOption("field", ApplicationCommandOptionType.String, "Field to edit", isRequired: true,
                        choices: new ApplicationCommandOptionChoiceProperties[]
                        {
                            new() { Name = "Name", Value = "name" },
                            new() { Name = "Author", Value = "author" },
                            new() { Name = "Description", Value = "description" },
                            new() { Name = "Category", Value = "category" },
                            new() { Name = "Version", Value = "version" },
                            new() { Name = "Download URL", Value = "downloadUrl" },
                            new() { Name = "Install Path", Value = "installPath" }
                        })
                    .AddOption("new_value", ApplicationCommandOptionType.String, "New value for the field", isRequired: true),

                new SlashCommandBuilder()
                .WithName("remove")
                .WithDescription("[ADMIN] Remove a mod from the database")
                .AddOption("mod_name", ApplicationCommandOptionType.String, "Name of the mod to remove", isRequired: true)
                .AddOption("category", ApplicationCommandOptionType.String, "Category of the mod", isRequired: true),

                new SlashCommandBuilder()
                .WithName("report")
                .WithDescription("Report a bug or an issue to Admin")
                .AddOption("issue_type", ApplicationCommandOptionType.String, "Type of issue", isRequired: true,
                        choices: new ApplicationCommandOptionChoiceProperties[]
                        {
                            new() { Name = "Bug Report", Value = "bug" },
                            new() { Name = "Mod Not Working", Value = "mod_issue" },
                            new() { Name = "Download Problem", Value = "download" },
                            new() { Name = "Other", Value = "other" }
                        })

                    .AddOption("description", ApplicationCommandOptionType.String, "Detailed description of the issue", isRequired: true)
                    .AddOption("mod_name", ApplicationCommandOptionType.String, "Related mod name (if applicable)", isRequired: false),

                new SlashCommandBuilder()
                .WithName("ping")
                .WithDescription("Check bot latency and response time"),

                new SlashCommandBuilder()
                .WithName("lookup")
                .WithDescription("Get detaild information about a specific mod")
                .AddOption("mod_name", ApplicationCommandOptionType.String, "Name of the mod to look up", isRequired: true)
            };

            foreach (var command in commands)
            {
                try
                {
                    await _client.CreateGlobalApplicationCommandAsync(command.Build());
                    Console.WriteLine($"Registered Command: {command.Name}");
                } catch (Exception ex)
                {
                    Console.WriteLine($"Failed to register command {command.Name}: {ex.Message}");
                }
            }

            Console.WriteLine("All command has successfully registered.");
        }

        public async Task HandleCommandAsync(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "upload":
                    await HandleUploadCommand(command);
                    break;
                case "add":
                    await HandleAddCommand(command);
                    break;
                case "edit":
                    await HandleEditCommand(command);
                    break;
                case "remove":
                    await HandleRemoveCommand(command);
                    break;
                case "report":
                    await HandleReportCommand(command);
                    break;
                case "ping":
                    await HandlePingCommand(command);
                    break;
                case "lookup":
                    await HandleLookupCommand(command);
                    break;

                default:
                    await command.RespondAsync("Unknown command.", ephemeral: true);
                    break;
            }
        }

        private async Task HandleUploadCommand(SocketSlashCommand command)
        {
            var embed = _core.CreateUploadSubmission(command);
            var channel = _client.GetChannel(UPLOAD_CHANNEL_ID) as IMessageChannel;

            if (channel != null)
            {
                await channel.SendMessageAsync(embed: embed);
                await command.RespondAsync("Your mod submission has been sent to Admin for review!", ephemeral: true);
            } else
            {
                await command.RespondAsync("An error occurred: channel id is null", ephemeral: true);
            }
        }

        private async Task HandleAddCommand(SocketSlashCommand command)
        {
            if (!IsAdmin(command.User.Id))
            {
                await command.RespondAsync("You don't have permission to use this command.", ephemeral: true);
                return;
            }

            var result = await _core.AddModToDatabase(command);
            await command.RespondAsync(result, ephemeral: true);
        }

        private async Task HandleEditCommand(SocketSlashCommand command)
        {
            if (!IsAdmin(command.User.Id))
            {
                await command.RespondAsync("You don't have permission to use this command.", ephemeral: true);
                return;
            }

            var result = await _core.EditModInDatabase(command);
            await command.RespondAsync(result, ephemeral: true);

        }

        private async Task HandleRemoveCommand(SocketSlashCommand command)
        {
            if (!IsAdmin(command.User.Id))
            {
                await command.RespondAsync("You don't have permission to use this command.", ephemeral: true);
                return;
            }

            var result = await _core.RemoveModFromDatabase(command);
            await command.RespondAsync(result, ephemeral: true);
        }

        private async Task HandleReportCommand(SocketSlashCommand command)
        {
            var embed = _core.CreateBugReport(command);
            var channel = _client.GetChannel(REPORT_CHANNEL_ID) as IMessageChannel;

            if (channel != null)
            {
                await channel.SendMessageAsync(embed: embed);
                await command.RespondAsync("Your report has been submitted. Thank you! contacting Admin...", ephemeral: true);
            }
            else
            {
                await command.RespondAsync("Error: Could not find the report channel.", ephemeral: true);
            }
        }

        private async Task HandlePingCommand(SocketSlashCommand command)
        {
            var latency = _client.Latency;
            var embed = _core.CreatePingResponse(latency);
            await command.RespondAsync(embed: embed);
        }

        private async Task HandleLookupCommand(SocketSlashCommand command)
        {
            var modName = command.Data.Options.First().Value.ToString();
            var embed = await _core.LookupMod(modName);
            await command.RespondAsync(embed: embed);
        }

        private bool IsAdmin(ulong userId)
        {
            return _AdminIds.Contains(userId);
        }
    }
}
