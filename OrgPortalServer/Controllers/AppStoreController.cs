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
using System.Web.Mvc;

namespace OrgPortalServer.Controllers
{
    public class AppStoreController : Controller
    {
        public ActionResult Index()
        {
            return View(IoCContainerFactory.Current.GetInstance<ApplicationRepository>().Applications);
        }
        // GET api/<controller>
        public ActionResult Detail(string id, string version)
        {
            try
            {
                IEnumerable<Application> appList =
                         IoCContainerFactory.Current
                        .GetInstance<ApplicationRepository>()
                        .Applications.Where(a => a.PackageFamilyName == id &&
                                                 a.Version == version);
                if (appList != null && appList.Count() > 0)
                {
                    var app = appList.Take(1).ToList()[0]; //TODO: better way?
                    return View(app);
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

    }
}