using OrgPortalMonitor.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Threading;
using OrgPortalMonitor.Common;
using RunProcessAsTask;
using System.Windows.Forms;

namespace OrgPortalMonitor
{
    public class Installer
    {
        private /*static readonly*/ string _serviceURI = string.Empty; //"http://orgportal/api/";
        public string ServiceURI
        {
            get { return _serviceURI; }
            set { _serviceURI = value; }
        }
        public string OrgPortalPackageFamilyName = "OrgPortal_m64ba5zfsemg0";
        //private string OrgPortalUrlParameter;
        //private string PackageFamilyNameParameter;
        //private System.Windows.Forms.TextBox txtLogOutput;
        public string OrgPortalPackageTempPath { get; set; }
        public string OrgPortalPackageLocalPath { get; set; }
        public string CachePath { get; set; }

        public System.Windows.Forms.NotifyIcon NotifyIcon { get; set; }
        public System.Windows.Forms.TextBox Output { get; set; }
        public FileSystemWatcher OrgPortalWatcher { get; set; }
        public FileSystemWatcher CacheWatcher { get; set; }

        private void SetCachePath()
        {
            CachePath = System.Environment
                        .GetFolderPath(System.Environment
                                       .SpecialFolder.InternetCache);

            if (!CachePath.EndsWith(@"\")) CachePath += @"\";
        }

        private void SetOrgPortalPackageTempPath()
        {
            //TempPath is where app install requests will be saved and also temporary app package files and logs will remain in place
            var orgPortalTempPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            if (!orgPortalTempPath.EndsWith(@"\")) orgPortalTempPath += @"\";
            orgPortalTempPath += @"Packages\" + OrgPortalPackageFamilyName + @"\TempState\";
            if (!System.IO.Directory.Exists(orgPortalTempPath))
            {
                System.IO.Directory.CreateDirectory(orgPortalTempPath);
            }
            this.OrgPortalPackageTempPath = orgPortalTempPath;
        }

        private void SetOrgPortalPackageLocalPath()
        {
            // LocalPath will be used for InstalledPackages.txt saving
            var orgPortalLocalPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            if (!orgPortalLocalPath.EndsWith(@"\")) orgPortalLocalPath += @"\";
            orgPortalLocalPath += @"Packages\" + OrgPortalPackageFamilyName + @"\LocalState\";
            if (!System.IO.Directory.Exists(orgPortalLocalPath))
            {
                System.IO.Directory.CreateDirectory(orgPortalLocalPath);
            }
            this.OrgPortalPackageLocalPath = orgPortalLocalPath;
        }

        #region Specific app install
        public Installer(string orgPortalUrl, AppInfo appInfo, System.Windows.Forms.NotifyIcon notifyIcon, System.Windows.Forms.TextBox txtLogOutput)
        {
            InstallerAppConstruct(orgPortalUrl, appInfo.Version, appInfo.Name, appInfo.Description, appInfo.PackageFamilyName, notifyIcon, txtLogOutput);
        }

        public Installer(string orgPortalUrl,
                         string orgPortalPackageFamilyName,
                         System.Windows.Forms.NotifyIcon notifyIcon,
                         System.Windows.Forms.TextBox output,
                         FileSystemWatcher orgPortalWatcher,
                         FileSystemWatcher cacheWatcher)
        {
            this.NotifyIcon = notifyIcon;
            this.NotifyIcon.ShowBalloonTip(500, "OrgPortal", "The OrgPortal monitor has started", System.Windows.Forms.ToolTipIcon.Info);

            if (!string.IsNullOrEmpty(orgPortalUrl))
            {
                _serviceURI = orgPortalUrl;
            }
            else
            {
                if (!string.IsNullOrEmpty(OrgPortalMonitor.Properties.Settings.Default.OrgPortalUrl))
                {
                    _serviceURI = OrgPortalMonitor.Properties.Settings.Default.OrgPortalUrl;
                }
            }

            this.Output = output;
            this.OrgPortalWatcher = orgPortalWatcher;
            this.CacheWatcher = cacheWatcher;

            //this.PackageFamilyName = orgPortalPackageFamilyName;
            //if (string.IsNullOrEmpty(PackageFamilyName))
            //{
            //    PackageFamilyName = OrgPortalPackageFamilyName;
            //}

            #region OrgPortalPackageTempPath
            SetOrgPortalPackageTempPath();
            #endregion

            #region PackageLocalPath
            SetOrgPortalPackageLocalPath();
            #endregion

            #region CachePath
            SetCachePath();
            #endregion

        }


        public Installer(string orgPortalUrl, string version, string name, string description, string packageFamilyName, System.Windows.Forms.NotifyIcon notifyIcon, System.Windows.Forms.TextBox txtLogOutput)
        {
            InstallerAppConstruct(orgPortalUrl, version, name, description, packageFamilyName, notifyIcon, txtLogOutput);

            //var appInfo = new AppInfo();
            //appInfo.Version = version;
            //appInfo.Name = name;
            //appInfo.Description = description;
            //appInfo.PackageFamilyName = packageFamilyName;
            //InstallerConstruct(orgPortalUrl, appInfo, notifyIcon, txtLogOutput);

        }

        private void InstallerConstruct(string orgPortalUrl, AppInfo appInfo, System.Windows.Forms.NotifyIcon notifyIcon, System.Windows.Forms.TextBox txtLogOutput)
        {
            InstallerAppConstruct(orgPortalUrl, appInfo.Version, appInfo.Name, appInfo.Description, appInfo.PackageFamilyName, notifyIcon, txtLogOutput);
        }

        private void InstallerAppConstruct(string orgPortalUrl, string version, string name, string description, string packageFamilyName, System.Windows.Forms.NotifyIcon notifyIcon, System.Windows.Forms.TextBox txtLogOutput)
        {
            this.NotifyIcon = notifyIcon;
            this.NotifyIcon.ShowBalloonTip(250, "OrgPortal", "The OrgPortal installer has started to install " + name + " v" + version, System.Windows.Forms.ToolTipIcon.Info);

            this.Output = txtLogOutput;
            this.PackageFamilyName = packageFamilyName;

            #region _serviceURI
            if (!string.IsNullOrEmpty(orgPortalUrl))
            {
                _serviceURI = orgPortalUrl;
            }
            else
            {
                if (!string.IsNullOrEmpty(OrgPortalMonitor.Properties.Settings.Default.OrgPortalUrl))
                {
                    _serviceURI = OrgPortalMonitor.Properties.Settings.Default.OrgPortalUrl;
                }
            }
            #endregion

            #region OrgPortalPackageTempPath
            SetOrgPortalPackageTempPath();
            #endregion

            #region PackageLocalPath
            SetOrgPortalPackageLocalPath();
            #endregion

            #region CachePath
            SetCachePath();
            #endregion

        }
        #endregion

        public async Task StartOrgPortalFileWatcher(string path)
        {
            if (OrgPortalWatcher != null)
            {
                IsWatchingOrgPortal = true;
                this.Output.AppendText("Monitor started at " + DateTime.Now + Environment.NewLine);
                this.Output.AppendText(System.Environment.NewLine + "Watching OrgPortal app TempState folder " + path + " for App Install Requests " + Environment.NewLine);

                this.OrgPortalWatcher.Path = path;
                OrgPortalWatcher.Created += Watcher_Created;

                await ProcessExistingOrgPortalRequestFiles();
            }
        }

        public async Task StartFileWatcher2(string path)
        {
            if (CacheWatcher != null)
            {
                IsWatchingCache = true;
                this.Output.AppendText(System.Environment.NewLine + "Watching Cache folder " + this.CachePath + " for Auto Install and Auto Update apps" + Environment.NewLine);

                this.CacheWatcher.Path = CachePath;
                CacheWatcher.Created += Watcher_Created;

                await ProcessExistingCacheRequestFiles();
            }
        }

        public async Task StartInstalledAppsFileWatcher()
        {
            this.Output.AppendText(System.Environment.NewLine + "Watching Apps TempState folder for App Update Requests " + Environment.NewLine);
            //this.Watcher.Path = path;
            //FileSystemWatcher appWatcher
            IsWatchingInstalledApps = true;
            WatcherList = new List<FileSystemWatcher>();
            if (InstalledAppList != null)
            {
                foreach (var app in InstalledAppList)
                {
                    if (app.PackageFamilyName != OrgPortalPackageFamilyName)
                    { //OrgPortal_m64ba5zfsemg0
                        FileSystemWatcher appWatcher = new FileSystemWatcher();
                        appWatcher.EnableRaisingEvents = true;
                        appWatcher.Path = appTempPath(app);
                        appWatcher.Filter = "*.rt2win";
                        appWatcher.Created += Watcher_Created;
                        WatcherList.Add(appWatcher);
                    }
                }
                await ProcessExistingAppsTempRequestFiles();
            }
        }

        public async Task ProcessExistingOrgPortalRequestFiles()
        {
            if (!IsInstalling)
            {
                var existingFiles = Directory.EnumerateFiles(this.OrgPortalPackageTempPath, "*.rt2win");
                if (existingFiles.Count() > 0)
                {
                    this.Output.AppendText(System.Environment.NewLine + "Processing OrgPortal app requests at " + this.OrgPortalPackageTempPath + Environment.NewLine);
                    foreach (var item in existingFiles)
                    {
                        await ProcessRequest(item);
                    }
                }
            }
        }

        public async Task ProcessExistingAppsTempRequestFiles()
        {
            if (!IsInstalling)
            {
                foreach (var app in InstalledAppList)
                {
                    if (app.PackageFamilyName != OrgPortalPackageFamilyName)
                    {
                        var tempPath = appTempPath(app);
                        var existingFiles = Directory.EnumerateFiles(tempPath, "*.rt2win");
                        if (existingFiles.Count() > 0)
                        {
                            this.Output.AppendText(System.Environment.NewLine + "Processing OrgPortal app requests at " + tempPath + Environment.NewLine);
                            foreach (var item in existingFiles)
                            {
                                await ProcessRequest(item);
                            }
                        }
                    }
                }
            }
        }

        private static string appTempPath(AppInfo app)
        {
            var tempPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            if (!tempPath.EndsWith(@"\")) tempPath += @"\";
            tempPath += @"Packages\" + app.PackageFamilyName + @"\TempState\";
            if (!System.IO.Directory.Exists(tempPath))
            {
                System.IO.Directory.CreateDirectory(tempPath);
            }
            return tempPath;
        }

        public async Task ProcessExistingCacheRequestFiles()
        {
            if (!IsAutoInstalling)
            {
                var existingFiles = Directory.EnumerateFiles(this.CachePath, "*.rt2win");
                if (existingFiles.Count() > 0)
                {
                    this.Output.AppendText(System.Environment.NewLine + "Processing Auto Install and Auto Update app requests at " + this.CachePath + Environment.NewLine);
                    foreach (var item in existingFiles)
                    {
                        await ProcessRequest(item);
                    }
                }
            }
        }

        async void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            System.Threading.Thread.Sleep(500);
            this.Output.AppendText(System.Environment.NewLine + "New app request detected at " + e.FullPath + Environment.NewLine);
            await ProcessRequest(e.FullPath);
        }

        public async Task StopPackageTempFileWatcher()
        {
            IsWatchingOrgPortal = false;
            if (OrgPortalWatcher != null)
            {
                OrgPortalWatcher.Created -= Watcher_Created;
                this.Output.AppendText(System.Environment.NewLine + "\n\nStop Watching OrgPortal Temp folder " + this.OrgPortalPackageTempPath + Environment.NewLine);
                this.NotifyIcon.ShowBalloonTip(500, "OrgPortal", "The OrgPortal monitor has stopped", System.Windows.Forms.ToolTipIcon.Info);
                //Watcher.Dispose();
            }
        }

        public async Task StopCacheFileWatcher()
        {
            IsWatchingCache = false;
            if (CacheWatcher != null)
            {
                CacheWatcher.Created -= Watcher_Created;
                this.Output.AppendText(System.Environment.NewLine + "\n\nStop Watching INet Cache folder " + this.CachePath + Environment.NewLine);
                this.NotifyIcon.ShowBalloonTip(500, "OrgPortal", "The OrgPortal Auto Install and Auto Update monitor has stopped", System.Windows.Forms.ToolTipIcon.Info);
                //Watcher.Dispose();
            }
        }

        public async Task StopExistingAppsFileWatcher()
        {
            IsWatchingInstalledApps = false;
            if (WatcherList != null)
            {
                foreach (var appWatcher in WatcherList)
                {
                    appWatcher.Created -= Watcher_Created;
                }
                WatcherList.Clear();
                this.Output.AppendText(System.Environment.NewLine + "\n\nStop Watching Apps TempState folder for App Update Requests " + Environment.NewLine);
                this.NotifyIcon.ShowBalloonTip(500, "OrgPortal", "OrgPortal is stopping monitor installed apps update requests", System.Windows.Forms.ToolTipIcon.Info);
            }
        }
        public async Task ProcessRequest(string inputFilePath)
        {
            if (!IsProcessing)
            {
                IsProcessing = true;

                var logfilePath = inputFilePath.Replace(".rt2win", ".log");
                var outputDoc = new XElement("request");
                outputDoc.Add(new XElement("requestFile", inputFilePath));
                this.Output.AppendText(Environment.NewLine + ">> Processing " + inputFilePath + Environment.NewLine + Environment.NewLine);

                try
                {
                    var input = File.ReadAllLines(inputFilePath);
                    string command = string.Empty;

                    if (input.Count() > 0)
                    {
                        command = input[0];
                        outputDoc.Add(new XElement("command", command));
                    }

                    if (command == "install")
                    {
                        //var saveAt = input[9];
                        await ProcessInstallRequest(outputDoc, input[1], input[3], input[5], input[7] /*, (saveAt != null ? input[9] : string.Empty)*/);
                    }
                    else if (command == "getDevLicense")
                    {
                        ShowDevLicense(outputDoc);
                    }
                    else
                    {
                        outputDoc.Add(new XElement("success", "false"));
                        outputDoc.Add(new XElement("error", "Invalid command"));
                        this.Output.AppendText("Invalid command: " + command + Environment.NewLine);
                        this.Output.AppendText("  Input file ignored" + Environment.NewLine);
                        this.Output.AppendText("** FAILED " + Environment.NewLine);
                    }
                }
                catch (Exception ex)
                {
                    outputDoc.Add(new XElement("success", "false"));
                    outputDoc.Add(new XElement("error", ex.Message));
                    this.Output.AppendText("UNEXPECTED EXCEPTION " + Environment.NewLine);
                    this.Output.AppendText(ex.ToString() + Environment.NewLine);
                    this.Output.AppendText("** FAILED " + Environment.NewLine);
                    IsInstalling = false;
                    ExceptionLogger.LogException(ex, CachePath);
                }
                finally
                {
                    //File.Delete(inputFilePath);
                    var arquivoPartes = inputFilePath.Split('.');
                    var extensao = arquivoPartes[arquivoPartes.Length - 1];
                    File.Move(inputFilePath, inputFilePath.Replace("." + extensao, ".txt"));
                    this.Output.AppendText(Environment.NewLine + " << Processed " + inputFilePath + Environment.NewLine);
                    this.Output.AppendText(Environment.NewLine);
                    this.Output.AppendText(Environment.NewLine);
                    File.WriteAllText(logfilePath, outputDoc.ToString());
                    IsProcessing = false;
                }
            }
        }

        private async Task ProcessInstallRequest(XElement outputDoc,
                                                     string appxUrl,
                                                     string appxFile,
                                                     string certificateUrl,
                                                     string certificateFile/*,
                                                     string _workPath*/)
        {
            var appUriSegments = new System.Uri(appxUrl).Segments;
            //var appFileName = appUriSegments[appUriSegments.Length - 1] + ".appx";
            //var workPath = string.IsNullOrEmpty(_workPath) ? CachePath : _workPath;
            var workPath = CachePath;
            var appFileName = appxFile;
            var appFilePath = workPath + appFileName;
            var certificateFilePath = workPath + certificateFile;

            this.Output.AppendText("Starting Install Request at " + DateTime.Now + Environment.NewLine);
            this.Output.AppendText("App file name " + appFileName + Environment.NewLine);
            this.Output.AppendText("from " + appxUrl + Environment.NewLine);
            if (!string.IsNullOrEmpty(certificateFile))
            {
                this.Output.AppendText("Certificate file: " + certificateFile + Environment.NewLine);
                this.Output.AppendText("from " + certificateUrl + Environment.NewLine);
            }
            this.Output.AppendText("Saving at " + workPath + Environment.NewLine);

            var result = new InstallResult();
            IsInstalling = true;

            if (!string.IsNullOrEmpty(certificateFile) &&
                (certificateFile.Contains(".pfx") ||
                 certificateFile.Contains(".cer")))
            {
                //result.Error = DownloadFile(certificateUrl, certificateFilePath);
                result.Error = await DownloadFileTaskAsync(certificateUrl, certificateFilePath);

                if (string.IsNullOrWhiteSpace(result.Error))
                {
                    this.Output.AppendText("Installing " + certificateFilePath + " ... " + Environment.NewLine);
                    //result = await InstallCertificateAsync(certificateFilePath);
                    //TODO:await Task.Run(.... result = InstallCertificate(certificateFilePath) ....)
                    result = InstallCertificate(certificateFilePath);
                }
                else
                {
                    this.Output.AppendText("Error downloading certificate at " + certificateUrl + " : " + Environment.NewLine + result.Error + Environment.NewLine);
                }
            }

            if (string.IsNullOrWhiteSpace(result.Error))
            {
                if (!string.IsNullOrEmpty(result.Output))
                {
                    this.Output.Text += Environment.NewLine + result.Output;
                }

                //result.Error = DownloadFile(appxUrl, appFilePath);
                result.Error = await DownloadFileTaskAsync(appxUrl, appFilePath);

                if (string.IsNullOrWhiteSpace(result.Error))
                {
                    this.Output.AppendText("Installing " + appFilePath + " ... " + Environment.NewLine);

                    result = await InstallAppx(appFilePath);

                    if (!string.IsNullOrWhiteSpace(result.Error))
                    {
                        this.Output.Text += "** Error installing appx: " + Environment.NewLine + result.Error;
                    }

                    if (!string.IsNullOrEmpty(result.Output))
                    {
                        this.Output.Text += Environment.NewLine + result.Output;
                    }
                }
                else
                {
                    this.Output.AppendText("Error downloading appx at " + appxUrl + " : " + Environment.NewLine + result.Error + Environment.NewLine);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(result.Error))
                {
                    this.Output.Text += "** Error installing certificate: " + Environment.NewLine + result.Error;
                }
            }

            if (string.IsNullOrWhiteSpace(result.Error))
            {
                outputDoc.Add(new XElement("success", "true"));
                outputDoc.Add(new XElement("filePath", appFilePath));
                NotifyIcon.ShowBalloonTip(500, "OrgPortal ", appxUrl + " installed with sucess", System.Windows.Forms.ToolTipIcon.Info);
                this.Output.AppendText("** SUCCESS " + Environment.NewLine);

                //await RefreshInstalledAppList();
            }
            else
            {
                outputDoc.Add(new XElement("success", "false"));
                outputDoc.Add(new XElement("filePath", appFilePath));
                outputDoc.Add(new XElement("error", result.ToString()));
                NotifyIcon.ShowBalloonTip(500, "OrgPortal", appxUrl + " not installed with errors", System.Windows.Forms.ToolTipIcon.Warning);
                this.Output.AppendText("** FAILED " + Environment.NewLine);
                this.Output.AppendText(result.ToString() + Environment.NewLine);
            }
            IsInstalling = false;

        }

        public async Task<InstallResult> UninstallApp(string packageFamilyName)
        {
            var tcs = new TaskCompletionSource<InstallResult>();

            var result = new InstallResult();

            try
            {
                var sb = new StringBuilder();
                sb.Append(@"get-appxpackage -Name *" + packageFamilyName + @"* | Remove-appxpackage ");

                var process = new System.Diagnostics.Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;

                process.StartInfo.FileName = "powershell.exe";
                process.StartInfo.Arguments = sb.ToString();

                process.StartInfo.CreateNoWindow = false;
                //process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;


                //process.Start();
                var pr = await ProcessEx.RunAsync(process.StartInfo);

                //pr.Process.Exited += (sender, args) =>
                //{

                var stdout = pr.StandardOutput;
                var stderr = pr.StandardError;

                result.Output = stdout.Join(Environment.NewLine);
                result.Error = stderr.Join(Environment.NewLine);

                tcs.SetResult(result);

                await Task.Delay(2000);

                //    process.Dispose();
                //};

                //var stdout = process.StandardOutput;
                //var stderr = process.StandardError;

                //result.Output = stdout.ReadToEnd();
                //result.Error = stderr.ReadToEnd();

                //if (!process.HasExited)
                //{
                //    process.Kill();
                //}
                //stdout.Close();
                //stderr.Close();
            }
            catch (Exception ex)
            {
                if (string.IsNullOrWhiteSpace(result.Error))
                {
                    result.Error = ex.Message;
                }
                else
                {
                    result.Error += Environment.NewLine + ex.Message;
                }
                tcs.SetResult(result);
                ExceptionLogger.LogException(ex, CachePath);

            }
            //await GetInstalledPackages();

            //return result;
            return await tcs.Task;

        }

        public async Task<InstallResult> InstallAppx(string appxFilePath)
        {
            await RefreshDevLicenseOutput();

            //TODO: Check if AllowAllTrustedApps == 1, if not, set it.
            //HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\Appx\AllowAllTrustedApps = 1

            if (!DevLicenseEnabled)
            {
                ShowDevLicense();
                await RefreshDevLicenseOutput();
            }

            var tcs = new TaskCompletionSource<InstallResult>();
            result = new InstallResult();

            if (DevLicenseEnabled)
            {
                var process = new System.Diagnostics.Process
                {
                    EnableRaisingEvents = true,
                    StartInfo = { FileName = "powershell.exe" }
                };

                /*var*/
                try
                {
                    var sb = new StringBuilder();
                    sb.Append(@"add-appxpackage ");
                    sb.Append(appxFilePath);
                    sb.Append(" -ForceApplicationShutdown");

                    //TODO: Implement dependencies:
                    // –DependencyPath .\Dependencies\Microsoft.WinJS.1.0.RC.appx

                    // var process = new System.Diagnostics.Process();

                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.RedirectStandardOutput = true;

                    process.StartInfo.FileName = "powershell.exe";
                    process.StartInfo.Arguments = sb.ToString();

                    process.StartInfo.CreateNoWindow = false;
                    //process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;

                    //process.Start();
                    var pr = await ProcessEx.RunAsync(process.StartInfo);

                    //var stdout = process.StandardOutput;
                    //var stderr = process.StandardError;

                    var stdout = pr.StandardOutput;
                    var stderr = pr.StandardError;

                    result.Output = stdout.Join(Environment.NewLine);
                    result.Error = stderr.Join(Environment.NewLine);

                    //pr.Process.Exited += (sender, args) =>
                    //{
                    tcs.SetResult(result);
                    //    process.Dispose();
                    //};

                    //if (!process.HasExited)
                    //{
                    //    process.Kill();
                    //}

                    //stdout.Close();
                    //stderr.Close();
                }
                catch (Exception ex)
                {
                    if (string.IsNullOrWhiteSpace(result.Error))
                    {
                        result.Error = ex.Message;
                    }
                    else
                    {
                        result.Error += Environment.NewLine + ex.Message;
                    }

                    tcs.SetResult(result);
                    //process.Dispose();

                    ExceptionLogger.LogException(ex, CachePath);
                }
                finally
                {
                    File.Delete(appxFilePath);
                }
            }
            else
            {
                result.Error = "** ERROR ** Unable to install appx - Verify if your Developer license is enabled or you have sideloading key installed.";
                tcs.SetResult(result);
            }
            //await GetInstalledPackages();
            //return result;
            return await tcs.Task;
        }

        /*
         * importpfx.exe -f "somePfx.pfx" -p "somePassword" -t MACHINE -s "TRUSTEDPEOPLE"
         */
        public async Task<InstallResult> InstallCertificateAsync(string certificateFilePath)
        {
            var tcs = new TaskCompletionSource<InstallResult>();

            /*var*/
            result = new InstallResult();

            try
            {
                var sb = new StringBuilder();

                if (certificateFilePath.Contains(".pfx"))
                {
                    sb.Append(@"importpfx.exe -f ");
                    sb.Append(@"""");
                    sb.Append(certificateFilePath);
                    sb.Append(@"""");
                    sb.Append(@" -p """" -t MACHINE -s ""TRUSTEDPEOPLE"" ");
                }
                else if (certificateFilePath.Contains(".cer"))
                {
                    //Certutil -addstore -f "TRUSTEDPEOPLE" "someCertificate.cer"
                    //C:\Temp>certutil -addstore -f "TRUSTEDPEOPLE" .\Agile.WindowsApp_StoreKey.cer TRUSTEDPEOPLE
                    sb.Append(@"Certutil -addstore -f ");
                    sb.Append(@"""");
                    sb.Append("TRUSTEDPEOPLE");
                    sb.Append(@""" """);
                    sb.Append(certificateFilePath);
                    sb.Append(@"""");
                }

                //var startInfo = new ProcessStartInfo(pathToConsoleApp, arguments);
                //var cancellationToken = new CancellationTokenSource((60).Token;
                //var task = ProcessEx.RunAsync(startInfo, cancellationToken); 

                var process = new System.Diagnostics.Process();
                process.EnableRaisingEvents = true;

                process.StartInfo.UseShellExecute = true;
                //process.StartInfo.RedirectStandardError = true;
                //process.StartInfo.RedirectStandardOutput = true;

                process.StartInfo.FileName = "cmd.exe";
                //process.StartInfo.Arguments = "dir /w"; //sb.ToString()/* + " | more"*/;
                process.StartInfo.Arguments = " /c " + sb.ToString()/* + " | more"*/;
                process.StartInfo.Verb = "runas";

                process.StartInfo.CreateNoWindow = false;
                //process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;

                process.ErrorDataReceived += process_ErrorDataReceived;
                process.OutputDataReceived += process_OutputDataReceived;
                //process.EnableRaisingEvents = true;
                //process.Start();
                //process.WaitForExit();

                var pr = await ProcessEx.RunAsync(process.StartInfo);

                //process.BeginOutputReadLine();
                //process.BeginErrorReadLine();
                //var stdout = process.StandardOutput;
                //var stderr = process.StandardError;
                //result.Output = stdout.ReadToEnd();
                //result.Error = stderr.ReadToEnd();

                var stdout = pr.StandardOutput;
                var stderr = pr.StandardError;

                result.Output = stdout.Join(Environment.NewLine);
                result.Error = stderr.Join(Environment.NewLine);

                //pr.Process.Exited += (sender, args) =>
                //{
                tcs.SetResult(result);
                //    process.Dispose();
                //};

                //if (!process.HasExited)
                //    process.Kill();

                //stdout.Close();
                //stderr.Close();
            }
            catch (Exception ex)
            {
                if (string.IsNullOrWhiteSpace(result.Error))
                {
                    result.Error = ex.Message;
                }
                else
                {
                    result.Error += Environment.NewLine + ex.Message;
                }
                ExceptionLogger.LogException(ex, CachePath);
            }
            finally
            {
                File.Delete(certificateFilePath);
            }

            //return result;
            return await tcs.Task;

        }

        public InstallResult InstallCertificate(string certificateFilePath)
        {
            /*var*/
            result = new InstallResult();

            try
            {
                var sb = new StringBuilder();

                if (certificateFilePath.Contains(".pfx"))
                {
                    sb.Append(@"importpfx.exe -f ");
                    sb.Append(@"""");
                    sb.Append(certificateFilePath);
                    sb.Append(@"""");
                    sb.Append(@" -p """" -t MACHINE -s ""TRUSTEDPEOPLE"" ");
                }
                else if (certificateFilePath.Contains(".cer"))
                {
                    //Certutil -addstore -f "TRUSTEDPEOPLE" "someCertificate.cer"
                    //C:\Temp>certutil -addstore -f "TRUSTEDPEOPLE" .\Agile.WindowsApp_StoreKey.cer TRUSTEDPEOPLE
                    //sb.Append(@"Certutil -addstore -f ");
                    sb.Append(@"Certutil ");
                    sb.Append(@" -addstore -f ");
                    sb.Append(@"""");
                    sb.Append("TRUSTEDPEOPLE");
                    sb.Append(@""" """);
                    sb.Append(certificateFilePath);
                    sb.Append(@"""");
                }

                string tempFile = Path.GetTempFileName();

                var process = new System.Diagnostics.Process();
                process.EnableRaisingEvents = true;

                process.StartInfo.UseShellExecute = true;
                //process.StartInfo.RedirectStandardError = true;
                //process.StartInfo.RedirectStandardOutput = true;

                //process.StartInfo.FileName = "certutil";
                process.StartInfo.FileName = "cmd.exe";
                //process.StartInfo.Arguments = "dir /w"; //sb.ToString()/* + " | more"*/;
                //process.StartInfo.Arguments = " /c " + sb.ToString()/* + " | more"*/;
                //process.StartInfo.Arguments = " /c " + sb.ToString() + "\" > \"" + tempFile + "\"\"";
                //process.StartInfo.Arguments = sb.ToString() + " > \"" + tempFile + "\"";
                process.StartInfo.Arguments = " /c " + sb.ToString() + " > \"" + tempFile + "\"";

                process.StartInfo.Verb = "runas";

                process.StartInfo.CreateNoWindow = false;
                //process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;

                process.ErrorDataReceived += process_ErrorDataReceived;
                process.OutputDataReceived += process_OutputDataReceived;

                process.Start();
                process.WaitForExit();

                string output = File.ReadAllText(tempFile);
                File.Delete(tempFile);

                result.Output = output;
                //process.BeginOutputReadLine();
                //process.BeginErrorReadLine();
                //var stdout = process.StandardOutput;
                //var stderr = process.StandardError;
                //result.Output = stdout.ReadToEnd();
                //result.Error = stderr.ReadToEnd();

                ////var stdout = pr.StandardOutput;
                ////var stderr = pr.StandardError;
                ////result.Output = stdout.Join(Environment.NewLine);
                ////result.Error = stderr.Join(Environment.NewLine);
                ////pr.Process.Exited += (sender, args) =>
                ////{
                ////    process.Dispose();
                ////};

                //if (!process.HasExited)
                //     process.Kill();

                //stdout.Close();
                //stderr.Close();
            }
            catch (Exception ex)
            {
                if (string.IsNullOrWhiteSpace(result.Error))
                {
                    result.Error = ex.Message;
                }
                else
                {
                    result.Error += Environment.NewLine + ex.Message;
                }
                ExceptionLogger.LogException(ex, CachePath);
            }
            finally
            {
                File.Delete(certificateFilePath);
            }

            return result;
        }


        void process_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            result.Output += e.Data + "-" + Environment.NewLine;
        }

        void process_ErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            result.Error += e.Data + "-" + Environment.NewLine;
        }

        public async Task<InstallResult> GetInstalledPackages()
        {
            await Task.Delay(4000);

            var tcs = new TaskCompletionSource<InstallResult>();
            var result = new InstallResult();

            var fileName = "InstalledPackages.txt";
            var process = new System.Diagnostics.Process();
            var sb = new StringBuilder();
            try
            {
                sb.Append(@"get-appxpackage > " + this.OrgPortalPackageLocalPath + fileName);

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;

                process.StartInfo.FileName = "powershell.exe";
                process.StartInfo.Arguments = sb.ToString();

                process.StartInfo.CreateNoWindow = false;
                //process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;

                //process.Start();
                var pr = await ProcessEx.RunAsync(process.StartInfo);

                var stdout = pr.StandardOutput;
                var stderr = pr.StandardError;

                result.Output = stdout.Join(Environment.NewLine);
                result.Error = stderr.Join(Environment.NewLine);

                //pr.Process.Exited += (sender, args) =>
                //{
                tcs.SetResult(result);
                //    process.Dispose();
                //};


            }
            catch (Exception ex)
            {
                if (string.IsNullOrWhiteSpace(result.Error))
                {
                    result.Error = ex.Message;
                }
                else
                {
                    result.Error += Environment.NewLine + ex.Message;
                }
                tcs.SetResult(result);
                ExceptionLogger.LogException(ex);
            }

            try
            {
                if (process != null)
                {
                    //while (!process.HasExited)
                    //{
                    //    System.Threading.Thread.Sleep(15);
                    //}

                    System.IO.File.Copy(Path.Combine(this.OrgPortalPackageLocalPath, fileName),
                                        Path.Combine(this.CachePath, fileName), true);
                }

            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
            }

            return await tcs.Task;

        }

        public string DownloadFile(string fileUrl, string localFilePath)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(fileUrl, localFilePath);
                }
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Download of " + fileUrl + " failed.");
                sb.AppendLine(ex.Message);

                ExceptionLogger.LogException(ex, CachePath);

                return sb.ToString();
            }
            return null;
        }

        public async Task<string> DownloadFileTaskAsync(string fileUrl, string localFilePath)
        {
            try
            {
                using (var client = new WebClient())
                {
                    await client.DownloadFileTaskAsync(fileUrl, localFilePath);
                }
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Download of " + fileUrl + " failed.");
                sb.AppendLine(ex.Message);

                ExceptionLogger.LogException(ex, CachePath);

                return sb.ToString();
            }
            return null;
        }

        public void ShowDevLicense()
        {
            ShowDevLicense(new XElement("request"));
        }

        public void ShowDevLicense(XElement outputDoc)
        {
            RunAsAdmin("powershell.exe", @"Show-WindowsDeveloperLicenseRegistration");
            outputDoc.Add(new XElement("success", "true"));
        }

        public async Task UnregisterDevLicense()
        {
           await UnregisterDevLicense(new XElement("request"));
        }

        public async Task UnregisterDevLicense(XElement outputDoc)
        {
            var result = await RunAsAdminWithResult("powershell.exe", @"Unregister-WindowsDeveloperLicense -Force");

            if (string.IsNullOrEmpty(result.Error))
            {
                if (!string.IsNullOrEmpty(result.Output))
                {
                    MessageBox.Show("" + result.Output);

                    this.Output.Text += System.Environment.NewLine +
                                        "UnregisterDevLicense: " + result.Output + System.Environment.NewLine;
                }
            }
            else
            {
                //if (string.IsNullOrEmpty(result.Output))
                //{
                //    MessageBox.Show("No developer license installed");
                //}
                this.Output.Text += System.Environment.NewLine +
                                    "UnregisterDevLicense Error:  " + result.Error +
                                    System.Environment.NewLine +
                                    (!string.IsNullOrEmpty(result.Output) ?
                                     "UnregisterDevLicense Output: " + result.Output +
                                     System.Environment.NewLine : "");
            }
            outputDoc.Add(new XElement("success", "true"));
        }

        private async Task RefreshDevLicenseOutput()
        {
            DevLicenseOutput = await GetDevLicense();
            DevLicenseEnabled = !string.IsNullOrEmpty(DevLicenseOutput);
        }

        public async Task<string> GetDevLicense()
        {
            return await GetDevLicense(new XElement("request"));
        }

        public async Task<string> GetDevLicense(XElement outputDoc)
        {
            //bool returnVal = false;
            string returnVal = string.Empty;
            var result = await RunAsAdminWithResult("powershell.exe", @"Get-WindowsDeveloperLicense");
            if (string.IsNullOrEmpty(result.Error))
            {
                //result.Output = result.Output.Split(' ').Join(Environment.NewLine);
                //MessageBox.Show("" + result.Output);
                this.Output.Text += System.Environment.NewLine + 
                                    result.Output + System.Environment.NewLine;
                //returnVal = true;
                returnVal = result.Output;
            }
            else
            {
                //if (string.IsNullOrEmpty(result.Output))
                //{
                //    MessageBox.Show("No developer license installed");
                //}
                this.Output.Text += System.Environment.NewLine + 
                                    "GetDevLicense Error:  " + result.Error + 
                                    System.Environment.NewLine +
                                    (!string.IsNullOrEmpty(result.Output) ?
                                     "GetDevLicense Output: " + result.Output + 
                                     System.Environment.NewLine : "");
                //returnVal = false;
                returnVal = string.Empty;
            }
            outputDoc.Add(new XElement("success", "true"));
            return returnVal;
        }
        
        public async Task AutoInstallUpdateApps()
        {
            if (!IsProcessing && !IsInstalling)
            {
                IsAutoInstalling = true;
                this.Output.AppendText(System.Environment.NewLine + "Auto-install and auto-update apps... " + System.Environment.NewLine);

                ServerAppList = await GetRemoteAppList();
                DistinctServerAppList = await GetRemoteDistinctAppList(ServerAppList);

                await RefreshInstalledAppList();

                this.Output.AppendText(DistinctServerAppList.Count + " distinct apps available on server... " + System.Environment.NewLine);

                foreach (var serverApp in DistinctServerAppList)
                {
                    var installedApp = 
                        InstalledAppList.Where(
                            a => a.PackageFamilyName == serverApp.PackageFamilyName 
                        ).FirstOrDefault();

                    if ((installedApp != null                     && 
                         serverApp.InstallMode.StartsWith("Auto") && 
                         UpdateAvailable(serverApp, installedApp)   ) ||
                        (installedApp == null                && 
                         serverApp.InstallMode == "AutoInstall"))
                    {
                        if (serverApp.PackageFamilyName == OrgPortalPackageFamilyName) //OrgPortal_m64ba5zfsemg0
                        { // Only OrgPortal_m64ba5zfsemg0 should be installed in a different directory otherwise the appx install will try to delete files being used in the installation of the own app

                            this.Output.AppendText(System.Environment.NewLine + "** Package '" + OrgPortalPackageFamilyName + "' is available to install... " + System.Environment.NewLine);

                        }
                        var SavePath = CachePath; 

                        string requestFileName = SavePath 
                                                 + System.Guid.NewGuid().ToString() + ".rt2win";

                        await RequestApp(serverApp, SavePath, requestFileName);

                        //if (serverApp.PackageFamilyName == OrgPortalPackageFamilyName)
                        //{
                            await ProcessRequest(requestFileName); 
                        //}
                    }
                }

                this.Output.AppendText("Done with Server Auto Install and Auto Update Apps checking... " + System.Environment.NewLine);
                //Ideally here we would restore PackageTempPath and GetInstalledPackages but we are no sure that it finished
                //RestorePackageTempPathIfChanged();

                //await RefreshInstalledAppList();

                IsAutoInstalling = false;

            }
            //GetInstalledPackages();

        }

        public async Task RefreshInstalledAppList()
        {
            InstalledAppList = new List<AppInfo>();
            InstalledAppList = await GetInstalledApps(ServerAppList);
        }

        public static async Task RequestApp(AppInfo serverApp, string SavePath, string requestFileName)
        {
            if (serverApp != null)
            {
                var requestFile = System.IO.File.CreateText(requestFileName);
                using (requestFile)
                {
                    await requestFile.WriteLineAsync("install");
                    await requestFile.WriteLineAsync(serverApp.AppxUrl);
                    await requestFile.WriteLineAsync("appxFile");
                    await requestFile.WriteLineAsync(serverApp.PackageFile);
                    await requestFile.WriteLineAsync("certificateUrl");
                    await requestFile.WriteLineAsync(serverApp.CertificateUrl);
                    await requestFile.WriteLineAsync("certificateFile");
                    await requestFile.WriteLineAsync(serverApp.CertificateFile);
                    await requestFile.WriteLineAsync("saveAt");
                    await requestFile.WriteLineAsync(SavePath);
                    await requestFile.WriteLineAsync("version");
                    await requestFile.WriteLineAsync(serverApp.Version);
                    await requestFile.WriteLineAsync("name");
                    await requestFile.WriteLineAsync(serverApp.Name);
                    await requestFile.WriteLineAsync("description");
                    await requestFile.WriteLineAsync(serverApp.Description);
                    await requestFile.WriteLineAsync("backgroundColor");
                    await requestFile.WriteLineAsync(serverApp.BackgroundColor);
                    await requestFile.WriteLineAsync("imageUrl");
                    await requestFile.WriteLineAsync(serverApp.ImageUrl);

                    requestFile.Close();
                }
            }
        }


        private bool UpdateAvailable(AppInfo serverApp, AppInfo installedApp)
        {
            var result = false;
            var serverVersion = serverApp.Version.Split('.');
            var installedVersion = installedApp.Version.Split('.');
            if (int.Parse(serverVersion[0]) > int.Parse(installedVersion[0]))
                result = true;
            else if (int.Parse(serverVersion[1]) > int.Parse(installedVersion[1]))
                result = true;
            else if (int.Parse(serverVersion[2]) > int.Parse(installedVersion[2]))
                result = true;
            else if (int.Parse(serverVersion[3]) > int.Parse(installedVersion[3]))
                result = true;
            return result;
        }

        public async Task<List<AppInfo>> GetRemoteDistinctAppList(List<AppInfo> _remoteAppList)
        {
            List<AppInfo> remoteAppList = new List<AppInfo>();

            if (_remoteAppList == null || _remoteAppList.Count == 0)
            {
                remoteAppList = await GetRemoteAppList();
            }
            else
            {
                remoteAppList = _remoteAppList;
            }

            List<AppInfo> retorno = new List<AppInfo>();
            if (remoteAppList != null && remoteAppList.Count > 0)
            {
                try
                {
                    retorno = remoteAppList.OrderBy(a => a.PackageFamilyName)
                                            .ThenByDescending(a => a.Version)
                                            .GroupBy(a => a.PackageFamilyName)
                                            .Select(x => x.First()).ToList();
                    return retorno;
                }
                catch
                {
                    return retorno;
                }
                finally
                {
                }
            }
            else
            {
                return retorno;
            }
        }

        public async Task<List<AppInfo>> GetRemoteAppList()
        {
            var appList = new List<AppInfo>();
            var client = new System.Net.Http.HttpClient();
            if (!string.IsNullOrEmpty(ServiceURI))
            {
                var response = await client.GetAsync(ServiceURI + "Apps");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var reader = new Newtonsoft.Json.JsonTextReader(new StringReader(data));
                    var info = new List<Dictionary<string, string>>();
                    var infoItem = new Dictionary<string, string>();
                    try
                    {
                        while (reader.Read())
                        {
                            if (reader.TokenType == Newtonsoft.Json.JsonToken.EndObject)
                            {
                                info.Add(infoItem);
                                infoItem = new Dictionary<string, string>();
                            }
                            else if (reader.TokenType == Newtonsoft.Json.JsonToken.PropertyName && reader.Value != null)
                            {
                                var key = reader.Value.ToString();
                                reader.Read();
                                if (reader.Value != null)
                                    infoItem.Add(key, reader.Value.ToString());
                                else
                                    infoItem.Add(key, string.Empty);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Output.AppendText(ex.ToString());
                        ExceptionLogger.LogException(ex, OrgPortalPackageTempPath);
                    }
                    foreach (var obj in info)
                    {
                        var app = new AppInfo();
                        app.Name = obj["Name"] != null ? obj["Name"] : "";
                        app.DisplayName = obj["DisplayName"] != null ? obj["DisplayName"] : "";
                        app.PackageFamilyName = obj["PackageFamilyName"] != null ? obj["PackageFamilyName"] : "";
                        app.PackageFile = obj["PackageFile"] != null ? obj["PackageFile"] : "";
                        app.PackageName = obj["PackageName"] != null ? obj["PackageName"] : "";
                        app.AppxUrl = obj["AppxUrl"] != null ? obj["AppxUrl"] : "";
                        app.CertificateUrl = obj["CertificateUrl"] != null ? obj["CertificateUrl"] : "";
                        app.CertificateFile = obj["CertificateFile"] != null ? obj["CertificateFile"] : "";
                        app.Version = obj["Version"] != null ? obj["Version"] : "";
                        app.Description = obj["Description"] != null ? obj["Description"] : "";
                        app.ImageUrl = obj.ContainsKey("ImageUrl") && obj["ImageUrl"] != null ? obj["ImageUrl"] : "";
                        app.SmallImageUrl = obj.ContainsKey("SmallImageUrl") && obj["SmallImageUrl"] != null ? obj["SmallImageUrl"] : "";
                        app.InstallMode = obj["InstallMode"] != null ? obj["InstallMode"] : "AutoUpdate";
                        app.BackgroundColor = obj["BackgroundColor"] != null ? obj["BackgroundColor"] : "";
                        app.DateAdded = obj["DateAdded"] != null ? Convert.ToDateTime(obj["DateAdded"].ToString()) : DateTime.Now;
                        app.Category = obj["Category"] != null ? obj["Category"] : "";

                        appList.Add(app);
                    }
                }
            }
            return appList;
        }

        public async Task<List<AppInfo>> GetInstalledApps(List<AppInfo> apps)
        {
            if (InstalledAppList == null || InstalledAppList.Count == 0)  // avoid call powershell again if we already have installedAppList
            {
                await GetInstalledPackages();
            } // InstalledAppList will be cleared after AutoInstallUpdateApps call made by Tick timer and this method will be allowed to refresh the InstalledAppList again

            var appList = new List<AppInfo>();
            var filePath = OrgPortalPackageLocalPath + "InstalledPackages.txt";
            if (System.IO.File.Exists(filePath) && apps != null) 
            {
                var installedPackages = System.IO.File.ReadAllLines(filePath);
                var appLocal = new AppInfo();
                foreach (var line in installedPackages)
                {
                    if (line.StartsWith("Name"))
                        appLocal.Name = line.Substring(line.IndexOf(":") + 2);
                    if (line.StartsWith("Publisher"))
                        appLocal.Publisher = line.Substring(line.IndexOf(":") + 2);
                    if (line.StartsWith("PublisherId"))
                        appLocal.PublisherId = line.Substring(line.IndexOf(":") + 2);
                    else if (line.StartsWith("PackageFamilyName"))
                        appLocal.PackageFamilyName = line.Substring(line.IndexOf(":") + 2);
                    else if (line.StartsWith("Version"))
                        appLocal.Version = line.Substring(line.IndexOf(":") + 2);
                    else if (line.StartsWith("IsDevelopmentMode")) //so we know it's the last line that matter
                    {
                        foreach (var appRemote in apps)
                        {
                            if (appRemote.PackageFamilyName == appLocal.PackageFamilyName)
                            {
                                appLocal.UpdateAvailable = UpdateAvailable(appRemote, appLocal);
                                if (appLocal.UpdateAvailable) appLocal.NewVersionAvailable = appRemote.Version;
                                appLocal.ImageUrl = appRemote.ImageUrl;
                                appLocal.SmallImageUrl = appRemote.SmallImageUrl;
                                appLocal.AppxUrl = appRemote.AppxUrl;
                                appLocal.CertificateUrl = appRemote.CertificateUrl;
                                appLocal.CertificateFile = appRemote.CertificateFile;
                                appLocal.Category = appRemote.Category;
                                appLocal.Description = appRemote.Description;
                                appLocal.BackgroundColor = appRemote.BackgroundColor;
                                appLocal.DisplayName = appRemote.DisplayName;
                                appLocal.InstallMode = appRemote.InstallMode;
                                appList.Add(appLocal);
                                appLocal = new AppInfo();
                            }
                        }
                    }
                }
            }
            return appList;
        }

        internal void UnlockDevice(string key)
        {
            //1.Run “slmgr.vbs /ipk ” to install the key.
            //2.Run “slmgr.vbs /ato ec67814b-30e6-4a50-bf7b-d55daf729d1e” to activate the key online.
            RunAsAdmin("cmd.exe", string.Format(@" /c slmgr.vbs /ipk {0}", key));
            RunAsAdmin("cmd.exe", string.Format(@" /c slmgr.vbs /ato ec67814b-30e6-4a50-bf7b-d55daf729d1e"));
        }

        private void RunAsAdmin(string fileName, string parameter)
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.UseShellExecute = true;

            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = parameter;

            process.StartInfo.Verb = "runas";

            process.Start();

            while (!process.HasExited)
                System.Threading.Thread.Sleep(5);
        }
        private async Task<InstallResult> RunAsAdminWithResult(string fileName, string parameter)
        {
            var tcs = new TaskCompletionSource<InstallResult>();

            //var 
            result = new InstallResult();

            try
            {
                var process = new System.Diagnostics.Process();
                process.EnableRaisingEvents = true;

                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = fileName;
                process.StartInfo.Arguments = parameter;

                process.StartInfo.Verb = "runas";
                process.StartInfo.CreateNoWindow = false;
                //process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;

                process.ErrorDataReceived += process_ErrorDataReceived;
                process.OutputDataReceived += process_OutputDataReceived;
                //process.EnableRaisingEvents = true;
                //process.Start();
                //process.WaitForExit();

                var pr = await ProcessEx.RunAsync(process.StartInfo);

                //var stdout = process.StandardOutput;
                //var stderr = process.StandardError;
                var stdout = pr.StandardOutput;
                var stderr = pr.StandardError;
                result.Output = stdout.Join(Environment.NewLine);
                result.Error = stderr.Join(Environment.NewLine);

                //pr.Process.Exited += (sender, args) =>
                //{
                tcs.SetResult(result);
                //    process.Dispose();
                //};
                //

                //while (!process.HasExited)
                //    System.Threading.Thread.Sleep(5);
            }
            catch (Exception ex)
            {
                if (string.IsNullOrWhiteSpace(result.Error))
                {
                    result.Error = ex.Message;
                }
                else
                {
                    result.Error += Environment.NewLine + ex.Message;
                }
                ExceptionLogger.LogException(ex, CachePath);
            }
            return await tcs.Task;

        }
        public InstallResult result { get; set; }

        public List<AppInfo> InstalledAppList { get; set; }

        public List<AppInfo> ServerAppList { get; set; }
        public List<AppInfo> DistinctServerAppList { get; set; }

        public string PackageFamilyName { get; set; }

        //public string PackageFamilyName { get; set; }

        public bool IsInstalling { get; set; }

        public bool IsProcessing { get; set; }

        public bool IsAutoInstalling { get; set; }

        public List<FileSystemWatcher> WatcherList { get; set; }

        public bool CancelExecution { get; set; }



        public bool IsWatchingOrgPortal { get; set; }

        public bool IsWatchingCache { get; set; }

        public bool IsWatchingInstalledApps { get; set; }

        public string DevLicenseOutput { get; set; }

        public bool DevLicenseEnabled { get; set; }
    }
}
