using Microsoft.Extensions.DependencyInjection;

namespace tucodev.WPF.Core
{
    public class DI
    {
        static IServiceProvider serviceProvider;
        static IServiceCollection serviceCollection = new ServiceCollection();

        public void PublicServices(IServiceCollection serviceCollection)
        {
            serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public static IServiceProvider ServiceProvider { get { return serviceProvider; } }
        public static IServiceCollection ServiceCollection { get { return serviceCollection; } }
    }
}
