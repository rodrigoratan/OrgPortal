using OrgPortal.Domain;
using OrgPortal.Domain.Models;
using OrgPortal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrgPortalServer.Controllers
{
    public class ApplicationsController : Controller
    {
        public ActionResult Index()
        {
            return View(IoCContainerFactory.Current.GetInstance<ApplicationRepository>().Applications);
        }

        [HttpPost]
        public ActionResult Application(int categoryID, 
                                        string installMode,
                                        string publisherId, 
                                        HttpPostedFileBase appxFile, 
                                        HttpPostedFileBase certificateFile)
        {
            if (!string.IsNullOrEmpty(publisherId))
            {
                if (appxFile == null || certificateFile == null)
                {
                    TempData["WarningMessage"] = "Application (.appx) and Certificate (.cer or .pfx) files are required.";
                }
                else
                {
                    using (var uow = IoCContainerFactory.Current.GetInstance<UnitOfWork>())
                    {
                        uow.ApplicationRepository.Add(
                            new Application(
                                appxFile.InputStream,
                               (appxFile.FileName.Contains("\\") ? appxFile.FileName.Substring(appxFile.FileName.LastIndexOf("\\") + 1) : appxFile.FileName)
                               , certificateFile.InputStream,
                               (certificateFile.FileName.Contains("\\") ? certificateFile.FileName.Substring(certificateFile.FileName.LastIndexOf("\\") + 1) : certificateFile.FileName)
                               , publisherId
                               , categoryID
                               , installMode));

                        uow.Commit();
                    }
                    TempData["WarningMessage"] = "Application " + appxFile.FileName + " saved.";
                }
            }
            else
            {
                TempData["WarningMessage"] = "PublisherId is required to save.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(string id, string version)
        {
            using (var uow = IoCContainerFactory.Current.GetInstance<UnitOfWork>())
            {
                var app = uow.ApplicationRepository.Applications.Single(a => a.PackageFamilyName == id && a.Version == version);
                if (app != null)
                {
                    uow.ApplicationRepository.Remove(app);
                    uow.Commit();
                }
            }
            return Json(true);
        }
	}
}