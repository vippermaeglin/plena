/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Diagnostics;
using System.Windows.Forms;
using M4.DataServer.Interface;
using M4.M4v2.Workspace;
using M4Core.Entities;
using M4Utils.Language;
using System.ComponentModel;
using DSPlena;
using System.Threading; 

namespace M4
{
    static class Program
    {
        /*******************************************************
         * 
         * CHANGE HERE VERSIONS FOR THIS BUILD:
         * 
         * UPGRADE CODE: {B7C0801E-B354-449F-8FCE-76EA3406893B} //Never change this!
         * 
         * -PROJECT>M4 Properties>Application>Assembly Information:
         */
        public static string VERSION = "1.14.0.1";
         /* 
          * 
         * *****************************************************/
        public static LanguageDefault LanguageDefault;
        public static int LanguageStockChartX;
        public static LoginAuthentication LoginAuthentication;
        private static M4v2.Authentication.Login.FrmLogin _frmLogin;
        public static ApplicationContext _applicationContext;
        public static BackgroundWorker _preLoadWork;
        public static frmMain2 _frmMain2;
        public static Thread _threadDSPlena;
        public static SeamusLog Log = new SeamusLog(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + @"\Base\SYSTEM2");
            
        private static string[] _args;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Log.Info("\n************************************** ");
                _args = args;

                if (System.IO.Directory.Exists("C:\\Projeto Plena"))
                {
                    Log.Info("\nRegistering STOCKCHARTX... ");
                    string regCmd = "/C regsvr32 /s \"" + "C:\\Projeto Plena\\Plena Trading Platform Release 2\\StockChartX\\Source\\Release\\StockChartX.ocx\"";
                    //Process.Start("cmd.exe", regCmd);
                    Process process = new Process();
                    ProcessStartInfo processinfo = new System.Diagnostics.ProcessStartInfo();
                    processinfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    processinfo.FileName = "cmd.exe";
                    processinfo.Arguments = regCmd;
                    process.StartInfo = processinfo;
                    process.Start();

                    Log.Info("\nRegistering TRADESCRIPT... ");
                    string regCmd2 = "/C regsvr32 /s \"" + "C:\\Projeto Plena\\Plena Trading Platform Release 2\\TradeScript\\trunk\\ReleaseMinSize\\TradeScript.dll\"";
                    //Process.Start("cmd.exe", regCmd);
                    Process process2 = new Process();
                    ProcessStartInfo processinfo2 = new System.Diagnostics.ProcessStartInfo();
                    processinfo2.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    processinfo2.FileName = "cmd.exe";
                    processinfo2.Arguments = regCmd2;
                    process2.StartInfo = processinfo2;
                    process2.Start();
                }
                else if (System.IO.Directory.Exists("D:\\Projetos Visual\\Projeto Plena"))
                {
                    Log.Info("\nRegistering STOCKCHARTX... ");
                    string regCmd = "/C regsvr32 /s \"" + "D:\\Projetos Visual\\Projeto Plena\\Plena Trading Platform Release 2\\StockChartX\\Source\\Release\\StockChartX.ocx\"";
                    //Process.Start("cmd.exe", regCmd);
                    Process process = new Process();
                    ProcessStartInfo processinfo = new System.Diagnostics.ProcessStartInfo();
                    processinfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    processinfo.FileName = "cmd.exe";
                    processinfo.Arguments = regCmd;
                    process.StartInfo = processinfo;
                    process.Start();

                    Log.Info("\nRegistering TRADESCRIPT... ");
                    string regCmd2 = "/C regsvr32 /s \"" + "D:\\Projetos Visual\\Projeto Plena\\Plena Trading Platform Release 2\\TradeScript\\trunk\\ReleaseMinSize\\TradeScript.dll\"";
                    //Process.Start("cmd.exe", regCmd);
                    Process process2 = new Process();
                    ProcessStartInfo processinfo2 = new System.Diagnostics.ProcessStartInfo();
                    processinfo2.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    processinfo2.FileName = "cmd.exe";
                    processinfo2.Arguments = regCmd2;
                    process2.StartInfo = processinfo2;
                    process2.Start();
                }
                Log.Info("\nLoading Languages... ");
                LoadLanguage();

