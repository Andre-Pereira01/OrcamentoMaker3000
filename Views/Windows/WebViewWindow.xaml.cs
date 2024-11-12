using OrcamentoMaker3000.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Controls;
using Microsoft.Web.WebView2;

namespace OrcamentoMaker3000.Views.Windows
{
    public partial class WebViewWindow : Window
    {
        public WebViewWindow()
        {
            InitializeComponent();
            InitializeWebView();
        }
        private async void InitializeWebView()
        {
            // Inicialize o WebView2
            await webView.EnsureCoreWebView2Async(null);
            webView.Source = new Uri("https://www.viamichelin.pt/itinerarios");
        }

        // Evento de fechamento da janela
        private void Window_Closed(object sender, EventArgs e)
        {
            // Exibir o popup para inserir o valor
            var input = Microsoft.VisualBasic.Interaction.InputBox(
                "Insira o valor observado na página da ViaMichelin:",
                "Valor de Viagem",
                "0");

            if (double.TryParse(input, out double travelCost))
            {
                // Armazenar o valor no modelo ou realizar cálculos
                OrcamentoModel.Instance.TravelExpenses = travelCost * 4; // Multiplicação por 4 (ida e volta para dois carros)
                System.Windows.MessageBox.Show($"Despesas de viagem calculadas: {OrcamentoModel.Instance.TravelExpenses} €", "Cálculo Concluído");
            }
            else
            {
                System.Windows.MessageBox.Show("Valor inválido! Tente novamente.", "Erro", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

  
    }
}
