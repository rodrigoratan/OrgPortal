using OrgPortal.Domain;
using OrgPortal.Domain.Models;
using OrgPortal.Domain.Repositories;
using OrgPortal.Infrastructure.DAL.Mappings;
using OrgPortal.Infrastructure.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.Infrastructure.DAL
{
    public class UnitOfWorkImplementation : DbContext, UnitOfWork
    {
        public DbSet<Category> DBCategories { get; set; }
        public DbSet<Application> DBApplications { get; set; }

        private CategoryRepository _categoryRepository;
        public CategoryRepository CategoryRepository
        {
            get
            {
                if (_categoryRepository == null)
                    _categoryRepository = new CategoryRepositoryImplementation(this);
                return _categoryRepository;
            }
        }

        private ApplicationRepository _applicationRepository;
        public ApplicationRepository ApplicationRepository
        {
            get
            {
                if (_applicationRepository == null)
                    _applicationRepository = new ApplicationRepositoryImplementation(this);
                return _applicationRepository;
            }
        }

        static UnitOfWorkImplementation()
        {
            Database.SetInitializer<UnitOfWorkImplementation>(null);

            // A fix for a known issue with EF6.  This doesn't need to be used, it just needs to be
            // referenced somewhere in the project for the runtime to load the correct provider type.
            var instance = SqlProviderServices.Instance;

            if (!Directory.Exists(ConfigurationManager.AppSettings["AppFolder"]))
                Directory.CreateDirectory(ConfigurationManager.AppSettings["AppFolder"]);
        }

        public UnitOfWorkImplementation() : base("Name=OrgPortalDB") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new ApplicationMap());
        }

        public void Commit()
        {
            UpdateFileSystem();
            SaveChanges();
        }

        // TODO: Move file system operations to another infrastructure dependency assembly
        private void UpdateFileSystem()
        {
            foreach (var entry in ChangeTracker.Entries<Application>())
            {
                if (entry.State == EntityState.Modified || entry.State == EntityState.Added)
                {

                    if (!Directory.Exists(        Path.Combine(ConfigurationManager.AppSettings["AppFolder"], entry.Entity.PackageFamilyName, entry.Entity.Version)))
                        Directory.CreateDirectory(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], entry.Entity.PackageFamilyName, entry.Entity.Version));

                  //File.WriteAllBytes(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], entry.Entity.PackageFamilyName + ".appx"), entry.Entity.Package);
                    File.WriteAllBytes(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], entry.Entity.PackageFamilyName, entry.Entity.Version, entry.Entity.PackageFile                     ), entry.Entity.Package);
                    File.WriteAllBytes(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], entry.Entity.PackageFamilyName, entry.Entity.Version, entry.Entity.CertificateFile                 ), entry.Entity.Certificate);
                    File.WriteAllBytes(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], entry.Entity.PackageFamilyName, entry.Entity.Version, entry.Entity.PackageFamilyName + ".png"      ), entry.Entity.Logo);
                    File.WriteAllBytes(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], entry.Entity.PackageFamilyName, entry.Entity.Version, entry.Entity.PackageFamilyName + "-small.png"), entry.Entity.SmallLogo);
                }
                else if (entry.State == EntityState.Deleted)
                {
                    if (Directory.Exists(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], entry.Entity.PackageFamilyName, entry.Entity.Version)))
                    {
                      //File.Delete(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], entry.Entity.PackageFamilyName + ".appx"));
                        File.Delete(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], entry.Entity.PackageFamilyName, entry.Entity.Version, entry.Entity.PackageFile));
                        File.Delete(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], entry.Entity.PackageFamilyName, entry.Entity.Version, entry.Entity.CertificateFile));
                        File.Delete(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], entry.Entity.PackageFamilyName, entry.Entity.Version, entry.Entity.PackageFamilyName + ".png"));
                        File.Delete(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], entry.Entity.PackageFamilyName, entry.Entity.Version, entry.Entity.PackageFamilyName + "-small.png"));

                        Directory.Delete(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], entry.Entity.PackageFamilyName, entry.Entity.Version));
                    }
                    if (Directory.Exists(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], entry.Entity.PackageFamilyName)))
                    {
                        if (Directory.GetFiles(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], entry.Entity.PackageFamilyName)).Count() == 0)
                            Directory.Delete(  Path.Combine(ConfigurationManager.AppSettings["AppFolder"], entry.Entity.PackageFamilyName));

                    }

                }
            }
        }
    }
}
