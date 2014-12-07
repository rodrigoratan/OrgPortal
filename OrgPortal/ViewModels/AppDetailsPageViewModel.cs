using OrgPortal.Common;
using OrgPortal.DataModel;
using OrgPortalMonitor.DataModel;
using System;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace OrgPortal.ViewModels
{
    [Export]
    public class AppDetailsPageViewModel : PageViewModelBase
    {
        private readonly IMessageBox _messageBox;
        private readonly IPortalDataSource _dataSource;
        private readonly IFileSyncManager _fileManager;
        private AppInfo _installedItem;
        private AppInfo _installedItemVersion;

        [ImportingConstructor]
        public AppDetailsPageViewModel(INavigation navigation, 
            IMessageBox messageBox, 
            INavigationBar navBar,
            IPortalDataSource dataSource,
            IFileSyncManager fileManager,
            BrandingViewModel branding)
            : base(navigation, navBar, branding)
        {
            this._messageBox = messageBox;
            this._dataSource = dataSource;
            this._fileManager = fileManager;

            var version = Windows.ApplicationModel.Package.Current.Id.Version;
            AppVersion = string.Format("V.{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }

        private AppInfo _item;
        public AppInfo Item
        {
            get { return _item; }
            private set
            {
                _item = value;
                NotifyOfPropertyChange(() => Item);
            }
        }

        public bool IsInstalled
        {
            get { return _installedItem != null; }
        }

        public bool IsInstalledVersion
        {
            get { return _installedItemVersion != null; }
        }

        private bool _updateAvailable = false;
        public bool UpdateAvailable
        {
            get { return _updateAvailable; }
        }

        protected override async void DeserializeParameter(string value)
        {
            Item = Serializer.Deserialize<AppInfo>(value);
            AlbumApp = new List<PictureInfo>();
            if (Item.AppPictures != null)
            {
                foreach (var imageUrl in Item.AppPictures)
                {
                    AlbumApp.Add(new PictureInfo(imageUrl, ColorToBrush(Item.BackgroundColor)));
                }
            }
            await LoadData();
        }

        //TODO: "Convert" it to a Converter so Hex codes can be used directly on the binding #<3Converters
        public static SolidColorBrush ColorToBrush(string color)
        {
            color = color.Replace("#", "");
            if (color.Length == 6)
            {
                return new SolidColorBrush(ColorHelper.FromArgb(255,
                    byte.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber)));
            }
            else
            {
                return null;
            }
        }
        public async Task Install()
        {
            await _fileManager.RequestAppInstall(
                                Item.AppxUrl,
                                Item.PackageFile, 
                                Item.CertificateUrl, 
                                Item.CertificateFile, 
                                Item.Version,
                                Item.Name, 
                                Item.Description, 
                                Item.BackgroundColor, 
                                Item.ImageUrl
                            );

            await _messageBox.ShowAsync("Install request sent", "Install App " + Item.Name);
        }

        //public async Task Update()
        //{

        //}

        //public async Task Uninstall()
        //{

        //}

        private async Task LoadData()
        {
            var distinctApps = await _dataSource.GetDistinctAppListAsync();

            var installed = await _fileManager.GetInstalledApps(distinctApps); 

            _installedItem = 
                 installed
                .FirstOrDefault(a => a.PackageFamilyName == Item.PackageFamilyName);

            _installedItemVersion =
                 installed
                .FirstOrDefault(a => a.PackageFamilyName == Item.PackageFamilyName &&
                                     a.Version == Item.Version);

            NotifyOfPropertyChange(() => IsInstalled);
            NotifyOfPropertyChange(() => IsInstalledVersion);

            CheckUpdate();
        }

        private void CheckUpdate()
        {
            if (IsInstalledVersion)
            {
                Version itemVersion = new Version(Item.Version);
                Version installedVersion = new Version(_installedItemVersion.Version);

                _updateAvailable = itemVersion > installedVersion;
                NotifyOfPropertyChange(() => UpdateAvailable);
            }
        }

        public List<PictureInfo> AlbumApp { get; set; }

        private string _searchQueryText;
        public string SearchQueryText
        {
            get { return _searchQueryText; }
            set
            {
                _searchQueryText = value;
                NotifyOfPropertyChange(() => SearchQueryText);
            }
        }

        public void RunSearch()
        {
            if (!string.IsNullOrWhiteSpace(_searchQueryText))
            {
                Navigation.NavigateToViewModel<SearchPageViewModel>(_searchQueryText);
            }
        }


        public string AppVersion { get; set; }
    }
}