﻿using OrgPortal.Domain;
using OrgPortal.Domain.Models;
using OrgPortal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;

namespace OrgPortalServer.Controllers
{
    public class ApplicationsController : Controller
    {
        public ActionResult Index()
        {
            return View(IoCContainerFactory.Current.GetInstance<ApplicationRepository>().Applications);
        }

        public ActionResult Detail(string id, string version)
        {
            try
            {
                IEnumerable<Application> appList = 
                         IoCContainerFactory.Current
                        .GetInstance<ApplicationRepository>()
                        .Applications.Where(a => a.PackageFamilyName == id && 
                                                 a.Version           == version);
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

        [HttpPost]
        public ActionResult Application(int categoryID, 
                                        string installMode,
                                        string publisherId, 
                                        HttpPostedFileBase appxFile, 
                                        HttpPostedFileBase certificateFile)
        {
            if (!string.IsNullOrEmpty(publisherId))
            {
                if (appxFile == null)
                {
                    TempData["WarningMessage"] = "Application file (.appx) is required. Certificate (.cer or .pfx) is optional. ";
                }
                else
                {
                    using (var uow = IoCContainerFactory.Current.GetInstance<UnitOfWork>())
                    {
                        uow.ApplicationRepository.Add(
                            new Application(
                                appxFile.InputStream,
                               (appxFile.FileName.Contains("\\") ? 
                                appxFile.FileName.Substring(appxFile.FileName.LastIndexOf("\\") + 1) : 
                                appxFile.FileName),
                               (certificateFile != null ? certificateFile.InputStream : null),
                               (certificateFile != null ? 
                                   (certificateFile.FileName.Contains("\\") ? 
                                    certificateFile.FileName.Substring(certificateFile.FileName.LastIndexOf("\\") + 1) : 
                                    certificateFile.FileName) : 
                                   string.Empty),
                               publisherId,
                               categoryID,
                               installMode
                            )
                        );

                        uow.Commit();
                    }
                    TempData["WarningMessage"] = "Application " + appxFile.FileName + " saved.";
                }
            }
            else
            {
                TempData["WarningMessage"] = "PublisherId is required to save. Enter 'None' if unknown at this point (may require editing later). ";
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

        public ActionResult DeleteWithRedirect(string id, string version)
        {
            using (var uow = IoCContainerFactory.Current.GetInstance<UnitOfWork>())
            {
                var app = uow.ApplicationRepository.Applications
                         .Single(a => a.PackageFamilyName == id && a.Version == version);
                if (app != null)
                {
                    uow.ApplicationRepository.Remove(app);
                    uow.Commit();
                }
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult ApplicationImage(string packageFamilyName,
                                             HttpPostedFileBase imageFile,
                                             string version)
        {
            if (!string.IsNullOrEmpty(packageFamilyName))
            {
                if (imageFile == null)
                {
                    TempData["WarningMessage"] = "PNG Image files are required.";
                }
                else
                {
                    #region Old
                    using (var uow = IoCContainerFactory.Current.GetInstance<UnitOfWork>())
                    {
                        uow.PictureRepository.Add(
                            new Picture(
                              packageFamilyName
                            , version
                            , imageFile.InputStream
                            , (imageFile.FileName.Contains("\\") ?
                              imageFile.FileName.Substring(imageFile.FileName.LastIndexOf("\\") + 1)
                             : imageFile.FileName)
                            )
                        );

                        uow.Commit();
                    }
                    #endregion

                    TempData["WarningMessage"] = "Image " + imageFile.FileName + " saved to packageFamilyName " + packageFamilyName + " and version " + version + ".";
                }
            }
            else
            {
                TempData["WarningMessage"] = "Package Family Name is required to save.";
            }

            return RedirectToAction("Detail", new
            {
                id = packageFamilyName + "/",
                version = version
            });

        }

        private bool IsValid(HttpPostedFileBase imageFile, int width, int height)
        {
            if (imageFile == null)
                return true;
            // TODO: Support other image file types?  Check the requirements for the Windows app.
            if (!imageFile.ContentType.Equals("image/png", StringComparison.InvariantCultureIgnoreCase))
                return false;
            using (Image image = Image.FromStream(imageFile.InputStream, true, true))
                if (image.Width != width || image.Height != height)
                    return false;
            imageFile.InputStream.Position = 0;
            return true;
        }

        public static IEnumerable<string> GetAppPicturesByPackage(string packageFamilyName, string version)
        {
            List<string> listPictures = new List<string>();
            listPictures =
            IoCContainerFactory
            .Current
            .GetInstance<PictureRepository>().GetImagesApi(packageFamilyName, version);
            return listPictures;
        }

 
	}
}