using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OrgPortalMonitor.Common;

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
            //var FA = new FileAssociation();
            //FA.Extension = "blah";
            //FA.ContentType = "application/blah";
            //FA.FullName = "blah Project File";
            //FA.ProperName = "blahFile";
            //FA.AddCommand("open", string.Format("\"{0}\" \"%1\"", System.Reflection.Assembly.GetExecutingAssembly().Location));
            ////"C:\\mydir\\myprog.exe %1");
            //FA.IconPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //FA.IconIndex = 0;
            //FA.Create();

            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                //MessageBox.Show("You can attach debug now");
                Thread.Sleep(5000);
            }

            var currentProcess = Process.GetCurrentProcess();
            //var foundProcess = Process.GetProcesses().FirstOrDefault(p => p.ProcessName.Equals(currentProcess.ProcessName));
            //var processExists = Process.GetProcesses().Any(p => p.ProcessName.Equals(currentProcess.ProcessName));
            var foundProcesses = Process.GetProcesses().Where(p => p.ProcessName.Equals(currentProcess.ProcessName));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //with args(user open file with the program)
            //•args[0] is the application path.
            //•args[1] will be the file path.
            //•args[n] will be any other arguments passed in.
            string[] args = Environment.GetCommandLineArgs();

            var RequestQueryString = Utils.GetQueryStringParameters();

            if (
                 (args != null && args.Length > 1) || 
                 (RequestQueryString["mode"] != null && RequestQueryString["mode"] == "install") || 
                 (AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData != null &&
                  AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData.Count() > 0)
               )
            {
                string fileName = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData != null ?
                                       AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0] :
                                       ""; 

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
            }
            //without args
            else
            {
                if (foundProcesses != null)
                {
                    if (foundProcesses.Count() <= 1)
                    {
                        Application.Run(new Form1()); //main window
                        //Application.Run(new AppRequest());
                    }
                    else if (foundProcesses.Count() > 1)
                    {
                        MessageBox.Show("There's already another instance of OrgPortal monitor running...");
                    }
                }
                else
                {
                    Application.Run(new Form1()); //main window
                }
            }
        }
    }
}

