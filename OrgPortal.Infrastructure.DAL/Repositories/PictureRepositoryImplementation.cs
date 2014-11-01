using OrgPortal.Domain.Models;
using OrgPortal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.Infrastructure.DAL.Repositories
{
    public class PictureRepositoryImplementation : PictureRepository
    {
        private UnitOfWorkImplementation _unitOfWork;

        public PictureRepositoryImplementation(UnitOfWorkImplementation unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public /*async*/ void Add(Picture image)
        {
            try
            {
                var folderPath =
                    Path.Combine(ConfigurationManager.AppSettings["AppFolder"]
                                ,image.PackageFamilyName
                                ,image.Version
                                ,"Album");

                var imageName = image.FileName;
                var imageData = image.Image;

                if (!System.IO.Directory.Exists(folderPath))
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                }
                var newFilePath = Path.Combine(folderPath, imageName);
                if (System.IO.File.Exists(newFilePath))
                {
                    System.IO.File.Delete(newFilePath);
                }

                System.IO.File.WriteAllBytes(newFilePath, imageData);
            }
            catch
            {
            }
            //var imageFile = System.IO.File.Create(newFilePath);
            //using (imageFile)
            //{
            //    //TODO: is it right? test it...
            //    imageFile.WriteAsync(imageData, 0, imageData.Length);
            //    //await requestFile.WriteLineAsync("install");
            //}
            //TODO: add file to FS
        }

        public void Remove(Picture image)
        {
            string packageFamilyName = image.PackageFamilyName;
            string version = image.Version;
            string imageName = image.FileName;

            var folderPath =
                Path.Combine(ConfigurationManager.AppSettings["AppFolder"]
                            , packageFamilyName
                            , version
                            , "Album");

            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.Delete(Path.Combine(folderPath, imageName));
            }
        }

        public List<string> GetImages(string packageFamilyName, string version)
        {
            var retorno = new List<string>();

            foreach (var path in GetImagesPath(packageFamilyName, version))
            {
                retorno.Add(path.Substring(path.LastIndexOf('\\') + 1));
            }
            return retorno;
        }

        public List<string> GetImagesPath(string packageFamilyName, string version)
        {
            var retorno = new List<string>();
            try
            {
                var folderPath =
                    Path.Combine(ConfigurationManager.AppSettings["AppFolder"]
                                , packageFamilyName
                                , version
                                , "Album");

                if (System.IO.Directory.Exists(folderPath))
                {
                    var existingFiles = Directory.EnumerateFiles(folderPath, "*.*");
                    foreach (var item in existingFiles)
                    {
                        //retorno.Add(item.Substring(item.LastIndexOf('\\') + 1));
                        retorno.Add(item);
                    }
                }
                return retorno;
            }
            catch
            {
                return new List<string>();
            }
        }

        public byte[] GetImage(Picture picture)
        {
            try
            {
                return GetImage(picture.PackageFamilyName, picture.Version, picture.FileName);
            }
            catch
            {
                return new byte[0];
            }
        }

        public byte[] GetImage(string packageFamilyName, string version, string image)
        {
            try
            {
                var folderPath =
                    Path.Combine(ConfigurationManager.AppSettings["AppFolder"]
                                , packageFamilyName
                                , version
                                , "Album");

                if (!System.IO.Directory.Exists(folderPath) ||
                    !File.Exists(Path.Combine(folderPath, image)))
                {
                    return new byte[0];
                }
                else
                {
                    return File.ReadAllBytes(
                                Path.Combine(
                                    ConfigurationManager.AppSettings["AppFolder"], 
                                    packageFamilyName, 
                                    version, 
                                    "Album",
                                    image
                                )
                            );
                }
            }
            catch
            {
                return new byte[0];
            }
        }
    }
}
