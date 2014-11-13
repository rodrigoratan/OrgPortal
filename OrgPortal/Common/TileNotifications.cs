using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationsExtensions.BadgeContent;
using NotificationsExtensions.TileContent;
using Windows.UI.Notifications;

namespace OrgPortal.Common
{
    public static class TileNotifications
    {

        public static void UpdateBadgeWithNumber(int number)
        {
            // Note: This sample contains an additional project, NotificationsExtensions.
            // NotificationsExtensions exposes an object model for creating notifications, but you can also modify the xml
            // of the notification directly. See the additional function UpdateBadgeWithNumberWithStringManipulation to see how to do it
            // by modifying strings directly.

            BadgeNumericNotificationContent badgeContent = new BadgeNumericNotificationContent((uint)number);

            // Send the notification to the application’s tile.
            BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(badgeContent.CreateNotification());

            //OutputTextBlock.Text = badgeContent.GetContent();
            //rootPage.NotifyUser("Badge sent", NotifyType.StatusMessage);
        }


        public static void UpdateBadgeWithGlyph(int index)
        {
            // Note: This sample contains an additional project, NotificationsExtensions.
            // NotificationsExtensions exposes an object model for creating notifications, but you can also modify the xml
            // of the notification directly. See the additional function UpdateBadgeWithGlyphWithStringManipulation to see how to do it
            // by modifying strings directly.

            // Note: usually this would be created with new BadgeGlyphNotificationContent(GlyphValue.Alert) or any of the values of GlyphValue.
            BadgeGlyphNotificationContent badgeContent = new BadgeGlyphNotificationContent((GlyphValue)index);

            // Send the notification to the application’s tile.
            BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(badgeContent.CreateNotification());

            //OutputTextBlock.Text = badgeContent.GetContent();
            //rootPage.NotifyUser("Badge sent", NotifyType.StatusMessage);
        }

        public static ITileNotificationContent OneOrFiveImageTile(string StoryHeader1, string StoryText1, string ImageUrl1, string ImageUrl2, string ImageUrl3, string ImageUrl4, string ImageUrl5)
        {
            #region if (appList.Count <= 3)
            ITileSquare150x150PeekImageAndText02 square150x150ImageText1 = TileContentFactory.CreateTileSquare150x150PeekImageAndText02();
            square150x150ImageText1.TextHeading.Text = StoryHeader1;
            square150x150ImageText1.TextBodyWrap.Text = StoryText1;
            square150x150ImageText1.Image.Src = ImageUrl1;

            ITileWide310x150PeekImageCollection05 wide310x150PeekTileContent1 = TileContentFactory.CreateTileWide310x150PeekImageCollection05(); //TileWide310x150PeekImageCollection05
            wide310x150PeekTileContent1.TextHeading.Text = StoryHeader1;
            wide310x150PeekTileContent1.TextBodyWrap.Text = StoryText1;
            wide310x150PeekTileContent1.ImageMain.Src = ImageUrl1;
            wide310x150PeekTileContent1.ImageSecondary.Src = ImageUrl1;
            wide310x150PeekTileContent1.ImageSmallColumn1Row1.Src = ImageUrl2;
            wide310x150PeekTileContent1.ImageSmallColumn2Row1.Src = ImageUrl3;
            wide310x150PeekTileContent1.ImageSmallColumn1Row2.Src = ImageUrl4;
            wide310x150PeekTileContent1.ImageSmallColumn2Row2.Src = ImageUrl5;
            wide310x150PeekTileContent1.Square150x150Content = square150x150ImageText1;

            ITileSquare310x310ImageCollectionAndText02 square310x310FiveImageCollection = TileContentFactory.CreateTileSquare310x310ImageCollectionAndText02(); //TileWide310x150PeekImageCollection05
            square310x310FiveImageCollection.TextCaption1.Text = StoryHeader1;
            square310x310FiveImageCollection.TextCaption2.Text = StoryText1;
            square310x310FiveImageCollection.ImageMain.Src = ImageUrl1;
            square310x310FiveImageCollection.ImageSmall1.Src = ImageUrl2;
            square310x310FiveImageCollection.ImageSmall2.Src = ImageUrl3;
            square310x310FiveImageCollection.ImageSmall3.Src = ImageUrl4;
            square310x310FiveImageCollection.ImageSmall4.Src = ImageUrl5;
            square310x310FiveImageCollection.Wide310x150Content = wide310x150PeekTileContent1;

            ITileNotificationContent tileN = square310x310FiveImageCollection;
            #endregion
            return tileN;
        }

