using Newtonsoft.Json;
using OrgPortalMonitor.DataModel;
using System.Collections.Generic;
using System.Composition;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;

namespace OrgPortal.DataModel
{
    [Export(typeof(IPortalDataSource))]
    [Shared]
    public class PortalDataSource : IPortalDataSource
    {
        private /*static readonly*/ string _serviceURI = "http://orgportal/api/";

        public async Task<List<AppInfo>> GetAppListAsync()
        {
            try
            {
                List<AppInfo> retorno = new List<AppInfo>();
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(_serviceURI + "Apps");
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var apps = JsonConvert.DeserializeObject<List<AppInfo>>(data);

                        //return apps;
                        retorno = apps;
                    }
                }

                return retorno;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<AppInfo>> GetAppListWithPicturesAsync()
        {
            try
            {
                List<AppInfo> retorno = new List<AppInfo>();
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(_serviceURI + "Apps");
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var apps = JsonConvert.DeserializeObject<List<AppInfo>>(data);

                        //return apps;
                        retorno = apps;
                    }
                }

                foreach (var item in retorno)
                {
                    item.AppPictures = await GetAppPicturesAsync(item.PackageFamilyName, item.Version);
                }

                return retorno;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<string>> GetAppPicturesAsync(string id, string version)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(string.Format("{0}Pictures/{1}/?version={2}", _serviceURI, id, version));
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var pictures = JsonConvert.DeserializeObject<List<string>>(data);

                    return pictures;
                }
            }

            return null;
        }

        public async Task<List<AppInfo>> GetDistinctAppListAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(_serviceURI + "Apps");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var apps = JsonConvert.DeserializeObject<List<AppInfo>>(data);
                    List<AppInfo> returnApps = new List<AppInfo>();
                    returnApps = apps.OrderBy(a => a.PackageFamilyName)
                                     .ThenByDescending(a=>a.Version)
                                     .GroupBy(a => a.PackageFamilyName)
                                     .Select(x => x.First()).ToList();
                    return returnApps;
                }
            }

            return null;
        }

        //TODO: code is duplicated from Installer.cs , plan to add a Shared Project for shared code
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

        public async Task<List<AppInfo>> SearchAppsAsync(string query)
        {
            using (var client = new HttpClient())
            {
                var requestUrl = string.Format("{0}Apps?search={1}", _serviceURI, query);
                var response = await client.PostAsync(requestUrl, null);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var apps = JsonConvert.DeserializeObject<List<AppInfo>>(data);

                    return apps;
                }
            }

            return null;
        }

        public async Task<OrgInfo> LoadPortalDataAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(_serviceURI + "OrgPortal");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var info = JsonConvert.DeserializeObject<List<string>>(data);
                    if (info.Count > 1)
                    {
                        var org = new OrgInfo()
                        {
                            Name = info[0],
                            FeatureURL = info[1]
                        };

                        return org;
                    }
                }
            }

            return null;
        }

        public async Task<BrandingInfo> GetBrandingDataAsync()
        {
            try
            {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(_serviceURI + "Branding");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var branding = JsonConvert.DeserializeObject<BrandingInfo>(data);

                    return branding;
                }
            }
            }
            catch (System.Net.WebException ex)
            {
                LogException(ex);
                return null;
            }
            catch (System.ObjectDisposedException ex)
            {
                LogException(ex);
                return null;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                LogException(ex);
                return null;
            }
            catch (System.Exception ex)
            {
                LogException(ex);
                return null;
                //throw;
            }
//System.Net.Sockets.SocketException
//System.Net.WebException
//System.ObjectDisposedException
//

            return null;
        }

        private static void LogException(System.Exception ex)
        {
            Debug.WriteLine("Message:");
            Debug.WriteLine(ex.Message);
            Debug.WriteLine("InnerException:");
            Debug.WriteLine(ex.InnerException);
            Debug.WriteLine("StackTrace:");
            Debug.WriteLine(ex.StackTrace);
        }

        public async Task<List<CategoryInfo>> GetCategoryListAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(_serviceURI + "AppCategories");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var categories = JsonConvert.DeserializeObject<List<CategoryInfo>>(data);

                    return categories;
                }
            }

            return null;
        }

        public async Task<List<AppInfo>> GetAppsForCategoryAsync(int categoryId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(string.Format("{0}{1}{2}", _serviceURI, "AppCategories/", categoryId));
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var category = JsonConvert.DeserializeObject<CategoryInfo>(data);

                    return category.Apps.ToList();
                }
            }

            return null;
        }
    }
}