using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.VisualStyles;
using System.Windows.Threading;
using Victoria.DesktopApp.View;
using Victoria.UI.SharedWPF;
using Victoria.ViewModelWPF;

namespace Victoria.DesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 
    public partial class App : Application
    {

        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(App));
        private void Application_Startup(object sender, StartupEventArgs e)
        {            
            Application.Current.DispatcherUnhandledException += this.Application_DispatcherUnhandledException;
            //Disable shutdown when the dialog closes
            Current.ShutdownMode = ShutdownMode.OnLastWindowClose;

            Inicializar_Logger();

            logger.Info("INICIO VICTORIA SUITE");
            
            if (e.Args.Any() && e.Args[0].IndexOf(".vic", System.StringComparison.Ordinal) > 0)
            {
                var simulationXmlURI = e.Args[0];
                var mainWindow = new MainWindow(simulationXmlURI, false);
                mainWindow.Show();
            }
            else
            {
                var welcomeView = new WelcomeView();
                if (welcomeView.ShowDialog() == false)
                {
                    Application.Current.Shutdown(0);
                }
            }
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Exception realerror = e.Exception;
            while (realerror.InnerException != null)
                realerror = realerror.InnerException;
            MessageBox.Show(realerror.Message);
        }

        private void Inicializar_Logger()
        {
            String path = AppDomain.CurrentDomain.BaseDirectory;//System.Reflection.Assembly.GetExecutingAssembly().Location;
            
            var appender = (log4net.Appender.FileAppender)log4net.LogManager.GetRepository().GetAppenders()
                                                    .Where(x => x.GetType().ToString().Contains("FileAppender")).First();

            //appender.File = path + @"Logs\Registro "+DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss")+".log";
            appender.File = path + @"Logs\Victoria_V.log";

            appender.ActivateOptions();
        }
    }
}
