using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortalMonitor.DataModel
{
    [DataContract]
    public class AppInfo
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string PackageFamilyName { get; set; }
        [DataMember]
        public string PackageFile { get; set; }
        [DataMember]
        public string PackageName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string AppxUrl { get; set; }
        [DataMember]
        public string CertificateUrl { get; set; }
        [DataMember]
        public string CertificateFile { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
        [DataMember]
        public string SmallImageUrl { get; set; }
        [DataMember]
        public string Version { get; set; }
        [DataMember]
        public string InstallMode { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string BackgroundColor { get; set; }
        [DataMember]
        public DateTime DateAdded { get; set; }
        [DataMember]
        public string PublisherDisplayName { get; set; }
        [DataMember]
        public string PublisherId { get; set; }
        public bool UpdateAvailable { get; set; }
    }
}
