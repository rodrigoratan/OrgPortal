using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Deployment.Application;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OrgPortalMonitor.Common
{
    public static class Utils
    {
        //var RequestQueryString = GetQueryStringParameters();
        public static NameValueCollection GetQueryStringParameters()
        {
            NameValueCollection nameValueTable = new NameValueCollection();
            try
            {
                if (ApplicationDeployment.IsNetworkDeployed &&
                    ApplicationDeployment.CurrentDeployment.ActivationUri != null)
                {
                    string queryString = ApplicationDeployment.CurrentDeployment.ActivationUri.Query;
                    nameValueTable = HttpUtility.ParseQueryString(queryString);
                }
                return (nameValueTable);
            }
            catch /*(Exception ex)*/
            {
                //OutputException(ex);
                return (nameValueTable);
            }
        }
        public static string Join<TItem>(this IEnumerable<TItem> enumerable, string separator)
        {
            return string.Join(separator, enumerable);
        }

    }
}
