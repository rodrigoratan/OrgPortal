using OrgPortal.Domain;
using OrgPortal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace OrgPortalServer.Controllers
{
    public class SmallLogoController : ApiController
    {
        //// GET api/<controller>/packagefamilyname
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

            var logo = IoCContainerFactory.Current.GetInstance<ApplicationRepository>().GetSmallLogo(id, applicationLastVersion.Version);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new MemoryStream(logo));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            response.Content.Headers.ContentLength = logo.Length;
            return response;
        }

        public HttpResponseMessage Get(string id, string version)
        {
            var logo = IoCContainerFactory.Current.GetInstance<ApplicationRepository>().GetSmallLogo(id, version);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new MemoryStream(logo));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            response.Content.Headers.ContentLength = logo.Length;
            return response;
        }

    }
}
