using Ionic.Zip;
using OrgPortal.Domain.Extensions;
using OrgPortal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.ComponentModel;

namespace OrgPortal.Domain.Models
{
    public class Application
    {
        public string Name { get; set; }

        [DisplayName("Package Name")]
        public string PackageName { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }

        [DisplayName("Processor Architecture")]
        public string ProcessorArchitecture { get; set; }

        [DisplayName("Display Name")]
        public string DisplayName { get; set; }

        [DisplayName("Publisher Display Name")]
        public string PublisherDisplayName { get; set; }

        [DisplayName("Publisher Id")]
        public string PublisherId { get; set; }
        public string Publisher { get; set; }

        [DisplayName("Install Mode")]
        public string InstallMode { get; set; }

        [DisplayName("Package Family Name")]
        public string PackageFamilyName { get; set; }

        [DisplayName("Package File")]
        public string PackageFile { get; set; }

        [DisplayName("Certificate File")]
        public string CertificateFile { get; set; }

        [DisplayName("Date Added")]
        public DateTime DateAdded { get; set; }

        [DisplayName("Background Color")]
        public string BackgroundColor { get; set; }

        [DisplayName("Category ID")]
        public int CategoryID { get; set; }

        // TODO: Make this into a proper navigation property somehow
        public Category Category
        {
            get { return IoCContainerFactory.Current.GetInstance<CategoryRepository>().Categories.Single(c => c.ID == CategoryID); }
        }

        private byte[] _package;
        public byte[] Package
        {
            get
            {
                if (_package == null || _package.Length == 0)
                    _package = IoCContainerFactory
                                .Current
                                .GetInstance<ApplicationRepository>()
                                .GetAppx(PackageFamilyName
                                        ,PackageFile
                                        ,Version);
                return _package;
            }
            set { _package = value; }
        }

        private byte[] _certificate;
        public byte[] Certificate
        {
            get
            {
                if (_certificate == null || _certificate.Length == 0)
                    _certificate = IoCContainerFactory.Current.GetInstance<ApplicationRepository>().GetCertificate(PackageFamilyName, CertificateFile, Version);
                return _certificate;
            }
            set { _certificate = value; }
        }

        private byte[] _logo;
        public byte[] Logo
        {
            get
            {
                if (_logo == null || _logo.Length == 0)
                    _logo = IoCContainerFactory.Current.GetInstance<ApplicationRepository>().GetLogo(PackageFamilyName, Version);
                return _logo;
            }
            set { _logo = value; }
        }

        private byte[] _smallLogo;
        public byte[] SmallLogo
        {
            get
            {
                if (_smallLogo == null || _smallLogo.Length == 0)
                    _smallLogo = IoCContainerFactory.Current.GetInstance<ApplicationRepository>().GetSmallLogo(PackageFamilyName, Version);
                return _smallLogo;
            }
            set { _smallLogo = value; }
        }


        public Application() { }
        //public Application(Application appRef)
        //{
        //    this = appRef;
        //}

        public Application(Stream dataAppx,
                           string fileAppx,
                           Stream certificateData,
                           string certificateFile,
                           string publisherId,
                           int categoryID, 
                           string installMode)
        {
            CategoryID = categoryID;
            InstallMode = installMode;
            DateAdded = DateTime.UtcNow;

            PublisherId = publisherId;
            ExtractValuesFromPackage(dataAppx);
            dataAppx.Seek(0, SeekOrigin.Begin);
            Package = dataAppx.ReadFully();

            certificateData.Seek(0, SeekOrigin.Begin);
            Certificate = certificateData.ReadFully();

            //TODO: This is not correct.  Publisher needs to be the Publisher ID, which is a hash of something. Need to figure out how to calculate/fetch the Publisher ID.
            //RESOLVED: For now this Hash (Publisher ID) is a required entry field to allow upload the app to the store so we can match the PackageFamilyName
            PackageFamilyName = PackageName + "_" + PublisherId;
            PackageFile = fileAppx;
            CertificateFile = certificateFile;
        }

        //TODO: Move all of this extraction logic into an infrastructure assembly to get the Zip references out of the domain?
        private void ExtractValuesFromPackage(Stream data)
        {
            using (var zipArchive = ZipFile.Read(data))
            {
                var manifest = GetManifestFromZip(zipArchive);
                ExtractName(manifest);
                ExtractPackageName(manifest);
                ExtractDisplayName(manifest);
                ExtractDescription(manifest);
                ExtractVersion(manifest);
                ExtractPublisher(manifest);
                ExtractPublisherDisplayName(manifest);
                ExtractProcessorArchitecture(manifest);
                ExtractLogo(zipArchive, manifest);
                ExtractStoreLogo(zipArchive, manifest);
                ExtractBackgroundColor(manifest);
            }
        }

        private static XDocument GetManifestFromZip(ZipFile zipArchive)
        {
            // Juggling streams because they were closed when I accessed them before
            // See here: http://stackoverflow.com/q/8100590/328193
            // This can probably be made more efficient
            var manifestFile = zipArchive.Entries.Single(e => e.FileName == "AppxManifest.xml");
            var manifestData = new MemoryStream();
            manifestFile.Extract(manifestData);
            var manifest = XDocument.Load(new MemoryStream(manifestData.ToArray()));
            return manifest;
        }

        private void ExtractName(XDocument manifest)
        {
            //Name = ExtractValueFromVisualElementsNode(manifest, "DisplayName");
            Name = manifest
                  .Descendants()
                  .Single(d => d.Name.LocalName == "Properties")
                  .Descendants().Single(d => d.Name.LocalName == "DisplayName")
                  .Value;
        }

        private void ExtractPackageName(XDocument manifest)
        {
            PackageName = manifest
                  .Descendants()
                  .Single(d => d.Name.LocalName == "Identity")
                  .Attributes().Single(a => a.Name.LocalName == "Name")
                  .Value;
        }


