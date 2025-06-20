using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

// TODO: Fix /lookup -> Lookup Error An error occurred while looking up the mod: Index was outside the bounds of the array.
// TODO: Logs
// You can just translate all to javascript and upload them for free, check comments below ->

namespace ModMateManager
{
    internal class Core
    {
        private const string JSON_FILE_PATH = "mods_database.json"; // only for debug, it will have to access mods_data.json file the same way the program does.
        // If you're poor, either you translate the whole ModMateManager Bot to javascript and upload it on a free website OR just remove it and you update the mods etc manually...
        public class ModData
        {
            public string name { get; set; }
            public string category { get; set; }
            public string author { get; set; }
            public string description { get; set; }
            public string downloadUrl { get; set; }
            public string installPath { get; set; }
            public long fileSize { get; set; }
            public string version { get; set; }
            public bool isCore { get; set; }
            public string installType { get; set; }
            public List<string> dependencies { get; set; } = new List<string>();
            public string iniPath { get; set; } = "";
            public string iniFileName { get; set; } = "";

        }

        public class ModDatabase
        {
            public List<string> categories { get; set; } = new List<string>();
            public List<ModData> mods { get; set; } = new List<ModData>();
        }

        public Embed CreateUploadSubmission(SocketSlashCommand command)
        {
            var options = command.Data.Options.ToList();

            var embed = new EmbedBuilder()
                .WithTitle("🆕 New Mod Submission")
                .WithColor(Color.Blue)
                .WithTimestamp(DateTimeOffset.Now)
                .WithFooter($"Submitted by {command.User.Username}", command.User.GetAvatarUrl())
                 .AddField("📋 Mod Name", GetOptionValue(options, "name"), true)
                .AddField("👤 Author", GetOptionValue(options, "author"), true)
                .AddField("📂 Category", GetOptionValue(options, "category"), true)
                .AddField("🔖 Version", GetOptionValue(options, "version"), true)
                .AddField("📝 Description", GetOptionValue(options, "description"), false)
                .AddField("🔗 Download URL", GetOptionValue(options, "download_url"), false);

            var installPath = GetOptionValue(options, "install_path");
            if (!string.IsNullOrEmpty(installPath))
                embed.AddField("📂 Install Path", installPath, true);

            var dependencies = GetOptionValue(options, "dependencies");
            if (!string.IsNullOrEmpty(dependencies))
                embed.AddField("🔗 Dependencies", dependencies, true);

            var installType = GetOptionValue(options, "install_type");
            if (!string.IsNullOrEmpty(installType))
                embed.AddField("⚙️ Install Type", installType, true);

            embed.AddField("👤 Submitter ID", command.User.Id.ToString(), true);

            return embed.Build(); 
        }

        public Embed CreatePingResponse(int latency)
        {
            var responseTime = DateTime.Now;

            var embed = new EmbedBuilder()
                .WithTitle("Pong!")
                .WithColor(GetLatencyColor(latency))
                .WithTimestamp(DateTimeOffset.Now)
                .AddField("Latency", $"{latency}ms", true)
                .AddField("Response Time", $"{responseTime::HH::mm::ss}", true)
                .AddField("Status", GetLatencyStatus(latency), true)
                .WithFooter("ModMate Manager Bot");

            return embed.Build();
        }

