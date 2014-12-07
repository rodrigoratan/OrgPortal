using OrgPortalMonitor.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace OrgPortal.DataModel
{
    public interface IFileSyncManager
    {
        Task RequestAppInstall(string appxUrl, 
                               string appxFile, 
                               string certificateUrl,
                               string certificateFile,
                               string version,
                               string name,
                               string description,
                               string backgroundColor, 
                               string imageUrl);
        Task<List<AppInfo>> GetInstalledApps(List<AppInfo> apps);
        Task UpdateDevLicense();
        Task<IReadOnlyList<StorageFile>> ReadFiles();

    }
}
