using System.Windows;
using tucodev.WPF.Core;
using Tucodev.Core.Interfaces;

namespace Tucodev.WPF.Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : BootstrapperBase
    {
        public override List<string> DllAllowed { get { return new List<string>() { "tucodev" }; } }
        protected override bool IsNotifiyIconMode { get { return false; } }
        //protected override Icon NotifyIconIcon { get { return Launcher.Properties.Resources.acaptain; } }
        protected override string NotifyIconTitle { get { return "Enjoy the app!"; } }


    }
}
