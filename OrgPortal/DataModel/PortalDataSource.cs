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
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(_serviceURI + "Apps");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var apps = JsonConvert.DeserializeObject<List<AppInfo>>(data);

                    return apps;
                }
            }

            return null;
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