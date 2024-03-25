//using System.ComponentModel.Composition;
using System.Windows;
using tucodev.WPF.Core.Interfaces.Interfaces;

namespace tucodev.WPF.Core.Mainframe
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    //[Export(typeof(IMainWindow))]
    public partial class MainWindow : Window, IMainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void SetDataContext(IMainWindowViewModel viewModel)
        {
            this.DataContext = viewModel;
        }

    }
}
