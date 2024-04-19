using System.Windows;
using System.Windows.Markup;
using Tucodev.Core.Interfaces;
using Tucodev.WPF.Core;

namespace tucodev.WPF.ComicLand.Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : BootstrapperBase
    {
        public override List<string> DllAllowed { get { return new List<string>() { "tucodev" }; } }
        protected override bool IsNotifiyIconMode { get { return false; } }
        //protected override Icon NotifyIconIcon { get { return Launcher.Properties.Resources.acaptain; } }
        protected override string NotifyIconTitle { get { return "Comicland"; } }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (MainWindow == null)
                Application.Current.Shutdown();
            else
            {// para que la app se cierre al cerrar la ventana principal
                // TODO FRAN: mover a bootstrapper.
                MainWindow.Closed += (o, ee) => Application.Current.Shutdown();
                MainWindow.Show();
                //NotificationManager.AddLog(Severity.INFO, "SGV initializated", "SGV");
            }
        }

        public override void RegisterCustomMainFrame()
        {
            // para que funcione la custom, la Window tiene que crearse antes del base.onstartup();
            //MainWindow = new MainWindow();// DI.ServiceProvider.GetRequiredService<IMainWindow>();
        }
    }
}