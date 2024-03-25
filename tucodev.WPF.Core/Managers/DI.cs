using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;
using tucodev.Core.Mainframe;
using tucodev.WPF.Core.Mainframe;
using Tucodev.Core.Interfaces;
using Tucodev.Core.Models;

namespace tucodev.WPF.Core
{
    public static class DI
    {
        private static MyCompositionContainer _container;

        public static void PublicServices(IServiceCollection serviceCollection)
        {
            DI.serviceCollection = serviceCollection;
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public static IServiceProvider ServiceProvider { get; private set; }
        private static IServiceCollection serviceCollection;
        public static event EventHandler<ModuleLoadedEventArgs> ModuleLoaded;

        public static void RegisterDependencies(this IServiceCollection services, IEnumerable<Assembly> assembliesToLoad)
        {
            services.AddScoped<IMainWindow, MainWindow>();
            services.AddScoped<IMainWindowViewModel, MainWindowViewModel>();
            //services.AddScoped
            //services.AddSingleton<IPluginItem>(p=>p.GetRequiredService<IPluginItem>());
            LoadModules(assembliesToLoad, services);
        }

        /// <summary>
        /// Tries to do the MEF composition of all assemblies and preloaded objects.
        /// Throws a ModuleLoadErrorException if the composition fails
        /// </summary>
        /// <param name="preloadedObjects"></param>
        /// <param name="assembliesToLoad"></param>
        private static void LoadModules(IEnumerable<Assembly> assembliesToLoad, IServiceCollection services)
        {
            LoadType(assembliesToLoad, typeof(IPluginItem), services);
            LoadType(assembliesToLoad, typeof(IPageManager), services);
            LoadType(assembliesToLoad, typeof(IViewsManager), services);
        }

        private static void LoadType(IEnumerable<Assembly> distinctAssemblies, Type myType, IServiceCollection services)
        {
            foreach (var assembly in distinctAssemblies)
            {
                var typeInstances = assembly
                .GetTypes()
                .Where(x => x.IsAssignableTo(myType))
                .Where(x => !x.IsInterface)
                .Where(x => !x.IsAbstract)
                .ToArray();

                if (!typeInstances.Any())
                {
                    continue;
                }

                foreach (var typeInstance in typeInstances)
                {
                    // this is the wireup that allows you to DI your instances
                    services.AddScoped(myType, typeInstance);
                }
            }
        }
    }

    /// <summary>
    /// CompositionContainer que lanza un evento cada vez que 
    /// carga una "parte" MEF, para tener idea del progreso de la composición
    /// </summary>
    internal class MyCompositionContainer : CompositionContainer
    {
        public delegate void PartLoadedDlg(string name);
        public event PartLoadedDlg OnPartLoaded;

        public MyCompositionContainer(ComposablePartCatalog catalog, params ExportProvider[] providers)
            : base(catalog, providers)
        {
        }


        private List<string> _parsed = new List<string>();
        public List<string> ParsedParts { get { return _parsed; } }

        protected override IEnumerable<Export> GetExportsCore(System.ComponentModel.Composition.Primitives.ImportDefinition definition, AtomicComposition atomicComposition)
        {
            if (!_parsed.Contains(definition.ContractName))
            {
                _parsed.Add(definition.ContractName);
                if (OnPartLoaded != null)
                {
                    OnPartLoaded(definition.ContractName);
                }
            }

            return base.GetExportsCore(definition, atomicComposition);
        }
    }
}
