using Microsoft.Extensions.DependencyInjection;
using tucodev.WPF.Core;
using Tucodev.Core.Interfaces;

namespace tucodev.WPF.Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : BootstrapperBase
    {
        public override List<string> DllAllowed { get { return new List<string>() { "tucodev" }; } }
        protected override bool IsNotifiyIconMode { get { return false; } }
        //protected override Icon NotifyIconIcon { get { return Launcher.Properties.Resources.acaptain; } }
        protected override string NotifyIconTitle { get { return "Yolo Enjoyment"; } }

        public override void RegisterMainFrame(IServiceCollection services)
        {
            services.AddScoped<IMainWindow, MainFrame>();
        }
    }
}
