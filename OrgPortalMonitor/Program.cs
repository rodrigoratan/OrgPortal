using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OrgPortalMonitor.Common;

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
                    MessageBox.Show("You will have " + RequestQueryString["debugDelay"] + "s after you click OK to attach the debugger (or you can attach before clicking OK)");
                }

                Thread.Sleep(debugDelay);
            }
            #endregion

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
            if (
                 (args != null && args.Length > 1) || 
                 (RequestQueryString["mode"] != null && RequestQueryString["mode"] == "install") || 
                 (AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData != null &&
                  AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData.Count() > 0)
               )
            {
                #region AppRequest
                string fileName = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData != null ?
                                       AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0] :
                                                                                                                   "" ; 
                if (!System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)

                if (args != null && args.Length > 1)
                {
                    //string executable = args[0];
                    fileName = args[1];
                }
                if (fileName.Contains("file:///"))
                {
                    fileName = fileName.Replace("file:///", "");
                }
                
                if (RequestQueryString["mode"] != null && RequestQueryString["mode"] == "install") //TODO: ...
                {
                    Application.Run(new AppRequest());
                }
                
                //Check file exists
                if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                {
                    Application.Run(new AppRequest(fileName));
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

                        MessageBox.Show("There's already another instance of OrgPortal monitor running...[" + foundProcesses.Join(Environment.NewLine) + "]");
                    }
                }
                else
                {
                    Application.Run(new Form1()); //main window
                }
                #endregion
            }
        }
    }
}

