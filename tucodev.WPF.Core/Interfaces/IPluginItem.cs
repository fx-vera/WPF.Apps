using System.ComponentModel;
using System.Windows.Input;

namespace tucodev.WPF.Core.Interfaces.Interfaces
{
    /// <summary>
    /// Interface IPluginItem
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IPluginItem //: INotifyPropertyChanged
    {
        string Id { get; set; }

        string Name { get; set; }

        ICommand Command { get; set; }
    }
}
