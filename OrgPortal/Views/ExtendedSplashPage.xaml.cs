using Windows.UI.Xaml.Controls;

namespace OrgPortal.Views
{
    public sealed partial class ExtendedSplashPage : Page
    {
        public ExtendedSplashPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ImageFadeInStoryboard.Begin();
        }
    }
}
