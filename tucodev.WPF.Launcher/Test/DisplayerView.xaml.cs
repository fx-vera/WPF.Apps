using System.Windows.Controls;
using Tucodev.Core.Interfaces;

namespace tucodev.WPF.Launcher.Test
{
    /// <summary>
    /// Lógica de interacción para DisplayerView.xaml
    /// </summary>
    public partial class DisplayerView : UserControl, IContainerView
    {
        public DisplayerView()
        {
            InitializeComponent();
        }
    }
}
