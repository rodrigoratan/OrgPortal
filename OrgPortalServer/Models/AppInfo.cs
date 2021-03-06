﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace OrgPortalServer.Models
{
    public class AppInfo
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string PackageFamilyName { get; set; }
        public string PackageFile { get; set; }
        public string PackageName { get; set; }
        public string CertificateFile { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string InstallMode { get; set; }
        public DateTime DateAdded { get; set; }
        public string Category { get; set; }
        public string BackgroundColor { get; set; }
        public string PublisherDisplayName { get; set; }
        public string PublisherId { get; set; }

        public IEnumerable<string> AppPictures { get; set; }

        public AppInfo()
        {
            AppPictures = new List<string>();
        }
        public string AppxUrl
        {
            get
            {
                // TODO: There must be better ways to construct these URLs
                var uri = new Uri(ConfigurationManager.AppSettings["OrgUrl"]);
                return "http://" + uri.Authority + "/api/appx/" + PackageFamilyName + "/?version=" + Version;
            }
        }

        public string CertificateUrl
        {
            get
            {
                var uri = new Uri(ConfigurationManager.AppSettings["OrgUrl"]);
                return "http://" + uri.Authority + "/api/certificate/" + PackageFamilyName + "/?version=" + Version; //PackageFamilyName é o id correto ao inves de //CertificateFile; 
            }
        }

        public string ImageUrl
        {
            get
            {
                var uri = new Uri(ConfigurationManager.AppSettings["OrgUrl"]);
                return "http://" + uri.Authority + "/api/logo/" + PackageFamilyName + "/?version=" + Version;
            }
        }

        public string SmallImageUrl
        {
            get
            {
                var uri = new Uri(ConfigurationManager.AppSettings["OrgUrl"]);
                return "http://" + uri.Authority + "/api/smalllogo/" + PackageFamilyName + "/?version=" + Version;
            }
        }

        public string AlbumBaseUrl
        {
            get
            {
                var uri = new Uri(ConfigurationManager.AppSettings["OrgUrl"]);
                return "http://"        + uri.Authority + 
                        "/api/picture/" + PackageFamilyName + 
                        "/?version="    + Version +
                        "&filename=";
            }
        }
    }
}