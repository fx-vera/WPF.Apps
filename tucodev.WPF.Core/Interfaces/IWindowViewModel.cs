using System.ComponentModel;

namespace tucodev.WPF.Core.Interfaces.Interfaces
{
    public interface IWindowViewModel : INotifyPropertyChanged
    {
        IViewModel ViewModel { get; set; }
    }
}