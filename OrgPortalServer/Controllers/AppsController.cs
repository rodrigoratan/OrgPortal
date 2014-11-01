using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using OrgPortalServer.Models;
using OrgPortal.Domain.Repositories;
using OrgPortal.Domain;
using OrgPortal.Domain.Models;

namespace OrgPortalServer.Controllers
{
    public class AppsController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<AppInfo> Get()
        {
            List<Application> listApp = new List<Application>();
            listApp =
            IoCContainerFactory
            .Current
            .GetInstance<ApplicationRepository>()
            .Applications
            .ToList();
            IEnumerable<AppInfo> retorno =
                 listApp
                .Select(a =>
                        new AppInfo
                        {
                            Name = a.Name,
                            DisplayName = a.DisplayName,
                            PackageFamilyName = a.PackageFamilyName,
                            PackageFile = a.PackageFile,
                            PackageName = a.PackageName,
                            CertificateFile = a.CertificateFile,
                            Description = a.Description,
                            Version = a.Version,
                            PublisherDisplayName = a.PublisherDisplayName,
                            PublisherId = a.PublisherId,
                            InstallMode = a.InstallMode,
                            Category = a.Category.Name,
                            DateAdded = a.DateAdded,
                            BackgroundColor = a.BackgroundColor,
                            AppPictures = ApplicationsController.GetAppPicturesByPackage(a.PackageFamilyName, a.Version)
                        });
            return retorno;
        }



        // GET api/<controller>/packagefamilyname
        public AppInfo Get(string id)
        {
            //var applicationLastVersion = IoCContainerFactory.Current.GetInstance<ApplicationRepository>().Applications.Where(a => a.PackageFamilyName == id).OrderByDescending(a => a.Version).Single();
            //return IoCContainerFactory.Current.GetInstance<ApplicationRepository>().Applications.ToList()

            try
            {
                List<Application> listApp = IoCContainerFactory.Current.GetInstance<ApplicationRepository>().Applications.ToList();

                AppInfo retorno =
                    //.Where(a => a.PackageFamilyName == id)
                        listApp
                       .OrderByDescending(a => a.Version)
                       .Select(a =>
                               new AppInfo
                               {
                                   Name = a.Name,
                                   DisplayName = a.DisplayName,
                                   PackageFamilyName = a.PackageFamilyName,
                                   PackageFile = a.PackageFile,
                                   PackageName = a.PackageName,
                                   CertificateFile = a.CertificateFile,
                                   Description = a.Description,
                                   Version = a.Version,
                                   PublisherDisplayName = a.PublisherDisplayName,
                                   PublisherId = a.PublisherId,
                                   InstallMode = a.InstallMode,
                                   Category = a.Category.Name,
                                   DateAdded = a.DateAdded,
                                   BackgroundColor = a.BackgroundColor,
                                   AppPictures = ApplicationsController.GetAppPicturesByPackage(a.PackageFamilyName, a.Version)
                               }).First();
                      //.Single(a => a.PackageFamilyName == id);

                return retorno;
            }
            catch /*(Exception ex)*/
            {
                return default(AppInfo);
            }
        }

        // POST api/<controller>/?search=<string>
        public IEnumerable<AppInfo> Post(string search)
        {
            return IoCContainerFactory
                   .Current
                   .GetInstance<ApplicationRepository>()
                   .Applications
                   .Where(a => a.Name.ToLower().Contains(search.ToLower()))
                   .ToList()
                   .Select(a =>
                           new AppInfo
                           {
                                Name = a.Name,
                                DisplayName = a.DisplayName,
                                PackageFamilyName = a.PackageFamilyName,
                                PackageFile = a.PackageFile,
                                PackageName = a.PackageName,
                                CertificateFile = a.CertificateFile,
                                Description = a.Description,
                                Version = a.Version,
                                PublisherDisplayName = a.PublisherDisplayName,
                                PublisherId = a.PublisherId,
                                InstallMode = a.InstallMode,
                                Category = a.Category.Name,
                                DateAdded = a.DateAdded,
                                BackgroundColor = a.BackgroundColor,
                                AppPictures = ApplicationsController.GetAppPicturesByPackage(a.PackageFamilyName, a.Version)
                           });
        }
    }
}