        public async Task<Embed> LookupMod(string modName)
        {
            try
            {
                var database = await LoadDatabase();
                var mod = database.mods.FirstOrDefault(m =>
                    m.name.Equals(modName, StringComparison.OrdinalIgnoreCase));

                if (mod == null)
                {
                    var notFoundEmbed = new EmbedBuilder()
                        .WithTitle("❌ Mod Not Found")
                        .WithColor(Color.Red)
                        .WithDescription($"No mod found with the name: **{modName}**")
                        .WithTimestamp(DateTimeOffset.Now);

                    return notFoundEmbed.Build();
                }

                var embed = new EmbedBuilder()
                  .WithTitle($"📦 {mod.name}")
                  .WithColor(GetCategoryColor(mod.category))
                  .WithTimestamp(DateTimeOffset.Now)
                  .WithFooter("ModMate Manager Database")
                  .AddField("👤 Author", mod.author, true)
                  .AddField("📂 Category", mod.category, true)
                  .AddField("🔖 Version", mod.version, true)
                  .AddField("📝 Description", mod.description, false)
                //  .AddField("🔗 Download URL", mod.downloadUrl.Length > 100 ?
                  //    mod.downloadUrl.Substring(0, 97) + "..." : mod.downloadUrl, false)
                  //.AddField("📁 Install Path", string.IsNullOrEmpty(mod.installPath) ? "Default" : mod.installPath, true)
                  .AddField("⚙️ Install Type", char.ToUpper(mod.installType[0]) + mod.installType.Substring(1), true)
               //   .AddField("📦 File Size", FormatFileSize(mod.fileSize), true)
                  .AddField("🔥 Core Mod", mod.isCore ? "Yes" : "No", true);

                if (mod.dependencies.Any())
                    embed.AddField("🔗 Dependencies", string.Join(", ", mod.dependencies), false);

                return embed.Build();
            }
            catch (Exception ex)
            {
                var errorEmbed = new EmbedBuilder()
                     .WithTitle("❌ Lookup Error")
                    .WithColor(Color.Red)
                    .WithDescription($"An error occurred while looking up the mod: {ex.Message}")
                    .WithTimestamp(DateTimeOffset.Now);

                return errorEmbed.Build();
            }
        }

        private Color GetCategoryColor(string category)
        {
            return category switch
            {
                "LSPDFR Core" => Color.Gold,
                "Plugins" => Color.Blue,
               // "Models" => Color.Green,
                "ELS" => Color.Purple,
                "Mods Updates" => Color.Orange,
                "Scripts" => Color.Teal,
                _ => Color.LightGrey
            };
        }

        private Color GetLatencyColor(int latency)
        {
            return latency switch
            {
                < 100 => Color.Green,
                < 200 => Color.Orange,
                _ => Color.Red
            };
        }

        private string GetLatencyStatus(int latency)
        {
            return latency switch
            {
                < 100 => "Excellent",
                < 200 => "Good",
                < 300 => "Fair",
                _ => "Poor"
            };
        }

        public Embed CreateBugReport(SocketSlashCommand command)
        {
            var options = command.Data.Options.ToList();

            var embed = new EmbedBuilder()
                .WithTitle("🐛 Bug Report")
                .WithColor(Color.Red)
                .WithTimestamp(DateTimeOffset.Now)
                .WithFooter($"Reported by {command.User.Username}", command.User.GetAvatarUrl())
                .AddField("📋 Issue Type", GetOptionValue(options, "issue_type"), true)
                .AddField("👤 Reporter", $"{command.User.Username} ({command.User.Id})", true)
                .AddField("📝 Description", GetOptionValue(options, "description"), false);

            var modName = GetOptionValue(options, "mod_name");
            if (!string.IsNullOrEmpty(modName))
                embed.AddField("🎮 Related Mod", modName, true);

            return embed.Build();
        }

        public async Task<string> AddModToDatabase(SocketSlashCommand command)
        {
            try
            {
                var options = command.Data.Options.ToList();
                var database = await LoadDatabase();

                var newMod = new ModData
                {
                    name = GetOptionValue(options, "name"),
                    author = GetOptionValue(options, "author"),
                    description = GetOptionValue(options, "description"),
                    category = GetOptionValue(options, "category"),
                    version = GetOptionValue(options, "version"),
                    downloadUrl = GetOptionValue(options, "download_url"),
                    installPath = GetOptionValue(options, "install_path"),
                    fileSize = GetOptionLongValue(options, "file_size"),
                    iniPath = GetOptionValue(options, "iniPath"),
                    iniFileName = GetOptionValue(options, "iniFileName"),
                    isCore = GetOptionBoolValue(options, "is_core"),
                    installType = GetOptionValue(options, "install_type") ?? "standard"
                };

                var deps = GetOptionValue(options, "dependencies");
                if (!string.IsNullOrEmpty(deps))
                    newMod.dependencies = deps.Split(',').Select(d => d.Trim()).ToList();

                if (database.mods.Any(m => m.name.Equals(newMod.name, StringComparison.OrdinalIgnoreCase)))
                    return $"❌ Mod '{newMod.name}' already exists in the database.";

                database.mods.Add(newMod);
                await SaveDatabase(database);

                return $"✅ Successfully added '{newMod.name}' to the database.";
            }
            catch (Exception ex)
            {
                return $"❌ An error occurred adding mod: {ex.Message}";
            }
        }

