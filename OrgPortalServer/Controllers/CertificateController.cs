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
    public class CertificateController : ApiController
    {
        // GET api/<controller>/packagefamilyname
        public HttpResponseMessage Get(string id)
        {
            var applicationLastVersion = IoCContainerFactory
                                        .Current
                                        .GetInstance<ApplicationRepository>()
                                        .Applications
                                        .ToList()
                                        .Where(a => a.PackageFamilyName == id)
                                        .OrderByDescending(a => a.Version)
                                        .First();

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new MemoryStream(applicationLastVersion.Certificate));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentLength = applicationLastVersion.Certificate.Length;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = applicationLastVersion.CertificateFile };
            return response;
        }

        public HttpResponseMessage GetVersion(string id, string version)
        {
            var applicationVersion = IoCContainerFactory
                                    .Current
                                    .GetInstance<ApplicationRepository>()
                                    .Applications
                                    .Single(a => a.PackageFamilyName == id   && 
                                                 a.Version           == version);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new MemoryStream(applicationVersion.Certificate));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentLength = applicationVersion.Certificate.Length;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = applicationVersion.CertificateFile };
            return response;
        }
    }
}