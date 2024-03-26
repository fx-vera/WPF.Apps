using Microsoft.Extensions.DependencyInjection;
using Tucodev.Core.Interfaces;
using Tucodev.Core.MVVM;
using Tucodev.WPF.Core.Utils;

namespace tucodev.WPF.Launcher.Test
{
    public class PluginItem : PluginItemBase
    {
        IServiceProvider serviceProvider;
        public PluginItem(IServiceProvider services)
        {
            Id = "{4D36ABEE-72EA-4B3C-AA37-22A3D0CA613D}";
            Name = "Marvel Comics";
            Command = this.MenuItemCommand(this, new Func<ViewModelBase>(() => { return new DisplayerViewModel(); }));
            serviceProvider = services ?? throw new ArgumentNullException(nameof(services));
        }

        public RelayCommand MenuItemCommand(PluginItemBase menuItem, Func<ViewModelBase> pageCreator)
        {
            return new RelayCommand(param => OnCommand(menuItem, pageCreator()));
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
            var pm = serviceProvider.GetRequiredService<IPageManager>();
            pm.SetPageInMainWindow(page);
        }
    }

    public class PluginItem2 : PluginItemBase
    {
        IServiceProvider _services;
        public PluginItem2(IServiceProvider services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            Id = "{4D36ABEE-72EA-4B3C-AA37-22A3D0CA61FF}";
            Name = "Marvel Comics 2";
            Command = this.MenuItemCommand(this, new Func<ViewModelBase>(() => { return new DisplayerViewModel(); }));
        }

        public RelayCommand MenuItemCommand(PluginItemBase menuItem, Func<ViewModelBase> pageCreator)
        {
            return new RelayCommand(param => OnCommand(menuItem, pageCreator()));
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
            var pm = _services.GetRequiredService<IPageManager>();
            pm.SetPageInMainWindow(page);
        }
    }

    public class PluginItemViewMapping : ViewViewModelMappingBase<DisplayerViewModel, DisplayerView> { }

    public class PluginItem2ViewMapping : ViewViewModelMappingBase<DisplayerViewModel, DisplayerView> { }
}
