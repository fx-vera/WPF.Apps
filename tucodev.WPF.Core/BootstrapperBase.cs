using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using tucodev.Core.Mainframe;
using tucodev.WPF.Core.Mainframe;
using Tucodev.Core.Interfaces;
using Tucodev.Core.Models;
using MessageBox = System.Windows.MessageBox;

namespace tucodev.WPF.Core
{
    /// <summary>
    /// Base class to manage the basic application operations and the main window.
    /// This class can be extended depending on requirements
    /// </summary>
    public abstract class BootstrapperBase : System.Windows.Application, IBootstrapperBase
    {
        NotifyIcon notifyIcon;

        /// <summary>
        /// Contructor called by Application.
        /// </summary>
        public BootstrapperBase()
        {
            UnhandledExceptionHandler();
        }

        public abstract List<string> DllAllowed { get; }

        protected abstract bool IsNotifiyIconMode { get; }
        protected virtual string NotifyIconTitle { get; }
        protected virtual Icon NotifyIconIcon { get; }

        IMainWindowViewModel mainViewModel;

        public void OnStartup()
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            //Load application modules
            IEnumerable<Assembly> assembliesToCompose = GetAssembliesForIoC().Distinct(new AssemblyNameComparer());

            string loadError = string.Empty;
            bool modulesLoadedOk = false;

            try
            {
                var serviceCollection = new ServiceCollection();
                RegisterMainFrame(serviceCollection);
                RegisterMainWindow(serviceCollection);
                RegisterDependencies(serviceCollection, assembliesToCompose);
                DI.PublicServices(serviceCollection);
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                modulesLoadedOk = true;
            }
            catch (Exception ex)
            {
                loadError = ex.Message;
            }

            if (!modulesLoadedOk)
            {
                System.Windows.MessageBox.Show(loadError, "Error Loading Application Modules.");
                Current.Shutdown();
                return;
            }

            LoadApplicationViews();

            if (!IsNotifiyIconMode)
            {
                Open_Click(null, null);
            }
            else
            {
                notifyIcon = new NotifyIcon
                {
                    Icon = NotifyIconIcon,
                    Text = NotifyIconTitle,
                    Visible = true,
                };

                notifyIcon.DoubleClick += (sender, ex) =>
                {
                    Open_Click(null, null);
                };

                notifyIcon.ContextMenuStrip = new ContextMenuStrip();
                notifyIcon.ContextMenuStrip.Items.Add("Open", null, Open_Click);
                notifyIcon.ContextMenuStrip.Items.Add("Close", null, Close_Click);
            }
        }

        public void UnhandledExceptionHandler()
        {
            Utils.UnhandledExceptionHandler.Init();
        }

        protected sealed override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            OnStartup();
        }

        /// <summary>
        /// Function called to create the main application window.
        /// Can be overrided.
        /// </summary>
        /// <returns></returns>
        protected virtual Window CreateMainWindow()
        {
            Window mainWindow = null;
            mainViewModel = DI.ServiceProvider.GetRequiredService<IMainWindowViewModel>();

            var mainw = DI.ServiceProvider.GetRequiredService<IMainWindow>();
            if (mainw is Window)
            {
                mainw.SetDataContext(mainViewModel);
                mainWindow = (Window)mainw;
                mainWindow.Dispatcher.BeginInvoke(new Action(() => mainWindow.SetCurrentValue(Window.TopmostProperty, false)), System.Windows.Threading.DispatcherPriority.ApplicationIdle, null);
            }

            return mainWindow;
        }

        #region Private

        /// <summary>
        /// Retrieves the list of assemblies containing parts or modules 
        /// that must be composed using MEF and instantiated through IoC.
        /// In this case, it only adds the current executing assembly. 
        /// If a subclass overrides this, it should call this base implementation, 
        /// take the returned list and then add whatever it wants to it, and return it.
        /// </summary>
        /// <returns>The list of assemblies to be loaded and composed</returns>
        private List<Assembly> GetAssembliesForIoC()
        {
            List<Assembly> possiblePlugins = new List<Assembly>();
            string exeDir = Path.GetDirectoryName(GetType().Assembly.Location);

            string[] dllsInDir = Directory.GetFiles(exeDir, "*.dll");

            for (int i = 0; i < dllsInDir.Length; i++)
            {
                string dllWithPath = dllsInDir[i];
                string dllName = Path.GetFileName(dllWithPath);

                bool tryLoadDll = DllAllowed.Any(x => dllName.IndexOf(x, StringComparison.InvariantCultureIgnoreCase) >= 0);

                if (tryLoadDll)
                {
                    possiblePlugins.Add(Assembly.LoadFile(dllWithPath));
                }
            }

            return possiblePlugins;
        }

        private void LoadApplicationViews()
        {
            DI.ServiceProvider.GetService<IViewsManager>()?.LoadAvailableViews();
        }

        private void Open_Click(object sender, EventArgs e)
        {
            MainWindow = CreateMainWindow();
            if (MainWindow == null)
            {
                MessageBox.Show("Error", "Error creating the MainWindow for the Application Modules in Applivery.MarvelComics.Desktop.");
            }
            else
            {
                LoadPlugins();
                ((IMainWindowViewModel)MainWindow.DataContext).SetSelectedPlugin();
                MainWindow.Show();
            }
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Current.Shutdown();
        }

        private void LoadPlugins()
        {
            LoadPluginEventArgs args;
            var pluginItems = DI.ServiceProvider.GetServices<IPluginItem>();

            foreach (var item in pluginItems)
            {
                args = new LoadPluginEventArgs
                {
                    Id = item.Id,
                    Item = item,
                    IsCommand = true
                };
                mainViewModel.SetPlugins(args);
            }
        }

        public virtual void RegisterDependencies(IServiceCollection services, IEnumerable<Assembly> assembliesToLoad)
        {
            LoadModules(assembliesToLoad, services);
        }

        public virtual void RegisterMainFrame(IServiceCollection services)
        {
            services.AddScoped<IMainWindow, MainFrame>();
        }

        public virtual void RegisterMainWindow(IServiceCollection services)
        {
            services.AddScoped<IMainWindowViewModel, MainWindowViewModel>();
        }

        /// <summary>
        /// Tries to do the MEF composition of all assemblies and preloaded objects.
        /// Throws a ModuleLoadErrorException if the composition fails
        /// </summary>
        /// <param name="preloadedObjects"></param>
        /// <param name="assembliesToLoad"></param>
        private void LoadModules(IEnumerable<Assembly> assembliesToLoad, IServiceCollection services)
        {
            LoadType(assembliesToLoad, typeof(IPluginItem), services);
            LoadType(assembliesToLoad, typeof(IPageManager), services);
            LoadType(assembliesToLoad, typeof(IViewsManager), services);
        }

        private void LoadType(IEnumerable<Assembly> distinctAssemblies, Type myType, IServiceCollection services)
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

        #endregion Private
    }

    class AssemblyNameComparer : IEqualityComparer<Assembly>
    {
        public bool Equals(Assembly x, Assembly y)
        {
            return x == y || x.GetName().Name == y.GetName().Name;
        }

        public int GetHashCode(Assembly obj)
        {
            return obj.GetHashCode();
        }
    }
}