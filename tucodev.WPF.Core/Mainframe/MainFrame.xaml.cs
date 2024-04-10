using System.Windows;
using Tucodev.Core.Interfaces;

namespace Tucodev.WPF.Core.Mainframe
{
    /// <summary>
    /// Lógica de interacción para MainFrame.xaml
    /// </summary>
    public partial class MainFrame : Window, IMainFrame
    {
        public MainFrame()
        {
            InitializeComponent();
        }
    }
}