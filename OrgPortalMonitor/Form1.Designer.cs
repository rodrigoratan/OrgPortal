namespace OrgPortalMonitor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.txtOrgPortalUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkAutoStart = new System.Windows.Forms.CheckBox();
            this.txtPackageFamilyName = new System.Windows.Forms.TextBox();
            this.lblPackageFamilyName = new System.Windows.Forms.Label();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.txtLogOutput = new System.Windows.Forms.TextBox();
            this.tabServerApps = new System.Windows.Forms.TabPage();
            this.dgvServerApps = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServerVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Installed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Install = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tabInstalled = new System.Windows.Forms.TabPage();
            this.dgvInstalled = new System.Windows.Forms.DataGridView();
            this.DisplayName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InstalledVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InstallMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PackageFamilyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabMultiple = new System.Windows.Forms.TabPage();
            this.dgvPackages = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.clPackageFamilyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.monitorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoConnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.appsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshInstalledListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unlockDeviceForSideloadingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getDeveloperLicenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.v54ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.fileSystemWatcher2 = new System.IO.FileSystemWatcher();
            this.extraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processExistingAppRequestsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processExistingAutoInstallAndAutoUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.tabLog.SuspendLayout();
            this.tabServerApps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServerApps)).BeginInit();
            this.tabInstalled.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInstalled)).BeginInit();
            this.tabMultiple.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackages)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher2)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "OrgPortal";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.BalloonTipClicked += new System.EventHandler(this.notifyIcon1_BalloonTipClicked);
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.Filter = "*.rt2win";
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 60000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(624, 441);
            this.panel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabSettings);
            this.tabControl1.Controls.Add(this.tabLog);
            this.tabControl1.Controls.Add(this.tabServerApps);
            this.tabControl1.Controls.Add(this.tabInstalled);
            this.tabControl1.Controls.Add(this.tabMultiple);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(624, 417);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 2;
            this.tabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl1_Selecting);
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.btnStartStop);
            this.tabSettings.Controls.Add(this.txtOrgPortalUrl);
            this.tabSettings.Controls.Add(this.label1);
            this.tabSettings.Controls.Add(this.chkAutoStart);
            this.tabSettings.Controls.Add(this.txtPackageFamilyName);
            this.tabSettings.Controls.Add(this.lblPackageFamilyName);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(616, 391);
            this.tabSettings.TabIndex = 0;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // btnStartStop
            // 
            this.btnStartStop.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnStartStop.Location = new System.Drawing.Point(19, 163);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(284, 23);
            this.btnStartStop.TabIndex = 7;
            this.btnStartStop.Text = "Connect and Start Monitor";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtOrgPortalUrl
            // 
            this.txtOrgPortalUrl.Location = new System.Drawing.Point(19, 38);
            this.txtOrgPortalUrl.Name = "txtOrgPortalUrl";
            this.txtOrgPortalUrl.Size = new System.Drawing.Size(284, 20);
            this.txtOrgPortalUrl.TabIndex = 5;
            this.txtOrgPortalUrl.TextChanged += new System.EventHandler(this.txtOrgPortalUrl_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "OrgPortal API Url";
            // 
            // chkAutoStart
            // 
            this.chkAutoStart.AutoSize = true;
            this.chkAutoStart.Location = new System.Drawing.Point(19, 131);
            this.chkAutoStart.Name = "chkAutoStart";
            this.chkAutoStart.Size = new System.Drawing.Size(175, 17);
            this.chkAutoStart.TabIndex = 2;
            this.chkAutoStart.Text = "Auto Connect and Start Monitor";
            this.chkAutoStart.UseVisualStyleBackColor = true;
            this.chkAutoStart.CheckedChanged += new System.EventHandler(this.chkAutoStart_CheckedChanged);
            // 
            // txtPackageFamilyName
            // 
            this.txtPackageFamilyName.Location = new System.Drawing.Point(19, 93);
            this.txtPackageFamilyName.Name = "txtPackageFamilyName";
            this.txtPackageFamilyName.ReadOnly = true;
            this.txtPackageFamilyName.Size = new System.Drawing.Size(284, 20);
            this.txtPackageFamilyName.TabIndex = 1;
            this.txtPackageFamilyName.Text = "OrgPortal_m64ba5zfsemg0";
            this.txtPackageFamilyName.TextChanged += new System.EventHandler(this.txtPackageFamilyName_TextChanged);
            // 
            // lblPackageFamilyName
            // 
            this.lblPackageFamilyName.AutoSize = true;
            this.lblPackageFamilyName.Location = new System.Drawing.Point(16, 77);
            this.lblPackageFamilyName.Name = "lblPackageFamilyName";
            this.lblPackageFamilyName.Size = new System.Drawing.Size(394, 13);
            this.lblPackageFamilyName.TabIndex = 0;
            this.lblPackageFamilyName.Text = "OrgPortal PackageFamilyName (FileWatcher will monitor this Package TempState)";
            // 
            // tabLog
            // 
            this.tabLog.Controls.Add(this.txtLogOutput);
            this.tabLog.Location = new System.Drawing.Point(4, 22);
            this.tabLog.Name = "tabLog";
            this.tabLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabLog.Size = new System.Drawing.Size(616, 391);
            this.tabLog.TabIndex = 1;
            this.tabLog.Text = "Log";
            this.tabLog.UseVisualStyleBackColor = true;
            // 
            // txtLogOutput
            // 
            this.txtLogOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogOutput.Location = new System.Drawing.Point(3, 3);
            this.txtLogOutput.Multiline = true;
            this.txtLogOutput.Name = "txtLogOutput";
            this.txtLogOutput.ReadOnly = true;
            this.txtLogOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLogOutput.Size = new System.Drawing.Size(610, 385);
            this.txtLogOutput.TabIndex = 2;
            // 
            // tabServerApps
            // 
            this.tabServerApps.Controls.Add(this.dgvServerApps);
            this.tabServerApps.Location = new System.Drawing.Point(4, 22);
            this.tabServerApps.Name = "tabServerApps";
            this.tabServerApps.Padding = new System.Windows.Forms.Padding(3);
            this.tabServerApps.Size = new System.Drawing.Size(616, 391);
            this.tabServerApps.TabIndex = 4;
            this.tabServerApps.Text = "Server Apps";
            this.tabServerApps.UseVisualStyleBackColor = true;
            // 
            // dgvServerApps
            // 
            this.dgvServerApps.AllowUserToAddRows = false;
            this.dgvServerApps.AllowUserToDeleteRows = false;
            this.dgvServerApps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvServerApps.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.ServerVersion,
            this.Installed,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.Install});
            this.dgvServerApps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvServerApps.Location = new System.Drawing.Point(3, 3);
            this.dgvServerApps.Name = "dgvServerApps";
            this.dgvServerApps.ReadOnly = true;
            this.dgvServerApps.Size = new System.Drawing.Size(610, 385);
            this.dgvServerApps.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "App Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // ServerVersion
            // 
            this.ServerVersion.HeaderText = "Version";
            this.ServerVersion.Name = "ServerVersion";
            this.ServerVersion.ReadOnly = true;
            this.ServerVersion.Width = 70;
            // 
            // Installed
            // 
            this.Installed.HeaderText = "Is Installed";
            this.Installed.Name = "Installed";
            this.Installed.ReadOnly = true;
            this.Installed.Width = 70;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Install Mode";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 70;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.HeaderText = "Package Family Name";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // Install
            // 
            this.Install.HeaderText = "Install";
            this.Install.Name = "Install";
            this.Install.ReadOnly = true;
            this.Install.Text = "Install App";
            this.Install.UseColumnTextForButtonValue = true;
            // 
            // tabInstalled
            // 
            this.tabInstalled.Controls.Add(this.dgvInstalled);
            this.tabInstalled.Location = new System.Drawing.Point(4, 22);
            this.tabInstalled.Name = "tabInstalled";
            this.tabInstalled.Padding = new System.Windows.Forms.Padding(3);
            this.tabInstalled.Size = new System.Drawing.Size(616, 391);
            this.tabInstalled.TabIndex = 2;
            this.tabInstalled.Text = "Installed Apps";
            this.tabInstalled.UseVisualStyleBackColor = true;
            // 
            // dgvInstalled
            // 
            this.dgvInstalled.AllowUserToAddRows = false;
            this.dgvInstalled.AllowUserToDeleteRows = false;
            this.dgvInstalled.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInstalled.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DisplayName,
            this.InstalledVersion,
            this.InstallMode,
            this.PackageFamilyName});
            this.dgvInstalled.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInstalled.Location = new System.Drawing.Point(3, 3);
            this.dgvInstalled.Name = "dgvInstalled";
            this.dgvInstalled.ReadOnly = true;
            this.dgvInstalled.Size = new System.Drawing.Size(610, 385);
            this.dgvInstalled.TabIndex = 0;
            // 
            // DisplayName
            // 
            this.DisplayName.HeaderText = "App Name";
            this.DisplayName.Name = "DisplayName";
            this.DisplayName.ReadOnly = true;
            // 
            // InstalledVersion
            // 
            this.InstalledVersion.HeaderText = "Version";
            this.InstalledVersion.Name = "InstalledVersion";
            this.InstalledVersion.ReadOnly = true;
            // 
            // InstallMode
            // 
            this.InstallMode.HeaderText = "Install Mode";
            this.InstallMode.Name = "InstallMode";
            this.InstallMode.ReadOnly = true;
            // 
            // PackageFamilyName
            // 
            this.PackageFamilyName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PackageFamilyName.HeaderText = "Package Family Name";
            this.PackageFamilyName.Name = "PackageFamilyName";
            this.PackageFamilyName.ReadOnly = true;
            // 
            // tabMultiple
            // 
            this.tabMultiple.Controls.Add(this.dgvPackages);
            this.tabMultiple.Location = new System.Drawing.Point(4, 22);
            this.tabMultiple.Name = "tabMultiple";
            this.tabMultiple.Size = new System.Drawing.Size(616, 391);
            this.tabMultiple.TabIndex = 3;
            this.tabMultiple.Text = "Multiple Packages (Beta)";
            this.tabMultiple.UseVisualStyleBackColor = true;
            // 
            // dgvPackages
            // 
            this.dgvPackages.AllowUserToOrderColumns = true;
            this.dgvPackages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPackages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.clPackageFamilyName});
            this.dgvPackages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPackages.Location = new System.Drawing.Point(0, 0);
            this.dgvPackages.Name = "dgvPackages";
            this.dgvPackages.Size = new System.Drawing.Size(616, 391);
            this.dgvPackages.TabIndex = 1;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewCheckBoxColumn1.HeaderText = "Auto Update";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Width = 80;
            // 
            // clPackageFamilyName
            // 
            this.clPackageFamilyName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.clPackageFamilyName.HeaderText = "Package Family Name";
            this.clPackageFamilyName.Name = "clPackageFamilyName";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.monitorToolStripMenuItem,
            this.appsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(624, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // monitorToolStripMenuItem
            // 
            this.monitorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoConnectToolStripMenuItem,
            this.startToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.monitorToolStripMenuItem.Name = "monitorToolStripMenuItem";
            this.monitorToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.monitorToolStripMenuItem.Text = "Monitor";
            // 
            // autoConnectToolStripMenuItem
            // 
            this.autoConnectToolStripMenuItem.CheckOnClick = true;
            this.autoConnectToolStripMenuItem.Name = "autoConnectToolStripMenuItem";
            this.autoConnectToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.autoConnectToolStripMenuItem.Text = "Auto Connect";
            this.autoConnectToolStripMenuItem.CheckedChanged += new System.EventHandler(this.autoConnectToolStripMenuItem_CheckedChanged);
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.startToolStripMenuItem.Text = "Connect and Start";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // appsToolStripMenuItem
            // 
            this.appsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshInstalledListToolStripMenuItem,
            this.unlockDeviceForSideloadingToolStripMenuItem,
            this.getDeveloperLicenseToolStripMenuItem,
            this.extraToolStripMenuItem});
            this.appsToolStripMenuItem.Name = "appsToolStripMenuItem";
            this.appsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.appsToolStripMenuItem.Text = "Apps";
            // 
            // refreshInstalledListToolStripMenuItem
            // 
            this.refreshInstalledListToolStripMenuItem.Name = "refreshInstalledListToolStripMenuItem";
            this.refreshInstalledListToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.refreshInstalledListToolStripMenuItem.Text = "Refresh Installed List";
            // 
            // unlockDeviceForSideloadingToolStripMenuItem
            // 
            this.unlockDeviceForSideloadingToolStripMenuItem.Name = "unlockDeviceForSideloadingToolStripMenuItem";
            this.unlockDeviceForSideloadingToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.unlockDeviceForSideloadingToolStripMenuItem.Text = "Unlock device for side-loading";
            // 
            // getDeveloperLicenseToolStripMenuItem
            // 
            this.getDeveloperLicenseToolStripMenuItem.Name = "getDeveloperLicenseToolStripMenuItem";
            this.getDeveloperLicenseToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.getDeveloperLicenseToolStripMenuItem.Text = "Get developer license";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.v54ToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // v54ToolStripMenuItem
            // 
            this.v54ToolStripMenuItem.Name = "v54ToolStripMenuItem";
            this.v54ToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.v54ToolStripMenuItem.Text = "OrgPortal 1.x.61";
            // 
            // fileSystemWatcher2
            // 
            this.fileSystemWatcher2.EnableRaisingEvents = true;
            this.fileSystemWatcher2.Filter = "*.rt2win";
            this.fileSystemWatcher2.SynchronizingObject = this;
            // 
            // extraToolStripMenuItem
            // 
            this.extraToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.processExistingAppRequestsToolStripMenuItem,
            this.processExistingAutoInstallAndAutoUpdateToolStripMenuItem});
            this.extraToolStripMenuItem.Name = "extraToolStripMenuItem";
            this.extraToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.extraToolStripMenuItem.Text = "Extra";
            // 
            // processExistingAppRequestsToolStripMenuItem
            // 
            this.processExistingAppRequestsToolStripMenuItem.Name = "processExistingAppRequestsToolStripMenuItem";
            this.processExistingAppRequestsToolStripMenuItem.Size = new System.Drawing.Size(313, 22);
            this.processExistingAppRequestsToolStripMenuItem.Text = "Process Existing App Requests";
            // 
            // processExistingAutoInstallAndAutoUpdateToolStripMenuItem
            // 
            this.processExistingAutoInstallAndAutoUpdateToolStripMenuItem.Name = "processExistingAutoInstallAndAutoUpdateToolStripMenuItem";
            this.processExistingAutoInstallAndAutoUpdateToolStripMenuItem.Size = new System.Drawing.Size(313, 22);
            this.processExistingAutoInstallAndAutoUpdateToolStripMenuItem.Text = "Process Existing Auto Install and Auto Update";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 300);
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Zollie R2 - OrgPortal Monitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
            this.tabLog.ResumeLayout(false);
            this.tabLog.PerformLayout();
            this.tabServerApps.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvServerApps)).EndInit();
            this.tabInstalled.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInstalled)).EndInit();
            this.tabMultiple.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackages)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher2)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.NotifyIcon notifyIcon1;
    private System.IO.FileSystemWatcher fileSystemWatcher1;
    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabSettings;
    private System.Windows.Forms.Button btnStartStop;
    private System.Windows.Forms.TextBox txtOrgPortalUrl;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckBox chkAutoStart;
    private System.Windows.Forms.TextBox txtPackageFamilyName;
    private System.Windows.Forms.Label lblPackageFamilyName;
    private System.Windows.Forms.TabPage tabLog;
    private System.Windows.Forms.TextBox txtLogOutput;
    private System.Windows.Forms.TabPage tabServerApps;
    private System.Windows.Forms.DataGridView dgvServerApps;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private System.Windows.Forms.DataGridViewTextBoxColumn ServerVersion;
    private System.Windows.Forms.DataGridViewTextBoxColumn Installed;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private System.Windows.Forms.DataGridViewButtonColumn Install;
    private System.Windows.Forms.TabPage tabInstalled;
    private System.Windows.Forms.DataGridView dgvInstalled;
    private System.Windows.Forms.DataGridViewTextBoxColumn DisplayName;
    private System.Windows.Forms.DataGridViewTextBoxColumn InstalledVersion;
    private System.Windows.Forms.DataGridViewTextBoxColumn InstallMode;
    private System.Windows.Forms.DataGridViewTextBoxColumn PackageFamilyName;
    private System.Windows.Forms.TabPage tabMultiple;
    private System.Windows.Forms.DataGridView dgvPackages;
    private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
    private System.Windows.Forms.DataGridViewTextBoxColumn clPackageFamilyName;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem monitorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem appsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem refreshInstalledListToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem unlockDeviceForSideloadingToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem getDeveloperLicenseToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem autoConnectToolStripMenuItem;
    private System.ComponentModel.BackgroundWorker backgroundWorker1;
    private System.Windows.Forms.ToolStripMenuItem v54ToolStripMenuItem;
    private System.IO.FileSystemWatcher fileSystemWatcher2;
    private System.Windows.Forms.ToolStripMenuItem extraToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem processExistingAppRequestsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem processExistingAutoInstallAndAutoUpdateToolStripMenuItem;
  }
}

