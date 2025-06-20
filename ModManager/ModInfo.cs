using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ModManager
{
    public class ModInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("category")] // needs to be valid category from categories in ModsDatabase
        public string Category { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("downloadUrl")]
        public string DownloadUrl { get; set; }

        [JsonProperty("installPath")] 
        public string InstallPath { get; set; }

        [JsonProperty("fileSize")]
        public long FileSize { get; set; }

        [JsonProperty("dependencies")]
        public List<string> Dependencies { get; set; } = new List<string>();

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("isCore")]
        public bool IsCore { get; set; }

        [JsonProperty("installType")]
        public string InstallType { get; set; } = "standard";

        // ini file name and path kept them seprated, cleaner and easy to debug

        // what the name of the ' ini ' file? IF YOU WANT TO ADD A MOD:
        // NOTE: YOU HAVE TO PUT AN EXTENION (.ini) end of the name of the file or wtv the extension of the settings file is, mods_data.json
        [JsonProperty("iniFileName")]
        public string IniFileName { get; set; } = "";
        // in what folder inside the GTAV directory the ini settings file located? 
        [JsonProperty("iniPath")]
        public string IniPath { get; set; }


        // requirements has been disabled, use " dependencies " instead.

        //  [JsonProperty("requirements")]
        //  public string Requirements { get; set; }


    }

    public class ModsDatabase
    {
        [JsonProperty("categories")]
        public List<string> Categories { get; set; } = new List<string>();

        [JsonProperty("mods")]
        public List<ModInfo> Mods { get; set; } = new List<ModInfo>();

    }

}
