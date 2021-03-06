﻿using OrgPortal.Common;
using OrgPortal.DataModel;
using OrgPortalMonitor.DataModel;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.ViewModels
{
    [Export]
    public class InstalledAppsPageViewModel : PageViewModelBase
    {
        private readonly IMessageBox _messageBox;
        private readonly IPortalDataSource _dataSource;
        private readonly IFileSyncManager _fileManager;


        [ImportingConstructor]
        public InstalledAppsPageViewModel(INavigation navigation, 
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


        private List<AppInfo> _installedApps;
        public List<AppInfo> InstalledApps
        {
            get { return _installedApps; }
            private set
            {
                _installedApps = value;
                NotifyOfPropertyChange(() => InstalledApps);
            }
        }

        private string _appCount;
        public string AppCount
        {
            get { return _appCount; }
            private set
            {
                _appCount = value;
                NotifyOfPropertyChange(() => AppCount);
            }
        }

        protected override async void OnInitialize()
        {
            base.OnInitialize();
            await LoadData();
        }

        private async Task LoadData()
        {

            var distinctApps = await _dataSource.GetDistinctAppListAsync();

            var installed = await _fileManager.GetInstalledApps(distinctApps);
            if (installed != null)
            {
                InstalledApps = new List<AppInfo>(installed);
                string format = InstalledApps.Count == 1 ? "{0} app" : "{0} apps";
                AppCount = string.Format(format, InstalledApps.Count);
            }

            //var apps = await _fileManager.GetInstalledApps(new List<AppInfo>()); //TODO: feed with server app list
            //if (apps != null)
            //{
            //    InstalledApps = new List<AppInfo>(apps);

            //    string format = InstalledApps.Count == 1 ? "{0} app" : "{0} apps";
            //    AppCount = string.Format(format, InstalledApps.Count);
            //}            
        }

        public void ShowAppDetails(Windows.UI.Xaml.Controls.ItemClickEventArgs param)
        {
            if (param.ClickedItem is AppInfo)
            {
                Navigation.NavigateToViewModel<AppDetailsPageViewModel>(param.ClickedItem);
            }
        }

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