using Caliburn.Micro;
using OrgPortal.Common;
using OrgPortal.Views;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Reflection;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Notifications;
using NotificationsExtensions.TileContent;
using NotificationsExtensions.BadgeContent;
using Windows.Data.Xml.Dom;

namespace OrgPortal
{
    sealed partial class App : CaliburnApplication
    {
        

        private CompositionHost Container { get; set; }

        public App()
        {
            Application.Current.UnhandledException += Current_UnhandledException;

            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = Windows.Globalization.Language.CurrentInputMethodLanguageTag;

            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);

            //clear
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            BadgeUpdateManager.CreateBadgeUpdaterForApplication().Clear();

            this.InitializeComponent();

        }

        void Current_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //ExceptionLogger.LogException(e.Exception); //tbi
            e.Handled = true; // avoid app closing
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
//#if DEBUG
//            if (System.Diagnostics.Debugger.IsAttached)
//            {
//                this.DebugSettings.EnableFrameRateCounter = true;
//            }
//#endif

            if (e.PreviousExecutionState == ApplicationExecutionState.NotRunning)
                DisplayRootView<ExtendedSplashPage>();
            else
                DisplayRootView<MainPage>();

            UpdateLiveTile();

            //    //Associate the frame with a SuspensionManager key                                
            //    SuspensionManager.RegisterFrame(rootFrame, "AppFrame");
            //    // Set the default language
            //    rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

            
            //    if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
            //    {
            //        // Restore the saved session state only when appropriate
            //        try
            //        {
            //            await SuspensionManager.RestoreAsync();
            //        }
            //        catch (SuspensionManagerException)
            //        {
            //            //Something went wrong restoring state.
            //            //Assume there is no state and continue
            //        }
            //    }
        }

        private void UpdateLiveTile()
        {
            //List<OrgPortalMonitor.DataModel.AppInfo> appList = new List<OrgPortalMonitor.DataModel.AppInfo>();
            //appList.Add(new OrgPortalMonitor.DataModel.AppInfo() { DisplayName = "Story 1", SmallImageUrl = "" });
            //appList.Add(new OrgPortalMonitor.DataModel.AppInfo() { DisplayName = "Story 2", SmallImageUrl = "" });
            //appList.Add(new OrgPortalMonitor.DataModel.AppInfo() { DisplayName = "Story 3", SmallImageUrl = "" });
            //appList.Add(new OrgPortalMonitor.DataModel.AppInfo() { DisplayName = "Story 4", SmallImageUrl = "" });
            //appList.Add(new OrgPortalMonitor.DataModel.AppInfo() { DisplayName = "Story 5", SmallImageUrl = "" });

            string StoryHeader1 = "Story 1";
            string StoryHeader2 = "Story 2";
            string StoryHeader3 = "Story 3";
            string StoryHeader4 = "Story 4";
            string StoryHeader5 = "Story 5";
            string StoryHeader6 = "Story 6";

            string StoryText1 = "One Lorem Ipsum est Dolor sit amet 1";
            string StoryText2 = "Two Lorem Ipsum est Dolor sit amet 2";
            string StoryText3 = "Three Lorem Ipsum est Dolor sit amet 3";
            string StoryText4 = "Four Lorem Ipsum est Dolor sit amet 4";
            string StoryText5 = "Five Lorem Ipsum est Dolor sit amet 5";
            string StoryText6 = "Six Lorem Ipsum est Dolor sit amet 6";

            string ImageUrl1 = "http://store.zollie.com.br/api/logo/34993Zollie.UnimedAgile_mcdpzngym7t32/?version=1.1.0.1";//string.Empty;
            string ImageUrl2 = "http://store.zollie.com.br/api/logo/34993Zollie.ZollieOrgPortal_mcdpzngym7t32/?version=1.1.0.1";//string.Empty;
            string ImageUrl3 = "http://store.zollie.com.br/api/logo/34993Zollie.UnimedAgile_mcdpzngym7t32/?version=1.1.0.1";//string.Empty;
            string ImageUrl4 = "http://store.zollie.com.br/api/logo/34993Zollie.ZollieOrgPortal_mcdpzngym7t32/?version=1.1.0.1";//string.Empty;
            string ImageUrl5 = "http://store.zollie.com.br/api/logo/34993Zollie.UnimedAgile_mcdpzngym7t32/?version=1.1.0.1";//string.Empty;
            string ImageUrl6 = "http://store.zollie.com.br/api/logo/34993Zollie.ZollieOrgPortal_mcdpzngym7t32/?version=1.1.0.1";//string.Empty;

            int appListCount = 6;

            #region first notification

            //TODO: move code outside app so it can be used to generate the XML on the fly for periodic tile updates
            //square310x310ImageAndText1.CreateNotification().Content

            TileNotification tileNotification = new TileNotification(new XmlDocument());

            if (appListCount <= 3)
            {
                ITileNotificationContent tileN = TileNotifications.IndividualImageTextTile(StoryHeader1, StoryText1, ImageUrl1);
                TileNotifications.CreateTileUpdaterForApplicationHelper(ref tileNotification, "1", tileN);
            }
            else if (appListCount >= 4)
            {
                ITileNotificationContent tileN = TileNotifications.OneOrFiveImageTile(StoryHeader1, StoryText1, ImageUrl1, ImageUrl2, ImageUrl3, ImageUrl4, ImageUrl5);
                TileNotifications.CreateTileUpdaterForApplicationHelper(ref tileNotification, "1", tileN);
            }

            #endregion first notification

            #region second notification
            if (appListCount <= 3)
            {
                // second notification
                ITileNotificationContent tileN = TileNotifications.IndividualImageTextTile(StoryHeader2, StoryText2, ImageUrl2);
                TileNotifications.CreateTileUpdaterForApplicationHelper(ref tileNotification, "2", tileN);
            }
            else if (appListCount >= 4)
            {
                // second notification
                ITileNotificationContent tileN = TileNotifications.OneOrFiveImageTile(StoryHeader2, StoryText2, ImageUrl2, ImageUrl3, ImageUrl4, ImageUrl5, ImageUrl6);
                TileNotifications.CreateTileUpdaterForApplicationHelper(ref tileNotification, "2", tileN);
            }

            #endregion second notification

            #region third, fourth and fifth notification
            if (appListCount <= 3)
            {
                // third notification
                ITileNotificationContent tileN = TileNotifications.IndividualImageTextTile(StoryHeader3, StoryText3, ImageUrl3);
                TileNotifications.CreateTileUpdaterForApplicationHelper(ref tileNotification, "3", tileN);
            }
            else if (appListCount >= 4)
            {
                // third notification
                ITileNotificationContent tileN = TileNotifications.OneOrFiveImageTile(StoryHeader3, StoryText3, ImageUrl3, ImageUrl4, ImageUrl5, ImageUrl6, ImageUrl1);
                TileNotifications.CreateTileUpdaterForApplicationHelper(ref tileNotification, "3", tileN);

                // fourth notification
                tileN = TileNotifications.OneOrFiveImageTile(StoryHeader4, StoryText4, ImageUrl4, ImageUrl5, ImageUrl6, ImageUrl1, ImageUrl2);
                TileNotifications.CreateTileUpdaterForApplicationHelper(ref tileNotification, "4", tileN);

                // fifth notification
                tileN = TileNotifications.OneOrFiveImageTile(StoryHeader5, StoryText5, ImageUrl5, ImageUrl6, ImageUrl1, ImageUrl2, ImageUrl3);
                TileNotifications.CreateTileUpdaterForApplicationHelper(ref tileNotification, "5", tileN);
            }
            #endregion third, fourth and fifth notification

        }


        private void UpdateLiveTileOld()
        {

            string StoryHeader1 = "Story 1";
            string StoryHeader2 = "Story 2";
            string StoryHeader3 = "Story 3";
            string StoryHeader4 = "Story 4";
            string StoryHeader5 = "Story 5";
            string StoryHeader6 = "Story 6";

            string StoryText1 = "One Lorem Ipsum est Dolor sit amet 1";
            string StoryText2 = "Two Lorem Ipsum est Dolor sit amet 2";
            string StoryText3 = "Three Lorem Ipsum est Dolor sit amet 3";
            string StoryText4 = "Four Lorem Ipsum est Dolor sit amet 4";
            string StoryText5 = "Five Lorem Ipsum est Dolor sit amet 5";
            string StoryText6 = "Six Lorem Ipsum est Dolor sit amet 6";

            string ImageUrl1 = "http://store.zollie.com.br/api/logo/34993Zollie.UnimedAgile_mcdpzngym7t32/?version=1.1.0.1";//string.Empty;
            string ImageUrl2 = "http://store.zollie.com.br/api/logo/34993Zollie.ZollieOrgPortal_mcdpzngym7t32/?version=1.1.0.1";//string.Empty;
            string ImageUrl3 = "http://store.zollie.com.br/api/logo/34993Zollie.UnimedAgile_mcdpzngym7t32/?version=1.1.0.1";//string.Empty;
            string ImageUrl4 = "http://store.zollie.com.br/api/logo/34993Zollie.ZollieOrgPortal_mcdpzngym7t32/?version=1.1.0.1";//string.Empty;
            string ImageUrl5 = "http://store.zollie.com.br/api/logo/34993Zollie.UnimedAgile_mcdpzngym7t32/?version=1.1.0.1";//string.Empty;
            string ImageUrl6 = "http://store.zollie.com.br/api/logo/34993Zollie.ZollieOrgPortal_mcdpzngym7t32/?version=1.1.0.1";//string.Empty;

            #region first notification

            #region square310x310TextTileContent1
            ITileSquare310x310Text09 square310x310TextTileContent1 = TileContentFactory.CreateTileSquare310x310Text09();
            square310x310TextTileContent1.TextHeadingWrap.Text = StoryHeader1;

            ITileWide310x150Text03 wide310x150TextTileContent1 = TileContentFactory.CreateTileWide310x150Text03();

            wide310x150TextTileContent1.TextHeadingWrap.Text = StoryHeader1;

            ITileSquare150x150Text04 square150x150TextTileContent1 = TileContentFactory.CreateTileSquare150x150Text04();
            square150x150TextTileContent1.TextBodyWrap.Text = StoryHeader1;

            wide310x150TextTileContent1.Square150x150Content = square150x150TextTileContent1;
            square310x310TextTileContent1.Wide310x150Content = wide310x150TextTileContent1;

            // Set the contentId on the Square310x310 tile
            square310x310TextTileContent1.ContentId = "Main_1";

            // Tag the notification and send it to the tile
            TileNotification tileNotification = square310x310TextTileContent1.CreateNotification();

            tileNotification.Tag = "1";
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);

            #endregion

            #region square310x310TextTileContent2 && wide310x150TextTileContent2
            // Create the first notification for the second set of stories with binding for all 3 tiles sizes
            ITileSquare310x310TextList03 square310x310TextTileContent2 = TileContentFactory.CreateTileSquare310x310TextList03();
            square310x310TextTileContent2.TextHeading1.Text = StoryHeader2;
            square310x310TextTileContent2.TextHeading2.Text = StoryHeader3;
            square310x310TextTileContent2.TextHeading3.Text = StoryHeader4;

            ITileWide310x150Text03 wide310x150TextTileContent2 = TileContentFactory.CreateTileWide310x150Text03();
            wide310x150TextTileContent2.TextHeadingWrap.Text = StoryHeader2;

            ITileSquare150x150Text04 square150x150TextTileContent2 = TileContentFactory.CreateTileSquare150x150Text04();
            square150x150TextTileContent2.TextBodyWrap.Text = StoryHeader2;

            wide310x150TextTileContent2.Square150x150Content = square150x150TextTileContent2;
            square310x310TextTileContent2.Wide310x150Content = wide310x150TextTileContent2;

            // Set the contentId on the Square310x310 tile
            square310x310TextTileContent2.ContentId = "Additional_1";

            // Tag the notification and send it to the tile
            tileNotification = square310x310TextTileContent2.CreateNotification();
            tileNotification.Tag = "2";
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            #endregion

            #endregion

            #region second notification
            // Create the second notification for the second set of stories with binding for all 3 tiles sizes
            // Notice that we only replace the Wide310x150 and Square150x150 binding elements,
            // and keep the Square310x310 content the same - this will cause the Square310x310 to be ignored for this notification,
            // since the contentId for this size is the same as in the first notification of the second set of stories.
            //ITileWide310x150Text03 wide310x150TextTileContent3 = TileContentFactory.CreateTileWide310x150Text03();
            ITileWide310x150SmallImageAndText04 wide310x150ImageTextTileContent3 = TileContentFactory.CreateTileWide310x150SmallImageAndText04();
            wide310x150ImageTextTileContent3.TextHeading.Text = StoryHeader3;
            wide310x150ImageTextTileContent3.TextBodyWrap.Text = StoryText3;

            ITileSquare150x150Text04 square150x150TextTileContent3 = TileContentFactory.CreateTileSquare150x150Text04();
            square150x150TextTileContent3.TextBodyWrap.Text = StoryHeader3;

            wide310x150ImageTextTileContent3.Square150x150Content = square150x150TextTileContent3;
            square310x310TextTileContent2.Wide310x150Content = wide310x150ImageTextTileContent3;

            // Tag the notification and send it to the tile
            tileNotification = square310x310TextTileContent2.CreateNotification();
            tileNotification.Tag = "3";
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            #endregion

            #region third notification - second set
            // Create the third notification for the second set of stories with binding for all 3 tiles sizes
            // Notice that we only replace the Wide310x150 and Square150x150 binding elements,
            // and keep the Square310x310 content the same again - this will cause the Square310x310 to be ignored for this notification,
            // since the contentId for this size is the same as in the first notification of the second set of stories.
            //ITileWide310x150Text03 wide310x150TextTileContent4 = TileContentFactory.CreateTileWide310x150Text03();
            ITileWide310x150SmallImageAndText04 wide310x150ImageTextTileContent4 = TileContentFactory.CreateTileWide310x150SmallImageAndText04();

            wide310x150ImageTextTileContent4.TextHeading.Text = StoryHeader4;
            wide310x150ImageTextTileContent4.TextBodyWrap.Text = StoryText4;

            ITileSquare150x150Text04 square150x150TextTileContent4 = TileContentFactory.CreateTileSquare150x150Text04();
            square150x150TextTileContent4.TextBodyWrap.Text = StoryHeader4;

            wide310x150ImageTextTileContent4.Square150x150Content = square150x150TextTileContent4;
            square310x310TextTileContent2.Wide310x150Content = wide310x150ImageTextTileContent4;

            // Tag the notification and send it to the tile
            tileNotification = square310x310TextTileContent2.CreateNotification();
            tileNotification.Tag = "4";
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            #endregion  third

        }

        protected override void Configure()
        {
            var configuration = new ContainerConfiguration()
                .WithAssembly(typeof(App).GetTypeInfo().Assembly);

            Container = configuration.CreateContainer();
        }

        protected override void PrepareViewFirst(Frame rootFrame)
        {
            var nav = Container.GetExport<INavigation>();
            nav.Initialize(new FrameAdapter(rootFrame));
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] { typeof(App).GetTypeInfo().Assembly };
        }

        protected override object GetInstance(Type service, string key)
        {
            object instance = null;
            
            if (!string.IsNullOrEmpty(key))
                instance = Container.GetExport(service, key);
            else
                instance = Container.GetExport(service);

            if (instance != null)
                return instance;

            throw new Exception("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return Container.GetExports(service);
        }

        protected override void BuildUp(object instance)
        {
            Container.SatisfyImports(instance);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        protected override async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
        
    }
}