using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace OrgPortal.DataModel
{
    public class PictureInfo 
    {
        public string FileName
        {
            get
            {
                var fileNameQs = "filename=";

                var fileName = PictureUrl.Contains(fileNameQs) ?
                               PictureUrl
                              .Substring(PictureUrl.LastIndexOf(fileNameQs) + 
                                         fileNameQs.Length) :
                                               string.Empty ;

                return !string.IsNullOrEmpty(fileName)   ?
                                             fileName    :
                                             string.Empty;
            }
        }

        public string Title
        {
            get
            {
                var title = string.Empty;
                if (FileName.Split(SplitChar).Count() == 2)
                {
                    title = FileName.Split(SplitChar)[0];
                }
                else if (
                         FileName.Split(SplitChar).Count() == 3 ||
                         FileName.Split(SplitChar).Count() == 4 ||
                         FileName.Split(SplitChar).Count() == 5 )
                {
                    title = FileName.Split(SplitChar)[1];
                }
                return title;
            }
        }

        public string SubTitle
        {
            get
            {
                var subtitle = string.Empty;
                if (FileName.Split(SplitChar).Count() == 5)
                {
                    subtitle = FileName.Split(SplitChar)[3];
                }
                return subtitle;
            }
        }

        //public string PictureUrl { get; set {} }
        private string _pictureUrl;
        public string PictureUrl
        {
            get { return _pictureUrl; }
            set
            {
                _pictureUrl = value;
            }
        }

        public char SplitChar { get; set; }

        public PictureInfo()
        {
            SplitChar = '.';
            BackgroundColor = new SolidColorBrush(Windows.UI.Colors.Transparent);
        }

        public PictureInfo(char splitChar)
        {
            SplitChar = splitChar;
            BackgroundColor = new SolidColorBrush(Windows.UI.Colors.Transparent);
        }

        public PictureInfo(char splitChar, SolidColorBrush backgroundColor)
        {
            SplitChar = splitChar;
            BackgroundColor = backgroundColor;
        }

        public PictureInfo(string pictureUrl)
        {
            PictureUrl = pictureUrl;
            SplitChar = '.';
            BackgroundColor = new SolidColorBrush(Windows.UI.Colors.Transparent);
        }

        public PictureInfo(string pictureUrl, SolidColorBrush backgroundColor)
        {
            PictureUrl = pictureUrl;
            SplitChar = '.';
            BackgroundColor = backgroundColor;
        }

        public PictureInfo(string pictureUrl, char splitChar)
        {
            PictureUrl = pictureUrl;
            SplitChar = splitChar;
            BackgroundColor = new SolidColorBrush(Windows.UI.Colors.Transparent);
        }

        public PictureInfo(string pictureUrl, char splitChar, SolidColorBrush backgroundColor)
        {
            PictureUrl = pictureUrl;
            SplitChar = splitChar;
            BackgroundColor = backgroundColor;
        }

        public Windows.UI.Xaml.Media.SolidColorBrush BackgroundColor { get; set; }
    }
}
