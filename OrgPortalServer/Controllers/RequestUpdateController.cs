using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OrgPortalServer.Models;
using System.Net.Http.Headers;
using System.Web;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using OrgPortal.Domain;
using OrgPortal.Domain.Repositories;
using OrgPortal.Domain.Models;

namespace OrgPortalServer.Controllers
{
    public class RequestUpdateController : ApiController
    {
        const string InstallRequestFileName = "InstallRequest.txt";
        // GET api/<controller>/packagefamilyname
        // http://orgportal/api/Appx/Agile for Windows_CN=C42B4C41-BEC2-494C-AFE8-5E95519F8A0C
        public HttpResponseMessage Get(string id)
        {
            try
            {
                var applicationLastVersion =
                        IoCContainerFactory
                        .Current
                        .GetInstance<ApplicationRepository>()
                        .Applications
                        .ToList()
                        .Where(a => a.PackageFamilyName == id)
                        .OrderByDescending(a => a.Version)
                        .First();

                AppInfo appInfo = ApplicationToAppInfo(applicationLastVersion);
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                string appRequest = string.Join(Environment.NewLine, InstallAppRequestArray(appInfo.AppxUrl, appInfo.PackageFile, appInfo.CertificateUrl, appInfo.CertificateFile));
                response.Content = new StringContent(appRequest);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = InstallRequestFileName;
                return response;
            }
            catch (Exception ex)
            {
                //ExceptionLogger.LogException(ex);
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        private static AppInfo ApplicationToAppInfo(Application application)
        {
            AppInfo appInfo =
                    new AppInfo()
                    {
                        Name = application.Name,
                        DisplayName = application.DisplayName,
                        PackageFamilyName = application.PackageFamilyName,
                        PackageFile = application.PackageFile,
                        PackageName = application.PackageName,
                        CertificateFile = application.CertificateFile,
                        Description = application.Description,
                        Version = application.Version,
                        PublisherDisplayName = application.PublisherDisplayName,
                        PublisherId = application.PublisherId,
                        InstallMode = application.InstallMode,
                        Category = application.Category.Name,
                        DateAdded = application.DateAdded,
                        BackgroundColor = application.BackgroundColor,
                        AppPictures = ApplicationsController.GetAppPicturesByPackage(application.PackageFamilyName, application.Version)
                    };
            return appInfo;
        }

        public static string[] InstallAppRequestArray(string appxUrl, string appxFile, string certificateUrl, string certificateFile)
        {
            var app = new string[] { "install",         appxUrl, 
                                     "appxFile",        appxFile, 
                                     "certificateUrl",  certificateUrl, 
                                     "certificateFile", certificateFile,
                                     "saveAt",          "INetCache" /*string.Empty*/
                                   };
            return app;
        }

        private bool UpdateAvailable(string serverAppVersion, string installedAppVersion)
        {
            var result = false;
            var serverVersion = serverAppVersion.Split('.');
            var installedVersion = installedAppVersion.Split('.');
            if (int.Parse(serverVersion[0]) > int.Parse(installedVersion[0]))
                result = true;
            else if (int.Parse(serverVersion[1]) > int.Parse(installedVersion[1]))
                result = true;
            else if (int.Parse(serverVersion[2]) > int.Parse(installedVersion[2]))
                result = true;
            else if (int.Parse(serverVersion[3]) > int.Parse(installedVersion[3]))
                result = true;
            return result;
        }

        public HttpResponseMessage Get(string id, string version)
        {
            //var queryItems = Request.RequestUri.ParseQueryString();
            //string version = queryItems["version"];
            
           
            try
            {
                //var applicationVersion = IoCContainerFactory
                //                         .Current
                //                         .GetInstance<ApplicationRepository>()
                //                         .Applications.Single(a => a.PackageFamilyName == id   
                //                                                && a.Version           == version);

                var applicationLastVersion =
                        IoCContainerFactory
                        .Current
                        .GetInstance<ApplicationRepository>()
                        .Applications
                        .ToList()
                        .Where(a => a.PackageFamilyName == id)
                        .OrderByDescending(a => a.Version)
                        .First();

                AppInfo appInfo = ApplicationToAppInfo(applicationLastVersion);
                string appRequest = string.Empty;
                if (UpdateAvailable(appInfo.Version, version))
                {
                    appRequest = string.Join(Environment.NewLine, InstallAppRequestArray(appInfo.AppxUrl, appInfo.PackageFile, appInfo.CertificateUrl, appInfo.CertificateFile));
                }

                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(appRequest);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = InstallRequestFileName;
                return response;
            }
            catch (Exception ex)
            {
                //ExceptionLogger.LogException(ex);
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}