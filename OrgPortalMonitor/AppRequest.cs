using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OrgPortalMonitor.Common;
using OrgPortalMonitor.DataModel;
using OrgPortalMonitor.Properties;

namespace OrgPortalMonitor
{
    public partial class AppRequest : Form
    {

        public AppRequest()
        {
            InitializeComponent();

            //RequestQueryString = Utils.GetQueryStringParameters();
            this.Load += AppRequest_Load;
            this.FormClosing += AppRequest_FormClosing;
        }

        public AppRequest(string filePath)
        {
            InitializeComponent();

            //progressBarApp.Minimum = 0;
            //progressBarApp.Maximum = 100;
            //progressBarApp.Value = 0;

            this.FilePath = filePath;
            //RequestQueryString = new System.Collections.Specialized.NameValueCollection();
            SetAppVersion();
            this.Load += AppRequest_Load;
            this.FormClosing += AppRequest_FormClosing;
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

        async void AppRequest_Load(object sender, EventArgs e)
        {

            SetAppVersion();

            progressBarApp.Value = 0;
            this.ShowInTaskbar = true;
            this.Visible = true;

            TaskbarProgress.SetState(this.Handle, TaskbarProgress.TaskbarStates.Indeterminate);

            RequestQueryString = Utils.GetQueryStringParameters();
            string[] args = Environment.GetCommandLineArgs();
            //•args[0] is the application path.
            //•args[1] will be the file path.
            //•args[n] will be any other arguments passed in.

            //btnCancel.Click += (o, a) =>
            //{
            //    _reallyClose = true;
            //    this.Close();
            //};

            #region PackageFamilyName
            PackageFamilyNameParameter = RequestQueryString["PackageFamilyName"] != null ? RequestQueryString["PackageFamilyName"] : "";
            if (PackageFamilyNameParameter != null &&
                !string.IsNullOrEmpty(PackageFamilyNameParameter))
            {
                //txtPackageFamilyName.Text = PackageFamilyNameParameter;

                //Settings.Default.PackageFamilyName = PackageFamilyNameParameter;
                //Settings.Default.Save();
            }
            #endregion

            #region OrgPortalUrl
            OrgPortalUrlParameter = RequestQueryString["OrgPortalUrl"] != null ? RequestQueryString["OrgPortalUrl"] : "";
            if (OrgPortalUrlParameter != null &&
                !string.IsNullOrEmpty(OrgPortalUrlParameter))
            {
                //txtOrgPortalUrl.Text = OrgPortalUrlParameter;

                //Settings.Default.OrgPortalUrl = OrgPortalUrlParameter;
                //Settings.Default.Save();
            }
            #endregion 

            progressBarApp.Value = 1;

            IsLoaded = true;

            if (!string.IsNullOrEmpty(FilePath))
            {
                var input = File.ReadAllLines(FilePath);
                //  install[0], appxFile[2], certificateUrl[4], certificateFile[6], saveAt[8], version[10],  name[12], description[14], background[16], imageUrl[18]
                //    input[1],    input[3],          input[5],           input[7],  input[9],   input[11], input[13],       input[15],      input[17],    input[19]
                appVersion = input[11] != null ? input[11] : "0.0.0.0";
                appName = input[13] != null ? input[13] : "app name";
                appDescription = input[15] != null ? input[15] : "app description";
                appBackground = input[17] != null ? input[17] : "#000000";
                appImage = input[19] != null ? input[19] : "";

                lblStatus1.Text = appName + " v" + appVersion;
                lblStatus2.Text = appDescription;

                pictureBox1.ImageLocation = appImage;
                pictureBox2.ImageLocation = appImage.Contains("/logo/") ? appImage.Replace("/logo/", "/smalllogo/") : appImage;

                pictureBox1.BackColor = ColorTranslator.FromHtml(appBackground);
                pictureBox2.BackColor = ColorTranslator.FromHtml(appBackground);
                await StartInstall();
            }

            //Thread.Sleep(5000);
            //this.Close();

        }


        private async Task StartInstall()
        {
            if (!IsStarted)
            {
                if (IsLoaded)
                {
                    IsStarted = true;

                    if (RequestQueryString["mode"] != null && RequestQueryString["mode"] == "install")
                    {
                        //PackageFamilyNameParameter
                    }

                    try
                    {
                        appInfo = new AppInfo();
                        appInfo.Version = appVersion;
                        appInfo.Name = appName;
                        appInfo.Description = appDescription;
                        appInfo.BackgroundColor = appBackground;
                        appInfo.PackageFamilyName = PackageFamilyNameParameter;
                        appInfo.ImageUrl = appImage;

                        _installer = new Installer(OrgPortalUrlParameter,
                                                   appInfo,
                                                   this.notifyIcon1,
                                                   txtLogOutput
                                                  );

                        //_installer.StartOrgPortalFileWatcher(_installer.OrgPortalPackageTempPath);
                        //Thread.Sleep(150);
                        await Task.Delay(150);

                        SetProgress(20);

                        lblStatus3.Text = "Connected @ " + _installer.ServiceURI;
                        //toolStripMainStatusLabel.Text = "Connected @ " + _installer.ServiceURI;
                        //toolStripStatusAdicional.Text = " & " + _installer.OrgPortalPackageTempPath;
                        //_installer.StartFileWatcher2(_installer.CachePath);

                        await _installer.ProcessRequest(this.FilePath);

                        SetProgress(35);

                        //if (!_installer.IsAutoInstalling)
                        //{
                        //    _installer.ProcessExistingCacheRequestFiles();
                        //}
                        //await RefreshInstalledApps();
                        //Thread.Sleep(2500);
                        await Task.Delay(2500);

                        if (!_installer.IsProcessing && !_installer.IsInstalling && !_installer.IsAutoInstalling)
                        {
                            SetProgress(100);
                            progressBarApp.Value = 100;

                            var lblStatus1Buffer = lblStatus1.Text; // += " - Completed";
                            lblStatus1.Text = lblStatus1Buffer + " - Completed - Click on the image for more info";

                            btnCancel.Text = "Close";
                            await Task.Delay(1000);

                            double progressoEspera = 1;
                            double segundosPassados = 0;
                            double delayInSecsBeforeDownload = 30;

                            progressBarClose.Visible = true;

                            while (segundosPassados < delayInSecsBeforeDownload)
                            {
                                segundosPassados++;
                                await Task.Delay(1000);
                                progressoEspera = Math.Round((segundosPassados / Convert.ToDouble(delayInSecsBeforeDownload)) * 100);
                                btnCancel.Text = string.Format(" Closing em {0}s ", 
                                                               (delayInSecsBeforeDownload - Convert.ToInt32(segundosPassados))
                                                              );
                                progressBarClose.Value = Convert.ToInt16(Math.Round(progressoEspera));
                            }

                            //btnCancel.Text = "Fechar (" + 1 + ")";
                            await Task.Delay(1000);
                            this._reallyClose = true;
                            this.Close();
                        }
                        //btnCancel.Text += "*";

                    }
                    catch (Exception ex)
                    {
                        ExceptionLogger.LogException(ex);
                        lblStatus3.Text = ExceptionLogger.CreateMiniErrorMessage(ex);
                        TaskbarProgress.SetState(this.Handle, TaskbarProgress.TaskbarStates.Error);
                    }
                }
            }
            else
            {
                await _installer.StopPackageTempFileWatcher();
                _installer = null;
                IsStarted = false;
            }

            //txtOrgPortalUrl.Enabled = !IsStarted;
        }

        private void SetProgress(int value)
        {
            TaskbarProgress.SetValue(this.Handle, value, 100);
            progressBarApp.Value = value;
        }

        async void AppRequest_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_reallyClose)
            {
                this.ShowInTaskbar = false;
                this.Visible = false;
                if (_installer != null)
                {
                    await _installer.StopCacheFileWatcher();
                    await _installer.StopPackageTempFileWatcher();
                    await _installer.StopExistingAppsFileWatcher();
                    _installer = null;
                }
            }
            else
            {
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
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
            this.Visible = true;
            this.BringToFront();
        }

