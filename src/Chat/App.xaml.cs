using System.Windows;
using Chat.Core;

namespace Chat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Custom startup, so we load our IoC immediately before anything else
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Let the base application do what it needs
            base.OnStartup(e);

            // Setup the main application
            ApplicationSetup();

            // Log it
            IoC.Logger.Log("Application starting...", LogLevel.Informative);

            // Show the main window
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }

        /// <summary>
        /// Configures our application ready for use
        /// </summary>
        private void ApplicationSetup()
        {
            // Setup IoC
            IoC.Setup();

            // Bind a logger
            IoC.Kernel.Bind<ILogFactory>().ToConstant(new BaseLogFactory(new[]
            {
                // TODO: Add ApplicationSettings, so we can set/edit a log location
                //       For now just log to the path where this application is running
                new FileLogger("log.txt")
            }));

            // Bind a task manager
            IoC.Kernel.Bind<ITaskManager>().ToConstant(new TaskManager());

            // Bind a file manager
            IoC.Kernel.Bind<IFileManager>().ToConstant(new FileManager());

            // Bind a UI Manager
            IoC.Kernel.Bind<IUIManager>().ToConstant(new UIManager());
        }
    }
}
