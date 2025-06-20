using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;

// TODO: fix issue when closing the window while downloading doesn't show any warnings, stops downloading and doesn't clean propery the " temp " file


// TODO: auto detect gtav.exe and open gta main directory
// TODO: play modded ( open ragepluginhook exe ) 
// TODO: add a refresh button to refresh the whole menu

namespace ModManager
{
    public partial class Form1 : Form
    {
        private Dictionary<string, ModInfo> availableMods;

        private List<string> selectedMods = new List<string>();

        private DownloadManager downloadManager;

        private ToolTip toolTip = new ToolTip(); // much easier in the long run
        public Form1()
        {

            InitializeComponent();

            LoadSettings();
            LoadModData();
            InitializeTooltips();
            LoadInstalledModsTab();

        }

        private void LoadSettings()
        {
            var settings = Handler.Instance.Settings;

            if (!string.IsNullOrEmpty(settings.GTAPath) && Directory.Exists(settings.GTAPath))
            {
                pathTextBox.Text = settings.GTAPath;
            }
        }

        private void LoadModData()
        {
            DataManager.Instance.LoadDatabase();
            availableMods = DataManager.Instance.GetModsDictionary();

            categoryListView.Items.Clear();
            foreach (var category in DataManager.Instance.Database.Categories)
            {
                categoryListView.Items.Add(new ListViewItem(category));
            }

            if (categoryListView.Items.Count > 0)
            {
                categoryListView.Items[0].Selected = true;
            }
        }

        private void LoadInstalledModsTab()
        {
            this.installedListView.Items.Clear();
            var settings = Handler.Instance.Settings;

            foreach (var im in settings.InstalledMods)
            {
                var item = new ListViewItem(im.Name);
                item.SubItems.Add(im.Version);
                this.installedListView.Items.Add(item);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var settings = Handler.Instance.Settings;
            settings.GTAPath = pathTextBox.Text;
            Handler.Instance.SaveSettings();
        }
        private void browseButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select GTA V Installation Directory";
                if (!string.IsNullOrEmpty(pathTextBox.Text) && Directory.Exists(pathTextBox.Text))
                {
                    folderDialog.SelectedPath = pathTextBox.Text;
                }
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;
                    if (File.Exists(Path.Combine(selectedPath, "GTA5.exe")))
                    {
                        pathTextBox.Text = selectedPath;
                        var settings = Handler.Instance.Settings;
                        settings.GTAPath = selectedPath;
                        Handler.Instance.SaveSettings();
                    } else
                    {
                        MessageBox.Show("GTA5.exe was not found in the selected directory. Please select the correct GTA V installation folder.",
                            "Invalid Directory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                }
            }
        }

        private void categoryListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (categoryListView.SelectedItems.Count > 0) {
                string selectedCategory = categoryListView.SelectedItems[0].Text;

                modTitleLabel.Text = "";
                modAuthorLabel.Text = "";
                modDescriptionTextBox.Text = "";
                btnOpenModSettings.Enabled = selectedCategory != "Mods Updates"; // disable open settings button when on Mods Updates
                modsCheckedListBox.Items.Clear();
                foreach (var mod in DataManager.Instance.GetModsByCategory(selectedCategory))
                {
                    modsCheckedListBox.Items.Add(mod.Name, selectedMods.Contains(mod.Name));
                }
            }

        }

        private void modsCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string modName = modsCheckedListBox.Items[e.Index].ToString();

