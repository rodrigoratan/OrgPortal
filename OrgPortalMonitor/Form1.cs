using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using OrgPortalMonitor.DataModel;
using OrgPortalMonitor.Properties;

namespace OrgPortalMonitor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            try
            {
                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                {
                    System.Deployment.Application.ApplicationDeployment ad =
                    System.Deployment.Application.ApplicationDeployment.CurrentDeployment;
                    var version = ad.CurrentVersion;
                    this.Text += " - " + string.Format("V.{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision)
                               + " - " + this.ProductVersion;
                }
                else
                {
                    this.Text += " - " + this.ProductVersion;
                }
            }
            catch (Exception)
            {
                this.Text += " - " + this.ProductVersion;
            }

        }

        private Installer _installer;
        private bool _reallyClose;
        private int _autoInstallTimer = 0;
        private string btnStartStopOriginalTextBuffer;
        private string btnStripStartStopOriginalTextBuffer;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.Visible = false;

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
                MessageBox.Show("OrgPortal by Zollie (www.zollie.com.br)");
            };

            exitToolStripMenuItem.Click += (o, a) =>
            {
                _reallyClose = true;
                this.Close();
            };

            processExistingAppRequestsToolStripMenuItem.Click += (o, a) =>
            {
                if (_installer != null)
                {
                    _installer.ProcessExistingRequestFiles1();
                }
            };

            processExistingAutoInstallAndAutoUpdateToolStripMenuItem.Click += (o, a) =>
            {
                if (_installer != null)
                {
                    _installer.ProcessExistingRequestFiles2();
                }
            };

            BlockAllTabsExceptOneIfNotStarted("tabSettings");

            var RequestQueryString = GetQueryStringParameters();
            
            #region AutoStart
            var AutoStartParameter = RequestQueryString["AutoStart"] != null ? RequestQueryString["AutoStart"] : "";
            bool _autoStart = false;
            if (AutoStartParameter != null &&
                bool.TryParse(AutoStartParameter, out _autoStart))
            {
                chkAutoStart.Checked = _autoStart; //bool.Parse(AutoStartParameter);
                autoConnectToolStripMenuItem.Checked = _autoStart;
            }
            else
            {
                chkAutoStart.Checked = (Settings.Default.AutoStart);
                autoConnectToolStripMenuItem.Checked = (Settings.Default.AutoStart);
            }
            #endregion 
            #region PackageFamilyName
            var PackageFamilyNameParameter = RequestQueryString["PackageFamilyName"] != null ? RequestQueryString["PackageFamilyName"] : "";
            if (PackageFamilyNameParameter != null &&
                !string.IsNullOrEmpty(PackageFamilyNameParameter))
            {
                txtPackageFamilyName.Text = PackageFamilyNameParameter;
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

        }

        private void OrgPortalStatus()
        {

            string extraInfo = "";
            extraInfo += "OrgPortalUrl[" + txtOrgPortalUrl.Text + "]";
            extraInfo += "PackageFamilyName[" + txtPackageFamilyName.Text + "]";
            if (_installer != null)
            {
                extraInfo += "serviceURI[" + _installer._serviceURI + "]";
                extraInfo += "PackageTempPath[" + _installer.PackageTempPath + "]";
                extraInfo += "PackageLocalPath[" + _installer.PackageLocalPath + "]";
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
            //_installer.GetInstalledPackages();
            await RefreshInstalledApps();

        }

        private void UnlockDevice()
        {
            var dialog = new UnlockKeyDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var key = dialog.KeyValue;
                _installer.UnlockDevice(key);
            }
        }

        private void GetDevLicense()
        {
            _installer.GetDevLicense();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_reallyClose)
            {
                this.ShowInTaskbar = false;
                this.Visible = false;
                e.Cancel = true;
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
            //this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
            this.Visible = true;
            this.BringToFront();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            var minutes = int.Parse(ConfigurationManager.AppSettings["AutoInstallTime"]);
            _autoInstallTimer++;
            if (_autoInstallTimer > minutes && IsStarted)
                _autoInstallTimer = 1;
            if (_autoInstallTimer == 1 && _installer != null)
            {
                await _installer.AutoInstallUpdateApps();
                _installer.InstalledAppList = null;
                //InstalledAppList = _installer.GetInstalledApps(_installer.GetAppList());
            }
        }

        private async void chkAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.AutoStart = chkAutoStart.Checked;
            Settings.Default.Save();

            btnStartStop.Visible = !chkAutoStart.Checked;
            startToolStripMenuItem.Visible = !chkAutoStart.Checked;
            autoConnectToolStripMenuItem.Checked = ((CheckBox)sender).Checked;

            if (chkAutoStart.Checked && !IsStarted)
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
                IsStarted = true;
                btnStartStopOriginalTextBuffer = btnStartStop.Text;
                btnStripStartStopOriginalTextBuffer = startToolStripMenuItem.Text;
                btnStartStop.Text = "Disconnect OrgPortal Store and Stop Monitoring";
                startToolStripMenuItem.Text = "Stop and Disconnect";

                this.txtLogOutput.Text += "\n\n";

                _installer = new Installer(txtPackageFamilyName.Text,
                                           this.notifyIcon1,
                                           this.txtLogOutput,
                                           this.fileSystemWatcher1,
                                           this.fileSystemWatcher2
                                          );

                _installer.StartFileWatcher1(_installer.PackageTempPath);
                //_installer.StartFileWatcher2(_installer.CachePath);
                if (!_installer.IsAutoInstalling)
                {
                    _installer.ProcessExistingRequestFiles2();
                }

                await RefreshInstalledApps();
            }
            else
            {
                _installer.StopFileWatcher1();
                _installer = null;
                IsStarted = false;
                btnStartStop.Text = btnStartStopOriginalTextBuffer;
                startToolStripMenuItem.Text = btnStripStartStopOriginalTextBuffer;
            }

            txtOrgPortalUrl.Enabled = !IsStarted;
            BlockAllTabsExceptOneIfNotStarted("tabSettings");
        }

        private async Task RefreshInstalledApps()
        {
            dgvInstalled.Rows.Clear();
            dgvServerApps.Rows.Clear();

            if (_installer != null)
            {
                //if (_installer.ServerAppList == null || _installer.ServerAppList.Count == 0)
                //{
                _installer.ServerAppList = await _installer.GetRemoteAppList();
                //}
                var installedApps = _installer.GetInstalledApps(_installer.ServerAppList != null ? _installer.ServerAppList : new List<AppInfo>());
                foreach (var app in installedApps)
                {
                    dgvInstalled.Rows.Add(app.DisplayName, app.Version, app.InstallMode, app.PackageFamilyName);
                }

                foreach (var app in _installer.ServerAppList)
                {
                    var _installedItem = installedApps
                                        .FirstOrDefault(a => a.PackageFamilyName == app.PackageFamilyName &&
                                                             a.Version == app.Version);
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

        private NameValueCollection GetQueryStringParameters()
        {
            NameValueCollection nameValueTable = new NameValueCollection();
            try
            {
                if (ApplicationDeployment.IsNetworkDeployed &&
                    ApplicationDeployment.CurrentDeployment.ActivationUri != null)
                {
                    string queryString = ApplicationDeployment.CurrentDeployment.ActivationUri.Query;
                    nameValueTable = HttpUtility.ParseQueryString(queryString);
                }
                return (nameValueTable);
            }
            catch (Exception ex)
            {
                OutputException(ex);
                return (nameValueTable);
            }
        }

        private void OutputException(Exception ex)
        {
            try 
	        {
                if (_installer != null)
                {
                    ExceptionLogger.LogException(ex, _installer.PackageTempPath);
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

                if (_installer != null)
                {
                    ExceptionLogger.LogException(exc, _installer.PackageTempPath);
                }
                else
                {
                    ExceptionLogger.LogException(exc);
                }

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
                        if (_installer != null)
                        {
                            var serverApps = _installer.ServerAppList;
                            var requestApp = serverApps
                                            .Where(a => a.Version == version &&
                                                        a.PackageFamilyName == package).FirstOrDefault();

                            var fileNamePath = _installer.CachePath +
                                               System.Guid.NewGuid().ToString() +
                                               ".rt2win";
                            await Installer.RequestApp(
                                requestApp,
                                _installer.CachePath,
                                fileNamePath);
                            
                            _installer.ProcessRequest(fileNamePath); 
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
            }

        }

    }
}
