using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using OrgPortalMonitor.Common;
using OrgPortalMonitor.Properties;

//using Microsoft.VisualBasic; //import Microsoft.VisualBasic dll into the C# project to use Interaction.AppActivate

namespace OrgPortalMonitor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            #region TODO: mutex
            /* //http://odetocode.com/blogs/scott/archive/2004/08/20/the-misunderstood-mutex.aspx
             * //http://stackoverflow.com/questions/229565/what-is-a-good-pattern-for-using-a-global-mutex-in-c
            ////private static string appGuid = "c1a16151-11a1-1515-1919-16131a161719";
            //  private static appGuid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();
            //  using (Mutex mutex = new Mutex(false, @"Global\" + appGuid)) //global
            using (Mutex mutex = new Mutex(false, appGuid)) //this user
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("Instance already running");
                    return;
                }
                GC.Collect();
                Application.Run(new Form1());
            }
            //*/
            #endregion

            ImportRegistrySettings();

            var RequestQueryString = Utils.GetQueryStringParameters();

            #region debugdelay
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed && 
                (RequestQueryString["debugDelay"] != null))
            {
                int debugDelay = 5000;
                if (!Int32.TryParse(RequestQueryString["debugDelay"], out debugDelay))
                {
                    MessageBox.Show("Invalid Int32 value for debug delay. Attach the debugger now and wait " + (debugDelay / 1000) + "s to continue.");
                }
                else
                {
                    MessageBox.Show("You will have " + (debugDelay / 1000) + "s after you click OK to attach the debugger (or you can attach before clicking OK)");
                }

                //Thread.Sleep(debugDelay);
            }
            #endregion

            //Thread.Sleep(20000);
            //MessageBox.Show("OrgPortal is Loading...");

            var currentProcess = Process.GetCurrentProcess();
            //var foundProcess = Process.GetProcesses().FirstOrDefault(p => p.ProcessName.Equals(currentProcess.ProcessName));
            //var processExists = Process.GetProcesses().Any(p => p.ProcessName.Equals(currentProcess.ProcessName));
            var foundProcesses = Process.GetProcesses().Where(p => p.ProcessName.Equals(currentProcess.ProcessName));

            #region Activate another running instance
            //import Microsoft.VisualBasic dll into the C# project.
            //Process[] proc = Process.GetProcessesByName("notepad");
            //Interaction.AppActivate(proc[0].MainWindowTitle);
            //or
            //[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            // public static extern bool SetForegroundWindow(IntPtr hWnd);
            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            #region args
            //with args(user open file with the program)
            //•args[0] is the application path.
            //•args[1] will be the file path.
            //•args[n] will be any other arguments passed in.
            string[] args = Environment.GetCommandLineArgs(); //only useful when installed local and opened by command line

            /* When you publish an app with ClickOnce and then launch it by double-clicking an associated file, the path to that file actually gets stored here:
               AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0]
             # http://stackoverflow.com/a/4607961/2119731:
             * See MSDN's documentation for it here:
               http://msdn.microsoft.com/en-us/library/system.runtime.hosting.activationarguments.aspx
               Plus a tutorial on adding file associations to "Published" projects:
               http://blogs.msdn.com/b/mwade/archive/2008/01/30/how-to-add-file-associations-to-a-clickonce-application.aspx */
            #endregion
            if ((args != null && args.Length > 1)                                               || 
                (RequestQueryString["mode"] != null && RequestQueryString["mode"].ToLower() == "install") ||
                (AppDomain.CurrentDomain.SetupInformation.ActivationArguments                    != null && 
                 AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData     != null &&
                 AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData.Count() > 0 &&
                 (
                   !AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0].Contains("http://") ||
                   (RequestQueryString["mode"] != null && RequestQueryString["mode"].ToLower() == "install")
                 )

                )
               )
            {
                #region AppRequest
                string fileName = AppDomain.CurrentDomain.SetupInformation.ActivationArguments                              != null && 
                                  AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData               != null &&
                                  AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData.Count()        >    0 &&
                                 !AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0].Contains("http://") ?
                                                      AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0] :
                                                                                                                                  "" ;
                //if (!System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                //{
                if (args != null && args.Length > 1)
                {
                    //string executable = args[0];
                    fileName = args[1];
                }
                //}

                fileName = System.Web.HttpUtility.UrlDecode(fileName);

                #region handle protocol and file activation
                if (fileName.Contains("file:///"))
                {
                    fileName = fileName.Replace("file:///", "");
                } 
                else if (fileName.Contains(@"orgportal:///"))
                {
                    fileName = fileName.Replace(@"orgportal:///", "");
                } 
                else if (fileName.Contains(@"orgportal://install/file/"))
                {
                    fileName = fileName.Replace(@"orgportal://install/file/", "");
                }
                else if (fileName.Contains(@"orgportal://install/"))
                {
                    fileName = fileName.Replace(@"orgportal://install/", "");
                }
                else if (fileName.Contains(@"orgportal://"))
                {
                    fileName = fileName.Replace(@"orgportal://", "");
                }
                if (fileName.Contains(@".rt2win/"))
                {
                    fileName = fileName.Replace(@".rt2win/", ".rt2win");
                }

                #endregion

                if (RequestQueryString["mode"] != null && RequestQueryString["mode"].ToLower() == "install") //TODO: ...
                {
                    Application.Run(new AppRequest());
                }
                else
                {
                    //Check file exists
                    if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                    {
                        Application.Run(new AppRequest(fileName));
                    }
                    else
                    {
                        MessageBox.Show("Unable to open file " + fileName);
                    }
                }
                #endregion
            }
            //without args
            else
            {
                #region Main instance
                if (foundProcesses != null)
                {
                    if (foundProcesses.Count() <= 1)
                    {
                        Application.Run(new Form1()); //main window
                        //Application.Run(new AppRequest());
                    }
                    else if (foundProcesses.Count() > 1)
                    {
                        string titulos=string.Empty;
                        foreach (var process in foundProcesses)
                        {
                            titulos+=process.MainWindowTitle+Environment.NewLine;
                            //process.Kill();
                        }
                        MessageBox.Show("There's already another instance of OrgPortal monitor running...[" + titulos + "]");
                    }
                }
                else
                {
                    Application.Run(new Form1()); //main window
                }
                #endregion
            }

        }

        // Requires "using System.Security.Principal;"

        public static bool IsElevated
        {
            get
            {
                return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        private static void ImportRegistrySettings()
        {
            try
            {
                if (IsElevated)
                {
                    //TODO: comment before production
                    //MessageBox.Show("You are running as administrador. We won't ask for permissions unless strictly necessary");

                    RegistryKey orgPortal = Registry.ClassesRoot.CreateSubKey("OrgPortal");
                    Utils.CreateOrgPortalUrlProtocolSubKeys(orgPortal);
                    //commenting because it was unable to write @ [HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx] due to permission errors even whem isElevated==true
                    //CreateDontRequireDevLicenseRegistry();
                    if (!OrgPortalMonitor.Properties.Settings.Default.RequireDevLicense)
                    {
                        if (!File.Exists(Path.Combine(Utils.CurrentPath, Utils.DontRequiredDevLicenseRegistryFileName)))
                        {
                            MessageBox.Show("Running OrgPortal for the first time or you have changed the Require Developer License settings.\n\nPlease say YES to all security prompts you see, we will try to ask it only when strictly necessary.", "Welcome to OrgPortal");

                            string requiredRegistryTemplate = "REGEDIT4" + Environment.NewLine + Environment.NewLine;
                            requiredRegistryTemplate += Utils.ReturnRegistryDontRequireDevLicense();
                            if (Utils.WriteRequiredRegistry(requiredRegistryTemplate, Utils.DontRequiredDevLicenseRegistryFileName))
                            {
                                if (Utils.ImportRequiredRegistry(Utils.DontRequiredDevLicenseRegistryFileName))
                                {
                                    Utils.SaveDontRequireDevLicenseInstalledIfFileExists();
                                }
                            }
                        }
                    }
                }
                else
                {
                    //MessageBox.Show("Not elevated. Using alternate method to register OrgPortal:// protocol.", "OrgPortal");

                    //Make sure next time it runs under admin privileges
                    //SetAlwaysRunAsAdmin();

                    var RequestQueryString = Utils.GetQueryStringParameters();

                    RegistryKey orgPortal = Registry.CurrentUser.CreateSubKey("OrgPortal");
                    Utils.CreateOrgPortalUrlProtocolSubKeys(orgPortal);

                    bool alwaysDontRequireDevLicense = false;

                    var RequireDeveloperLicenseParameter = 
                        RequestQueryString["RequireDeveloperLicense"] != null ? 
                            RequestQueryString["RequireDeveloperLicense"] : 
                            "";
                    
                    bool _requireDeveloperLicense = true;
                    if (!string.IsNullOrEmpty(RequireDeveloperLicenseParameter) &&
                        bool.TryParse(RequireDeveloperLicenseParameter, out _requireDeveloperLicense))
                    {
                        Settings.Default.RequireDevLicense = _requireDeveloperLicense;
                        Settings.Default.Save();
                    }

                    //"open" /*change to "runas" to run as admin - test using clickonce*/
                    string shellParameter =
                           RequestQueryString["shellParameter"] != null ?
                               RequestQueryString["shellParameter"] :
                               "open";

                    string requiredRegistryTemplate = "REGEDIT4" + Environment.NewLine + Environment.NewLine;

                    if (!File.Exists(Path.Combine(Utils.CurrentPath, Utils.RegisterProtocolRegistryFileName)))
                    {
                        MessageBox.Show("Running OrgPortal for the first time.\n\nPlease say YES to all security prompts you see, we will try to ask it only when strictly necessary.", "Welcome to OrgPortal");

                        requiredRegistryTemplate += Utils.ReturnRegistryRegisterProtocol(shellParameter);

                        if (!OrgPortalMonitor.Properties.Settings.Default.RequireDevLicense ||
                            alwaysDontRequireDevLicense)
                        {
                            requiredRegistryTemplate += Utils.ReturnRegistryDontRequireDevLicense();
                            Settings.Default.DontRequireDevLicenseInstalled = true;
                        }

                        if (Utils.WriteRequiredRegistry(requiredRegistryTemplate, Utils.RegisterProtocolRegistryFileName))
                        {
                            if (Utils.ImportRequiredRegistry(Utils.RegisterProtocolRegistryFileName))
                            {
                                if (File.Exists(Path.Combine(Utils.CurrentPath, Utils.RegisterProtocolRegistryFileName)))
                                {
                                    //TODO: save DontRequireDevLicenseInstalled 
                                    Settings.Default.Save();
                                }
                            }
                        }
                    }
                    else // already have imported RegisterProtocolRegistry
                    {
                        if (
                              !alwaysDontRequireDevLicense && // if alwaysDontRequireDevLicense is true, then if already happened
                              !OrgPortalMonitor.Properties.Settings.Default.RequireDevLicense && // if RequireDevLicense is true, dont import
                              !Settings.Default.DontRequireDevLicenseInstalled
                           ) 
                        {
                            if (!File.Exists(Path.Combine(Utils.CurrentPath, Utils.DontRequiredDevLicenseRegistryFileName)))
                            {
                                MessageBox.Show("We must import some settings to the registry because you changed the Require Developer License setting.\n\nPlease say YES to all security prompts you see, we will try to ask it only when strictly necessary.", "Welcome to OrgPortal");
                                requiredRegistryTemplate += Utils.ReturnRegistryDontRequireDevLicense();
                                if (Utils.WriteRequiredRegistry(requiredRegistryTemplate, Utils.DontRequiredDevLicenseRegistryFileName))
                                {
                                    if (Utils.ImportRequiredRegistry(Utils.DontRequiredDevLicenseRegistryFileName))
                                    {
                                        Utils.SaveDontRequireDevLicenseInstalledIfFileExists();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
            }
        }


    }
}

