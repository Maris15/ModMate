using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace ModManager
{
    public class DataManager
    {
        private static DataManager _instance;
        private string _dataFilePath = Path.Combine(Application.StartupPath, "mods_data.json");

        private const string RemoteJsonUrl = "http://localhost:8000/mods_data.json"; // TODO: here is to put your server link that upload the " json " file

        public ModsDatabase Database { get; private set; }

        public static DataManager Instance 
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataManager();
                }
                return _instance;
            }
        }
        private DataManager()
        {
            LoadDatabase();
        }

        public void LoadDatabase()
        {
            string json = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(3);
                    json = client.GetStringAsync(RemoteJsonUrl).GetAwaiter().GetResult();

                }
                File.WriteAllText(_dataFilePath, json);
            }
            catch (Exception ex)
            {
                if (File.Exists(_dataFilePath))
                {
                    try
                    {
                        json = File.ReadAllText(_dataFilePath);
                    } catch (Exception rx)
                    {
                        MessageBox.Show(
                           $"Failed to read local mods_data.json:\n{rx.Message}",
                           "Database Error",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error);
                    }
                } else
                {
                    MessageBox.Show(
                    $"Could not fetch remote JSON ({ex.Message}) and no local file found.",
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
            }

            try
            {
                if (string.IsNullOrWhiteSpace(json))
                    throw new Exception("No JSON data available.");

                Database = JsonConvert.DeserializeObject<ModsDatabase>(json)
                           ?? throw new Exception("Deserialized result was null.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error parsing mods database JSON:\n{ex.Message}",
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Database = new ModsDatabase();
            }

        }
        public void SaveDatabase()
        {
            try
            {
                string json = JsonConvert.SerializeObject(Database, Formatting.Indented);
                File.WriteAllText(_dataFilePath, json);
            } 
            catch (Exception ex)
            {
                MessageBox.Show($"Error Saving mods: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Dictionary<string, ModInfo> GetModsDictionary()
        {
            Dictionary<string, ModInfo> dict = new Dictionary<string, ModInfo>();
            foreach (var mod in Database.Mods)
            {
                dict[mod.Name] = mod;
            }
            return dict;
        }

        public List<ModInfo> GetModsByCategory(string category)
        {
            return Database.Mods.Where(m => m.Category == category).ToList();
        }


        // Test Note: 
        // this method is no longer needed on release, this is for debugging
        // - Maris
        private ModsDatabase CreateDefaultDatabase()
       {
            ModsDatabase db = new ModsDatabase();

            // if you wish to put default db hardcoded if the json file fails or not exist, place the formatts here 
            // and edit LoadDatabase method to actually load this default db if the json file failed or not exist



            return db;
       }


    }

}
