using System.Windows;
using Tucodev.Core.Interfaces;

namespace tucodev.WPF.Core.Mainframe
{
    /// <summary>
    /// Lógica de interacción para MainFrame.xaml
    /// </summary>
    public partial class MainFrame : Window, IMainWindow
    {
        public MainFrame()
        {
            InitializeComponent();
        }

        //public void SetDataContext(IMainWindowViewModel viewModel)
        //{
        //    this.DataContext = viewModel;
        //}
    }
}