namespace tucodev.WPF.Core.Interfaces.Interfaces
{
    public interface IViewsManager
    {
        /// <summary>
        /// Gets the available views.
        /// </summary>
        /// <value>
        /// The available views.
        /// </value>
        public List<IVVMMappingBase> AvailableViews { get; }

        /// <summary>
        /// Loads the availiable views.
        /// </summary>
        public void LoadAvailableViews();

        /// <summary>
        /// Registers the view.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="view">The view.</param>
        public void RegisterView(Type viewModel, Type view);
    }
}
