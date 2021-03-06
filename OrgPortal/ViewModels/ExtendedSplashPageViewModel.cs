﻿using OrgPortal.Common;
using OrgPortal.DataModel;
using System.Composition;
using System.Threading.Tasks;

namespace OrgPortal.ViewModels
{
    [Export]
    public class ExtendedSplashPageViewModel : ViewModelBase
    {
        private readonly IPortalDataSource _dataSource;
        private readonly BrandingViewModel _brandingViewModel;


        [ImportingConstructor]
        public ExtendedSplashPageViewModel(INavigation navigation, IPortalDataSource dataSource, BrandingViewModel brandingViewModel)
            : base(navigation)
        {
            _dataSource = dataSource;
            _brandingViewModel = brandingViewModel;

            var version = Windows.ApplicationModel.Package.Current.Id.Version;
            AppVersion = string.Format("V.{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }

        protected override async void OnInitialize()
        {
            base.OnInitialize();
            await LoadBrandingInfo();
        }

        private async Task LoadBrandingInfo()
        {
            var branding = await _dataSource.GetBrandingDataAsync();
            if (branding != null)
            {
                await _brandingViewModel.UpdateAsync(branding);
            }
            await Task.Delay(5000);
            Navigation.NavigateToViewModel<MainPageViewModel>();
        }

        public string AppVersion { get; set; }

    }
}
