using OrgPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.Domain.Repositories
{
    public interface PictureRepository
    {
        void Add(Picture picture);

        void Remove(Picture picture);

        byte[] GetImage(string packageFamilyName, string version, string image);
        byte[] GetImage(Picture picture);

        List<string> GetImages(string packageFamilyName, string version);
    }
}
