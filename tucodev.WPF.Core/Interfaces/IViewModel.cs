using System.ComponentModel;

namespace tucodev.WPF.Core.Interfaces.Interfaces
{
    /// <summary>
    /// Interface to be followed by all controls that are the main content of an independent window
    /// It provides some common properties and functionalities so that a common window frame 
    /// can be provided as a container.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public interface IViewModel
    {
        /// <summary>
        /// Id to distinguish this page, for common events and saving the layout
        /// </summary>
        string Id { get; set; }
    }
}
