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
using Microsoft.Web.WebView2.Core;

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
            webView.CoreWebView2.Settings.AreDevToolsEnabled = true;

            //AutomateViaMichelin();
     
        }
        private async void AutomateViaMichelin()
        {
            try
            {
                await Task.Delay(2500); // Aguarde o carregamento inicial da página
                string location = OrcamentoModel.Instance.Location;

                // Script para clicar no botão de aceitar cookies, se necessário
                string acceptCookiesScript = @"
        (function() {
            var acceptButton = document.querySelector('.didomi-continue-without-agreeing');
            if (acceptButton) {
                acceptButton.click();
                console.log('Botão de cookies clicado');
            }
        })();";
                await webView.CoreWebView2.ExecuteScriptAsync(acceptCookiesScript);
                await Task.Delay(2000); // Aguarde o ajuste da página

                // Script para preencher e clicar no campo 'departure'
                string fillDepartureScript = @"
(function() {
    var departureField = document.getElementById('departure');
    if (departureField) {
        departureField.value = 'Monção 4950';
        departureField.dispatchEvent(new Event('input', { bubbles: true }));
        departureField.dispatchEvent(new Event('focus', { bubbles: true })); // Força o foco
        departureField.click(); // Simula o clique no campo
        setTimeout(() => { departureField.dispatchEvent(new Event('blur', { bubbles: true })); }, 1000); // Simula sair do campo para acionar sugestões
    }
})();
";

                await webView.CoreWebView2.ExecuteScriptAsync(fillDepartureScript);
                await Task.Delay(4000); // Aguarde para permitir que sugestões sejam carregadas

                // Script para selecionar a primeira sugestão do campo 'departure'
                string selectDepartureSuggestionScript = @"
        (function() {
            var departureSuggestion = document.querySelector('ul[role=listbox] li:first-child');
            if (departureSuggestion) {
                departureSuggestion.click();
                console.log('Sugestão de Partida selecionada');
            }
        })();";
                await webView.CoreWebView2.ExecuteScriptAsync(selectDepartureSuggestionScript);
                await Task.Delay(3000); // Aguarde o ajuste do campo após selecionar a sugestão

                // Script para preencher e clicar no campo 'arrival'
                string fillArrivalScript = $@"
        (function() {{
            var arrivalField = document.getElementById('arrival');
            if (arrivalField) {{
                arrivalField.value = '{location}';
                arrivalField.dispatchEvent(new Event('input', {{ bubbles: true }}));
                arrivalField.dispatchEvent(new Event('change', {{ bubbles: true }}));
                arrivalField.click(); // Simular o clique para abrir sugestões
                console.log('Campo Chegada preenchido e clicado');
            }}
        }})();";
                await webView.CoreWebView2.ExecuteScriptAsync(fillArrivalScript);
                await Task.Delay(4000); // Aguarde para permitir que sugestões sejam carregadas

                // Script para selecionar a primeira sugestão do campo 'arrival'
                string selectArrivalSuggestionScript = @"
        (function() {
            var arrivalSuggestion = document.querySelector('ul[role=listbox] li:first-child');
            if (arrivalSuggestion) {
                arrivalSuggestion.click();
                console.log('Sugestão de Chegada selecionada');
            }
        })();";
                await webView.CoreWebView2.ExecuteScriptAsync(selectArrivalSuggestionScript);
                await Task.Delay(3000); // Aguarde o ajuste do campo após selecionar a sugestão

                // Script para clicar no botão "Procurar um itinerário"
                string searchItineraryScript = @"
        (function() {
            var searchButton = document.querySelector('button span');
            if (searchButton) {
                searchButton.click();
                console.log('Botão Procurar um itinerário clicado');
            }
        })();";
                await webView.CoreWebView2.ExecuteScriptAsync(searchItineraryScript);
                await Task.Delay(5000); // Aguarde os resultados carregarem

                // Script para capturar os resultados
                string extractResultsScript = @"
        (function() {
            var resultContainer = document.querySelector('div.flex.gap-2.w-full');
            if (resultContainer) {
                console.log('Resultados encontrados');
                return resultContainer.innerText;
            }
            return 'RESULTADOS NÃO ENCONTRADOS';
        })();";
                string results = await webView.CoreWebView2.ExecuteScriptAsync(extractResultsScript);

                // Mostrar os resultados extraídos
                System.Windows.MessageBox.Show($"Resultados extraídos: {results}");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erro ao automatizar: {ex.Message}");
            }
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