        private void OrgPortalStatus()
        {
            string extraInfo = "";
            //extraInfo += "OrgPortalUrl[" + txtOrgPortalUrl.Text + "]";
            //extraInfo += "PackageFamilyName[" + txtPackageFamilyName.Text + "]";
            if (_installer != null)
            {
                extraInfo += "serviceURI[" + _installer.ServiceURI + "]";
                extraInfo += "PackageTempPath[" + _installer.OrgPortalPackageTempPath + "]";
                extraInfo += "PackageLocalPath[" + _installer.OrgPortalPackageLocalPath + "]";
            }
            extraInfo += Environment.NewLine;

            string fileName = "OrgPortalMonitor-StartupInfo-T" + Environment.CurrentManagedThreadId.ToString() + ".log";
            ExceptionLogger.ExtraLog(extraInfo, fileName);
            //ExceptionLogger.PurgeLogFiles();
        }

        public void OutputException(Exception ex)
        {
            try
            {
                if (_installer != null)
                {
                    ExceptionLogger.LogException(ex, _installer.OrgPortalPackageTempPath);
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
                    ExceptionLogger.LogException(exc, _installer.OrgPortalPackageTempPath);
                }
                else
                {
                    ExceptionLogger.LogException(exc);
                }

            }

        }

        public string AppVersion { get; set; }
        private Installer _installer;
        private bool _reallyClose;
        //private System.IO.FileSystemWatcher fileSystemWatcher1;
        //private System.IO.FileSystemWatcher fileSystemWatcher2;
        private string PackageFamilyNameParameter;
        private string OrgPortalUrlParameter;
        private string appVersion;
        private string appName;
        private string appDescription;
        private string appBackground;
        private AppInfo appInfo;
        private string appImage;

        public bool IsStarted { get; set; }

        public bool IsLoaded { get; set; }

        public string FilePath { get; set; }

        public System.Collections.Specialized.NameValueCollection RequestQueryString { get; set; }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //var dialog = MessageBox.Show("Are you sure you want to cancel?", "OrgPortal App Install", MessageBoxButtons.YesNo);
            //if (dialog == System.Windows.Forms.DialogResult.Yes)
            if (MessageBox.Show("Are you sure you want to cancel?", "OrgPortal App Install", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                if (_installer != null)
                {
                    _installer.CancelExecution = true;
                    _installer = null;
                }
                this._reallyClose = true;
                this.Close();
            }
            else
            {
                this._reallyClose = false;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = !pictureBox1.Visible;
            pictureBox2.Visible = !pictureBox1.Visible;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = !pictureBox1.Visible;
            pictureBox2.Visible = !pictureBox1.Visible;
        }
    }
}
