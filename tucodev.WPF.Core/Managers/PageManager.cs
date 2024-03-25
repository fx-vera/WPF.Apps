using tucodev.WPF.Core.Interfaces.Interfaces;
using tucodev.WPF.Core.Mainframe;

namespace tucodev.WPF.Core.Interfaces.Managers
{
    //[Export(typeof(IPageManager))]
    public abstract class PageManager : IPageManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageManager"/> class.
        /// </summary>
        //[ImportingConstructor]
        public PageManager()
        {
        }

        #region IPageManager functions

        public IWindowViewModel SetPageInMainWindow(IViewModel newPage)
        {
            if (newPage == null)
            {
                return null;
            }
            IWindowViewModel openedInstance = null;

            //var mainframe = IoC.Get<IMainWindowViewModel>();
            //openedInstance = CreateNewWindow(newPage);
            //mainframe.ViewModel = openedInstance.ViewModel;
            return openedInstance;
        }

        #endregion

        public IWindowViewModel CreateNewWindow(IViewModel newPage)
        {
            return new GenericViewModel(newPage);
        }
    }
}