using OrgPortal.Domain.Models;
using OrgPortal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.Infrastructure.DAL.Repositories
{
    public class ApplicationRepositoryImplementation : ApplicationRepository
    {
        private UnitOfWorkImplementation _unitOfWork;

        public IQueryable<Application> Applications
        {
            get { return _unitOfWork.DBApplications; }
        }

        public ApplicationRepositoryImplementation(UnitOfWorkImplementation unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(Application model)
        {
            _unitOfWork.DBApplications.Add(model);
        }

        public void Remove(Application model)
        {
            _unitOfWork.DBApplications.Remove(model);
        }

        // TODO: Move file system operations to another infrastructure dependency assembly
        public byte[] GetAppx(string packageFamilyName, string packageFile, string version)
        {
            //if (!File.Exists(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], packageFamilyName + ".appx")))
            if (!File.Exists(        Path.Combine(ConfigurationManager.AppSettings["AppFolder"], packageFamilyName, version, packageFile)))
                throw new ArgumentException("Unable to find the requested appx file.");
            //return File.ReadAllBytes(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], packageFamilyName + ".appx" ));
            return File.ReadAllBytes(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], packageFamilyName, version, packageFile));
        }

        public byte[] GetCertificate(string packageFamilyName, string certificateFile, string version)
        {
            if (!File.Exists(        Path.Combine(ConfigurationManager.AppSettings["AppFolder"], packageFamilyName, version, certificateFile)))
                throw new ArgumentException("Unable to find the request certficate file.");
            return File.ReadAllBytes(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], packageFamilyName, version, certificateFile));
        }

        public byte[] GetLogo(string packageFamilyName, string version)
        {
            if (!File.Exists(        Path.Combine(ConfigurationManager.AppSettings["AppFolder"], packageFamilyName, version, packageFamilyName + ".png")))
                return new byte[0];
            return File.ReadAllBytes(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], packageFamilyName, version, packageFamilyName + ".png"));
        }

        public byte[] GetSmallLogo(string packageFamilyName, string version)
        {
            if (!File.Exists(        Path.Combine(ConfigurationManager.AppSettings["AppFolder"], packageFamilyName, version, packageFamilyName + "-small.png")))
                return new byte[0];
            return File.ReadAllBytes(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], packageFamilyName, version, packageFamilyName + "-small.png"));
        }
    }
}
