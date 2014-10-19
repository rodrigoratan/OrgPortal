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
    public class AppxController : ApiController
    {
        // GET api/<controller>/packagefamilyname
        // http://orgportal/api/Appx/Agile for Windows_CN=C42B4C41-BEC2-494C-AFE8-5E95519F8A0C
        public HttpResponseMessage Get(string id)
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

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new MemoryStream(applicationLastVersion.Package));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentLength = applicationLastVersion.Package.Length;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = applicationLastVersion.PackageFile /*application.PackageFamilyName + ".appx"*/ };
            return response;
        }

        public HttpResponseMessage Get(string id, string version)
        {
            //var queryItems = Request.RequestUri.ParseQueryString();
            //string version = queryItems["version"];
            var applicationVersion = IoCContainerFactory
                                     .Current
                                     .GetInstance<ApplicationRepository>()
                                     .Applications.Single(a => a.PackageFamilyName == id   && 
                                                               a.Version           == version);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new MemoryStream(applicationVersion.Package));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentLength = applicationVersion.Package.Length;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = applicationVersion.PackageFile /*application.PackageFamilyName + ".appx"*/ };
            return response;
        }
    }
}