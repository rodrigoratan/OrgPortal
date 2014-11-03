using OrgPortal.Domain.Repositories;
using OrgPortal.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OrgPortal.Domain.Models
{
    public class Picture //: Application
    {
        public string FileName { get; set; }
        public string Version { get; set; }
        public string PackageFamilyName { get; set; }
        public string PackageFile { get; set; }

        private byte[] _image;
        public byte[] Image
        {
            get
            {
                if (_image == null || _image.Length == 0)
                    _image = IoCContainerFactory
                            .Current
                            .GetInstance<PictureRepository>()
                            .GetImage(PackageFamilyName
                                     ,PackageFile
                                     ,Version);
                return _image;
            }
            set { _image = value; }
        }
        
        public Picture() { }

        public Picture(string packageFamilyName
                      ,string version
                      ,Stream imageData
                      ,string fileName)
        {
            PackageFamilyName = packageFamilyName;
            Version = version
;
            FileName = fileName;
            imageData.Seek(0, SeekOrigin.Begin);
            Image = imageData.ReadFully();
        }

    }
}
