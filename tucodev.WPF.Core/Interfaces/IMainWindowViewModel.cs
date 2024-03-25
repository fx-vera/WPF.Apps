using System.ComponentModel;
using tucodev.WPF.Core.Interfaces.Models;

namespace tucodev.WPF.Core.Interfaces.Interfaces
{
    public interface IMainWindowViewModel : INotifyPropertyChanged // TODO FRAN: se puede quitar el INotifyPropertyChanged??
    {
        string Title { get; }

        string Id { get; set; }
        IViewModel ViewModel { get; set; }

        void SetSelectedPlugin();
        void SetPlugins(LoadPluginEventArgs args);
    }
}
