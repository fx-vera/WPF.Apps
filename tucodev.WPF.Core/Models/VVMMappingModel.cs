using tucodev.WPF.Core.Interfaces.Interfaces;

namespace tucodev.WPF.Core.Interfaces.Models
{
    public class VVMMappingModel: IVVMMappingModel
    {
        public Type View { get; set; }
        public Type ViewModel { get; set; }

        public VVMMappingModel() { }

        public VVMMappingModel(Type viewModel, Type view)
        {
            View = view;
            ViewModel = viewModel;
        }
    }
}
