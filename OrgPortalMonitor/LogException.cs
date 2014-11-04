using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortalMonitor
{
    public static class ExceptionLogger
    {
        public static void LogException(Exception currentException)
        {
            string exceptionMessage = CreateErrorMessage(currentException);

            //string fileName = "Exception-" + DateTime.Today.ToString("yyyyMMdd") + string.Format("-T{0}", Environment.CurrentManagedThreadId) + ".log";
            //var cachePath = System.Environment
            //                      .GetFolderPath(System.Environment
            //                                    .SpecialFolder.InternetCache);

            //if (!cachePath.EndsWith(@"\")) cachePath += @"\";

            //LogException(exceptionMessage, cachePath, fileName);

            LogException(exceptionMessage);
        }

        public static void LogException(Exception currentException, string exceptionsStoragePath)
        {
            String exceptionMessage = CreateErrorMessage(currentException);
            string fileName = "Exception-" + DateTime.Today.ToString("yyyyMMdd") + string.Format("-T{0}", Environment.CurrentManagedThreadId) + ".log";
            LogException(exceptionMessage, exceptionsStoragePath, fileName);
        }

        public static void LogException(string exceptionMessage)
        {
            string fileName = "Exception-" + DateTime.Today.ToString("yyyyMMdd") + string.Format("-T{0}", Environment.CurrentManagedThreadId) + ".log";
            var cachePath = System.Environment
                            .GetFolderPath(System.Environment
                                          .SpecialFolder.InternetCache);

            if (!cachePath.EndsWith(@"\")) cachePath += @"\";

            LogException(exceptionMessage, cachePath, fileName);
        }

        public static async void LogException(string exceptionMessage, 
                                              string exceptionsStoragePath, 
                                              string fileName)
        {
            var ExceptionsStoragePath = string.Empty;
            if (!string.IsNullOrEmpty(exceptionsStoragePath))
            {
                ExceptionsStoragePath = System.IO.Path.Combine(exceptionsStoragePath, "Exceptions");
            }
            else
            {
                var cachePath = System.Environment
                                .GetFolderPath(System.Environment
                                              .SpecialFolder.InternetCache);
                ExceptionsStoragePath = System.IO.Path.Combine(exceptionsStoragePath, "Exceptions");
            }

            if (!System.IO.Directory.Exists(ExceptionsStoragePath))
            {
                System.IO.Directory.CreateDirectory(ExceptionsStoragePath);
            }

            await LogFileWrite(exceptionMessage, System.IO.Path.Combine(ExceptionsStoragePath, fileName));
        }

        private static async Task LogFileWrite(string messageContent, string requestFileName)
        {
            var requestFile = System.IO.File.CreateText(requestFileName);
            using (requestFile)
            {
                await requestFile.WriteAsync(messageContent);
                requestFile.Close();
            }
        }

        public static void ExtraLog(string logMessage)
        {
            string fileName = "ExtraLog-" + DateTime.Today.ToString("yyyyMMdd") + string.Format("-T{0}", Environment.CurrentManagedThreadId) + ".log";

            ExtraLog(logMessage, fileName);
        }

        public static async void ExtraLog(string logMessage, string fileName)
        {
            await LogFileWrite(string.Format(@"{0}{1}"
                                            , System.Environment.NewLine
                                            , "[" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "] "
                                            + logMessage)
                               , fileName);

            //LogFileWrite(logMessage, fileName);
        }

        public static void ExtraLogDebug(string p)
        {
            ExtraLog(p);
            Debug.WriteLine(p);
        }

        /// <summary>
        /// This method is for prepare the error message to log using Exception object
        /// </summary>
        /// <param name="currentException"></param>
        /// <returns></returns>
        private static String CreateErrorMessage(Exception currentException)
        {
            StringBuilder messageBuilder = new StringBuilder();
            try
            {
                messageBuilder.AppendLine("------------------------------------------------------------------------------");
                messageBuilder.AppendLine("--Thread: " + Environment.CurrentManagedThreadId + "-------------------------------------------------------------------");

                if (currentException.Source != null)
                {
                    messageBuilder.AppendLine("Error source: " + currentException.Source.ToString().Trim());
                }
                messageBuilder.AppendLine("Date/Time: " + DateTime.Now);
                messageBuilder.AppendLine("------------------------------------------------------------------------------");
                if (!string.IsNullOrEmpty(currentException.Message))
                {
                    messageBuilder.AppendLine("Method: " + currentException.Message.ToString().Trim());
                }
                messageBuilder.AppendLine("Erro: " + currentException.ToString());
                if (currentException.InnerException != null)
                {
                    messageBuilder.AppendLine("More details: " + currentException.InnerException.ToString());
                }
                messageBuilder.AppendLine("");
                return messageBuilder.ToString();
            }
            catch
            {
                messageBuilder.AppendLine("Unknown error.");
                return messageBuilder.ToString();
            }
        }



        internal static void PurgeLogFiles()
        {
            throw new NotImplementedException();
        }
    }
}