        public async Task<string> EditModInDatabase(SocketSlashCommand command)
        {
            try
            {
                var options = command.Data.Options.ToList();
                var database = await LoadDatabase();

                var modName = GetOptionValue(options, "mod_name");
                var field = GetOptionValue(options, "field");
                var newValue = GetOptionValue(options, "new_value");

                var mod = database.mods.FirstOrDefault(m => m.name.Equals(modName, StringComparison.OrdinalIgnoreCase));
                if (mod == null)
                    return $"❌ Mod '{modName}' not found in database";

                switch (field.ToLower())
                {
                    case "name":
                        mod.name = newValue;
                        break;
                    case "author":
                        mod.author = newValue;
                        break;
                    case "description":
                        mod.description = newValue;
                        break;
                    case "category":
                        mod.category = newValue;
                        break;
                    case "version":
                        mod.version = newValue;
                        break;
                    case "downloadurl":
                        mod.downloadUrl = newValue;
                        break;
                    case "installpath":
                        mod.installPath = newValue;
                        break;
                    case "iniPath":
                        mod.iniPath = newValue;
                        break;
                    case "iniFileName":
                        mod.iniFileName = newValue;
                        break;
                    default:
                        return $"❌ Invalid field '{field}'. Available fields: name, author, description, category, version, downloadUrl, installPath";
                }

                await SaveDatabase(database);
                return $"✅ Successfully updated '{modName}' - {field}: {newValue}";
            }
            catch (Exception ex)
            {
                return $"❌ An error occurred editing mod: {ex.Message}";
            }
        }

        public async Task<string> RemoveModFromDatabase(SocketSlashCommand command)
        {
            try
            {
                var options = command.Data.Options.ToList();
                var database = await LoadDatabase();

                var modName = GetOptionValue(options, "mod_name");
                var category = GetOptionValue(options, "category");

                var mod = database.mods.FirstOrDefault(m =>
                    m.name.Equals(modName, StringComparison.OrdinalIgnoreCase) &&
                    m.category.Equals(category, StringComparison.OrdinalIgnoreCase));

                if (mod == null)
                    return $"❌ Mod '{modName}' in category '{category}' not found in database.";

                database.mods.Remove(mod);
                await SaveDatabase(database);

                return $"✅ Successfully removed '{modName}' from the database.";

            }
            catch (Exception ex)
            {
                return $"❌ An error occurred removing mod: {ex.Message}";
            }
        }

        private async Task<ModDatabase> LoadDatabase()
        {
            try
            {
                if (!File.Exists(JSON_FILE_PATH))
                {
                    var defaultDb = new ModDatabase
                    {
                        categories = new List<string> { "LSPDFR Core", "Plugins", "Models", "ELS", "Mods Updates", "Scripts" },
                        mods = new List<ModData>()
                    };
                    await SaveDatabase(defaultDb);
                    return defaultDb;
                }

                var jsonString = await File.ReadAllTextAsync(JSON_FILE_PATH);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };

                return JsonSerializer.Deserialize<ModDatabase>(jsonString, options) ?? new ModDatabase();
            } catch (Exception ex)
            {
                Console.WriteLine($"An error occurred loading database: {ex.Message}");
                return new ModDatabase();
            }
        }

        private async Task SaveDatabase(ModDatabase database)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var jsonString = JsonSerializer.Serialize(database, options);
                await File.WriteAllTextAsync(JSON_FILE_PATH, jsonString);
                Console.WriteLine("Database saved successfully.");

            } catch (Exception ex)
            {
                Console.WriteLine($"An error occurred saving the database {ex.Message}");
                throw;
            }
        }

        private string GetOptionValue(List<SocketSlashCommandDataOption> options, string name)
        {
            return options.FirstOrDefault(x => x.Name == name)?.Value?.ToString() ?? "";
        }



        private long GetOptionLongValue(List<SocketSlashCommandDataOption> options, string name)
        {
            var value = options.FirstOrDefault(x => x.Name == name)?.Value;
            if (value == null) return 0;
            return Convert.ToInt64(value);
        }

        private bool GetOptionBoolValue(List<SocketSlashCommandDataOption> options, string name)
        {
            var value = options.FirstOrDefault(x => x.Name == name)?.Value;
            if (value == null) return false;
            return Convert.ToBoolean(value);
        }
    }
}
