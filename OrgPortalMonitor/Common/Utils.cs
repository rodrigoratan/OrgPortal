using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Microsoft.Win32;
using OrgPortalMonitor.Properties;

namespace OrgPortalMonitor.Common
{
    public static class Utils
    {
        public static string TemplateDontRequireDevLicenseFileName = "RegistryDontRequireDevLicense.txt";
        public static string TemplateRegisterProtocolTemplateFileName = "RegistryRegisterProtocolTemplate.txt";
        public static string DontRequiredDevLicenseRegistryFileName = "DontRequiredDevLicense.reg";
        public static string RegisterProtocolRegistryFileName = "RegisterProtocol.reg";

        //var RequestQueryString = GetQueryStringParameters();
        public static NameValueCollection GetQueryStringParameters()
        {
            NameValueCollection nameValueTable = new NameValueCollection();
            try
            {
                if (ApplicationDeployment.IsNetworkDeployed &&
                    ApplicationDeployment.CurrentDeployment.ActivationUri != null)
                {
                    string queryString = ApplicationDeployment.CurrentDeployment.ActivationUri.Query;
                    nameValueTable = HttpUtility.ParseQueryString(queryString);
                }
                return (nameValueTable);
            }
            catch /*(Exception ex)*/
            {
                //OutputException(ex);
                return (nameValueTable);
            }
        }
        public static string Join<TItem>(this IEnumerable<TItem> enumerable, string separator)
        {
            return string.Join(separator, enumerable);
        }

        public static bool ImportRequiredRegistry(string fileName)
        {
            //Import (merge) a .REG file:   REGEDIT.EXE [ /L:system | /R:user ] [ /S ] importfile.REG 
            if(!RunAsAdmin("cmd.exe", string.Format(@" /c regedit /S {0} ", Path.Combine(CurrentPath, fileName))))
            {
                File.Delete(Path.Combine(CurrentPath, fileName));
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void SetAlwaysRunAsAdmin()
        {
            //reg add "HKCU\Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers" /v "<Path to your exe>" /t REG_SZ /d RUNASADMIN
            //"add \"HKCU\Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers\" /v \"{0}\" /t REG_SZ /d RUNASADMIN"

            //TODO: won't work with ClickOnce deployment (consider another forms of deployment that includes update checking)
            // RunAsAdmin("reg.exe", string.Format("add \"HKCU\\Software\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Layers\" /v \"{0}\" /t REG_SZ /d RUNASADMIN", Application.ExecutablePath));
        }

        public static bool WriteRequiredRegistry(string registryTemplate, string fileName)
        {
            #region RequiredRegistry
            try
            {
                using (TextWriter tw = new StreamWriter(Path.Combine(CurrentPath, fileName)))
                {
                    tw.Write(registryTemplate);
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                return false;
            }
            #endregion
        }

        public static string ReturnRegistryDontRequireDevLicense()
        {
            string retValue = string.Empty;
            #region RegistryDontRequireDevLicense
            string registryDontRequireDevLicenseTemplate = string.Empty;
            if (true)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(Path.Combine(Utils.CurrentPath,
                                                                           Utils.TemplateDontRequireDevLicenseFileName)))
                    {
                        registryDontRequireDevLicenseTemplate = sr.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionLogger.LogException(ex);
                }

                retValue += Environment.NewLine +
                            registryDontRequireDevLicenseTemplate;
            }
            #endregion
            return retValue;
        }

        public static string ReturnRegistryRegisterProtocol(string shellParameter)
        {
            string retValue = string.Empty;
            #region RegistryRegisterProtocol
            string registryRegisterProtocolTemplate = string.Empty;
            try
            {
                using (StreamReader sr = new StreamReader(Path.Combine(CurrentPath, TemplateRegisterProtocolTemplateFileName)))
                {
                    registryRegisterProtocolTemplate = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
            }

            retValue +=
                string.Format(registryRegisterProtocolTemplate,
                              Path.GetFileName(Application.ExecutablePath),
                              Application.ExecutablePath.Replace("\\", "\\\\"),
                              shellParameter
                             );

            #endregion
            return retValue;
        }

        public static void CreateOrgPortalUrlProtocolSubKeys(RegistryKey orgPortal)
        {
            orgPortal.SetValue("", "URL:OrgPortal Protocol");
            orgPortal.SetValue("URL Protocol", "");

            RegistryKey defaultIcon = orgPortal.CreateSubKey("DefaultIcon");
            defaultIcon.SetValue("", Path.GetFileName(Application.ExecutablePath));

            RegistryKey shell = orgPortal.CreateSubKey("shell");

            RegistryKey command;
            if (false) // true to register to run as administrator (may not work if using clickonce to deploy)
            {
                RegistryKey runas = shell.CreateSubKey("runas");
                command = runas.CreateSubKey("command");
            }
            else
            {
                RegistryKey open = shell.CreateSubKey("open");
                command = open.CreateSubKey("command");
            }

            command.SetValue("", Application.ExecutablePath + " %1");
        }

        public static void CreateDontRequireDevLicenseRegistry()
        {
            //[HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx]
            //"AllowDevelopmentWithoutDevLicense"=dword:00000001
            //"AllowAllTrustedApps"=dword:00000001
            try
            {
                RegistryKey windowsKey = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Policies").OpenSubKey("Microsoft").OpenSubKey("Windows");
                RegistryKey windowsAppx = windowsKey.GetSubKeyNames().Contains("Appx") ? windowsKey.OpenSubKey("Appx") : windowsKey.CreateSubKey("Appx");
                var ac = windowsAppx.GetAccessControl(System.Security.AccessControl.AccessControlSections.All);

                //TODO: below code returns permission errors even when running under admin account, will try to import it by file
                windowsAppx.SetValue("AllowDevelopmentWithoutDevLicense", "00000001", RegistryValueKind.DWord);
                windowsAppx.SetValue("AllowAllTrustedApps", "00000001", RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
            }

        }

        public static bool RunAsAdmin(string fileName, string parameter)
        {
            try
            {
                var process = new System.Diagnostics.Process();
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = fileName;
                process.StartInfo.Arguments = parameter;
                process.StartInfo.Verb = "runas";
                process.Start();

                while (!process.HasExited)
                    System.Threading.Thread.Sleep(5);

                return true;

            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                return false;
            }

        }

        public static void SaveDontRequireDevLicenseInstalledIfFileExists()
        {
            if (File.Exists(Path.Combine(Utils.CurrentPath, Utils.DontRequiredDevLicenseRegistryFileName)))
            {
                Settings.Default.DontRequireDevLicenseInstalled = true;
                Settings.Default.Save();
            }
        }

        public static string CurrentPath { get { return Application.ExecutablePath.Replace(Path.GetFileName(Application.ExecutablePath), string.Empty); } }


    }
}
