using OrgPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.Domain.Repositories
{
    public interface ApplicationRepository
    {
        IQueryable<Application> Applications { get; }
        void Add(Application model);
        void Remove(Application model);

        byte[] GetAppx(string packageFamilyName, string packageFile, string version);
        byte[] GetCertificate(string packageFamilyName, string certificateFile, string version);
        byte[] GetLogo(string packageFamilyName, string version);
        byte[] GetSmallLogo(string packageFamilyName, string version);

    }
}