                if (Properties.Settings.Default.IsFirstUse)
                {

                    Log.Info("\nPLENA IS RUNNIG FIRST TIME! ");
                    Properties.Settings.Default.Upgrade();

                    Properties.Settings.Default.DateInstallation = DateTime.Now;
                    Properties.Settings.Default.IsFirstUse = false;
                    Properties.Settings.Default.Save();
                }

                Log.Info("\nStarting App Styles... ");
                Application.EnableVisualStyles();
                Log.Info("\nSetCompatibleTextRenderingDefault... ");
                Application.SetCompatibleTextRenderingDefault(false);
                
                Log.Info("\nChecking files version... ");
                try
                {
                    VersionChecker.VersionChecker.CheckAndCreateDirectories();
                    VersionChecker.VersionChecker.CheckFileVersions(VERSION);
                }
                catch (Exception ex)
                {
                    Log.Info("\nERROR CHECKING VERSION FILES! ");
                    MessageBox.Show("Erro ao iniciar arquivos na base do usuário, entre em contato com nosso suporte!","ERRO");
                    return;
                }


                Log.Info("\nCreating FrmLogin... ");
                _frmLogin = new M4v2.Authentication.Login.FrmLogin();

                Log.Info("\nStarting Login... ");
                _applicationContext = new ApplicationContext(_frmLogin);

                Log.Info("\nLoading Theme... ");
                ManagerWorkspace.Instance().LoadTheme();

                Log.Info("\nShow FrmLogin... ");
                _frmLogin.Show();
                Application.Idle += OnAppIdle;


                Log.Info("\nStarting DSPlena... ");
                //Comment this just for tests:
                StartDSPlena();
                Log.Info("\nStarting M4... ");

                Application.Run(_applicationContext);
                StopDSPlena();
            }
            catch (Exception ex)
            {
                Log.Info(ex.Message + "\n" + ex.StackTrace);
            }
        }

        private static void OnAppIdle(object sender, EventArgs e)
        {
            if (_applicationContext.MainForm != null) 
                return;

            
            Application.Idle -= OnAppIdle;
            _frmMain2 = _args.Length > 0 ? new frmMain2(_args[0]) : new frmMain2();


            Cursor.Current = Cursors.WaitCursor;

            /*_preLoadWork = new BackgroundWorker();
            _preLoadWork.DoWork += new DoWorkEventHandler(_preLoadWork_DoWork);
            _preLoadWork.RunWorkerCompleted += new RunWorkerCompletedEventHandler
                    (_preLoadWork_RunWorkerCompleted);

            _preLoadWork.RunWorkerAsync();*/

            //Instanciate a dummy handle to form so you can use async methods:
            IntPtr dummy = _frmMain2.Handle;
 
            _applicationContext.MainForm = _frmMain2;

            _frmMain2.PreLoad(_frmLogin);

            /*if(!_frmMain2.PreLoad(_frmLogin))
            {
                //Exit if preload fail!
                _frmLogin.Close();
                _frmLogin = null;

                Cursor.Current = Cursors.Default;
                StopDSPlena();

            }*/

            _applicationContext.MainForm.Show();

            _frmLogin.Close();
            _frmLogin = null;

            Cursor.Current = Cursors.Default;
        }

        private static void  _preLoadWork_DoWork(object sender, DoWorkEventArgs e)
        {
           _frmMain2.PreLoad(_frmLogin);
        }

        private static void _preLoadWork_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _applicationContext.MainForm = _frmMain2;

            _applicationContext.MainForm.Show();


            _frmLogin.Close();
            _frmLogin = null;

            Cursor.Current = Cursors.Default;
        }

        private static void callbackfunction(IAsyncResult res)
        {
        }

        private static void LoadLanguage()
        {
            //Linguagem default será inglês
            LanguageDefault = new English();
            LanguageStockChartX = 0;

            if (true /*Settings.Default.DictionaryLanguage.Equals("PortugueseBrazil")*/)
            {
                LanguageDefault = new PortugueseBrazil();
                LanguageStockChartX = 1;
            }
        }

        private static void StartDSPlena()
        {
            _threadDSPlena = new Thread(DSPlena.DSPlena.Instance().Start);
            _threadDSPlena.Name = "ThreadDSPlena";
            _threadDSPlena.SetApartmentState(ApartmentState.STA);
            _threadDSPlena.IsBackground = true;
            _threadDSPlena.Start(); 
        }
        private static void StopDSPlena()
        {
            DSPlena.DSPlena.Instance().Close();
        }
    }
}

