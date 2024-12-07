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
                TargetPageType = typeof(Views.Pages.Orcamento),
                LayoutTransform = new System.Windows.Media.ScaleTransform(1.5, 1.5) 
            },
             new NavigationViewItem()
            {
                Content = "Definições",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.Definicoes),
                LayoutTransform = new System.Windows.Media.ScaleTransform(1.5, 1.5)
            }

        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Acerca",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Info24 },
                TargetPageType = typeof(Views.Pages.Acerca),
                LayoutTransform = new System.Windows.Media.ScaleTransform(1.5, 1.5)
            }
        };

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };
    }
}
