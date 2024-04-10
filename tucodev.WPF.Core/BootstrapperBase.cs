using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Tucodev.Core.Interfaces;
using Tucodev.Core.Mainframe;
using MessageBox = System.Windows.MessageBox;

namespace Tucodev.WPF.Core
{
    /// <summary>
    /// Base class to manage the basic application operations and the main window.
    /// This class can be extended depending on requirements
    /// </summary>
    public abstract class BootstrapperBase : System.Windows.Application, IBootstrapperBase
    {
        NotifyIcon notifyIcon;
        IServiceProvider serviceProvider;
        IServiceCollection serviceCollection;
        IMainFrameViewModel mainViewModel;

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


        protected override void OnStartup(StartupEventArgs e)
        {
            RegisterCustomMainFrame();
            base.OnStartup(e);
            OnStartup();
        }

        public void OnStartup()
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            //Load application modules
            IEnumerable<Assembly> assembliesToCompose = GetAssembliesForIoC().Distinct(new AssemblyNameComparer());

            string loadError = string.Empty;
            bool modulesLoadedOk = false;

            try
            {
                serviceCollection = new ServiceCollection();
                RegisterMainWindowViewModel(serviceCollection);
                RegisterDependencies(serviceCollection, assembliesToCompose);
                serviceProvider = serviceCollection.BuildServiceProvider();
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
            mainViewModel = serviceProvider.GetRequiredService<IMainFrameViewModel>();

            RegisterMainFrame();

            SetDataContextToMainWindow(mainViewModel);

            serviceProvider.GetRequiredService<IPageManager>()?.LoadAvailableViews();

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

        /// <summary>
        /// Function called to create the main application window.
        /// Can be overrided.
        /// </summary>
        /// <returns></returns>
        private void SetDataContextToMainWindow(IMainFrameViewModel mainViewModel)
        {
            if (MainWindow is IMainFrame)
            {
                //((IMainWindow)MainWindow).SetDataContext(mainViewModel);
                MainWindow.DataContext = mainViewModel;
                MainWindow.Dispatcher.BeginInvoke(new Action(() => MainWindow.SetCurrentValue(Window.TopmostProperty, false)), System.Windows.Threading.DispatcherPriority.ApplicationIdle, null);
            }
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

        private void Open_Click(object sender, EventArgs e)
        {
            if (MainWindow == null)
            {
                MessageBox.Show("Error", "Error creating the MainWindow for the Application Modules in Applivery.MarvelComics.Desktop.");
            }
            else
            {
                LoadPlugins();
                ((IMainFrameViewModel)MainWindow.DataContext).DisplaySelectedPlugin(((IMainFrameViewModel)MainWindow.DataContext).Plugins.FirstOrDefault()?.Id);
                MainWindow.Show();
            }
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Current.Shutdown();
        }

        private void LoadPlugins()
        {
            var pluginItems = serviceProvider.GetServices<IPluginItemBase>();

            foreach (var item in pluginItems)
            {
                mainViewModel.LoadPlugin(item.Id, item.Name, item.Command);
            }
        }

        public virtual void RegisterDependencies(IServiceCollection services, IEnumerable<Assembly> assembliesToLoad)
        {
            LoadType(assembliesToLoad, typeof(IPluginItemBase), services);
            LoadType(assembliesToLoad, typeof(IPageManager), services);
            LoadType(assembliesToLoad, typeof(IVVMMappingBase), services);
            LoadType(assembliesToLoad, typeof(IMainFrame), services);
            LoadType(assembliesToLoad, typeof(IMainFrame), services);
        }

        public void RegisterMainFrame()
        {
            if (MainWindow == null && serviceProvider.GetRequiredService<IMainFrame>() is Window window)
            {
                MainWindow = window;
            }
        }

        public virtual void RegisterCustomMainFrame()
        {

        }

        public virtual void RegisterMainWindowViewModel(IServiceCollection services)
        {
            services.AddSingleton<IMainFrameViewModel, MainFrameViewModel>();
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
                    services.AddSingleton(myType, typeInstance);
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