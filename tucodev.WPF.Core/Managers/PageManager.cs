using Tucodev.Core.Interfaces;

namespace tucodev.WPF.Core.Interfaces.Managers
{
    //[Export(typeof(IPageManager))]
    public abstract class PageManager : Tucodev.Core.Managers.PageManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageManager"/> class.
        /// </summary>
        //[ImportingConstructor]
        public PageManager()
        {
        }

        #region IPageManager functions

        public override IWindowViewModel SetPageInMainWindow(IViewModel newPage)
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

        //public IWindowViewModel CreateNewWindow(IViewModel newPage)
        //{
        //    return new GenericViewModel(newPage);
        //}
    }
}