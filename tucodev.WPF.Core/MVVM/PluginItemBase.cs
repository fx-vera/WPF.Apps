using System.Windows.Input;
using Tucodev.Core.Interfaces;

namespace tucodev.WPF.Core.Interfaces.MVVM
{
    public class PluginItemBase : IPluginItem
    {
        public PluginItemBase()
        {
        }

        public PluginItemBase(string id, string name, ICommand command)
        {
            Id = id;
            Name = name;
            Command = command;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public ICommand Command { get; set; }

        //public event PropertyChangedEventHandler PropertyChanged; // TODO FRAN: eventos de notificacion.
    }
}
