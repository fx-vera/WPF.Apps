namespace tucodev.WPF.Core.Interfaces.Interfaces
{
    public interface IBootstrapperBase
    {
        /// <summary>
        /// List of substrings of the dll names that WILL be allowed when trying MEF composition
        /// </summary>
        List<string> DllAllowed { get; }
        void OnStartup();

        /// <summary>
        /// Here implements a method to handle the unhandled exceptios.
        /// </summary>
        void UnhandledExceptionHandler();
    }
}
