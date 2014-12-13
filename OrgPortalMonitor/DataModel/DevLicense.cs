using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortalMonitor.DataModel
{
    public class DevLicense
    {
        private string _dateLicense = string.Empty;
        public string DateLicense
        {
            get { return _dateLicense; }
            set { _dateLicense = value; }
        }

        public string DevLicenseOutput { get; set; }

        public DateTime DatetimeLicense {
            get
            {

                DateTime _datetimeLicense;
                if (DateTime.TryParse(_dateLicense, out _datetimeLicense))
                {
                    return _datetimeLicense;
                }
                else
                {
                    return DateTime.MaxValue;
                }
            } 
        }

        public bool IsValid { get; set; }
        
    }
}
