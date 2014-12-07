using OrgPortal.Domain;
using OrgPortal.Domain.Models;
using OrgPortal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Web.Http;

namespace OrgPortalServer.Controllers
{
    public class PicturesController : ApiController
    {

        public IEnumerable<string> Get(string id, string version)
        {
            return IoCContainerFactory
                    .Current
                    .GetInstance<PictureRepository>()
                    .GetImagesApi(id, version);

        }
    }
}
