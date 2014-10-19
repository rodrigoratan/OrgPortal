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
        public string TempPath { get; set; }
        public string LocalPath { get; set; }
        public System.Windows.Forms.NotifyIcon NotifyIcon { get; set; }
        public System.Windows.Forms.TextBox Output { get; set; }
        public FileSystemWatcher Watcher { get; set; }

        public Installer(System.Windows.Forms.NotifyIcon notifyIcon, System.Windows.Forms.TextBox output, FileSystemWatcher watcher)
        {
            this.NotifyIcon = notifyIcon;
            this.Output = output;
            this.Watcher = watcher;

            this.Output.AppendText("Monitor started at " + DateTime.Now + Environment.NewLine);
            this.NotifyIcon.ShowBalloonTip(500, "OrgPortal", "The OrgPortal monitor has started", System.Windows.Forms.ToolTipIcon.Info);

            var tempPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            if (!tempPath.EndsWith(@"\")) tempPath += @"\";
            tempPath += @"Packages\OrgPortal_m64ba5zfsemg0\TempState\";
            if (!System.IO.Directory.Exists(tempPath))
            {
                System.IO.Directory.CreateDirectory(tempPath);
            }
            this.TempPath = tempPath;

            var localPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            if (!localPath.EndsWith(@"\")) localPath += @"\";
            localPath += @"Packages\OrgPortal_m64ba5zfsemg0\LocalState\";
            if (!System.IO.Directory.Exists(localPath))
            {
                System.IO.Directory.CreateDirectory(localPath);
            }
            this.LocalPath = localPath;

            this.Watcher.Path = this.TempPath;
        }

        public void StartFileWatcher()
        {
            this.Output.AppendText("Watching folder " + this.TempPath + Environment.NewLine);
            Watcher.Created += (s, e) =>
              {
                  System.Threading.Thread.Sleep(500);
                  ProcessRequest(e.FullPath);
              };

            var existingFiles = Directory.EnumerateFiles(this.TempPath, "*.rt2win");
            foreach (var item in existingFiles)
            {
                ProcessRequest(item);
            }
        }

        private void ProcessRequest(string inputFilePath)
        {
            var logfilePath = inputFilePath.Replace(".rt2win", ".log");
            var outputDoc = new XElement("request");
            outputDoc.Add(new XElement("requestFile", inputFilePath));
            this.Output.AppendText(">>Processing " + inputFilePath + Environment.NewLine);

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
                    this.Output.AppendText("**FAILED" + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                outputDoc.Add(new XElement("success", "false"));
                outputDoc.Add(new XElement("error", ex.Message));
                this.Output.AppendText("UNEXPECTED EXCEPTION" + Environment.NewLine);
                this.Output.AppendText(ex.ToString() + Environment.NewLine);
                this.Output.AppendText("**FAILED" + Environment.NewLine);
            }
            finally
            {
                //File.Delete(inputFilePath);
                var arquivoPartes = inputFilePath.Split('.');
                var extensao = arquivoPartes[arquivoPartes.Length-1];
                File.Move(inputFilePath, inputFilePath.Replace("." + extensao, ".txt"));
                this.Output.AppendText("<<Processed " + inputFilePath + Environment.NewLine);
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
            var appFilePath = TempPath + appFileName;
            var certificateFilePath = TempPath + certificateFile;

            this.Output.AppendText("Starting Install Request at " + DateTime.Now + Environment.NewLine);
            this.Output.AppendText("App file name " + appFileName + Environment.NewLine);
            this.Output.AppendText("from " + appxUrl + Environment.NewLine);
            if (!string.IsNullOrEmpty(certificateFile))
            {
                this.Output.AppendText("Certificate file: " + certificateFile + Environment.NewLine);
                this.Output.AppendText("from " + certificateUrl + Environment.NewLine);
            }
            this.Output.AppendText("Saving at " + TempPath + Environment.NewLine);

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
                NotifyIcon.ShowBalloonTip(500, "OrgPortal", appxUrl + " installed", System.Windows.Forms.ToolTipIcon.Info);
                this.Output.AppendText("**SUCCESS" + Environment.NewLine);
            }
            else
            {
                outputDoc.Add(new XElement("success", "false"));
                outputDoc.Add(new XElement("filePath", appFilePath));
                outputDoc.Add(new XElement("error", result.ToString()));
                NotifyIcon.ShowBalloonTip(500, "OrgPortal", appxUrl + " not installed", System.Windows.Forms.ToolTipIcon.Warning);
                this.Output.AppendText("**FAILED" + Environment.NewLine);
                this.Output.AppendText(result.ToString() + Environment.NewLine);
            }

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
            sb.Append(@"get-appxpackage > " + this.LocalPath + "InstalledPackages.txt");

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

        public static readonly string _serviceURI = "http://orgportal/api/";

        public async Task AutoInstallUpdateApps()
        {
            this.Output.AppendText("Auto-install and auto-update apps...");

            var serverAppList = await GetAppList();
            var installedAppList = GetInstalledApps();
            foreach (var serverApp in serverAppList)
            {
                var installedApp = installedAppList.Where(a => a.PackageFamilyName == serverApp.PackageFamilyName).FirstOrDefault();
                if ((installedApp != null && serverApp.InstallMode.StartsWith("Auto") && UpdateAvailable(serverApp, installedApp)) ||
                    (installedApp == null && serverApp.InstallMode == "AutoInstall"))
                {
                    var requestFile = System.IO.File.CreateText(TempPath + System.Guid.NewGuid().ToString() + ".rt2win");
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

        private async Task<List<AppInfo>> GetAppList()
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
                    app.PackageFamilyName = obj["PackageFamilyName"] != null ? obj["PackageFamilyName"] : "";
                    app.PackageFile = obj["PackageFile"] != null ? obj["PackageFile"] : "";
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

        private List<AppInfo> GetInstalledApps()
        {
            GetInstalledPackages();
            var appList = new List<AppInfo>();
            var filePath = LocalPath + "InstalledPackages.txt";
            if (System.IO.File.Exists(filePath))
            {
                var installedPackages = System.IO.File.ReadAllLines(filePath);
                var info = new AppInfo();
                foreach (var line in installedPackages)
                {
                    if (line.StartsWith("Name"))
                        info.Name = line.Substring(line.IndexOf(":") + 2);
                    else if (line.StartsWith("PackageFamilyName"))
                        info.PackageFamilyName = line.Substring(line.IndexOf(":") + 2);
                    else if (line.StartsWith("Version"))
                        info.Version = line.Substring(line.IndexOf(":") + 2);
                    else if (line.StartsWith("IsDevelopmentMode"))
                    {
                        appList.Add(info);
                        info = new AppInfo();
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
    }
}
