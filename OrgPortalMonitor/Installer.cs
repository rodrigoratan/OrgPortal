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
        public /*static readonly*/ string _serviceURI = "http://orgportal/api/";
        public const string OrgPortalPackageFamilyName = "OrgPortal_m64ba5zfsemg0";
        private string PackageTempPathBuffer;
        public string PackageTempPath { get; set; }
        public string PackageLocalPath { get; set; }
        public System.Windows.Forms.NotifyIcon NotifyIcon { get; set; }
        public System.Windows.Forms.TextBox Output { get; set; }
        public FileSystemWatcher Watcher { get; set; }

        public Installer(string packageFamilyName, System.Windows.Forms.NotifyIcon notifyIcon, System.Windows.Forms.TextBox output, FileSystemWatcher watcher)
        {
            if (!string.IsNullOrEmpty(OrgPortalMonitor.Properties.Settings.Default.OrgPortalUrl))
            {
                _serviceURI = OrgPortalMonitor.Properties.Settings.Default.OrgPortalUrl;
            }

            this.NotifyIcon = notifyIcon;
            this.Output = output;
            this.Watcher = watcher;
            this.PackageFamilyName = packageFamilyName;

            this.Output.AppendText("Monitor started at " + DateTime.Now + Environment.NewLine);
            this.NotifyIcon.ShowBalloonTip(500, "OrgPortal", "The OrgPortal monitor has started", System.Windows.Forms.ToolTipIcon.Info);

            if (string.IsNullOrEmpty(PackageFamilyName))
            {
                //string 
                PackageFamilyName = OrgPortalPackageFamilyName;
            }

            //TempPath is where app install requests will be saved and also temporary app package files and logs will remain in place
            var tempPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            if (!tempPath.EndsWith(@"\")) tempPath += @"\";
            tempPath += @"Packages\" + PackageFamilyName + @"\TempState\";
            if (!System.IO.Directory.Exists(tempPath))
            {
                System.IO.Directory.CreateDirectory(tempPath);
            }
            this.PackageTempPath = tempPath;

            // LocalPath will be used for InstalledPackages.txt saving
            var localPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            if (!localPath.EndsWith(@"\")) localPath += @"\";
            localPath += @"Packages\" + PackageFamilyName + @"\LocalState\";
            if (!System.IO.Directory.Exists(localPath))
            {
                System.IO.Directory.CreateDirectory(localPath);
            }
            this.PackageLocalPath = localPath;

        }

        public void StartFileWatcher(string path)
        {
            this.Watcher.Path = path;

            this.Output.AppendText("Watching folder " + path + Environment.NewLine);
            Watcher.Created += Watcher_Created;

            ProcessExistingRequestFiles();
        }

        void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            System.Threading.Thread.Sleep(500);
            ProcessRequest(e.FullPath);
        }

        private void ProcessExistingRequestFiles()
        {
            var existingFiles = Directory.EnumerateFiles(this.PackageTempPath, "*.rt2win");
            foreach (var item in existingFiles)
            {
                ProcessRequest(item);
            }
        }

        public void StopFileWatcher()
        {
            Watcher.Created -= Watcher_Created;
            this.Output.AppendText("\n\nStop Watching folder " + this.PackageTempPath + Environment.NewLine);
            this.NotifyIcon.ShowBalloonTip(500, "OrgPortal", "The OrgPortal monitor has stopped", System.Windows.Forms.ToolTipIcon.Info);
            //Watcher.Dispose();
        }


        private void ProcessRequest(string inputFilePath)
        {
            var logfilePath = inputFilePath.Replace(".rt2win", ".log");
            var outputDoc = new XElement("request");
            outputDoc.Add(new XElement("requestFile", inputFilePath));
            this.Output.AppendText(Environment.NewLine + ">> Processing " + inputFilePath + Environment.NewLine);
            this.Output.AppendText(Environment.NewLine);

            try
            {
                var input = File.ReadAllLines(inputFilePath);
                var command = input[0];
                outputDoc.Add(new XElement("command", command));

                if (command == "install")
                {
                    ProcessInstallRequest(outputDoc, input[1], input[3], input[5], input[7]);
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
            }
            finally
            {
                //File.Delete(inputFilePath);
                var arquivoPartes = inputFilePath.Split('.');
                var extensao = arquivoPartes[arquivoPartes.Length-1];
                File.Move(inputFilePath, inputFilePath.Replace("." + extensao, ".txt"));
                this.Output.AppendText(Environment.NewLine + " << Processed " + inputFilePath + Environment.NewLine);
                this.Output.AppendText(Environment.NewLine);
                this.Output.AppendText(Environment.NewLine);
                File.WriteAllText(logfilePath, outputDoc.ToString());
            }
        }

        private /*async*/ void ProcessInstallRequest(XElement outputDoc,
                                                     string appxUrl,
                                                     string appxFile,
                                                     string certificateUrl,
                                                     string certificateFile)
        {
            var appUriSegments = new System.Uri(appxUrl).Segments;
            //var appFileName = appUriSegments[appUriSegments.Length - 1] + ".appx";
            var appFileName = appxFile;
            var appFilePath = PackageTempPath + appFileName;
            var certificateFilePath = PackageTempPath + certificateFile;

            this.Output.AppendText("Starting Install Request at " + DateTime.Now + Environment.NewLine);
            this.Output.AppendText("App file name " + appFileName + Environment.NewLine);
            this.Output.AppendText("from " + appxUrl + Environment.NewLine);
            if (!string.IsNullOrEmpty(certificateFile))
            {
                this.Output.AppendText("Certificate file: " + certificateFile + Environment.NewLine);
                this.Output.AppendText("from " + certificateUrl + Environment.NewLine);
            }
            this.Output.AppendText("Saving at " + PackageTempPath + Environment.NewLine);

            var result = new InstallResult();

            if (!string.IsNullOrEmpty(certificateFile) &&
                (certificateFile.Contains(".pfx") ||
                 certificateFile.Contains(".cer")))
            {
                result.Error = DownloadFile(certificateUrl, certificateFilePath);
                //result.Error = await DownloadFileTaskAsync(fileUrl, filePath);

                if (string.IsNullOrWhiteSpace(result.Error))
                {
                    result = InstallCertificate(certificateFilePath);
                }
            }

            if (string.IsNullOrWhiteSpace(result.Error))
            {
                result.Error = DownloadFile(appxUrl, appFilePath);

                if (string.IsNullOrWhiteSpace(result.Error))
                {
                    result = InstallAppx(appFilePath);
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

            //TODO: test
            StopFileWatcher();
            PackageTempPath = PackageTempPathBuffer;
            //this.Watcher.Path = this.PackageTempPath;
            StartFileWatcher(PackageTempPath);

            GetInstalledPackages();
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
                    process.Kill();

                stdout.Close();
                stderr.Close();
            }
            catch (Exception ex)
            {
                if (string.IsNullOrWhiteSpace(result.Error))
                    result.Error = ex.Message;
                else
                    result.Error += Environment.NewLine + ex.Message;
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
                    result.Error = ex.Message;
                else
                    result.Error += Environment.NewLine + ex.Message;
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
            var sb = new StringBuilder();
            sb.Append(@"get-appxpackage > " + this.PackageLocalPath + "InstalledPackages.txt");

            var process = new System.Diagnostics.Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;

            process.StartInfo.FileName = "powershell.exe";
            process.StartInfo.Arguments = sb.ToString();

            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            process.Start();

            while (!process.HasExited)
                System.Threading.Thread.Sleep(5);
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
            this.Output.AppendText("Auto-install and auto-update apps...");

            //var 
            ServerAppList = await GetRemoteAppList();
            //var 
            InstalledAppList = GetInstalledApps(ServerAppList);

            PackageTempPathBuffer = PackageTempPath;
            foreach (var serverApp in ServerAppList)
            {
                var installedApp = InstalledAppList.Where(a => a.PackageFamilyName == serverApp.PackageFamilyName).FirstOrDefault();

                if ((installedApp != null && serverApp.InstallMode.StartsWith("Auto") && UpdateAvailable(serverApp, installedApp)) ||
                    (installedApp == null && serverApp.InstallMode == "AutoInstall"))
                {
                    //var RequestPath = PackageTempPath;
                    //OrgPortal_m64ba5zfsemg0
                    if (serverApp.PackageFamilyName == OrgPortalPackageFamilyName)
                    {
                        //RequestPath = StorageFolder.
                        var cachePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.InternetCache);
                        //RequestPath = cachePath;
                        if (!cachePath.EndsWith(@"\")) cachePath += @"\";
                        StopFileWatcher();
                        PackageTempPath = cachePath;
                        //this.Watcher.Path = this.PackageTempPath;
                        StartFileWatcher(PackageTempPath);

                    }
                    string requestFileName = /*RequestPath*/ PackageTempPath + System.Guid.NewGuid().ToString() + ".rt2win";
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
                        requestFile.Close();
                    }
                    if (serverApp.PackageFamilyName == OrgPortalPackageFamilyName)
                    {
                        //ProcessRequest(requestFileName);
                    }
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

        public async Task<List<AppInfo>> GetRemoteAppList()
        {
            var appList = new List<AppInfo>();
            var client = new System.Net.Http.HttpClient();

            var response = await client.GetAsync(_serviceURI + "Apps");
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

        public string PackageFamilyName { get; set; }
    }
}
