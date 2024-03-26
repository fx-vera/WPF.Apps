using Microsoft.Extensions.DependencyInjection;
using Tucodev.Core.Interfaces;

namespace tucodev.WPF.Core.Interfaces.Managers
{
    public class PageManager : Tucodev.Core.Managers.PageManager, IPageManager
    {
        IServiceProvider _services;
        /// <summary>
        /// Initializes a new instance of the <see cref="PageManager"/> class.
        /// </summary>
        public PageManager(IServiceProvider services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #region IPageManager functions

        public override IWindowViewModel SetPageInMainWindow(IViewModel newPage)
        {
            if (newPage == null)
            {
                return null;
            }

            var mainframe = _services.GetRequiredService<IMainWindowViewModel>();

            IWindowViewModel openedInstance = CreateNewWindow(newPage);
            mainframe.ViewModel = openedInstance.ViewModel;
            return openedInstance;
        }

        #endregion
    }
}