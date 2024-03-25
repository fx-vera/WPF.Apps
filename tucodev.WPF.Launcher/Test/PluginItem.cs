using Tucodev.Core.Interfaces;
using tucodev.WPF.Core.Utils;
using Tucodev.Core.MVVM;
using tucodev.WPF.Core;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Composition;

namespace tucodev.WPF.Launcher.Test
{
    [Export(typeof(IPluginItem))]
    public class PluginItem : PluginItemBase
    {
        public PluginItem()
        {
            Id = "{4D36ABEE-72EA-4B3C-AA37-22A3D0CA613D}";
            Name = "Marvel Comics";
            Command = this.MenuItemCommand(this, new Func<ViewModelBase>(() => { return new DisplayerViewModel(); }));
        }

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
            page.Id = menuItem.Id;
            var pm = DI.ServiceProvider.GetRequiredService<IPageManager>();
            pm.SetPageInMainWindow(page);
        }
    }

    [Export(typeof(IPluginItem))]
    public class PluginItem2 : PluginItemBase
    {
        public PluginItem2()
        {
            Id = "{4D36ABEE-72EA-4B3C-AA37-22A3D0CA61FF}";
            Name = "Marvel Comics 2";
            Command = this.MenuItemCommand(this, new Func<ViewModelBase>(() => { return new DisplayerViewModel(); }));
        }

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
            page.Id = menuItem.Id;
            var pm = DI.ServiceProvider.GetRequiredService<IPageManager>();
            pm.SetPageInMainWindow(page);
        }
    }
}
