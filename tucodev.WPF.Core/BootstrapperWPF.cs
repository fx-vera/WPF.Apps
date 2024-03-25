using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows;
using tucodev.WPF.Core.Interfaces.Interfaces;
using tucodev.WPF.Core.Interfaces.Managers;
using tucodev.WPF.Core.Interfaces.Models;
using MessageBox = System.Windows.MessageBox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Text;
using tucodev.WPF.Core.Mainframe;

namespace tucodev.WPF.Core
{

    /// <summary>
    /// Base class to manage the basic application operations and the main window.
    /// This class can be extended depending on requirements
    /// </summary>
    public abstract class BootstrapperWPF : System.Windows.Application, IBootstrapperBase
    {
        NotifyIcon notifyIcon;

        /// <summary>
        /// Contructor called by Application.
        /// </summary>
        public BootstrapperWPF()
        {
            UnhandledExceptionHandler();
        }

        public abstract List<string> DllAllowed { get; }

        protected abstract bool IsNotifiyIconMode { get; }
        protected virtual string NotifyIconTitle { get; }
        protected virtual Icon NotifyIconIcon { get; }

        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        public void OnStartup()
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            //Load application modules
            List<Assembly> assembliesToCompose = GetAssembliesForIoC();

            assembliesToCompose.Add(GetType().Assembly);

            Dictionary<Type, object> knownInstances = new Dictionary<Type, object>
            {
                { GetType(), this }
            };

            string loadError = string.Empty;
            bool modulesLoadedOk = false;

            try
            {
                var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory());
               //.AddJsonFile("appsettings.json", false, true);

                Configuration = builder.Build();

                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                ServiceProvider = serviceCollection.BuildServiceProvider();
                //MEFIoCManager.Instance.LoadModules(knownInstances, assembliesToCompose);
                modulesLoadedOk = true;
            }
            catch (Exception ex)
            {
                loadError = ex.Message;
            }

            if (!modulesLoadedOk)
            {
                System.Windows.MessageBox.Show(loadError, "Error Loading Application Modules in Applivery.MarvelComics.Desktop.");
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

        private void ConfigureServices(ServiceCollection serviceCollection)
        {
            //serviceCollection.RegisterInfrastructureDependencies(Configuration);
            //serviceCollection.RegisterLogicDependencies();
            serviceCollection.AddTransient(typeof(MainWindow));
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
         /*   IMainWindowViewModel mainViewModel = IoC.Get<IMainWindowViewModel>();
            if (mainViewModel == null)
            {
                MessageBox.Show("Could not load main window component. Closing down.", "Error");
                return mainWindow;
            }
            IMainWindow mainw = IoC.Get<IMainWindow>();
            if (mainw is Window)
            {
                mainw.SetDataContext(mainViewModel);
                mainWindow = (Window)mainw;
                mainWindow.Dispatcher.BeginInvoke(new Action(() => mainWindow.SetCurrentValue(Window.TopmostProperty, false)), System.Windows.Threading.DispatcherPriority.ApplicationIdle, null);
            }
            */
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
            //IoC.Get<IViewsManager>()?.LoadAvailableViews();
            //MEFIoCManager.Instance.LoadModules(new Dictionary<Type, object>(), GetAssembliesForIoC());
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
           /* IMainWindowViewModel mainViewModel = IoC.Get<IMainWindowViewModel>();
            LoadPluginEventArgs args;
            var pluginItems = IoC.GetAll<IPluginItem>();

            foreach (var item in pluginItems)
            {
                args = new LoadPluginEventArgs
                {
                    Id = item.Id,
                    Item = item,
                    IsCommand = true
                };
                mainViewModel.SetPlugins(args);
            }*/
        }

        #endregion Private
    }
}