        public static ITileNotificationContent IndividualImageTextTile(string StoryHeader1, string StoryText1, string ImageUrl1)
        {
            ITileSquare150x150PeekImageAndText02 square150x150ImageText1 = TileContentFactory.CreateTileSquare150x150PeekImageAndText02();
            square150x150ImageText1.TextHeading.Text = StoryHeader1;
            square150x150ImageText1.TextBodyWrap.Text = StoryText1;
            square150x150ImageText1.Image.Src = ImageUrl1;

            ITileWide310x150PeekImage01 wide310x150PeekImage1 = TileContentFactory.CreateTileWide310x150PeekImage01();
            wide310x150PeekImage1.TextHeading.Text = StoryHeader1;
            wide310x150PeekImage1.TextBodyWrap.Text = StoryText1;
            wide310x150PeekImage1.Image.Src = ImageUrl1;
            wide310x150PeekImage1.Square150x150Content = square150x150ImageText1;

            ITileSquare310x310ImageAndText02 square310x310ImageAndText1 = TileContentFactory.CreateTileSquare310x310ImageAndText02();
            square310x310ImageAndText1.TextCaption1.Text = StoryHeader1;
            square310x310ImageAndText1.TextCaption2.Text = StoryText1;
            square310x310ImageAndText1.Image.Src = ImageUrl1;
            square310x310ImageAndText1.Wide310x150Content = wide310x150PeekImage1;

            //TODO: move code outside app so it can be used to generate the XML on the fly for periodic tile updates
            //square310x310ImageAndText1.CreateNotification().Content

            ITileNotificationContent tileN = square310x310ImageAndText1;
            return tileN;
        }

        public static void CreateTileUpdaterForApplicationHelper(ref TileNotification tileNotification,
                                                                  string _tag,
                                                                  ITileNotificationContent tileN)
        {
            tileNotification = tileN.CreateNotification();
            if (!string.IsNullOrEmpty(_tag))
            {
                tileNotification.Tag = _tag;
            }
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }
    }

    public class TileGlyph
    {
        public string Name { get; private set; }
        public bool IsAvailable { get; private set; }
        public TileGlyph(string name, bool isAvailable)
        {
            this.Name = name;
            this.IsAvailable = isAvailable;
        }
        public override string ToString()
        {
            return Name;
        }
    }
    public class TileGlyphCollection : ObservableCollection<TileGlyph>
    {
        public TileGlyphCollection()
        {
            // Some glyphs are only available on Windows
#if WINDOWS_PHONE_APP
            const bool windows = false;
            const bool phone = true;
#else
            const bool windows = true;
            const bool phone = false;
#endif

            Add(new TileGlyph("none", windows | phone));
            Add(new TileGlyph("activity", windows));
            Add(new TileGlyph("alert", windows | phone));
            Add(new TileGlyph("available", windows));
            Add(new TileGlyph("away", windows));
            Add(new TileGlyph("busy", windows));
            Add(new TileGlyph("newMessage", windows));
            Add(new TileGlyph("paused", windows));
            Add(new TileGlyph("playing", windows));
            Add(new TileGlyph("unavailable", windows));
            Add(new TileGlyph("error", windows));
            Add(new TileGlyph("attention", windows | phone));
            Add(new TileGlyph("alarm", windows));
        }
    }
}
