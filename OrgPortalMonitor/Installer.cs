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
        public const string OrgPortalPackageFamilyName = "OrgPortal_m64ba5zfsemg0";
        private string PackageTempPathBuffer;
        public string PackageTempPath { get; set; }
        public string PackageLocalPath { get; set; }
        public string CachePath { get; set; }

        public System.Windows.Forms.NotifyIcon NotifyIcon { get; set; }
        public System.Windows.Forms.TextBox Output { get; set; }
        public FileSystemWatcher Watcher { get; set; }
        public FileSystemWatcher Watcher2 { get; set; }

        public Installer(string orgPortalUrl, string packageFamilyName, System.Windows.Forms.NotifyIcon notifyIcon, System.Windows.Forms.TextBox output, FileSystemWatcher watcher, FileSystemWatcher watcher2)
        {

            //if (!string.IsNullOrEmpty(OrgPortalMonitor.Properties.Settings.Default.OrgPortalUrl))
            //{
            //    _serviceURI = OrgPortalMonitor.Properties.Settings.Default.OrgPortalUrl;
            //}
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

            this.NotifyIcon = notifyIcon;
            this.Output = output;
            this.Watcher = watcher;
            this.Watcher2 = watcher2;
            this.PackageFamilyName = packageFamilyName;

            this.Output.AppendText("Monitor started at " + DateTime.Now + Environment.NewLine);
            this.NotifyIcon.ShowBalloonTip(500, "OrgPortal", "The OrgPortal monitor has started", System.Windows.Forms.ToolTipIcon.Info);

            if (string.IsNullOrEmpty(PackageFamilyName))
            {
                //string 
                PackageFamilyName = OrgPortalPackageFamilyName;
            }

            #region PackageTempPath
            //TempPath is where app install requests will be saved and also temporary app package files and logs will remain in place
            var tempPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            if (!tempPath.EndsWith(@"\")) tempPath += @"\";
            tempPath += @"Packages\" + PackageFamilyName + @"\TempState\";
            if (!System.IO.Directory.Exists(tempPath))
            {
                System.IO.Directory.CreateDirectory(tempPath);
            }
            this.PackageTempPath = tempPath;
            this.PackageTempPathBuffer = tempPath;
            #endregion

            #region PackageLocalPath
            // LocalPath will be used for InstalledPackages.txt saving
            var localPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            if (!localPath.EndsWith(@"\")) localPath += @"\";
            localPath += @"Packages\" + PackageFamilyName + @"\LocalState\";
            if (!System.IO.Directory.Exists(localPath))
            {
                System.IO.Directory.CreateDirectory(localPath);
            }
            this.PackageLocalPath = localPath;
            #endregion

            #region CachePath
            CachePath = System.Environment
                        .GetFolderPath(System.Environment
                                       .SpecialFolder.InternetCache);

            if (!CachePath.EndsWith(@"\")) CachePath += @"\";
            #endregion

        }

        public void StartFileWatcher1(string path)
        {
            this.Output.AppendText(System.Environment.NewLine + "Watching OrgPortal app TempState folder " + path + " for App Install Requests " + Environment.NewLine);
            this.Watcher.Path = path;
            Watcher.Created += Watcher_Created;
            ProcessExistingPackageTempRequestFiles();
        }

        public void StartFileWatcher2(string path)
        {
            this.Output.AppendText(System.Environment.NewLine + "Watching Cache folder " + this.CachePath + " for Auto Install and Auto Update apps" + Environment.NewLine);
            this.Watcher2.Path = CachePath;
            Watcher2.Created += Watcher_Created;
            ProcessExistingCacheRequestFiles();
        }

        public void ProcessExistingPackageTempRequestFiles()
        {
            if (!IsInstalling)
            {
                var existingFiles = Directory.EnumerateFiles(this.PackageTempPath, "*.rt2win");
                if (existingFiles.Count() > 0)
                {
                    this.Output.AppendText(System.Environment.NewLine + "Processing OrgPortal app requests at " + this.PackageTempPath + Environment.NewLine);
                    foreach (var item in existingFiles)
                    {
                        ProcessRequest(item);
                    }
                }
            }
        }

        public void ProcessExistingCacheRequestFiles()
        {
            if (!IsAutoInstalling)
            {
                var existingFiles = Directory.EnumerateFiles(this.CachePath, "*.rt2win");
                if (existingFiles.Count() > 0)
                {
                    this.Output.AppendText(System.Environment.NewLine + "Processing Auto Install and Auto Update app requests at " + this.CachePath + Environment.NewLine);
                    foreach (var item in existingFiles)
                    {
                        ProcessRequest(item);
                    }
                }
            }
        }

        void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            System.Threading.Thread.Sleep(500);
            this.Output.AppendText(System.Environment.NewLine + "New app request detected at " + e.FullPath + Environment.NewLine);
            ProcessRequest(e.FullPath);
        }

        public void StopPackageTempFileWatcher()
        {
            Watcher.Created -= Watcher_Created;
            this.Output.AppendText(System.Environment.NewLine + "\n\nStop Watching OrgPortal Temp folder " + this.PackageTempPath + Environment.NewLine);
            this.NotifyIcon.ShowBalloonTip(500, "OrgPortal", "The OrgPortal monitor has stopped", System.Windows.Forms.ToolTipIcon.Info);
            //Watcher.Dispose();
        }

        public void StopCacheFileWatcher()
        {
            Watcher2.Created -= Watcher_Created;
            this.Output.AppendText(System.Environment.NewLine + "\n\nStop Watching INet Cache folder " + this.CachePath + Environment.NewLine);
            this.NotifyIcon.ShowBalloonTip(500, "OrgPortal", "The OrgPortal Auto Install and Auto Update monitor has stopped", System.Windows.Forms.ToolTipIcon.Info);
            //Watcher.Dispose();
        }


        public void ProcessRequest(string inputFilePath)
        {
            if (!IsProcessing)
            {
                IsProcessing = true;

                var logfilePath = inputFilePath.Replace(".rt2win", ".log");
                var outputDoc = new XElement("request");
                outputDoc.Add(new XElement("requestFile", inputFilePath));
                this.Output.AppendText(Environment.NewLine + ">> Processing " + inputFilePath + Environment.NewLine);
                this.Output.AppendText(Environment.NewLine);

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
                        ProcessInstallRequest(outputDoc, input[1], input[3], input[5], input[7] /*, (saveAt != null ? input[9] : string.Empty)*/);
                    }
                    else if (command == "getDevLicense")
                    {
                        GetDevLicense(outputDoc);
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

        private /*async*/ void ProcessInstallRequest(XElement outputDoc,
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
                result.Error = DownloadFile(certificateUrl, certificateFilePath);
                //result.Error = await DownloadFileTaskAsync(fileUrl, filePath);

                if (string.IsNullOrWhiteSpace(result.Error))
                {
                    this.Output.AppendText("Installing " + certificateFilePath + " ... " + Environment.NewLine);
                    result = InstallCertificate(certificateFilePath);
                }
                else
                {
                    this.Output.AppendText("Error downloading certificate at " + certificateUrl + " : " + Environment.NewLine + result.Error + Environment.NewLine);
                }
            }

            if (string.IsNullOrWhiteSpace(result.Error))
            {
                result.Error = DownloadFile(appxUrl, appFilePath);

                if (string.IsNullOrWhiteSpace(result.Error))
                {
                    this.Output.AppendText("Installing " + appFilePath + " ... " + Environment.NewLine);
                    result = InstallAppx(appFilePath);
                }
                else
                {
                    this.Output.AppendText("Error downloading certificate at " + certificateUrl + " : " + Environment.NewLine + result.Error + Environment.NewLine);
                }
            }

            if (string.IsNullOrWhiteSpace(result.Error))
            {
                outputDoc.Add(new XElement("success", "true"));
                outputDoc.Add(new XElement("filePath", appFilePath));
                NotifyIcon.ShowBalloonTip(500, "OrgPortal ", appxUrl + " installed with sucess", System.Windows.Forms.ToolTipIcon.Info);
                this.Output.AppendText("** SUCCESS " + Environment.NewLine);
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

        public InstallResult InstallAppx(string appxFilePath)
        {
            var result = new InstallResult();

            try
            {
                var sb = new StringBuilder();
                sb.Append(@"add-appxpackage ");
                sb.Append(appxFilePath);
                sb.Append(" -ForceApplicationShutdown"); //TODO: confirm it's working

                var process = new System.Diagnostics.Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;

                process.StartInfo.FileName = "powershell.exe";
                process.StartInfo.Arguments = sb.ToString();

                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                process.Start();

                var stdout = process.StandardOutput;
                var stderr = process.StandardError;

                result.Output = stdout.ReadToEnd();
                result.Error = stderr.ReadToEnd();

                if (!process.HasExited)
                {
                    process.Kill();
                }
                stdout.Close();
                stderr.Close();
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
                File.Delete(appxFilePath);
            }

            return result;
        }

        /*
         * importpfx.exe -f "somePfx.pfx" -p "somePassword" -t MACHINE -s "TRUSTEDPEOPLE"
         */
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
                    sb.Append(@"Certutil -addstore -f ");
                    sb.Append(@"""");
                    sb.Append("TRUSTEDPEOPLE");
                    sb.Append(@""" """);
                    sb.Append(certificateFilePath);
                    sb.Append(@"""");
                }

                var process = new System.Diagnostics.Process();
                process.StartInfo.UseShellExecute = true;
                //process.StartInfo.RedirectStandardError = true;
                //process.StartInfo.RedirectStandardOutput = true;

                process.StartInfo.FileName = "cmd.exe";
                //process.StartInfo.Arguments = "dir /w"; //sb.ToString()/* + " | more"*/;
                process.StartInfo.Arguments = " /c " + sb.ToString()/* + " | more"*/;
                process.StartInfo.Verb = "runas";

                process.StartInfo.CreateNoWindow = false;
                //process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                process.ErrorDataReceived += process_ErrorDataReceived;
                process.OutputDataReceived += process_OutputDataReceived;
                process.EnableRaisingEvents = true;
                process.Start();
                process.WaitForExit();
                //process.BeginOutputReadLine();
                //process.BeginErrorReadLine();

                //var stdout = process.StandardOutput;
                //var stderr = process.StandardError;

                //result.Output = stdout.ReadToEnd();
                //result.Error = stderr.ReadToEnd();

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

            return result;
        }

        void process_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            result.Output += e.Data;
        }

        void process_ErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            result.Error += e.Data;
        }

        public void GetInstalledPackages()
        {
            var fileName = "InstalledPackages.txt";
            var process = new System.Diagnostics.Process();
            var sb = new StringBuilder();
            try
            {
                sb.Append(@"get-appxpackage > " + this.PackageLocalPath + fileName);

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;

                process.StartInfo.FileName = "powershell.exe";
                process.StartInfo.Arguments = sb.ToString();

                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                process.Start();

            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
            }

            try
            {
                while (!process.HasExited)
                {
                    System.Threading.Thread.Sleep(5);
                }

                System.IO.File.Copy(Path.Combine(this.PackageLocalPath, fileName),
                                    Path.Combine(this.CachePath, fileName), true);

            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
            }

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

        public void GetDevLicense()
        {
            GetDevLicense(new XElement("request"));
        }

        public void GetDevLicense(XElement outputDoc)
        {
            RunAsAdmin("powershell.exe", @"Show-WindowsDeveloperLicenseRegistration");
            outputDoc.Add(new XElement("success", "true"));
        }

        public async Task AutoInstallUpdateApps()
        {
            if (!IsProcessing && !IsInstalling)
            {
                IsAutoInstalling = true;
                this.Output.AppendText(System.Environment.NewLine + "Auto-install and auto-update apps... " + System.Environment.NewLine);

                ServerAppList = await GetRemoteAppList();
                DistinctServerAppList = await GetRemoteDistinctAppList(ServerAppList);

                InstalledAppList = GetInstalledApps(ServerAppList);

                //PackageTempPathBuffer = PackageTempPath;

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

                            //if (PackageTempPath != CachePath)
                            //{
                            //    this.Output.AppendText(System.Environment.NewLine + "Changing Monitor Folder temporary: " + CachePath + " . " + System.Environment.NewLine);

                            //    StopFileWatcher1();
                            //    PackageTempPath = CachePath;
                            //    StartFileWatcher1(PackageTempPath);
                            //}
                            //else
                            //{
                            //    this.Output.AppendText("Still same folder (1) " + PackageTempPath + " path. " + System.Environment.NewLine);
                            //}
                        }
                        //else // if not OrgPortal app, return to original path
                        //{
                        //    RestorePackageTempPathIfChanged();
                        //}

                        //if (!System.IO.Directory.Exists(PackageTempPath))
                        //{
                        //    System.IO.Directory.CreateDirectory(PackageTempPath);
                        //}
                        var SavePath = CachePath; 
                                       //((serverApp.PackageFamilyName == OrgPortalPackageFamilyName) ? 
                                       //  CachePath : 
                                       //  PackageTempPath);

                        string requestFileName = SavePath 
                                                 + System.Guid.NewGuid().ToString() + ".rt2win";

                        await RequestApp(serverApp, SavePath, requestFileName);

                        //if (serverApp.PackageFamilyName == OrgPortalPackageFamilyName)
                        //{
                            ProcessRequest(requestFileName); 
                        //}
                    }
                }

                this.Output.AppendText("Done with Server Auto Install and Auto Update Apps checking... " + System.Environment.NewLine);
                //Ideally here we would restore PackageTempPath and GetInstalledPackages but we are no sure that it finished
                //RestorePackageTempPathIfChanged();

                IsAutoInstalling = false;

            }
            //GetInstalledPackages();

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
                    requestFile.Close();
                }
            }
        }

        private void RestorePackageTempPathIfChanged()
        {
            //if (PackageTempPath != PackageTempPathBuffer)
            //{
            //    this.Output.AppendText("Changing Package Watcher Monitor to " + PackageTempPathBuffer + " folder path. " + System.Environment.NewLine);
            //    StopFileWatcher1();
            //    PackageTempPath = PackageTempPathBuffer;
            //    StartFileWatcher1(PackageTempPath);
            //}
            //else
            //{
            //    this.Output.AppendText("Still same folder (2) " + PackageTempPathBuffer + " path. " + System.Environment.NewLine);

            //}
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
                        ExceptionLogger.LogException(ex, PackageTempPath);
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
                        app.ImageUrl = obj.ContainsKey("LogoUrl") && obj["LogoUrl"] != null ? obj["LogoUrl"] : "Assets/DarkGray.png";
                        app.SmallImageUrl = obj.ContainsKey("LogoUrl") && obj["LogoUrl"] != null ? obj["LogoUrl"] : "Assets/DarkGray.png";
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

        public List<AppInfo> GetInstalledApps(List<AppInfo> apps)
        {
            if (InstalledAppList == null || InstalledAppList.Count == 0)  // avoid call powershell again if we already have installedAppList
            {
                GetInstalledPackages();
            } // InstalledAppList will be cleared after AutoInstallUpdateApps call made by Tick timer and this method will be allowed to refresh the InstalledAppList again

            var appList = new List<AppInfo>();
            var filePath = PackageLocalPath + "InstalledPackages.txt";
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
                                appLocal.AppxUrl = appRemote.AppxUrl;
                                appLocal.CertificateUrl = appRemote.CertificateUrl;
                                appLocal.CertificateFile = appRemote.CertificateFile;
                                appLocal.Category = appRemote.Category;
                                appLocal.Description = appRemote.Description;
                                appLocal.BackgroundColor = appRemote.BackgroundColor;
                                appLocal.DisplayName = appRemote.DisplayName;
                                appLocal.InstallMode = appRemote.InstallMode;
                                appLocal.SmallImageUrl = appRemote.SmallImageUrl;
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

        public InstallResult result { get; set; }

        public List<AppInfo> InstalledAppList { get; set; }

        public List<AppInfo> ServerAppList { get; set; }
        public List<AppInfo> DistinctServerAppList { get; set; }

        public string PackageFamilyName { get; set; }

        public bool IsInstalling { get; set; }

        public bool IsProcessing { get; set; }

        public bool IsAutoInstalling { get; set; }
    }
}
