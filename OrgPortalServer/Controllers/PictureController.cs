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
    public class PictureController : ApiController
    {
        //// GET api/picture/packagefamilyname/?version=1.0&filename=arquivo1.png
        public HttpResponseMessage Get(string id, string version, string filename)
        {
            //var picture = new OrgPortal.Domain.Models.Picture();
            ////id, version, filename
            //picture.PackageFamilyName = id;
            //picture.Version = version;
            //picture.FileName = filename;
            var image = IoCContainerFactory
                       .Current
                       .GetInstance<PictureRepository>()
                       .GetImage(id, version, filename);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new MemoryStream(image));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            response.Content.Headers.ContentLength = image.Length;
            response.Content.Headers.Expires = DateTimeOffset.Now;
            return response;
        }

        
    }
}
