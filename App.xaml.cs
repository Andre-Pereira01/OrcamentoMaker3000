using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrcamentoMaker3000.Views.Pages;
using System.IO;
using System.Reflection;
using System.Windows.Threading;
using OrcamentoMaker3000.Services;
using OrcamentoMaker3000.ViewModels.Pages;
using OrcamentoMaker3000.ViewModels.Windows;
using OrcamentoMaker3000.Views.Windows;
using Wpf.Ui;

namespace OrcamentoMaker3000
{
    /// Interaction logic for App.xaml
    public partial class App
    {
        // The.NET Generic Host provides dependency injection, configuration, logging, and other services.
        // https://docs.microsoft.com/dotnet/core/extensions/generic-host
        // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
        // https://docs.microsoft.com/dotnet/core/extensions/configuration
        // https://docs.microsoft.com/dotnet/core/extensions/logging
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)); })
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<ApplicationHostService>();

                // Page resolver service
                services.AddSingleton<IPageService, PageService>();

                // Theme manipulation
                services.AddSingleton<IThemeService, ThemeService>();

                // TaskBar manipulation
                services.AddSingleton<ITaskBarService, TaskBarService>();

                // Service containing navigation, same as INavigationWindow... but without window
                services.AddSingleton<INavigationService, NavigationService>();

                // Main window with navigation
                services.AddSingleton<INavigationWindow, MainWindow>();
                services.AddSingleton<MainWindowViewModel>();

                services.AddSingleton<Orcamento>();
                services.AddSingleton<Definicoes>();

                services.AddSingleton<SettingsViewModel>();

                services.AddSingleton<Acerca>();

                services.AddSingleton<WebViewWindow>();


            }).Build();

        /// Gets registered service.
        /// <typeparam name="T">Type of the service to get.</typeparam>
        /// <returns>Instance of the service or <see langword="null"/>.</returns>
        public static T GetService<T>()
            where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        /// Occurs when the application is loading.
        private void OnStartup(object sender, StartupEventArgs e)
        {

            string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Monção Brass", "Orçamentos Automatizados");
            string logsPath = Path.Combine(basePath, "logs");
            string configFilePath = Path.Combine(basePath, "config.json");
            string templateFilePath = Path.Combine(basePath, "modelo.docx");

            try
            {
                // Criar diretório base e logs
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);
                if (!Directory.Exists(logsPath))
                    Directory.CreateDirectory(logsPath);

                // Criar ou copiar arquivos
                if (!File.Exists(configFilePath))
                {
                    string sourceConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "config.json");
                    File.Copy(sourceConfig, configFilePath);
                }

                if (!File.Exists(templateFilePath))
                {
                    // Copie o modelo da pasta de instalação para o destino
                    string sourceTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "modelo.docx");
                    File.Copy(sourceTemplate, templateFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao inicializar diretórios e arquivos: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown(); // Encerrar a aplicação
            }

            _host.Start();

        }

        /// Occurs when the application is closing.
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        /// Occurs when an exception is thrown by an application but not handled.
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }
    }
}