        private void ExtractBackgroundColor(XDocument manifest)
        {
            BackgroundColor = ExtractValueFromVisualElementsNode(manifest, "BackgroundColor");
        }

        private void ExtractDescription(XDocument manifest)
        {
            Description = ExtractValueFromVisualElementsNode(manifest, "Description");
        }

        private void ExtractPublisher(XDocument manifest)
        {
            Publisher = manifest.Descendants().Single(d => d.Name.LocalName == "Identity")
                                .Attributes().Single(a => a.Name.LocalName == "Publisher")
                                .Value;
        }

        private void ExtractVersion(XDocument manifest)
        {
            Version = manifest.Descendants().Single(d => d.Name.LocalName == "Identity")
                              .Attributes().Single(a => a.Name.LocalName == "Version")
                              .Value;
        }

        private void ExtractDisplayName(XDocument manifest)
        {
            DisplayName = ExtractValueFromVisualElementsNode(manifest, "DisplayName");
            //DisplayName = manifest.Descendants().Single(d => d.Name.LocalName == "Properties")
            //                      .Descendants().Single(d => d.Name.LocalName == "DisplayName")
            //                      .Value;
        }

        private void ExtractPublisherDisplayName(XDocument manifest)
        {
            PublisherDisplayName = manifest.Descendants().Single(d => d.Name.LocalName == "Properties")
                                           .Descendants().Single(d => d.Name.LocalName == "PublisherDisplayName")
                                           .Value;
        }

        private void ExtractProcessorArchitecture(XDocument manifest)
        {
            ProcessorArchitecture = manifest.Descendants().Single(d => d.Name.LocalName == "Identity")
                                            .Attributes().Single(a => a.Name.LocalName == "ProcessorArchitecture")
                                            .Value;
        }

        private void ExtractLogo(ZipFile zipArchive, XDocument manifest)
        {
            var logoData = ExtractAssetImageFromVisualElementsNode(zipArchive, manifest, "Square150x150Logo");
            Logo = new MemoryStream(logoData.ToArray()).ReadFully();
        }

        private void ExtractStoreLogo(ZipFile zipArchive, XDocument manifest)
        {
            var logoFileName = manifest.Descendants().Single(d => d.Name.LocalName == "Properties")
                                       .Descendants().Single(d => d.Name.LocalName == "Logo")
                                       .Value;
            var logoFile = GetZipEntryForFileName(zipArchive, logoFileName);
            var logoData = new MemoryStream();
            if (logoFile != null)
                logoFile.Extract(logoData);
            SmallLogo = new MemoryStream(logoData.ToArray()).ReadFully();
        }

        private static MemoryStream ExtractAssetImageFromVisualElementsNode(ZipFile zipArchive, XDocument manifest, string attributeName)
        {
            var logoFileName = ExtractValueFromVisualElementsNode(manifest, attributeName);
            var logoFile = GetZipEntryForFileName(zipArchive, logoFileName);
            var logoData = new MemoryStream();
            if (logoFile != null)
                logoFile.Extract(logoData);
            return logoData;
        }

        private static ZipEntry GetZipEntryForFileName(ZipFile zipArchive, string logoFileName)
        {
            var fileName = Path.GetFileNameWithoutExtension(logoFileName);
            var fileExtension = Path.GetExtension(logoFileName);
            var logoFile = zipArchive.Entries.SingleOrDefault(e => Path.GetFileNameWithoutExtension(e.FileName).Equals(string.Format("{0}.scale-100", fileName), StringComparison.InvariantCultureIgnoreCase) &&
                                                                   Path.GetExtension(e.FileName).Equals(fileExtension, StringComparison.InvariantCultureIgnoreCase)) ??
                           zipArchive.Entries.SingleOrDefault(e => Path.GetFileNameWithoutExtension(e.FileName).Equals(string.Format("{0}", fileName), StringComparison.InvariantCultureIgnoreCase) &&
                                                                   Path.GetExtension(e.FileName).Equals(fileExtension, StringComparison.InvariantCultureIgnoreCase));
            return logoFile;
        }

        private static string ExtractValueFromVisualElementsNode(XDocument manifest, string attributeName)
        {

            try
            {
                var obj1 = manifest.Descendants().Single(d => d.Name.LocalName == "Applications");
                if (obj1 == null) return string.Empty;
                var obj2 = obj1.Descendants().First(d => d.Name.LocalName == "Application"); // TODO: What if there is more than one application?
                if (obj2 == null) return string.Empty;
                var obj3 = obj2.Descendants().Single(d => d.Name.ToString().EndsWith("VisualElements", StringComparison.InvariantCultureIgnoreCase));
                if (obj3 == null) return string.Empty;
                var obj4 = obj3.Attributes().SingleOrDefault(a => a.Name.LocalName == attributeName);
                if (obj4 == null && attributeName.EndsWith("Logo"))
                {
                    var _attributeName = string.Empty;
                    if (attributeName == "Square30x30Logo")
                    {
                        _attributeName = "SmallLogo";
                    }
                    else if (attributeName == "Square150x150Logo")
                    {
                        _attributeName = "Logo";
                    }
                    else if (attributeName == "SmallLogo")
                    {
                        _attributeName = "Square30x30Logo";
                    }
                    else if (attributeName == "Logo")
                    {
                        _attributeName = "Square150x150Logo";
                    }
                    obj4 = obj3.Attributes().SingleOrDefault(a => a.Name.LocalName == _attributeName);
                }
                if (obj4 == null)
                {
                    return string.Empty;
                }
                else
                {
                    return obj4;
                }
            }
            catch 
            {
                return string.Empty;
            }

        }
    }
}
