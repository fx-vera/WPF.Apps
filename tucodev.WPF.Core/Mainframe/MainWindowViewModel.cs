using System.Collections.ObjectModel;
using System.ComponentModel;
//using System.Composition;
using tucodev.WPF.Core.Interfaces.MVVM;
using Tucodev.Core.Interfaces;
using Tucodev.Core.Models;

namespace tucodev.WPF.Core.Mainframe
{
    //[System.ComponentModel.Composition.Export(typeof(IMainWindowViewModel))]
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel : IMainWindowViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="pm">The pm.</param>
        //[ImportingConstructor]
        public MainWindowViewModel()
        {
            Title = "YOLO Software";
            Plugins = new ObservableCollection<PluginItemBase>();
            Id = "{EA29CDC3-073D-4C08-BE0A-7D1C9DB1A27C}";
        }

        public string Title { get; set; }

        public ObservableCollection<PluginItemBase> Plugins { get; set; }

        public string Id { get; set; }

        public IViewModel ViewModel { get; set; }

        /// <summary>
        /// Registers upon creation the type of the view associated to this view model.
        /// We do this instead of creating a ViewMapping to allow subclasses to change the view
        /// </summary>
        public void RegisterAssociatedView()
        {
            //var viewsManager = IoC.Get<IViewsManager>();
            //viewsManager?.RegisterView(typeof(IMainWindowViewModel), typeof(IMainWindow));
        }

        public void SetSelectedPlugin()
        {
            Plugins.FirstOrDefault()?.Command.Execute(null);
        }

        public void SetPlugins(LoadPluginEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Item.Id))
            {
                return;
            }

            var plugin = Plugins.FirstOrDefault(x => x.Id.Equals(args.Item.Id, StringComparison.InvariantCultureIgnoreCase));
            if (plugin == null)
            {
                plugin = new PluginItemBase(args.Item.Id, args.Item.Name, args.Item.Command);
                Plugins.Add(plugin);
            }
        }
    }
}
