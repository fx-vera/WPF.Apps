using Microsoft.Extensions.DependencyInjection;
using Tucodev.ComicLand.Marvel.Views;
using Tucodev.Core.Interfaces;
using Tucodev.Core.MVVM;
using Tucodev.WPF.Core.Utils;

namespace Tucodev.ComicLand.Marvel.ViewModels
{
    /// <summary>
    /// Allow to set the plugin in the main window by IoC.
    /// </summary>
    public class ExportPlugin : PluginItemBase
    {
        IServiceProvider _services;

        public ExportPlugin(IServiceProvider services)
        {
            Id = "{4D36ABEE-72EA-4B3C-AA37-22A3D0CA613D}";
            Name = "Marvel Comics";
            Command = this.MenuItemCommand(this, new Func<ViewModelBase>(() => { return new DisplayerViewModel(); }));
            _services = services ?? throw new ArgumentNullException(nameof(services));
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
            var pm = _services.GetRequiredService<IPageManager>();
            pm.SetPageInMainWindow(page);
        }
    }

    public class ComicsDisplayerViewMapping : ViewViewModelMappingBase<DisplayerViewModel, DisplayerView> { }
}
