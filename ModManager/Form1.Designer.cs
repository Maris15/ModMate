using System.Windows.Forms;

namespace ModManager
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.topPanel = new System.Windows.Forms.Panel();
            this.pathLabel = new System.Windows.Forms.Label();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.categoryListView = new System.Windows.Forms.ListView();
            this.categoriesColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.modsTabPage = new System.Windows.Forms.TabPage();
            this.modsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.modDetailsPanel = new System.Windows.Forms.Panel();
            this.modTitleLabel = new System.Windows.Forms.Label();
            this.modAuthorLabel = new System.Windows.Forms.Label();
            this.modDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.downloadsTabPage = new System.Windows.Forms.TabPage();
            this.downloadListView = new System.Windows.Forms.ListView();
            this.modNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.progressColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.downloadButton = new System.Windows.Forms.Button();

            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.topPanel.Controls.Add(this.browseButton);
            this.topPanel.Controls.Add(this.pathTextBox);
            this.topPanel.Controls.Add(this.pathLabel);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Padding = new System.Windows.Forms.Padding(10);
            this.topPanel.Size = new System.Drawing.Size(900, 60);
            this.topPanel.TabIndex = 0;
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.Location = new System.Drawing.Point(10, 20);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(74, 13);
            this.pathLabel.TabIndex = 0;
            this.pathLabel.Text = "GTA V Path:";
            // 
            // pathTextBox
            // 
            this.pathTextBox.Location = new System.Drawing.Point(90, 17);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(500, 20);
            this.pathTextBox.TabIndex = 1;
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(600, 17);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(80, 23);
            this.browseButton.TabIndex = 2;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 60);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.categoryListView);
            this.splitContainer.Panel1MinSize = 180;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tabControl);
            this.splitContainer.Size = new System.Drawing.Size(900, 490);
            this.splitContainer.SplitterDistance = 200;
            this.splitContainer.TabIndex = 1;
            // 
            // categoryListView
            // 
            this.categoryListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.categoriesColumn});
            this.categoryListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categoryListView.FullRowSelect = true;
            this.categoryListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.categoryListView.Location = new System.Drawing.Point(0, 0);
            this.categoryListView.Name = "categoryListView";
            this.categoryListView.Size = new System.Drawing.Size(198, 488);
            this.categoryListView.TabIndex = 0;
            this.categoryListView.UseCompatibleStateImageBehavior = false;
            this.categoryListView.View = System.Windows.Forms.View.Details;
            this.categoryListView.SelectedIndexChanged += new System.EventHandler(this.categoryListView_SelectedIndexChanged);
            // 
            // categoriesColumn
            // 
            this.categoriesColumn.Text = "Categories";
            this.categoriesColumn.Width = 180;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.modsTabPage);
            this.tabControl.Controls.Add(this.downloadsTabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(698, 488);
            this.tabControl.TabIndex = 0;
            // 
            // modsTabPage
            // 
            this.modsTabPage.Controls.Add(this.modDetailsPanel);
            this.modsTabPage.Controls.Add(this.modsCheckedListBox);
            this.modsTabPage.Location = new System.Drawing.Point(4, 22);
            this.modsTabPage.Name = "modsTabPage";
            this.modsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.modsTabPage.Size = new System.Drawing.Size(690, 462);
            this.modsTabPage.TabIndex = 0;
            this.modsTabPage.Text = "Available Mods";
            this.modsTabPage.UseVisualStyleBackColor = true;
            // 
            // modsCheckedListBox
            // 
            this.modsCheckedListBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.modsCheckedListBox.FormattingEnabled = true;
            this.modsCheckedListBox.Location = new System.Drawing.Point(3, 3);
            this.modsCheckedListBox.Name = "modsCheckedListBox";
            this.modsCheckedListBox.Size = new System.Drawing.Size(200, 456);
            this.modsCheckedListBox.TabIndex = 0;
            this.modsCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.modsCheckedListBox_ItemCheck);
            this.modsCheckedListBox.SelectedIndexChanged += new System.EventHandler(this.modsCheckedListBox_SelectedIndexChanged);
            // 
            // modDetailsPanel
            // 
            this.modDetailsPanel.Controls.Add(this.modDescriptionTextBox);
            this.modDetailsPanel.Controls.Add(this.modAuthorLabel);
            this.modDetailsPanel.Controls.Add(this.modTitleLabel);
            this.modDetailsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modDetailsPanel.Location = new System.Drawing.Point(203, 3);
            this.modDetailsPanel.Name = "modDetailsPanel";
            this.modDetailsPanel.Padding = new System.Windows.Forms.Padding(10);
            this.modDetailsPanel.Size = new System.Drawing.Size(484, 456);
            this.modDetailsPanel.TabIndex = 1;
            // 
            // modTitleLabel
            // 
            this.modTitleLabel.AutoSize = true;
            this.modTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modTitleLabel.Location = new System.Drawing.Point(10, 10);
            this.modTitleLabel.Name = "modTitleLabel";
            this.modTitleLabel.Size = new System.Drawing.Size(0, 20);
            this.modTitleLabel.TabIndex = 0;
            // 
            // modAuthorLabel
            // 
            this.modAuthorLabel.AutoSize = true;
            this.modAuthorLabel.Location = new System.Drawing.Point(10, 40);
            this.modAuthorLabel.Name = "modAuthorLabel";
            this.modAuthorLabel.Size = new System.Drawing.Size(0, 13);
            this.modAuthorLabel.TabIndex = 1;
            // 
            // modDescriptionTextBox
            // 
            this.modDescriptionTextBox.Location = new System.Drawing.Point(10, 70);
            this.modDescriptionTextBox.Multiline = true;
            this.modDescriptionTextBox.Name = "modDescriptionTextBox";
            this.modDescriptionTextBox.ReadOnly = true;
            this.modDescriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.modDescriptionTextBox.Size = new System.Drawing.Size(450, 200);
            this.modDescriptionTextBox.TabIndex = 2;
            // 
            // downloadsTabPage
            // 
            this.downloadsTabPage.Controls.Add(this.downloadListView);
            this.downloadsTabPage.Location = new System.Drawing.Point(4, 22);
            this.downloadsTabPage.Name = "downloadsTabPage";
            this.downloadsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.downloadsTabPage.Size = new System.Drawing.Size(690, 462);
            this.downloadsTabPage.TabIndex = 1;
            this.downloadsTabPage.Text = "Downloads";
            this.downloadsTabPage.UseVisualStyleBackColor = true;
            // 
            // downloadListView
            // 
            this.downloadListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.modNameColumn,
            this.statusColumn,
            this.progressColumn});
            this.downloadListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.downloadListView.FullRowSelect = true;
            this.downloadListView.Location = new System.Drawing.Point(3, 3);
            this.downloadListView.Name = "downloadListView";
            this.downloadListView.Size = new System.Drawing.Size(684, 456);
            this.downloadListView.TabIndex = 0;
            this.downloadListView.UseCompatibleStateImageBehavior = false;
            this.downloadListView.View = System.Windows.Forms.View.Details;
            // 
            // modNameColumn
            // 
            this.modNameColumn.Text = "Mod";
            this.modNameColumn.Width = 300;
            // 
            // statusColumn
            // 
            this.statusColumn.Text = "Status";
            this.statusColumn.Width = 180;
            // 
            // progressColumn
            // 
            this.progressColumn.Text = "Progress";
            this.progressColumn.Width = 180;
            // 
            // bottomPanel
            // 
            this.bottomPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.bottomPanel.Controls.Add(this.downloadButton);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 550);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(900, 50);
            this.bottomPanel.TabIndex = 2;
            // 
            // downloadButton
            // 
            this.downloadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadButton.Location = new System.Drawing.Point(700, 10);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(180, 30);
            this.downloadButton.TabIndex = 0;
            this.downloadButton.Text = "Download Selected Mods";
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.bottomPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mod Manager"; // TODO: name
            this.ResumeLayout(false);
            //
            // OpenModSettingsButton
            //
            this.btnOpenModSettings = new System.Windows.Forms.Button();
            this.btnOpenModSettings.Location = new System.Drawing.Point(10, 280);
            this.btnOpenModSettings.Name = "btnOpenModSettings";
            this.btnOpenModSettings.Size = new System.Drawing.Size(120, 23);
            this.btnOpenModSettings.TabIndex = 3;
            this.btnOpenModSettings.Text = "Open Mod Settings";
            this.btnOpenModSettings.UseVisualStyleBackColor = true;
            this.btnOpenModSettings.Enabled = true;
            this.btnOpenModSettings.Click += new System.EventHandler(this.btnOpenModSettings_Click);
            this.modDetailsPanel.Controls.Add(this.btnOpenModSettings);
            this.ResumeLayout(false);


            this.infoTabPage = new System.Windows.Forms.TabPage();
            this.infoLabel = new System.Windows.Forms.Label();
            this.linkDiscord = new System.Windows.Forms.LinkLabel();

            // 
            // infoLabel
            // 
            this.infoLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.infoLabel.AutoSize = false;
            this.infoLabel.Height = 380;
            this.infoLabel.Padding = new System.Windows.Forms.Padding(10);
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.infoLabel.Text =
                "ModMate Manager v1.0\n" +
                "By: Maris\n\n" +
                "Welcome to the Mod Manager! This program simplifies mod management for GTA V:\n" +
                "• Browse to your GTA V installation path\n" +
                "• Select a category on the left\n" +
                "• Check the mods you want and click Download\n" +
                "• Use “Open Mod Settings” to adjust downloaded mod INI files, if present\n\n" +
                "More info:\n" +
                "- Dependencies: you’ll get a warning if there is missing requirements unchecked for a specific mod. IF you already HAVE the required mod IGNORE the warning\n" +
                "- To play LSPDFR or other RageHook mods, launch RagePluginHook.exe from your GTAV folder. make a shortcut to your desktop for easier access\n" +
                "- READ each mod’s description before downloading.\n" +
                "- Models that uploaded is pre-packaged, for example vehicle models ( replaced or add-on ) rpf are packages it will auto install them in mods folder! " +
                " now other vehicle models you want to install it will be overwritten therefore if you wish the models NOT to be overwritten uninstall them first then download the new vehicle models\n" +
                "- if you want a custom models ( you choose what to use ) etc... then use openIV" +
                "- Mods Update category is only updating mods, meaning if there is a new updates DO NOT download full version JUST the update in the Mods update category\n\n" +
                "I AM NOT RESPONSIBLE IF ANYTHING HAPPENED TO YOUR GAME! I MADE THIS DOCUEMNTARY FOR YOU TO KNOW THE BASIC STUFF\n\n" +
                "Any bugs? Leave a report on Discord server:\n";

            // 
            // linkDiscord
            // 
            this.linkDiscord.AutoSize = true;
            this.linkDiscord.Location = new System.Drawing.Point(10, this.infoLabel.Bottom + 5);
            this.linkDiscord.Name = "linkDiscord";
            this.linkDiscord.Text = "https://discord.gg/yourInviteCode";
            this.linkDiscord.LinkArea = new System.Windows.Forms.LinkArea(0, this.linkDiscord.Text.Length);
            this.linkDiscord.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDiscord_LinkClicked);

            // 
            // infoTabPage
            // 
            this.infoTabPage.Controls.Add(this.linkDiscord);
            this.infoTabPage.Controls.Add(this.infoLabel);
            this.infoTabPage.Location = new System.Drawing.Point(4, 22);
            this.infoTabPage.Name = "infoTabPage";
            this.infoTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.infoTabPage.Size = new System.Drawing.Size(690, 462);
            this.infoTabPage.TabIndex = 2;
            this.infoTabPage.Text = "Documentary";
            this.infoTabPage.UseVisualStyleBackColor = true;

            // Add to TabControl
            this.tabControl.Controls.Add(this.infoTabPage);

            //
            // discord link
            //

            this.linkDiscord = new System.Windows.Forms.LinkLabel();
            this.linkDiscord.AutoSize = true;
            this.linkDiscord.Location = new System.Drawing.Point(10, 200);   // adjust Y as needed
            this.linkDiscord.Name = "linkDiscord";
            this.linkDiscord.Size = new System.Drawing.Size(300, 13);
            this.linkDiscord.TabIndex = 4;
            this.linkDiscord.TabStop = true;
            this.linkDiscord.Text = "Join my Discord server: https://discord.gg/yourinvite";

            this.linkDiscord.LinkColor = System.Drawing.Color.Blue;
            this.linkDiscord.VisitedLinkColor = System.Drawing.Color.Purple;
            this.linkDiscord.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDiscord_LinkClicked);


            // 
            // uninstall button
            //

            this.uninstallButton = new System.Windows.Forms.Button();
            this.uninstallButton.Anchor = ((System.Windows.Forms.AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.uninstallButton.Location = new System.Drawing.Point(500, 10);
            this.uninstallButton.Name = "uninstallButton";
            this.uninstallButton.Size = new System.Drawing.Size(180, 30);
            this.uninstallButton.TabIndex = 1;
            this.uninstallButton.Text = "Uninstall Selected Mods";
            this.uninstallButton.UseVisualStyleBackColor = true;
            this.uninstallButton.Click += new System.EventHandler(this.uninstallButton_Click);
            this.bottomPanel.Controls.Add(this.uninstallButton);

            //
            // installed mods 
            //

            this.installedTabPage = new System.Windows.Forms.TabPage();
            this.installedListView = new System.Windows.Forms.ListView();
            this.installedModColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.installedVersionColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));

            // 
            // 
            this.installedListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
    this.installedModColumn,
    this.installedVersionColumn});
            this.installedListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.installedListView.FullRowSelect = true;
            this.installedListView.HideSelection = false;
            this.installedListView.Location = new System.Drawing.Point(3, 3);
            this.installedListView.Name = "installedListView";
            this.installedListView.Size = new System.Drawing.Size(684, 456);
            this.installedListView.TabIndex = 0;
            this.installedListView.UseCompatibleStateImageBehavior = false;
            this.installedListView.View = System.Windows.Forms.View.Details;
            this.installedListView.SelectedIndexChanged += new System.EventHandler(this.installedListView_SelectedIndexChanged);

            // 
            // 
            this.installedModColumn.Text = "Mod";
            this.installedModColumn.Width = 300;
            // 
            this.installedVersionColumn.Text = "Version";
            this.installedVersionColumn.Width = 120;

            this.installedTabPage.Controls.Add(this.installedListView);
            this.installedTabPage.Location = new System.Drawing.Point(4, 22);
            this.installedTabPage.Name = "installedTabPage";
            this.installedTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.installedTabPage.Size = new System.Drawing.Size(690, 462);
            this.installedTabPage.TabIndex = 3;        // next index
            this.installedTabPage.Text = "Installed Mods";
            this.installedTabPage.UseVisualStyleBackColor = true;

            this.tabControl.Controls.Add(this.installedTabPage);


        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Label pathLabel;
        private System.Windows.Forms.TextBox pathTextBox;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ListView categoryListView;
        private System.Windows.Forms.ColumnHeader categoriesColumn;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage modsTabPage;
        private System.Windows.Forms.TabPage downloadsTabPage;
        private System.Windows.Forms.CheckedListBox modsCheckedListBox;
        private System.Windows.Forms.Panel modDetailsPanel;
        private System.Windows.Forms.Label modTitleLabel;
        private System.Windows.Forms.Label modAuthorLabel;
        private System.Windows.Forms.TextBox modDescriptionTextBox;
        private System.Windows.Forms.ListView downloadListView;
        private System.Windows.Forms.ColumnHeader modNameColumn;
        private System.Windows.Forms.ColumnHeader statusColumn;
        private System.Windows.Forms.ColumnHeader progressColumn;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Button downloadButton;
        private System.Windows.Forms.Button btnOpenModSettings;
        private System.Windows.Forms.TabPage infoTabPage;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.LinkLabel linkDiscord;
        private System.Windows.Forms.Button uninstallButton;

        private System.Windows.Forms.TabPage installedTabPage;
        private System.Windows.Forms.ListView installedListView;
        private System.Windows.Forms.ColumnHeader installedModColumn;
        private System.Windows.Forms.ColumnHeader installedVersionColumn;

    }
}