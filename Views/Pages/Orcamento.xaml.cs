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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Words.NET;
using System.IO;
using Ookii.Dialogs.Wpf;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using OrcamentoMaker3000.Models;



namespace OrcamentoMaker3000.Views.Pages
{
    /// <summary>
    /// Interaction logic for Orcamento.xaml
    /// </summary>
    public partial class Orcamento : Page
    {
        public Orcamento()
        {
            InitializeComponent();
            // Preencher o comboBox de Duração com as opções
            DuraçãoCombo.ItemsSource = new int[] { 15, 30, 45, 60, 90 };

            // Definir os valores do dicionário de salários por duração
            OrcamentoModel.Instance.ValuePerMusician = new System.Collections.Generic.Dictionary<int, double>
            {
                { 15, 25.00 },
                { 30, 40.00 },
                { 45, 60.00 },
                { 60, 75.00 },
                { 90, 90.00 }
            };

            OrcamentoModel.Instance.ExtraSalary = new System.Collections.Generic.Dictionary<int, double>
            {
                { 30, 15.00 },
                { 100, 20.00 },
                { 200, 30.00 },
                { 999, 40.00 }
            };
        }

        // Função que processa o orçamento e gera o resultado
        private void GerarOrcamento(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ClientNameTextBox.Text))
            {
                MessageBox.Show("Por favor, preencha o nome do cliente.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(ServicoText.Text))
            {
                MessageBox.Show("Por favor, preencha o tipo de serviço.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(LocalTextBox.Text))
            {
                MessageBox.Show("Por favor, preencha o local do evento.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (DatePicker.SelectedDate == null)
            {
                MessageBox.Show("Por favor, selecione a data do evento.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(DistanciaText.Text))
            {
                MessageBox.Show("Por favor, preencha a distância do evento.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (string.IsNullOrWhiteSpace(HorarioBox.Text))
            {
               HorarioBox.Text = "A definir";
            }
            if(string.IsNullOrWhiteSpace(ClientPhoneTextBox.Text))
            {
                ClientPhoneTextBox.Text = "N/A";
            }
            if (string.IsNullOrWhiteSpace(ClientEmailTextBox.Text))
            {
                ClientEmailTextBox.Text = "N/A";
            }
            // Preencher os valores no modelo
            OrcamentoModel.Instance.ClientName = ClientNameTextBox.Text;
            OrcamentoModel.Instance.ClientPhone = ClientPhoneTextBox.Text;
            OrcamentoModel.Instance.ClientEmail = ClientEmailTextBox.Text;
            OrcamentoModel.Instance.ServiceType = ServicoText.Text;
            OrcamentoModel.Instance.Date = DatePicker.SelectedDate ?? DateTime.Now;
            OrcamentoModel.Instance.Schedule = HorarioBox.Text;
            OrcamentoModel.Instance.Location = LocalTextBox.Text;
            OrcamentoModel.Instance.Duration = int.Parse(DuraçãoCombo.SelectedValue.ToString());
            OrcamentoModel.Instance.CreationDate = DateTime.Now;
            OrcamentoModel.Instance.Distance = double.Parse(DistanciaText.Text);


            // Cálculo do valor base por músico
            double baseValue = OrcamentoModel.Instance.ValuePerMusician[OrcamentoModel.Instance.Duration];
            double totalBaseValue = baseValue * OrcamentoModel.Instance.NumberMusicians;
            OrcamentoModel.Instance.TotalBaseValue = totalBaseValue;

            // Calcular despesas de viagem
            double travelExpenses = OrcamentoModel.Instance.KilometerPrice * OrcamentoModel.Instance.Distance;
            OrcamentoModel.Instance.TravelExpenses = travelExpenses;

            // Calcular salário extra baseado na distância
            double extraSalary = CalcularExtraSalary(OrcamentoModel.Instance.Distance);

            // Calcular a poupança do grupo e o salário do manager
            double groupSavings = totalBaseValue * OrcamentoModel.Instance.GroupSavings;
            double managerSalary = totalBaseValue * OrcamentoModel.Instance.ManagerSalary;


            // Cotação final
            double cotation = totalBaseValue + extraSalary + travelExpenses + groupSavings + managerSalary + OrcamentoModel.Instance.AlimentationExpenses + OrcamentoModel.Instance.ExtraExpenses;
            // Arredondar para o valor mais próximo de 50 ou 00
            OrcamentoModel.Instance.Cotation = Math.Ceiling(cotation / 50) * 50;

            PreencherDocumentoWord();
            //MessageBox.Show($"Orçamento gerado com sucesso!\nCotação: {OrcamentoModel.Instance.Cotation:C2}", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Função para calcular o salário extra com base na distância
        private double CalcularExtraSalary(double distance)
        {
            //if (distance < 30)
            //    return 10;
            //else if (distance < 100)
            //    return 15;
            //else if (distance < 150)
            //    return 20;
            //else
            //    return 30;

            // Encontrar a faixa de distância correta no dicionário
            foreach (var range in OrcamentoModel.Instance.ExtraSalary.Keys.OrderBy(k => k))
            {
                if (distance <= range)
                {
                    return OrcamentoModel.Instance.ExtraSalary[range];
                }
            }

            // Caso a distância seja maior que 150 km, retorna o valor de 999
            return OrcamentoModel.Instance.ExtraSalary[999];
        }

        private string _savePath = OrcamentoModel.Instance.SavePath;

        private void Escolher_Diretorio(object sender, RoutedEventArgs e)
        {
            var dialog = new VistaFolderBrowserDialog
            {
                Description = "Selecione a pasta onde os orçamentos serão salvos",
                UseDescriptionForTitle = true, // Mostra o título corretamente
                SelectedPath = _savePath
            };

            if (dialog.ShowDialog() == true)
            {
                _savePath = dialog.SelectedPath; // Armazenar o caminho da pasta selecionada
                MessageBox.Show($"Pasta selecionada: {_savePath}", "Diretório Escolhido", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void PreencherDocumentoWord()
        {
            string templatePath = System.IO.Path.Combine(OrcamentoModel.Instance.SavePath, "modelo.docx");
            string outputPath = System.IO.Path.Combine(_savePath, $"Orcamento_{OrcamentoModel.Instance.NumberOrc}_{OrcamentoModel.Instance.ClientName}.docx");

            File.Copy(templatePath, outputPath, true); // Copiar o arquivo de modelo para o local de saída

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(outputPath, true))
            {
                var body = wordDoc.MainDocumentPart.Document.Body;
                foreach (var text in body.Descendants<Text>())
                {
                    // Substituir placeholders pelos valores reais
                    if (text.Text.Contains("neneim")) text.Text = text.Text.Replace("neneim", OrcamentoModel.Instance.NumberOrc.ToString());
                    if(text.Text.Contains("yearVar")) text.Text = text.Text.Replace("yearVar", DateTime.Now.Year.ToString());
                    if (text.Text.Contains("clientName")) text.Text = text.Text.Replace("clientName", OrcamentoModel.Instance.ClientName);
                    if (text.Text.Contains("clientPhone")) text.Text = text.Text.Replace("clientPhone", OrcamentoModel.Instance.ClientPhone);
                    if (text.Text.Contains("clientEmail")) text.Text = text.Text.Replace("clientEmail", OrcamentoModel.Instance.ClientEmail);
                    if (text.Text.Contains("thisVar")) text.Text = text.Text.Replace("thisVar", OrcamentoModel.Instance.ServiceType);
                    if (text.Text.Contains("date")) text.Text = text.Text.Replace("date", OrcamentoModel.Instance.Date.ToString("dd/MM/yyyy"));
                    if (text.Text.Contains("schedule")) text.Text = text.Text.Replace("schedule", OrcamentoModel.Instance.Schedule);
                    if (text.Text.Contains("location")) text.Text = text.Text.Replace("location", OrcamentoModel.Instance.Location);
                    //if (text.Text.Contains("creationDate")) text.Text = text.Text.Replace("creationDate", OrcamentoModel.Instance.CreationDate.ToString("dd/MM/yyyy"));
                    if (text.Text.Contains("creationDate"))
                    {
                        // Formatar a data como "dd de [Mês] de yyyy"
                        string formattedDate = OrcamentoModel.Instance.CreationDate.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("pt-PT"));
                        text.Text = text.Text.Replace("creationDate", formattedDate);
                    }
                    if (text.Text.Contains("duration1")) text.Text = text.Text.Replace("duration1", OrcamentoModel.Instance.Duration.ToString() + "min");
                    if (text.Text.Contains("cotation1")) text.Text = text.Text.Replace("cotation1", OrcamentoModel.Instance.Cotation.ToString("C2"));
                }

                wordDoc.MainDocumentPart.Document.Save(); // Salvar o documento
            }

            MessageBox.Show($"Orçamento salvo em: {outputPath}", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
        }



    }

}
