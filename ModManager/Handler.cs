using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows.Forms;


namespace ModManager
{

    public class Handler
    {
       private static Handler _instance; // define the instance
       private readonly string _settingsFilePath;

        public AppSettings Settings { get; private set; }

        public static Handler Instance
        {
            get
            {
                if (_instance == null )
                {
                    _instance = new Handler();
                }
                return _instance;
            }
        }

        private Handler()
        {
            _settingsFilePath = Path.Combine(Application.StartupPath, "settings.json");
            LoadSettings();
        }

        private void LoadSettings()
        {
            try
            {
                if (File.Exists(_settingsFilePath))
                {
                    string json = File.ReadAllText(_settingsFilePath);
                    Settings = JsonConvert.DeserializeObject<AppSettings>(json); 
                } else
                {
                    Settings = new AppSettings();
                    SaveSettings();
                }
            }
            catch ( Exception ex )
            {
                MessageBox.Show($"Error loading settings: {ex.Message}", 
                    "Settings Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Settings = new AppSettings();
            }
        }

        public void SaveSettings()
        {
            try
            {
                string json = JsonConvert.SerializeObject(Settings, Formatting.Indented);
                File.WriteAllText(_settingsFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}",
                   "Settings Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


    }
}

public class AppSettings
{
    [JsonProperty("gtaPath")]
    public string GTAPath { get; set; } = "";

    [JsonProperty("autoCheckForUpdates")]
    public bool AutoCheckForUpodates { get; set; } = true;

    [JsonProperty("downloadLocation")]
    public string DownloadLocation { get; set; } = Path.Combine(Path.GetTempPath(), "ModManager");

    [JsonProperty("installMods")]
    public System.Collections.Generic.List<InstalledMod> InstalledMods { get; set; } = new System.Collections.Generic.List<InstalledMod>();
}

public class InstalledMod
{
    [JsonProperty("Name")]
    public string Name { get; set; }

    [JsonProperty("version")]
    public string Version { get; set; }

    [JsonProperty("installDate")]
    public DateTime InstallDate { get; set; }

    [JsonProperty("installedFiles")]
    public List<string> InstalledFiles { get; set; } = new List<string>();

}
