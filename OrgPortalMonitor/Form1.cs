using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using OrgPortalMonitor.Common;
using OrgPortalMonitor.DataModel;
using OrgPortalMonitor.Properties;

namespace OrgPortalMonitor
{
    public partial class Form1 : Form
    {
        public  Form1()
        {
            InitializeComponent();

            string[] args = Environment.GetCommandLineArgs();
            //•args[0] is the application path.
            //•args[1] will be the file path.
            //•args[n] will be any other arguments passed in.

            SetAppVersion();

        }

        private void SetAppVersion()
        {
            try
            {
                AppVersion = string.Empty;
                if (false/* || System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed*/)
                {
                    System.Deployment.Application.ApplicationDeployment ad =
                    System.Deployment.Application.ApplicationDeployment.CurrentDeployment;
                    var version = ad.CurrentVersion;
                    AppVersion = " - " + string.Format("V.{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision)
                               + " - " + this.ProductVersion;

                    this.Text += AppVersion;
                }
                else
                {
                    AppVersion = " - " + this.ProductVersion;
                    this.Text += AppVersion;
                }
            }
            catch (Exception)
            {
                AppVersion = " - " + this.ProductVersion;
                this.Text += AppVersion;
            }
        }

        private Installer _monitor;
        private bool _reallyClose;

        //public int CumulativeMinutesInstallTimer { get; set; }

        private int _autoInstallTimer = 0;
        public int AutoInstallTimer
        {
            get { return _autoInstallTimer; }
            set { _autoInstallTimer = value; }
        }

        private int _autoInstallMinutes = 0;
        public int AutoInstallMinutes
        {
            get { return _autoInstallMinutes; }
            set { _autoInstallMinutes = value; }
        }

        private string btnStartStopOriginalTextBuffer;
        private string btnStripStartStopOriginalTextBuffer;
        private AppRequest appRequest;

        private async void Form1_Load(object sender, EventArgs e)
        {
            //this.ShowInTaskbar = false;
            //this.Visible = false;

            this.notifyIcon1.Visible = true;
            var menu = this.notifyIcon1.ContextMenu = new ContextMenu();
            menu.MenuItems.Add(new MenuItem { Text = "Open Monitor", Name = "OpenItem", DefaultItem = true });
            menu.MenuItems.Add(new MenuItem { Text = "Refresh app list", Name = "RefreshAppList" });
            menu.MenuItems.Add(new MenuItem { Text = "Unlock device for side-loading", Name = "UnlockDevice" });
            menu.MenuItems.Add(new MenuItem { Text = "Get developer license", Name = "GetDevLicense" });
            menu.MenuItems.Add(new MenuItem { Text = "Exit", Name = "ExitItem" });
            menu.MenuItems[0].Click += (o, a) => DisplayForm();
            menu.MenuItems[1].Click += async (o, a) => await RefreshAppList();
            menu.MenuItems[2].Click += (o, a) => UnlockDevice();
            menu.MenuItems[3].Click += (o, a) => GetDevLicense();
            menu.MenuItems[4].Click += (o, a) =>
              {
                  _reallyClose = true;
                  this.Close();
              };

            startToolStripMenuItem.Click += async (o, a) =>
            {
                await ToggleStartStop();
            };

            refreshInstalledListToolStripMenuItem.Click += async (o, a) =>
            {
                await RefreshAppList();
            };

            unlockDeviceForSideloadingToolStripMenuItem.Click += (o, a) =>
            {
                UnlockDevice();
            };

            getDeveloperLicenseToolStripMenuItem.Click += (o, a) =>
            {
                GetDevLicense();
            };

            aboutToolStripMenuItem.Click += (o, a) =>
            {
                MessageBox.Show("OrgPortal V" + this.AppVersion + " by Zollie (www.zollie.com.br)");
            };

            exitToolStripMenuItem.Click += (o, a) =>
            {
                _reallyClose = true;
                this.Close();
            };

            //processExistingAppRequestsToolStripMenuItem.Click += (o, a) =>
            //{
            //    if (_monitor != null)
            //    {
            //        _monitor.ProcessExistingRequestFiles1();
            //    }
            //};

            //processExistingAutoInstallAndAutoUpdateToolStripMenuItem.Click += (o, a) =>
            //{
            //    if (_monitor != null)
            //    {
            //        _monitor.ProcessExistingRequestFiles2();
            //    }
            //};

            BlockAllTabsExceptOneIfNotStarted("tabSettings");

            var RequestQueryString = Utils.GetQueryStringParameters();
            
            #region AutoStart
            var AutoStartParameter = RequestQueryString["AutoStart"] != null ? RequestQueryString["AutoStart"] : "";
            bool _autoStart = false;
            if (AutoStartParameter != null &&
                bool.TryParse(AutoStartParameter, out _autoStart))
            {
                chkAutoStart.Checked = _autoStart; //bool.Parse(AutoStartParameter);
                autoConnectToolStripMenuItem.Checked = _autoStart;
                Settings.Default.AutoStart = _autoStart;
                Settings.Default.Save();
            }
            else
            {
                chkAutoStart.Checked = (Settings.Default.AutoStart);
                autoConnectToolStripMenuItem.Checked = (Settings.Default.AutoStart);
            }
            #endregion 

            #region AutoInstall
            var AutoInstallParameter = RequestQueryString["AutoInstall"] != null ? RequestQueryString["AutoInstall"] : "";
            bool _autoInstall = false;
            if (AutoInstallParameter != null &&
                bool.TryParse(AutoInstallParameter, out _autoInstall))
            {
                chkAutoInstall.Checked = _autoInstall; //bool.Parse(AutoInstallParameter);
                autoInstallToolStripMenuItem.Checked = _autoInstall;
                btnInstallUpdates.Enabled = !_autoInstall;

                Settings.Default.AutoInstall = _autoInstall;
                Settings.Default.Save();
            }
            else
            {
                chkAutoInstall.Checked = (Settings.Default.AutoInstall);
                autoInstallToolStripMenuItem.Checked = (Settings.Default.AutoInstall);
                btnInstallUpdates.Enabled = !chkAutoInstall.Checked;
            }
            #endregion 

            #region AutoInstallMinutes
            var AutoInstallMinutesParameter = RequestQueryString["AutoInstallMinutes"] != null ? RequestQueryString["AutoInstallMinutes"] : "";
            //int _autoInstallMinutes = 0;
            if (AutoInstallMinutesParameter != null &&
                Int32.TryParse(AutoInstallMinutesParameter, out _autoInstallMinutes))
            {
                txtAutoInstallTimer.Text = _autoInstallMinutes.ToString();

                Settings.Default.AutoInstallMinutes = _autoInstallMinutes;
                Settings.Default.Save();
            }
            else
            {
                AutoInstallMinutes = Settings.Default.AutoInstallMinutes;
                txtAutoInstallTimer.Text = Settings.Default.AutoInstallMinutes.ToString();
            }
            #endregion 

            #region PackageFamilyName
            var PackageFamilyNameParameter = RequestQueryString["PackageFamilyName"] != null ? RequestQueryString["PackageFamilyName"] : "";
            if (PackageFamilyNameParameter != null &&
                !string.IsNullOrEmpty(PackageFamilyNameParameter))
            {
                txtPackageFamilyName.Text = PackageFamilyNameParameter;
                Settings.Default.PackageFamilyName = PackageFamilyNameParameter;
                Settings.Default.Save();
            }
            else
            {
                if (!string.IsNullOrEmpty(Settings.Default.PackageFamilyName))
                {
                    txtPackageFamilyName.Text = Settings.Default.PackageFamilyName;
                }
            }
            #endregion

            #region OrgPortalUrl
            var OrgPortalUrlParameter = RequestQueryString["OrgPortalUrl"] != null ? RequestQueryString["OrgPortalUrl"] : "";
            if (OrgPortalUrlParameter != null &&
                !string.IsNullOrEmpty(OrgPortalUrlParameter))
            {
                txtOrgPortalUrl.Text = OrgPortalUrlParameter;

                Settings.Default.OrgPortalUrl = OrgPortalUrlParameter;
                Settings.Default.Save();
            }
            else
            {
                if (!string.IsNullOrEmpty(Settings.Default.OrgPortalUrl))
                {
                    txtOrgPortalUrl.Text = Settings.Default.OrgPortalUrl;
                }
            }
            #endregion 

            #region Window Location and Size
            // Set window location
            if (Settings.Default.WindowLocation != null)
            {
                if (Settings.Default.WindowLocation.X > 0 && Settings.Default.WindowLocation.Y > 0)
                {
                    this.Location = Settings.Default.WindowLocation;
                }
            }

            // Set window size
            if (Settings.Default.WindowSize != null)
            {
                this.Size = Settings.Default.WindowSize;
            }
            #endregion

            OrgPortalStatus();
            IsLoaded = true;

            await ToggleStartStop();

            //GatherMachineInfo();
        }

        private void GatherMachineInfo()
        {
            var windowsVersion        = ""; // await WindowsStoreSystemInformation.GetWindowsVersionAsync();
            var processor             = ""; // await WindowsStoreSystemInformation.GetProcessorArchitectureAsync();
            var deviceCategory        = ""; // await WindowsStoreSystemInformation.GetDeviceCategoryAsync();
            var deviceManufacturer    = ""; // await WindowsStoreSystemInformation.GetDeviceManufacturerAsync();
            var deviceModel           = ""; // await WindowsStoreSystemInformation.GetDeviceModelAsync();
            var osInfo                = ""; // new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation();
            string machineName        = ""; // osInfo.FriendlyName;
            string operatingSystem    = ""; // osInfo.OperatingSystem;
            string systemManufacturer = ""; // osInfo.SystemManufacturer;
            string systemProductName  = ""; // osInfo.SystemProductName;
            string systemSku          = ""; // osInfo.SystemSku;
            string systemId           = ""; // osInfo.Id;
            string deviceInfo         = ""; // string.Format(            
        }

        private void OrgPortalStatus()
        {
            string extraInfo = "";
            extraInfo += "OrgPortalUrl[" + txtOrgPortalUrl.Text + "]";
            extraInfo += "PackageFamilyName[" + txtPackageFamilyName.Text + "]";
            if (_monitor != null)
            {
                extraInfo += "serviceURI[" + _monitor.ServiceURI + "]";
                extraInfo += "PackageTempPath[" + _monitor.OrgPortalPackageTempPath + "]";
                extraInfo += "PackageLocalPath[" + _monitor.OrgPortalPackageLocalPath + "]";
            }
            extraInfo += Environment.NewLine;

            string fileName = "OrgPortalMonitor-StartupInfo-T" + Environment.CurrentManagedThreadId.ToString() + ".log";
            ExceptionLogger.ExtraLog(extraInfo, fileName);
            //ExceptionLogger.PurgeLogFiles();
        }

        private void BlockAllTabsExceptOneIfNotStarted(string tabName)
        {
            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                TabPage tp = tabControl1.TabPages[i];
                tp.Enabled = (tp.Name == tabName || tp.Name == "tabLog") || IsStarted;
                //tp.Visible = (tp.Name == tabName) || IsStarted;
            }
        }