            BeginInvoke(new Action(() =>
            {
                if (e.NewValue == CheckState.Checked)
                {
                   if (!selectedMods.Contains(modName))
                    {
                        selectedMods.Add(modName);

                        if (availableMods.ContainsKey(modName))
                        {
                            ModInfo mod = availableMods[modName];
                            if (mod.Dependencies != null && mod.Dependencies.Count > 0)
                            {
                                foreach (string dependency in mod.Dependencies)
                                {
                                    if (!selectedMods.Contains(dependency))
                                    {
                                        for (int i = 0; i < modsCheckedListBox.Items.Count; i++)
                                        {
                                            if (modsCheckedListBox.Items[i].ToString() == dependency) { 
                                            modsCheckedListBox.SetItemChecked(i, true);
                                           break;
                                            }
                                        }
                                    }
                                    MessageBox.Show($"The mod '{modName}' depends on '{dependency}', install it alone if you already have {dependency}.",
                                           "ATTENTION!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }

                        }

                    }

                } else
                {
                    selectedMods.Remove(modName);
                    // we check if other mods depend on this one
                    List<string> dependents = new List<string>();
                    foreach (var mod in availableMods.Values)
                    {
                        if (mod.Dependencies != null && mod.Dependencies.Contains(modName))
                        {
                            dependents.Add(mod.Name);
                        }
                    }
                    if (dependents.Count > 0)
                    {
                        string dependentsList = string.Join(",", dependents);
                        MessageBox.Show($"Warning: The following selected mods depend on '{modName}': {dependentsList}{Environment.NewLine}{Environment.NewLine}These mods will NOT work without it.",
                           "Dependency Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
            }));
        }

        private void modsCheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (modsCheckedListBox.SelectedIndex >= 0)
            {
                string modName = modsCheckedListBox.SelectedItem.ToString();

                // Update mod details panel
                if (availableMods.ContainsKey(modName))
                {
                    ModInfo mod = availableMods[modName];
                    modTitleLabel.Text = $"{mod.Name} v{mod.Version}";
                    modAuthorLabel.Text = $"Author: {mod.Author}";

                    // Create a more detailed description

                    string detailedDescription = $"{mod.Description}{Environment.NewLine}{Environment.NewLine}";
                 //   detailedDescription += $"Size: {FormatFileSize(mod.FileSize)}{Environment.NewLine}"; // Will be availble incoming updates once i make a system automatic to detect the filesize

                    // install path is only useful now for installation and very important
                    // so best solution is just type installation path in description of the mod
                    // maybe we try something else later? - maris

                    //  detailedDescription += $"Install Path: {mod.InstallPath}{Environment.NewLine}"; // disabled - maris
                    //  detailedDescription += $"Requirements: {mod.Requirements}{Environment.NewLine}"; // disabled, view ModInfo.cs for more details

                    if (mod.Dependencies != null && mod.Dependencies.Count > 0)
                    {
                        detailedDescription += $"Dependencies: {string.Join(", ", mod.Dependencies)}{Environment.NewLine}";
                    }

                    if (mod.IsCore)
                    {
                        detailedDescription += $"{Environment.NewLine}This is a core component required for many other mods/scripts to function.";
                    }

                    modDescriptionTextBox.Text = detailedDescription;
                }
            }
        }

        // Will be availble incoming updates

        /*
        private string FormatFileSize(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
            int suffixIndex = 0;
            double size = bytes;

            while (size >= 1024 && suffixIndex < suffixes.Length - 1)
            {
                suffixIndex++;
                size /= 1024;
            }

            return $"{size:0.##} {suffixes[suffixIndex]}";
        }
        */

        private async void downloadButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(pathTextBox.Text))
            {
                MessageBox.Show("Please select your GTA V installation path first.",
                    "GTA V Path Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Directory.Exists(pathTextBox.Text))
            {
                MessageBox.Show("The selected GTA V path is invalid or no longer exists.",
                    "Invalid Path", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedMods.Count == 0)
            {
                MessageBox.Show("Please select at least one mod to download.",
                    "No Mods Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            
            downloadButton.Enabled = false;

            
            tabControl.SelectedIndex = 1;

            
            downloadListView.Items.Clear();

            
            downloadManager = new DownloadManager(pathTextBox.Text, availableMods);
            downloadManager.DownloadProgressChanged += DownloadManager_ProgressChanged;
            downloadManager.DownloadCompleted += DownloadManager_Completed;

            
            foreach (string modName in selectedMods)
            {
                if (availableMods.ContainsKey(modName))
                {
                    ListViewItem item = new ListViewItem(modName);
                    item.SubItems.Add("Pending");
                    item.SubItems.Add("0%");
                    downloadListView.Items.Add(item);
                }
            }

            
            foreach (string modName in selectedMods)
            {
                if (availableMods.ContainsKey(modName))
                {
                   
                    ListViewItem item = FindListViewItemByModName(downloadListView, modName);
                    if (item != null)
                    {
                        item.SubItems[1].Text = "Downloading...";
                        await downloadManager.DownloadAndInstallModAsync(modName);
                    }
                }
            }

            downloadButton.Enabled = true;

            MessageBox.Show("All selected mods have been processed.",
                "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

            var settings = Handler.Instance.Settings;
            foreach (string modName in selectedMods)
            {
                if (availableMods.ContainsKey(modName))
                {

                    bool alreadyInstalled = false;
                    foreach (var installedMod in settings.InstalledMods)
                    {
                        if (installedMod.Name == modName)
                        {
                            installedMod.Version = availableMods[modName].Version;
                            installedMod.InstallDate = DateTime.Now;
                            alreadyInstalled = true;
                            break;
                        }
                    }

                    if (!alreadyInstalled)
                    {
                        settings.InstalledMods.Add(new InstalledMod
                        {
                            Name = modName,
                            Version = availableMods[modName].Version,
                            InstallDate = DateTime.Now
                        });
                    }
                }
            }

            Handler.Instance.SaveSettings();
        }

        private void DownloadManager_ProgressChanged(object sender, DownloadProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => DownloadManager_ProgressChanged(sender, e)));
                return;
            }

            ListViewItem item = FindListViewItemByModName(downloadListView, e.ModName);
            if (item != null)
            {
                item.SubItems[2].Text = $"{e.ProgressPercentage}%";
            }
        }

        private void DownloadManager_Completed(object sender, DownloadCompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => DownloadManager_Completed(sender, e)));
                return;
            }

            ListViewItem item = FindListViewItemByModName(downloadListView, e.ModName);
            if (item != null)
            {
                item.SubItems[1].Text = e.Success ? "Completed" : "Failed";

                if (!e.Success)
                {
                    MessageBox.Show($"Failed to download {e.ModName}: {e.Message}",
                        "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (e.Success)
            {
                // we get the settings instance
                var settings = Handler.Instance.Settings;

                var installedEntry = settings.InstalledMods
                                   .FirstOrDefault(im => im.Name == e.ModName);

                if (installedEntry == null)
                {
                    installedEntry = new InstalledMod
                    {
                        Name = e.ModName,
                        Version = availableMods[e.ModName].Version,
                        InstallDate = DateTime.Now,
                        InstalledFiles = new List<string>()
                    };
                    settings.InstalledMods.Add(installedEntry);
                } else
                {
                    installedEntry.InstalledFiles.Clear();
                    installedEntry.Version = availableMods[e.ModName].Version;
                    installedEntry.InstallDate = DateTime.Now;
                }
                installedEntry.InstalledFiles.AddRange(e.InstalledFiles);

                Handler.Instance.SaveSettings();
                LoadInstalledModsTab();
            }

        }

        private ListViewItem FindListViewItemByModName(ListView listView, string modName)
        {
            foreach (ListViewItem item in listView.Items)
            {
                if (item.Text == modName)
                {
                    return item;
                }
            }
            return null;
        }

        public void btnOpenModSettings_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != modsTabPage) return;

            var checkedItems = modsCheckedListBox.CheckedItems
                .Cast<string>()
                .ToList();

            if (checkedItems.Count == 0)
            {
                MessageBox.Show(
                "Please check at least one mod in the Available Mods list before opening settings.",
                "No Mods Checked", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            string gtaPath = pathTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(gtaPath) || !Directory.Exists(gtaPath))
            {
             MessageBox.Show(
            "Invalid GTA V path. Please select your GTA V installation folder first.",
            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return;
            }

            foreach (string modName in checkedItems)
            {
                if (!availableMods.TryGetValue(modName, out ModInfo selectedMod))
                {
                    MessageBox.Show($"Couldn't find data for mod '{modName}'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    continue;
                }
                if (string.IsNullOrWhiteSpace(selectedMod.IniPath) || string.IsNullOrWhiteSpace(selectedMod.IniFileName))
                {
                    MessageBox.Show($"Mod '{modName}' doesn't contain settings file to open.", "INI Not Defined", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    continue;
                }
                string iniDirectory = selectedMod.IniPath.TrimStart('\\', '/');
                string iniFullPath = Path.Combine(gtaPath, iniDirectory, selectedMod.IniFileName);

                if (!File.Exists(iniFullPath))
                {
                    MessageBox.Show($"Settings file for '{modName}' not found\n Make sure to install the mod first", "INI Missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    continue;
                }
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "notepad.exe",
                        Arguments = $"\"{iniFullPath}\"",
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to open notepad with '{modName}': \n{ex.Message}", "Error Launching Editor" ,MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void InitializeTooltips()
        {

            toolTip.AutoPopDelay = 5000;     
            toolTip.InitialDelay = 500;      
            toolTip.ReshowDelay = 200;
            toolTip.ShowAlways = true;

            toolTip.SetToolTip(this.downloadButton, "Downloads the selected mods.");
            toolTip.SetToolTip(this.browseButton, "Select your GTA V folder.");
            toolTip.SetToolTip(this.btnOpenModSettings, "Open settings of the selected mod if available.");
            toolTip.SetToolTip(this.uninstallButton, "Uninstall mods in 'installed mods' tap");
        }

        private void linkDiscord_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var url = "https://discord.gg/yourinvite";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });

           // Clipboard.SetText(url);
           // MessageBox.Show("Link copied to clipboard!", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void uninstallButton_Click(object sender, EventArgs e)
        {
            var sel = installedListView.SelectedItems;
            if (sel.Count == 0)
            {
                MessageBox.Show(
                    "Please select one or more mods to uninstall.",
                    "No Mod Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return;
            }

            // build a list of names
            var toUninstall = sel.Cast<ListViewItem>()
                                .Select(it => it.Text)
                                .ToList();

            var resp = MessageBox.Show(
                $"Are you sure you want to uninstall these mods?\n• {string.Join("\n• ", toUninstall)}",
                "Confirm Uninstall",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (resp != DialogResult.Yes) return;

            var settings = Handler.Instance.Settings;

            foreach (string modName in toUninstall)
            {
                var entry = settings.InstalledMods.FirstOrDefault(im => im.Name == modName);
                if (entry == null) continue;

                foreach (var file in entry.InstalledFiles)
                {
                    try { if (File.Exists(file)) File.Delete(file); }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to delete {Path.GetFileName(file)}:\n{ex.Message}",
                                        "Uninstall Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                var mod = availableMods[modName];
                string root = Path.Combine(
                    pathTextBox.Text,
                    mod.InstallPath.TrimStart('/', '\\').Replace('/', '\\')
                );
                DeleteEmptyDirectoriesRecursively(root);

                settings.InstalledMods.Remove(entry);
            }


            Handler.Instance.SaveSettings();


            LoadInstalledModsTab();

            MessageBox.Show(
                $"Uninstalled: {string.Join(", ", toUninstall)}",
                "Uninstall Complete",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void DeleteEmptyDirectoriesRecursively(string directory)
        {

            if (!Directory.Exists(directory)) return;


            foreach (var dir in Directory.GetDirectories(directory))
                DeleteEmptyDirectoriesRecursively(dir);

            if (Directory.GetFileSystemEntries(directory).Length == 0)
                Directory.Delete(directory, false);
        }

        private void installedListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool hasSelection = installedListView.SelectedItems.Count > 0;

            btnOpenModSettings.Enabled = hasSelection;

            uninstallButton.Enabled = hasSelection;
        }

    }

}