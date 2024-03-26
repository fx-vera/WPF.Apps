using System.Windows.Input;

namespace Yolo.Soft.Plugins.Comics.Marvel.Models
{
    //[AddINotifyPropertyChangedInterface]
    public class PluginItem : Yolo.Soft.Core.WPF.MVVM.PluginItem
    {
        public PluginItem(string id, string name, ICommand command) : base(id, name, command) { }
    }
}
