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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle36 = new System.Windows.Forms.DataGridViewCellStyle();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.requireDevLicense = new System.Windows.Forms.CheckBox();
            this.monitorApps = new System.Windows.Forms.GroupBox();
            this.monitorInstalledApps = new System.Windows.Forms.CheckBox();
            this.chkProcessExistingAppRequests = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripMainStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusAdicional = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtAutoInstallTimer = new System.Windows.Forms.TextBox();
            this.btnInstallUpdates = new System.Windows.Forms.Button();
            this.chkAutoInstall = new System.Windows.Forms.CheckBox();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.txtOrgPortalUrl = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkAutoStart = new System.Windows.Forms.CheckBox();
            this.txtPackageFamilyName = new System.Windows.Forms.TextBox();
            this.lblPackageFamilyName = new System.Windows.Forms.Label();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.txtLogOutput = new System.Windows.Forms.TextBox();
            this.tabAllServerApps = new System.Windows.Forms.TabPage();
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
            this.UpdateAvailable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.InstallMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PackageName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Uninstall = new System.Windows.Forms.DataGridViewButtonColumn();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.monitorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoConnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoInstallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.monitorInstalledAppsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showThisUIOnLoadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.appsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshInstalledListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unlockDeviceForSideloadingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verifyDeveloperLicenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registerDeveloperLicenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unregisterDeveloperLicenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OrgPortalWebToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.fileSystemWatcher2 = new System.IO.FileSystemWatcher();
            this.tabDistinctServerApps = new System.Windows.Forms.TabPage();
            this.dgvDistinctServerApps = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewButtonColumn1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.LicenseInfoDisplay = new System.Windows.Forms.Label();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.monitorApps.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabLog.SuspendLayout();
            this.tabAllServerApps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServerApps)).BeginInit();
            this.tabInstalled.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInstalled)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher2)).BeginInit();
            this.tabDistinctServerApps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDistinctServerApps)).BeginInit();
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
            this.panel1.Size = new System.Drawing.Size(744, 321);
            this.panel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabSettings);
            this.tabControl1.Controls.Add(this.tabLog);
            this.tabControl1.Controls.Add(this.tabDistinctServerApps);
            this.tabControl1.Controls.Add(this.tabAllServerApps);
            this.tabControl1.Controls.Add(this.tabInstalled);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(744, 297);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 2;
            this.tabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl1_Selecting);
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.groupBox2);
            this.tabSettings.Controls.Add(this.monitorApps);
            this.tabSettings.Controls.Add(this.statusStrip1);
            this.tabSettings.Controls.Add(this.txtAutoInstallTimer);
            this.tabSettings.Controls.Add(this.btnInstallUpdates);
            this.tabSettings.Controls.Add(this.chkAutoInstall);
            this.tabSettings.Controls.Add(this.btnStartStop);
            this.tabSettings.Controls.Add(this.txtOrgPortalUrl);
            this.tabSettings.Controls.Add(this.label2);
            this.tabSettings.Controls.Add(this.label1);
            this.tabSettings.Controls.Add(this.chkAutoStart);
            this.tabSettings.Controls.Add(this.txtPackageFamilyName);
            this.tabSettings.Controls.Add(this.lblPackageFamilyName);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(736, 271);
            this.tabSettings.TabIndex = 0;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.requireDevLicense);
            this.groupBox2.Controls.Add(this.LicenseInfoDisplay);
            this.groupBox2.Location = new System.Drawing.Point(364, 146);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(354, 79);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "License to install apps";
            // 
            // requireDevLicense
            // 
            this.requireDevLicense.AutoSize = true;
            this.requireDevLicense.Checked = true;
            this.requireDevLicense.CheckState = System.Windows.Forms.CheckState.Checked;
            this.requireDevLicense.Location = new System.Drawing.Point(12, 23);
            this.requireDevLicense.Name = "requireDevLicense";
            this.requireDevLicense.Size = new System.Drawing.Size(221, 17);
            this.requireDevLicense.TabIndex = 13;
            this.requireDevLicense.Text = "Require developer license to deploy apps";
            this.requireDevLicense.UseVisualStyleBackColor = true;
            // 
            // monitorApps
            // 
            this.monitorApps.Controls.Add(this.monitorInstalledApps);
            this.monitorApps.Controls.Add(this.chkProcessExistingAppRequests);
            this.monitorApps.Location = new System.Drawing.Point(12, 146);
            this.monitorApps.Name = "monitorApps";
            this.monitorApps.Size = new System.Drawing.Size(334, 79);
            this.monitorApps.TabIndex = 15;
            this.monitorApps.TabStop = false;
            this.monitorApps.Text = "Monitor Apps";
            // 
            // monitorInstalledApps
            // 
            this.monitorInstalledApps.AutoSize = true;
            this.monitorInstalledApps.Checked = true;
            this.monitorInstalledApps.CheckState = System.Windows.Forms.CheckState.Checked;
            this.monitorInstalledApps.Location = new System.Drawing.Point(11, 23);
            this.monitorInstalledApps.Name = "monitorInstalledApps";
            this.monitorInstalledApps.Size = new System.Drawing.Size(315, 17);
            this.monitorInstalledApps.TabIndex = 12;
            this.monitorInstalledApps.Text = "Monitor Installed Apps for Updates Requests from themselves";
            this.monitorInstalledApps.UseVisualStyleBackColor = true;
            // 
            // chkProcessExistingAppRequests
            // 
            this.chkProcessExistingAppRequests.AutoSize = true;
            this.chkProcessExistingAppRequests.Checked = true;
            this.chkProcessExistingAppRequests.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkProcessExistingAppRequests.Location = new System.Drawing.Point(11, 53);
            this.chkProcessExistingAppRequests.Name = "chkProcessExistingAppRequests";
            this.chkProcessExistingAppRequests.Size = new System.Drawing.Size(239, 17);
            this.chkProcessExistingAppRequests.TabIndex = 14;
            this.chkProcessExistingAppRequests.Text = "Process existing not processed app requests ";
            this.chkProcessExistingAppRequests.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMainStatusLabel,
            this.toolStripStatusAdicional});
            this.statusStrip1.Location = new System.Drawing.Point(3, 246);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.Size = new System.Drawing.Size(730, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripMainStatusLabel
            // 
            this.toolStripMainStatusLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMainStatusLabel.Name = "toolStripMainStatusLabel";
            this.toolStripMainStatusLabel.Size = new System.Drawing.Size(85, 17);
            this.toolStripMainStatusLabel.Text = "Não conectado";
            // 
            // toolStripStatusAdicional
            // 
            this.toolStripStatusAdicional.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.toolStripStatusAdicional.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
            this.toolStripStatusAdicional.Name = "toolStripStatusAdicional";
            this.toolStripStatusAdicional.Size = new System.Drawing.Size(76, 17);
            this.toolStripStatusAdicional.Text = "Sem Detalhes";
            // 
            // txtAutoInstallTimer
            // 
            this.txtAutoInstallTimer.Location = new System.Drawing.Point(648, 74);
            this.txtAutoInstallTimer.Name = "txtAutoInstallTimer";
            this.txtAutoInstallTimer.Size = new System.Drawing.Size(28, 20);
            this.txtAutoInstallTimer.TabIndex = 10;
            this.txtAutoInstallTimer.Text = "10";
            this.txtAutoInstallTimer.TextChanged += new System.EventHandler(this.txtAutoInstallTimer_TextChanged);
            // 
            // btnInstallUpdates
            // 
            this.btnInstallUpdates.Location = new System.Drawing.Point(364, 98);
            this.btnInstallUpdates.Name = "btnInstallUpdates";
            this.btnInstallUpdates.Size = new System.Drawing.Size(354, 23);
            this.btnInstallUpdates.TabIndex = 9;
            this.btnInstallUpdates.Text = "Install New Updates and Required Apps";
            this.btnInstallUpdates.UseVisualStyleBackColor = true;
            this.btnInstallUpdates.Click += new System.EventHandler(this.btnInstallUpdates_Click);
            // 
            // chkAutoInstall
            // 
            this.chkAutoInstall.AutoSize = true;
            this.chkAutoInstall.Checked = true;
            this.chkAutoInstall.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoInstall.Location = new System.Drawing.Point(364, 75);
            this.chkAutoInstall.Name = "chkAutoInstall";
            this.chkAutoInstall.Size = new System.Drawing.Size(280, 17);
            this.chkAutoInstall.TabIndex = 8;
            this.chkAutoInstall.Text = "Auto Install Updates and Required Apps  * (Run every";
            this.chkAutoInstall.UseVisualStyleBackColor = true;
            // 
            // btnStartStop
            // 
            this.btnStartStop.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnStartStop.Location = new System.Drawing.Point(12, 98);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(334, 23);
            this.btnStartStop.TabIndex = 7;
            this.btnStartStop.Text = "Connect and Start Monitor";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtOrgPortalUrl
            // 
            this.txtOrgPortalUrl.Location = new System.Drawing.Point(12, 31);
            this.txtOrgPortalUrl.Name = "txtOrgPortalUrl";
            this.txtOrgPortalUrl.Size = new System.Drawing.Size(334, 20);
            this.txtOrgPortalUrl.TabIndex = 5;
            this.txtOrgPortalUrl.TextChanged += new System.EventHandler(this.txtOrgPortalUrl_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(678, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "minutes)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "OrgPortal API Url";
            // 
            // chkAutoStart
            // 
            this.chkAutoStart.AutoSize = true;
            this.chkAutoStart.Location = new System.Drawing.Point(14, 75);
            this.chkAutoStart.Name = "chkAutoStart";
            this.chkAutoStart.Size = new System.Drawing.Size(175, 17);
            this.chkAutoStart.TabIndex = 2;
            this.chkAutoStart.Text = "Auto Connect and Start Monitor";
            this.chkAutoStart.UseVisualStyleBackColor = true;
            // 
            // txtPackageFamilyName
            // 
            this.txtPackageFamilyName.Location = new System.Drawing.Point(364, 31);
            this.txtPackageFamilyName.Name = "txtPackageFamilyName";
            this.txtPackageFamilyName.ReadOnly = true;
            this.txtPackageFamilyName.Size = new System.Drawing.Size(354, 20);
            this.txtPackageFamilyName.TabIndex = 1;
            this.txtPackageFamilyName.Text = "34993Zollie.ZollieOrgPortal_mcdpzngym7t32";
            this.txtPackageFamilyName.TextChanged += new System.EventHandler(this.txtPackageFamilyName_TextChanged);
            // 
            // lblPackageFamilyName
            // 
            this.lblPackageFamilyName.AutoSize = true;
            this.lblPackageFamilyName.Location = new System.Drawing.Point(361, 15);
            this.lblPackageFamilyName.Name = "lblPackageFamilyName";
            this.lblPackageFamilyName.Size = new System.Drawing.Size(203, 13);
            this.lblPackageFamilyName.TabIndex = 0;
            this.lblPackageFamilyName.Text = "Companion OrgPortal Store App Package";
            // 
            // tabLog
            // 
            this.tabLog.Controls.Add(this.txtLogOutput);
            this.tabLog.Location = new System.Drawing.Point(4, 22);
            this.tabLog.Name = "tabLog";
            this.tabLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabLog.Size = new System.Drawing.Size(756, 291);
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
            this.txtLogOutput.Size = new System.Drawing.Size(750, 285);
            this.txtLogOutput.TabIndex = 2;
            // 
            // tabAllServerApps
            // 
            this.tabAllServerApps.Controls.Add(this.dgvServerApps);
            this.tabAllServerApps.Location = new System.Drawing.Point(4, 22);
            this.tabAllServerApps.Name = "tabAllServerApps";
            this.tabAllServerApps.Padding = new System.Windows.Forms.Padding(3);
            this.tabAllServerApps.Size = new System.Drawing.Size(736, 271);
            this.tabAllServerApps.TabIndex = 4;
            this.tabAllServerApps.Text = "All Server Apps";
            this.tabAllServerApps.UseVisualStyleBackColor = true;
            // 
            // dgvServerApps
            // 
            this.dgvServerApps.AllowUserToAddRows = false;
            this.dgvServerApps.AllowUserToDeleteRows = false;
            this.dgvServerApps.AllowUserToOrderColumns = true;
            dataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle28.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle28.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle28.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle28.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle28.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle28.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvServerApps.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle28;
            this.dgvServerApps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvServerApps.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.ServerVersion,
            this.Installed,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.Install});
            dataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle29.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle29.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle29.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle29.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle29.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle29.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvServerApps.DefaultCellStyle = dataGridViewCellStyle29;
            this.dgvServerApps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvServerApps.Location = new System.Drawing.Point(3, 3);
            this.dgvServerApps.Name = "dgvServerApps";
            this.dgvServerApps.ReadOnly = true;
            dataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle30.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle30.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle30.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle30.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle30.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvServerApps.RowHeadersDefaultCellStyle = dataGridViewCellStyle30;
            this.dgvServerApps.Size = new System.Drawing.Size(730, 265);
            this.dgvServerApps.TabIndex = 1;
            this.dgvServerApps.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvServerApps_CellContentClick);
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.FillWeight = 60F;
            this.dataGridViewTextBoxColumn2.HeaderText = "App Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // ServerVersion
            // 
            this.ServerVersion.FillWeight = 50F;
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
            this.dataGridViewTextBoxColumn3.Width = 90;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.FillWeight = 70F;
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
            this.tabInstalled.Size = new System.Drawing.Size(756, 291);
            this.tabInstalled.TabIndex = 2;
            this.tabInstalled.Text = "Installed Apps";
            this.tabInstalled.UseVisualStyleBackColor = true;
            // 
            // dgvInstalled
            // 
            this.dgvInstalled.AllowUserToAddRows = false;
            this.dgvInstalled.AllowUserToDeleteRows = false;
            dataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle31.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle31.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle31.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle31.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle31.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle31.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvInstalled.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle31;
            this.dgvInstalled.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInstalled.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DisplayName,
            this.InstalledVersion,
            this.UpdateAvailable,
            this.InstallMode,
            this.PackageName,
            this.Uninstall});
            dataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle32.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle32.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle32.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle32.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle32.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle32.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvInstalled.DefaultCellStyle = dataGridViewCellStyle32;
            this.dgvInstalled.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInstalled.Location = new System.Drawing.Point(3, 3);
            this.dgvInstalled.Name = "dgvInstalled";
            this.dgvInstalled.ReadOnly = true;
            dataGridViewCellStyle33.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle33.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle33.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle33.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle33.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle33.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle33.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvInstalled.RowHeadersDefaultCellStyle = dataGridViewCellStyle33;
            this.dgvInstalled.Size = new System.Drawing.Size(750, 285);
            this.dgvInstalled.TabIndex = 0;
            this.dgvInstalled.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInstalled_CellContentClick);
            // 
            // DisplayName
            // 
            this.DisplayName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DisplayName.FillWeight = 60F;
            this.DisplayName.HeaderText = "App Name";
            this.DisplayName.Name = "DisplayName";
            this.DisplayName.ReadOnly = true;
            // 
            // InstalledVersion
            // 
            this.InstalledVersion.FillWeight = 50F;
            this.InstalledVersion.HeaderText = "Version";
            this.InstalledVersion.Name = "InstalledVersion";
            this.InstalledVersion.ReadOnly = true;
            this.InstalledVersion.Width = 70;
            // 
            // UpdateAvailable
            // 
            this.UpdateAvailable.HeaderText = "Update Available";
            this.UpdateAvailable.Name = "UpdateAvailable";
            this.UpdateAvailable.ReadOnly = true;
            this.UpdateAvailable.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // InstallMode
            // 
            this.InstallMode.HeaderText = "Install Mode";
            this.InstallMode.Name = "InstallMode";
            this.InstallMode.ReadOnly = true;
            // 
            // PackageName
            // 
            this.PackageName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PackageName.FillWeight = 70F;
            this.PackageName.HeaderText = "Package Name";
            this.PackageName.Name = "PackageName";
            this.PackageName.ReadOnly = true;
            // 
            // Uninstall
            // 
            this.Uninstall.HeaderText = "Uninstall";
            this.Uninstall.Name = "Uninstall";
            this.Uninstall.ReadOnly = true;
            this.Uninstall.Text = "Uninstall App";
            this.Uninstall.UseColumnTextForButtonValue = true;
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
            this.menuStrip1.Size = new System.Drawing.Size(744, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // monitorToolStripMenuItem
            // 
            this.monitorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.toolStripSeparator3,
            this.autoConnectToolStripMenuItem,
            this.autoInstallToolStripMenuItem,
            this.toolStripSeparator4,
            this.monitorInstalledAppsToolStripMenuItem,
            this.toolStripSeparator2,
            this.showThisUIOnLoadToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.monitorToolStripMenuItem.Name = "monitorToolStripMenuItem";
            this.monitorToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.monitorToolStripMenuItem.Text = "Monitor";
            // 
            // autoConnectToolStripMenuItem
            // 
            this.autoConnectToolStripMenuItem.CheckOnClick = true;
            this.autoConnectToolStripMenuItem.Name = "autoConnectToolStripMenuItem";
            this.autoConnectToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.autoConnectToolStripMenuItem.Text = "Auto Connect";
            this.autoConnectToolStripMenuItem.CheckedChanged += new System.EventHandler(this.autoConnectToolStripMenuItem_CheckedChanged);
            // 
            // autoInstallToolStripMenuItem
            // 
            this.autoInstallToolStripMenuItem.CheckOnClick = true;
            this.autoInstallToolStripMenuItem.Name = "autoInstallToolStripMenuItem";
            this.autoInstallToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.autoInstallToolStripMenuItem.Text = "Auto Install";
            this.autoInstallToolStripMenuItem.CheckedChanged += new System.EventHandler(this.autoInstallToolStripMenuItem_CheckedChanged);
            // 
            // monitorInstalledAppsToolStripMenuItem
            // 
            this.monitorInstalledAppsToolStripMenuItem.CheckOnClick = true;
            this.monitorInstalledAppsToolStripMenuItem.Name = "monitorInstalledAppsToolStripMenuItem";
            this.monitorInstalledAppsToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.monitorInstalledAppsToolStripMenuItem.Text = "Monitor Installed Apps";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.startToolStripMenuItem.Text = "Connect and Start";
            // 
            // showThisUIOnLoadToolStripMenuItem
            // 
            this.showThisUIOnLoadToolStripMenuItem.CheckOnClick = true;
            this.showThisUIOnLoadToolStripMenuItem.Name = "showThisUIOnLoadToolStripMenuItem";
            this.showThisUIOnLoadToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.showThisUIOnLoadToolStripMenuItem.Text = "Show this UI On Load";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // appsToolStripMenuItem
            // 
            this.appsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshInstalledListToolStripMenuItem,
            this.unlockDeviceForSideloadingToolStripMenuItem,
            this.verifyDeveloperLicenseToolStripMenuItem,
            this.registerDeveloperLicenseToolStripMenuItem,
            this.unregisterDeveloperLicenseToolStripMenuItem});
            this.appsToolStripMenuItem.Name = "appsToolStripMenuItem";
            this.appsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.appsToolStripMenuItem.Text = "Apps";
            // 
            // refreshInstalledListToolStripMenuItem
            // 
            this.refreshInstalledListToolStripMenuItem.Name = "refreshInstalledListToolStripMenuItem";
            this.refreshInstalledListToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.refreshInstalledListToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.refreshInstalledListToolStripMenuItem.Text = "Refresh Installed List";
            // 
            // unlockDeviceForSideloadingToolStripMenuItem
            // 
            this.unlockDeviceForSideloadingToolStripMenuItem.Name = "unlockDeviceForSideloadingToolStripMenuItem";
            this.unlockDeviceForSideloadingToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.unlockDeviceForSideloadingToolStripMenuItem.Text = "Unlock device for side-loading";
            // 
            // verifyDeveloperLicenseToolStripMenuItem
            // 
            this.verifyDeveloperLicenseToolStripMenuItem.Name = "verifyDeveloperLicenseToolStripMenuItem";
            this.verifyDeveloperLicenseToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.verifyDeveloperLicenseToolStripMenuItem.Text = "Verify developer license expiration";
            // 
            // registerDeveloperLicenseToolStripMenuItem
            // 
            this.registerDeveloperLicenseToolStripMenuItem.Name = "registerDeveloperLicenseToolStripMenuItem";
            this.registerDeveloperLicenseToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.registerDeveloperLicenseToolStripMenuItem.Text = "Register developer license";
            // 
            // unregisterDeveloperLicenseToolStripMenuItem
            // 
            this.unregisterDeveloperLicenseToolStripMenuItem.Enabled = false;
            this.unregisterDeveloperLicenseToolStripMenuItem.Name = "unregisterDeveloperLicenseToolStripMenuItem";
            this.unregisterDeveloperLicenseToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.unregisterDeveloperLicenseToolStripMenuItem.Text = "Unregister developer license";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.OrgPortalWebToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // OrgPortalWebToolStripMenuItem
            // 
            this.OrgPortalWebToolStripMenuItem.Name = "OrgPortalWebToolStripMenuItem";
            this.OrgPortalWebToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.OrgPortalWebToolStripMenuItem.Text = "OrgPortal Site";
            this.OrgPortalWebToolStripMenuItem.Click += new System.EventHandler(this.OrgPortalWebToolStripMenuItem_Click);
            // 
            // fileSystemWatcher2
            // 
            this.fileSystemWatcher2.EnableRaisingEvents = true;
            this.fileSystemWatcher2.Filter = "*.rt2win";
            this.fileSystemWatcher2.SynchronizingObject = this;
            // 
            // tabDistinctServerApps
            // 
            this.tabDistinctServerApps.Controls.Add(this.dgvDistinctServerApps);
            this.tabDistinctServerApps.Location = new System.Drawing.Point(4, 22);
            this.tabDistinctServerApps.Name = "tabDistinctServerApps";
            this.tabDistinctServerApps.Size = new System.Drawing.Size(736, 271);
            this.tabDistinctServerApps.TabIndex = 5;
            this.tabDistinctServerApps.Text = "Server Apps";
            this.tabDistinctServerApps.UseVisualStyleBackColor = true;
            // 
            // dgvDistinctServerApps
            // 
            this.dgvDistinctServerApps.AllowUserToAddRows = false;
            this.dgvDistinctServerApps.AllowUserToDeleteRows = false;
            this.dgvDistinctServerApps.AllowUserToOrderColumns = true;
            dataGridViewCellStyle34.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle34.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle34.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle34.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle34.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle34.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDistinctServerApps.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle34;
            this.dgvDistinctServerApps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDistinctServerApps.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewButtonColumn1});
            dataGridViewCellStyle35.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle35.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle35.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle35.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle35.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle35.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle35.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDistinctServerApps.DefaultCellStyle = dataGridViewCellStyle35;
            this.dgvDistinctServerApps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDistinctServerApps.Location = new System.Drawing.Point(0, 0);
            this.dgvDistinctServerApps.Name = "dgvDistinctServerApps";
            this.dgvDistinctServerApps.ReadOnly = true;
            dataGridViewCellStyle36.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle36.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle36.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle36.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle36.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle36.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle36.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDistinctServerApps.RowHeadersDefaultCellStyle = dataGridViewCellStyle36;
            this.dgvDistinctServerApps.Size = new System.Drawing.Size(736, 271);
            this.dgvDistinctServerApps.TabIndex = 2;
            this.dgvDistinctServerApps.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvServerApps_CellContentClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.FillWeight = 60F;
            this.dataGridViewTextBoxColumn1.HeaderText = "App Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.FillWeight = 50F;
            this.dataGridViewTextBoxColumn5.HeaderText = "Version";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 70;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Is Installed";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 70;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Install Mode";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 90;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn8.FillWeight = 70F;
            this.dataGridViewTextBoxColumn8.HeaderText = "Package Family Name";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewButtonColumn1
            // 
            this.dataGridViewButtonColumn1.HeaderText = "Install";
            this.dataGridViewButtonColumn1.Name = "dataGridViewButtonColumn1";
            this.dataGridViewButtonColumn1.ReadOnly = true;
            this.dataGridViewButtonColumn1.Text = "Install App";
            this.dataGridViewButtonColumn1.UseColumnTextForButtonValue = true;
            // 
            // LicenseInfoDisplay
            // 
            this.LicenseInfoDisplay.AutoSize = true;
            this.LicenseInfoDisplay.Location = new System.Drawing.Point(13, 47);
            this.LicenseInfoDisplay.MaximumSize = new System.Drawing.Size(330, 0);
            this.LicenseInfoDisplay.MinimumSize = new System.Drawing.Size(330, 0);
            this.LicenseInfoDisplay.Name = "LicenseInfoDisplay";
            this.LicenseInfoDisplay.Size = new System.Drawing.Size(330, 13);
            this.LicenseInfoDisplay.TabIndex = 4;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(191, 6);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(191, 6);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(191, 6);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(191, 6);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 321);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(760, 360);
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OrgPortal Sync Monitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.monitorApps.ResumeLayout(false);
            this.monitorApps.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabLog.ResumeLayout(false);
            this.tabLog.PerformLayout();
            this.tabAllServerApps.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvServerApps)).EndInit();
            this.tabInstalled.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInstalled)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher2)).EndInit();
            this.tabDistinctServerApps.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDistinctServerApps)).EndInit();
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
    private System.Windows.Forms.TabPage tabAllServerApps;
    private System.Windows.Forms.DataGridView dgvServerApps;
    private System.Windows.Forms.TabPage tabInstalled;
    private System.Windows.Forms.DataGridView dgvInstalled;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem monitorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem appsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem refreshInstalledListToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem unlockDeviceForSideloadingToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem registerDeveloperLicenseToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem autoConnectToolStripMenuItem;
    private System.ComponentModel.BackgroundWorker backgroundWorker1;
    private System.Windows.Forms.ToolStripMenuItem OrgPortalWebToolStripMenuItem;
    private System.IO.FileSystemWatcher fileSystemWatcher2;
    private System.Windows.Forms.CheckBox chkAutoInstall;
    private System.Windows.Forms.Button btnInstallUpdates;
    private System.Windows.Forms.TextBox txtAutoInstallTimer;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripMainStatusLabel;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusAdicional;
    private System.Windows.Forms.ToolStripMenuItem autoInstallToolStripMenuItem;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private System.Windows.Forms.DataGridViewTextBoxColumn ServerVersion;
    private System.Windows.Forms.DataGridViewTextBoxColumn Installed;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private System.Windows.Forms.DataGridViewButtonColumn Install;
    private System.Windows.Forms.CheckBox monitorInstalledApps;
    private System.Windows.Forms.DataGridViewTextBoxColumn DisplayName;
    private System.Windows.Forms.DataGridViewTextBoxColumn InstalledVersion;
    private System.Windows.Forms.DataGridViewCheckBoxColumn UpdateAvailable;
    private System.Windows.Forms.DataGridViewTextBoxColumn InstallMode;
    private System.Windows.Forms.DataGridViewTextBoxColumn PackageName;
    private System.Windows.Forms.DataGridViewButtonColumn Uninstall;
    private System.Windows.Forms.ToolStripMenuItem monitorInstalledAppsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem unregisterDeveloperLicenseToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem verifyDeveloperLicenseToolStripMenuItem;
    private System.Windows.Forms.CheckBox requireDevLicense;
    private System.Windows.Forms.CheckBox chkProcessExistingAppRequests;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.GroupBox monitorApps;
    private System.Windows.Forms.ToolStripMenuItem showThisUIOnLoadToolStripMenuItem;
    private System.Windows.Forms.TabPage tabDistinctServerApps;
    private System.Windows.Forms.DataGridView dgvDistinctServerApps;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
    private System.Windows.Forms.DataGridViewButtonColumn dataGridViewButtonColumn1;
    private System.Windows.Forms.Label LicenseInfoDisplay;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
  }
}

