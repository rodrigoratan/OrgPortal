using OrgPortalMonitor.DataModel;
using OrgPortal.Common;
using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;
using System.Diagnostics;
using System.Linq;

namespace OrgPortal.DataModel
{
    [Export(typeof(IFileSyncManager))]
    [Shared]
    public class FileSyncManager : IFileSyncManager
    {
        private const string WRITE_FILE_EXTENSION = ".rt2win";
        private const string READ_FILE_EXTENSION = ".win2rt";

        public async Task RequestAppInstall(string appxUrl, 
                                            string appxFile, 
                                            string certificateUrl,
                                            string certificateFile,
                                            string version,
                                            string name,
                                            string description,
                                            string backgroundColor, 
                                            string imageUrl)
        {
            var app = InstallAppRequestArray(appxUrl, appxFile, certificateUrl, certificateFile, version, name, description, backgroundColor, imageUrl);


            //var machineInfo = await MachineInfo.GatherMachineInfoArray();

            //Array.Resize(ref app, app.Length + 1);
            //app[app.Length - 1] = "new string";

            //TODO: instead sending stats to the monitor, send directly to the log endpoint
            //await WriteTempFile(app.Concat(machineInfo).ToArray());

            await WriteTempFile(app);


        }

        public static string[] InstallAppRequestArray(string appxUrl, 
                                                      string appxFile, 
                                                      string certificateUrl, 
                                                      string certificateFile,
                                                      string version,
                                                      string name,
                                                      string description,
                                                      string backgroundColor, 
                                                      string imageUrl)
        {
            var app = new string[] { "install",         appxUrl, 
                                     "appxFile",        appxFile, 
                                     "certificateUrl",  certificateUrl, 
                                     "certificateFile", certificateFile,
                                     "saveAt",          "INetCache", /*string.Empty*/
                                     "version",         version,
                                     "name",            name,
                                     "description",     description,
                                     "backgroundColor", backgroundColor,
                                     "imageUrl",        imageUrl
                                   };
            return app;
        }

        public async Task<List<AppInfo>> GetInstalledApps(List<AppInfo> apps)
        {
            var appList = new List<AppInfo>();
            var folder = GetLocalFolder();
            var file = await folder.TryGetItemAsync("InstalledPackages.txt") as IStorageFile;
            if (file != null)
            {
                var installedPackages = await FileIO.ReadLinesAsync(file);
                AppInfo appLocal = new AppInfo();
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
                    else if (line.StartsWith("IsDevelopmentMode")) //last line
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
                                appLocal.PublisherDisplayName = appRemote.PublisherDisplayName;
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

        //TODO: code is duplicated from Installer.cs , plan to add a Shared Project for shared code
        public static bool UpdateAvailable(AppInfo serverApp, AppInfo installedApp)
        {
            return UpdateAvailable(serverApp.Version, installedApp.Version);
        }

        private static bool UpdateAvailable(string serverAppVersion, string installedAppVersion)
        {
            var result = false;
            var serverVersion = serverAppVersion.Split('.');
            var installedVersion = installedAppVersion.Split('.');
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

        public async Task UpdateDevLicense()
        {
            var content = new string[] { "getDevLicense" };
            await WriteTempFile(content);
        }

        public async Task<IReadOnlyList<StorageFile>> ReadFiles()
        {
            var folder = GetSyncFolder();
            var criteria = new string[] { READ_FILE_EXTENSION };
            var query = folder.CreateFileQueryWithOptions(new QueryOptions(CommonFileQuery.DefaultQuery, criteria));
            //query.ContentsChanged += async (o, a) =>
            //{
            //    var files = await query.GetFilesAsync();
            //    var count = files.Count();
            //};
            return await query.GetFilesAsync();
        }

        private StorageFolder GetSyncFolder()
        {
            return ApplicationData.Current.TemporaryFolder;
        }

        private StorageFolder GetLocalFolder()
        {
            return ApplicationData.Current.LocalFolder;
        }

        private string GenerateFileName()
        {
            return string.Format("{0}{1}", Path.GetRandomFileName(), WRITE_FILE_EXTENSION);
        }

        private async Task<StorageFile> WriteTempFile(string[] content)
        {
            var folder = GetSyncFolder();
            var file = await folder.CreateFileAsync(GenerateFileName(), CreationCollisionOption.OpenIfExists);
            
            await FileIO.WriteLinesAsync(file, content);
            return file;
        }
    }
}
