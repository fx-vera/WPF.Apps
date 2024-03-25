using Microsoft.Extensions.DependencyInjection;
using tucodev.Core.Mainframe;
using tucodev.WPF.Core.Mainframe;
using Tucodev.Core.Interfaces;

namespace tucodev.WPF.Core
{
    public static class DI
    {
        public static void Init(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public static IServiceProvider ServiceProvider { get; private set; }

        public static void RegisterLogicDependencies(this IServiceCollection services)
        {
            services.AddScoped<IMainWindow, MainWindow>();
            services.AddScoped<IMainWindowViewModel, MainWindowViewModel>();
        }
    }
}