        private async Task RefreshAppList()
        {
            //_monitor.GetInstalledPackages();
            await RefreshInstalledApps();
        }

        private void UnlockDevice()
        {
            var dialog = new UnlockKeyDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var key = dialog.KeyValue;
                _monitor.UnlockDevice(key);
            }
        }

        private void GetDevLicense()
        {
            _monitor.ShowDevLicense();
        }

        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_reallyClose)
            {
                this.ShowInTaskbar = false;
                this.Visible = false;
                e.Cancel = true;
            }
            else
            {
                if (_monitor != null)
                {
                    await _monitor.StopCacheFileWatcher();
                    await _monitor.StopPackageTempFileWatcher();
                    await _monitor.StopExistingAppsFileWatcher();
                    _monitor = null;
                }
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            DisplayForm();
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            DisplayForm();
        }

        private void DisplayForm()
        {
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
            this.Visible = true;
            this.BringToFront();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            if (chkAutoInstall.Checked && IsStarted)
            {
                //txtAutoInstallTimer
                //var minutes = int.Parse(ConfigurationManager.AppSettings["AutoInstallTime"]);

                AtualizaAutoInstallTimerProperty();

                AutoInstallTimer++;
                //NextServerCheckInMinutes = "";

                if (AutoInstallTimer > AutoInstallMinutes || AutoInstallMinutes <= 1)
                    AutoInstallTimer = 1;

                if (AutoInstallTimer == 1 && _monitor != null)
                {
                    await _monitor.AutoInstallUpdateApps();
                    _monitor.InstalledAppList = null;
                    //InstalledAppList = _monitor.GetInstalledApps(_monitor.GetAppList());
                }
            }
        }

        private async void chkAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.AutoStart = chkAutoStart.Checked;
            Settings.Default.Save();

            //btnStartStop.Visible = !chkAutoStart.Checked;
            btnStartStop.Enabled = !chkAutoStart.Checked;

            startToolStripMenuItem.Visible = !chkAutoStart.Checked;
            autoConnectToolStripMenuItem.Checked = ((CheckBox)sender).Checked;
            if (chkAutoStart.Checked && !IsStarted && IsLoaded)
            {
                IsStarted = false; //reset so ToggleStartStop can toggle it
                await ToggleStartStop();
            }
        }

        private void autoConnectToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            chkAutoStart.Checked = ((ToolStripMenuItem)sender).Checked;
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            await ToggleStartStop();
        }

        private async Task ToggleStartStop()
        {
            if (!IsStarted)
            {
                if (IsLoaded)
                {
                    IsStarted = true;
                    btnStartStopOriginalTextBuffer = btnStartStop.Text;
                    btnStripStartStopOriginalTextBuffer = startToolStripMenuItem.Text;
                    btnStartStop.Text = "Disconnect OrgPortal Store and Stop Monitoring";
                    startToolStripMenuItem.Text = "Stop and Disconnect";

                    this.txtLogOutput.Text += "\n\n";

                    _monitor = new Installer(txtOrgPortalUrl.Text,
                                               txtPackageFamilyName.Text,
                                               this.notifyIcon1,
                                               this.txtLogOutput,
                                               this.fileSystemWatcher1,
                                               this.fileSystemWatcher2
                                              );

                    await _monitor.StartOrgPortalFileWatcher(_monitor.OrgPortalPackageTempPath);
                    toolStripMainStatusLabel.Text = "Connected @ " + _monitor.ServiceURI;
                    toolStripStatusAdicional.Text = " & " + _monitor.OrgPortalPackageTempPath;

                    //_monitor.StartFileWatcher2(_monitor.CachePath);
                    //TODO: Uncomment to ProcessExistingCacheRequestFiles on ToggleStartStop
                    //if (!_monitor.IsAutoInstalling)
                    //{
                    //    _monitor.ProcessExistingCacheRequestFiles();
                    //}
                    await RefreshInstalledApps();
                }
            }
            else
            {
                await _monitor.StopPackageTempFileWatcher();
                _monitor = null;
                IsStarted = false;
                btnStartStop.Text = btnStartStopOriginalTextBuffer;
                startToolStripMenuItem.Text = btnStripStartStopOriginalTextBuffer;
                toolStripMainStatusLabel.Text = "Disconnected ";// + _monitor.ServiceURI;
                toolStripStatusAdicional.Text = ""; // & " + _monitor.OrgPortalPackageTempPath;
            }

            txtOrgPortalUrl.Enabled = !IsStarted;
            BlockAllTabsExceptOneIfNotStarted("tabSettings");
        }

        private async Task RefreshInstalledApps()
        {
            dgvInstalled.Rows.Clear();
            dgvServerApps.Rows.Clear();

            if (_monitor != null)
            {
                //if (_monitor.ServerAppList == null || _monitor.ServerAppList.Count == 0)
                //{
                _monitor.ServerAppList = await _monitor.GetRemoteAppList();
                //}
                var installedApps = _monitor
                                    .GetInstalledApps(
                                        _monitor.ServerAppList != null ?
                                                _monitor.ServerAppList : 
                                                new List<AppInfo>());
                foreach (var app in installedApps)
                {
                    dgvInstalled.Rows.Add(app.DisplayName, app.Version, app.NewVersionAvailable, app.InstallMode, app.PackageFamilyName);
                }

                foreach (var app in _monitor.ServerAppList)
                {
                    AppInfo _installedItem = 
                            (installedApps != null) ? 
                                installedApps
                                .FirstOrDefault(a => a.PackageFamilyName == app.PackageFamilyName &&
                                                     a.Version           == app.Version) : new AppInfo();

                    dgvServerApps.Rows.Add(app.DisplayName, app.Version, _installedItem != null, app.InstallMode, app.PackageFamilyName);
                }
            }
        }

        public bool IsStarted { get; set; }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (!IsStarted)
            {
                e.Cancel = !e.TabPage.Enabled;
            }
        }

        private void txtOrgPortalUrl_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.OrgPortalUrl = ((TextBox)sender).Text;
            Settings.Default.Save();
        }

        private void txtPackageFamilyName_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.PackageFamilyName = ((TextBox)sender).Text;
            Settings.Default.Save();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            // Copy window location to app settings
            Settings.Default.WindowLocation = this.Location;

            // Copy window size to app settings
            if (this.WindowState == FormWindowState.Normal)
            {
                Settings.Default.WindowSize = this.Size;
            }
            else
            {
                Settings.Default.WindowSize = this.RestoreBounds.Size;
            }

            // Save settings
            Settings.Default.Save();

        }


        private async void dgvInstalled_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var senderGrid = (DataGridView)sender;
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                    e.RowIndex >= 0 && (e.ColumnIndex - 1) >= 0)
                {
                    //MessageBox.Show("Not implemented yet. Coming soon. ");

                    var packageFamilyNameColumn = senderGrid.Columns[e.ColumnIndex - 1];

                    if (packageFamilyNameColumn != null &&
                        packageFamilyNameColumn is DataGridViewTextBoxColumn &&
                        e.RowIndex >= 0)
                    {
                        string package = senderGrid[e.ColumnIndex - 1, e.RowIndex].Value as string;
                        string version = senderGrid[e.ColumnIndex - 4, e.RowIndex].Value as string;

                        if (_monitor != null)
                        {
                            var result = new InstallResult();
                            result = await _monitor.UninstallApp(package);
                            if (result != null)
                            {
                                if (string.IsNullOrWhiteSpace(result.Error))
                                {
                                    this.notifyIcon1.ShowBalloonTip(500, "OrgPortal ", package + " uninstalled with sucess", System.Windows.Forms.ToolTipIcon.Info);
                                    this.txtLogOutput.AppendText("** SUCCESS " + Environment.NewLine);
                                }
                                else
                                {
                                    this.notifyIcon1.ShowBalloonTip(500, "OrgPortal", package + " could not be uninstalled ", System.Windows.Forms.ToolTipIcon.Warning);
                                    this.txtLogOutput.AppendText("** FAILED " + Environment.NewLine);
                                    this.txtLogOutput.AppendText(result.ToString() + Environment.NewLine);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
            }
        }
        private async void dgvServerApps_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var senderGrid = (DataGridView)sender;
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                    e.RowIndex >= 0 && (e.ColumnIndex - 1) >= 0)
                {

                    //MessageBox.Show("Not implemented yet. Please use the OrgPortal app to request app installs.");

                    var packageFamilyNameColumn = senderGrid.Columns[e.ColumnIndex - 1];

                    if (packageFamilyNameColumn != null && 
                        packageFamilyNameColumn is DataGridViewTextBoxColumn &&
                        e.RowIndex >= 0)
                    {
                        string package = senderGrid[e.ColumnIndex - 1, e.RowIndex].Value as string;
                        string version = senderGrid[e.ColumnIndex - 4, e.RowIndex].Value as string;
                        if (_monitor != null)
                        {
                            var serverApps = _monitor.ServerAppList;
                            var requestApp = serverApps
                                            .Where(a => a.Version == version &&
                                                        a.PackageFamilyName == package).FirstOrDefault();

                            var fileNamePath = _monitor.CachePath +
                                               System.Guid.NewGuid().ToString() +
                                               ".rt2win";
                            await Installer.RequestApp(
                                requestApp,
                                _monitor.CachePath,
                                fileNamePath);

                            appRequest = new AppRequest(fileNamePath);
                            appRequest.Show();
                            //_monitor.ProcessRequest(fileNamePath); 
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
            }

        }

        private async void chkAutoInstall_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.AutoInstall = chkAutoInstall.Checked;
            Settings.Default.Save();

            btnInstallUpdates.Enabled = !chkAutoInstall.Checked;

            autoInstallToolStripMenuItem.Checked = chkAutoInstall.Checked;

            AutoInstallTimer = 0;

            if (chkAutoInstall.Checked)
            {
                if (_monitor != null && IsLoaded)
                {
                    //_monitor.StartFileWatcher2(_monitor.CachePath);
                    //if (!_monitor.IsAutoInstalling)
                    //{
                    await _monitor.ProcessExistingCacheRequestFiles();
                    //}
                }
            }
            //else
            //{
            //    if (_monitor != null)
            //    {
            //        _monitor.StopFileWatcher2();
            //    }
            //}
        }

        private async void btnInstallUpdates_Click(object sender, EventArgs e)
        {
            await _monitor.AutoInstallUpdateApps();
        }

        private void autoInstallToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            chkAutoInstall.Checked = ((ToolStripMenuItem)sender).Checked;
        }

        public string AppVersion { get; set; }

        public string NextServerCheckInMinutes { get; set; }

        public bool IsLoaded { get; set; }

        private void txtAutoInstallTimer_TextChanged(object sender, EventArgs e)
        {
            //int _autoInstallMinutes = 0;// ((TextBox)sender).Text;
            AtualizaAutoInstallTimerProperty();
        }

        private void AtualizaAutoInstallTimerProperty()
        {
            if (Int32.TryParse(txtAutoInstallTimer.Text, out _autoInstallMinutes))
            {
                Settings.Default.AutoInstallMinutes = _autoInstallMinutes;
                Settings.Default.Save();
            }
        }

        private void OrgPortalWebToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_monitor.ServiceURI.Contains("/api"))
                {
                    var url = _monitor.ServiceURI.Substring(0, _monitor.ServiceURI.IndexOf("/api"));
                    ProcessStartInfo sInfo = new ProcessStartInfo(url);
                    Process.Start(sInfo);
                }
            }
            catch
            {
            }
        }

        private async void monitorInstalledApps_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.MonitorInstalledApps = monitorInstalledApps.Checked;
            Settings.Default.Save();
           
            //TODO: create toolstrip
            //monitorInstalledAppsToolStripMenuItem.Checked = monitorInstalledApps.Checked;

            if (monitorInstalledApps.Checked)
            {
                if (_monitor != null && IsLoaded)
                {
                    //_monitor.StartFileWatcher2(_monitor.CachePath);
                    //if (!_monitor.IsAutoInstalling)
                    //{
                    if (_monitor.InstalledAppList != null && _monitor.InstalledAppList.Count > 0)
                    {
                        await _monitor.ProcessExistingAppsTempRequestFiles();
                    }
                    //}
                }
            }
        }

        public void OutputException(Exception ex)
        {
            try
            {
                if (_monitor != null)
                {
                    ExceptionLogger.LogException(ex, _monitor.OrgPortalPackageTempPath);
                }
                else
                {
                    ExceptionLogger.LogException(ex);
                }

                txtLogOutput.Text += "---------------------------"
                                  + "Erro: "
                                  + System.Environment.NewLine
                                  + ex.Message
                                  + System.Environment.NewLine
                                  + (ex.InnerException != null ?
                                        "[ Erro interno: "
                                        + ex.InnerException.Message
                                        + System.Environment.NewLine
                                        + "Em: "
                                        + System.Environment.NewLine
                                        + ex.InnerException.StackTrace
                                        + System.Environment.NewLine
                                        + "Origem: "
                                        + System.Environment.NewLine
                                        + ex.InnerException.Source
                                        + System.Environment.NewLine
                                        + "]"
                                        + System.Environment.NewLine
                                        : "")
                                   + "Em: "
                                   + System.Environment.NewLine
                                   + ex.StackTrace
                                   + System.Environment.NewLine
                                   + "Origem: "
                                   + System.Environment.NewLine
                                   + ex.Source
                                   + System.Environment.NewLine
                                   + "---------------------------"
                                   ;
            }
            catch (Exception exc)
            {
                txtLogOutput.Text += "Erro: "
                                      + System.Environment.NewLine
                                      + exc.Message;

                if (_monitor != null)
                {
                    ExceptionLogger.LogException(exc, _monitor.OrgPortalPackageTempPath);
                }
                else
                {
                    ExceptionLogger.LogException(exc);
                }

            }

        }

        
    }
}
