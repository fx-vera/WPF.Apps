
namespace tucodev.WPF.Core.Interfaces.Interfaces
{
    /// <summary>
    /// Interface to be exported through MEF to register 
    /// the Views to be assigned to ViewModels
    /// </summary>
    public interface IVVMMappingBase
    {
        List<IVVMMappingModel> Mappings { get; }
    }
}
