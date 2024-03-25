using Microsoft.Extensions.DependencyInjection;

namespace tucodev.WPF.Core
{
    public static class DI
    {
        public static void PublicServices(IServiceCollection serviceCollection)
        {
            //DI.serviceCollection = serviceCollection;
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public static IServiceProvider ServiceProvider { get; private set; }
        //private static IServiceCollection serviceCollection;

        /*
        public static void RegisterDependencies(this IServiceCollection services, IEnumerable<Assembly> assembliesToLoad)
        {
            services.AddScoped<IMainWindow, MainFrame>();
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
        */
    }
}
