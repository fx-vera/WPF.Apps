using System.Windows;
using Tucodev.Core.Interfaces;

namespace tucodev.WPF.ComicLand.Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainFrame
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}