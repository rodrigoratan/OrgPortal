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
                               string certificateFile);
        Task<List<AppInfo>> GetInstalledApps();
        Task UpdateDevLicense();
        Task<IReadOnlyList<StorageFile>> ReadFiles();


    }
}
