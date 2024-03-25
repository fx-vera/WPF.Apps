using tucodev.WPF.Core.Interfaces.Interfaces;
using tucodev.WPF.Core.Interfaces.Managers;
using tucodev.WPF.Core.Interfaces.MVVM;
using tucodev.WPF.Core.Utils;

namespace tucodev.WPF.Core.MVVM
{
    /// <summary>
    /// Class to Export the Plugin and make avaiable to IoC reading.
    /// </summary>
    public abstract class ExportPluginBase
    {
        protected ExportPluginBase()
        {
            LoadPlugins();
        }

        public abstract void LoadPlugins();

        public RelayCommand MenuItemCommand(PluginItemBase menuItem, Func<ViewModelBase> pageCreator)
        {
            return new RelayCommand(param => OnCommand(menuItem, pageCreator()), null);
        }

        /// <summary>
        /// Assign this command to your MenuItemBase.Command
        /// </summary>
        /// <param name="menuItem"></param>
        /// <param name="page"></param>
        /// <param name="showInMainframe"></param>
        private void OnCommand(PluginItemBase menuItem, ViewModelBase page)
        {
            //page.Id = menuItem.Id;
            //var pm = IoC.Get<IPageManager>();
            //pm.SetPageInMainWindow(page);
        }
    }
}
