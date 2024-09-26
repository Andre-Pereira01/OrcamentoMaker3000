using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace OrcamentoMaker3000.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "Orcamento Maker 3000";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {

            new NavigationViewItem()
            {
                Content = "Orçamento",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Document24},
                TargetPageType = typeof(Views.Pages.Orcamento)
            }

        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Definições",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.Definicoes)
            }
        };

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };
    }
}
