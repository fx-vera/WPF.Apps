//using System.Composition;
using tucodev.WPF.Core.Interfaces.Interfaces;
using tucodev.WPF.Core.Interfaces.Models;

namespace tucodev.WPF.Core.Interfaces.MVVM
{
    /// <summary>
    /// Base class used to register in the global application dictionary a mapping between 
    /// a specific viewmodel (not the interface it implements) and a view. 
    /// </summary>
    //[Export(typeof(IVVMMappingBase))]
    public class ViewViewModelMappingBase : IVVMMappingBase
    {
        private List<IVVMMappingModel> _mappings = new List<IVVMMappingModel>();
        public List<IVVMMappingModel> Mappings { get { return _mappings; } }

        public ViewViewModelMappingBase() { }

        public ViewViewModelMappingBase(IVVMMappingModel vm)
        {
            _mappings.Add(vm);
        }

        public ViewViewModelMappingBase(Type viewModel, Type view)
        {
            AddMapping(viewModel, view);
        }

        public void AddMapping(Type viewModel, Type view)
        {
            _mappings.Add(new VVMMappingModel(viewModel, view));
        }
    }

    public class ViewViewModelMappingBase<ViewModel, View> : ViewViewModelMappingBase
    {
        public ViewViewModelMappingBase() : base(typeof(ViewModel), typeof(View)) { }
    }
}
