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
using Microsoft.Office.Interop.Word;
using Newtonsoft.Json;



namespace OrcamentoMaker3000.Views.Pages
{
    /// <summary>
    /// Interaction logic for Orcamento.xaml
    /// </summary>
    public partial class Orcamento : System.Windows.Controls.Page
    {
        public Orcamento()
        {
            InitializeComponent();
            // Preencher o comboBox de Duração com as opções
            //DuraçãoCombo.ItemsSource = new int[] { 15, 30, 45, 60, 90 };

            LoadConfig();
        }
        
        private void LoadConfig()
        {
            // Definir o caminho do arquivo config.json
            string configFilePath = System.IO.Path.Combine(OrcamentoModel.Instance.SavePath, "config.json");

            if (File.Exists(configFilePath))
            {
                // Ler o JSON do arquivo config.json
                string json = File.ReadAllText(configFilePath);

                // Deserializar o JSON para o objeto Config
                var config = JsonConvert.DeserializeObject<Config>(json);

                // Inicializar as variáveis do modelo com os valores carregados
                OrcamentoModel.Instance.ValuePerMusician = config.ValuePerMusician ?? new Dictionary<int, double>();
                OrcamentoModel.Instance.ExtraSalary = config.ExtraSalary ?? new Dictionary<int, double>();
                OrcamentoModel.Instance.KilometerPrice = config.KilometerPrice;
                OrcamentoModel.Instance.GroupSavings = config.GroupSavings;
                OrcamentoModel.Instance.ManagerSalary = config.ManagerSalary;
            }
            else
            {
                MessageBox.Show("Ficheiro de configuração não encontrado.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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
            //OrcamentoModel.Instance.Duration = int.Parse(DuraçãoCombo.SelectedValue.ToString());
            OrcamentoModel.Instance.CreationDate = DateTime.Now;
            OrcamentoModel.Instance.Distance = double.Parse(DistanciaText.Text);

            OrcamentoModel.Instance.SelectedDurations.Clear();
            // Verificar quais durações foram selecionadas
            if (Duration15CheckBox.IsChecked == true) OrcamentoModel.Instance.SelectedDurations.Add(15);
            if (Duration30CheckBox.IsChecked == true) OrcamentoModel.Instance.SelectedDurations.Add(30);
            if (Duration45CheckBox.IsChecked == true) OrcamentoModel.Instance.SelectedDurations.Add(45);
            if (Duration60CheckBox.IsChecked == true) OrcamentoModel.Instance.SelectedDurations.Add(60);
            if (Duration90CheckBox.IsChecked == true) OrcamentoModel.Instance.SelectedDurations.Add(90);
           
            if (!OrcamentoModel.Instance.SelectedDurations.Any())
            {
                MessageBox.Show("Por favor, selecione pelo menos uma duração para o orçamento.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //// Verificar se há durações selecionadas


            //// Cálculo do valor base por músico
            //double baseValue = OrcamentoModel.Instance.ValuePerMusician[OrcamentoModel.Instance.Duration];
            //double totalBaseValue = baseValue * OrcamentoModel.Instance.NumberMusicians;
            //OrcamentoModel.Instance.TotalBaseValue = totalBaseValue;

            //// Calcular despesas de viagem
            //double travelExpenses = OrcamentoModel.Instance.KilometerPrice * OrcamentoModel.Instance.Distance;
            //OrcamentoModel.Instance.TravelExpenses = travelExpenses;

            //// Calcular salário extra baseado na distância
            //double extraSalary = CalcularExtraSalary(OrcamentoModel.Instance.Distance);

            //// Calcular a poupança do grupo e o salário do manager
            //double groupSavings = totalBaseValue * OrcamentoModel.Instance.GroupSavings;
            //double managerSalary = totalBaseValue * OrcamentoModel.Instance.ManagerSalary;


            //// Cotação final
            //double cotation = totalBaseValue + extraSalary + travelExpenses + groupSavings + managerSalary + OrcamentoModel.Instance.AlimentationExpenses + OrcamentoModel.Instance.ExtraExpenses;
            //// Arredondar para o valor mais próximo de 50 ou 00
            //OrcamentoModel.Instance.Cotation = Math.Ceiling(cotation / 50) * 50;

            AtualizarNumberOrc();

            PreencherDocumentoWord();
            //MessageBox.Show($"Orçamento gerado com sucesso!\nCotação: {OrcamentoModel.Instance.Cotation:C2}", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void AtualizarNumberOrc()
        {
            // Inicializar o número do orçamento com 20
            int numberOrc = 20;

            // Definir o diretório onde os orçamentos estão salvos
            string directoryPath = _savePath;

            if (Directory.Exists(directoryPath))
            {
                // Obter todos os arquivos do diretório, excluindo "modelo.docx" e "config.json"
                var files = Directory.GetFiles(directoryPath, "*.docx")
                                     .Where(f => !f.EndsWith("modelo.docx") && !f.EndsWith("config.json"));

                // Procurar o maior número de orçamento nos arquivos
                foreach (var file in files)
                {
                    // Extrair o número do arquivo
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(file);
                    string[] parts = fileName.Split('_');

                    if (parts.Length > 1 && int.TryParse(parts[1], out int currentNumber))
                    {
                        // Atualizar se o número atual for maior
                        if (currentNumber > numberOrc)
                        {
                            numberOrc = currentNumber;
                        }
                    }
                }
            }

            // Incrementar o número do orçamento
            OrcamentoModel.Instance.NumberOrc = numberOrc + 1;
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
                    if (text.Text.Contains("neneim")) text.Text = text.Text.Replace("neneim", "0" + OrcamentoModel.Instance.NumberOrc.ToString());
                    if(text.Text.Contains("yearVar")) text.Text = text.Text.Replace("yearVar", DateTime.Now.Year.ToString());
                    if (text.Text.Contains("clientName")) text.Text = text.Text.Replace("clientName", OrcamentoModel.Instance.ClientName);
                    if (text.Text.Contains("clientPhone")) text.Text = text.Text.Replace("clientPhone", OrcamentoModel.Instance.ClientPhone);
                    if (text.Text.Contains("clientEmail")) text.Text = text.Text.Replace("clientEmail", OrcamentoModel.Instance.ClientEmail);
                    if (text.Text.Contains("thisVar")) text.Text = text.Text.Replace("thisVar", OrcamentoModel.Instance.ServiceType);
                    if (text.Text.Contains("date"))
                    {
                        string formattedDate1 = OrcamentoModel.Instance.Date.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("pt-PT"));
                        text.Text = text.Text.Replace("date", formattedDate1);
                    }
                    if (text.Text.Contains("schedule")) text.Text = text.Text.Replace("schedule", OrcamentoModel.Instance.Schedule);
                    if (text.Text.Contains("location")) text.Text = text.Text.Replace("location", OrcamentoModel.Instance.Location);
                    if (text.Text.Contains("creationDate"))
                    {
                        string formattedDate2 = OrcamentoModel.Instance.CreationDate.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("pt-PT"));
                        text.Text = text.Text.Replace("creationDate", formattedDate2);
                    }
                    //if (text.Text.Contains("duration1")) text.Text = text.Text.Replace("duration1", OrcamentoModel.Instance.Duration.ToString() + "min");
                    //if (text.Text.Contains("cotation1")) text.Text = text.Text.Replace("cotation1", OrcamentoModel.Instance.Cotation.ToString("C2"));
                }

               
                DocumentFormat.OpenXml.Wordprocessing.Table table = body.Elements<DocumentFormat.OpenXml.Wordprocessing.Table>().FirstOrDefault();

                if (table != null)
                {
                    // Clonar a primeira linha como modelo (se existir)
                    DocumentFormat.OpenXml.Wordprocessing.TableRow templateRow = table.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().FirstOrDefault();

                    // Para cada duração selecionada, adicionar uma nova linha na tabela
                    foreach (int duration in OrcamentoModel.Instance.SelectedDurations)
                    {
                        // Calcular a cotação para a duração selecionada
                        double cotation = CalcularCotationParaDuracao(duration);

                        // Clonar a linha modelo, se houver
                        DocumentFormat.OpenXml.Wordprocessing.TableRow newRow = (templateRow != null)
                            ? (DocumentFormat.OpenXml.Wordprocessing.TableRow)templateRow.CloneNode(true)
                            : new DocumentFormat.OpenXml.Wordprocessing.TableRow(); // Caso não tenha uma linha para clonar, criar uma nova linha

                        // Remover o negrito de todas as células na linha clonada
                        foreach (var cell in newRow.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>())
                        {
                            foreach (var run in cell.Descendants<DocumentFormat.OpenXml.Wordprocessing.Run>())
                            {
                                // Remover o Bold (negrito)
                                var runProperties = run.GetFirstChild<DocumentFormat.OpenXml.Wordprocessing.RunProperties>();
                                if (runProperties != null)
                                {
                                    var boldElement = runProperties.GetFirstChild<DocumentFormat.OpenXml.Wordprocessing.Bold>();
                                    if (boldElement != null)
                                    {
                                        boldElement.Remove(); // Remover o elemento <Bold> para desfazer o negrito
                                    }
                                }
                            }
                        }

                        // Substituir o texto na célula de duração
                        var durationCell = newRow.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0);
                        var durationText = durationCell.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>().FirstOrDefault();
                        if (durationText != null)
                        {
                            durationText.Text = duration.ToString() + " min";
                        }

                        // Substituir o texto na célula de cotação
                        var cotationCell = newRow.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1);
                        var cotationText = cotationCell.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>().FirstOrDefault();
                        if (cotationText != null)
                        {
                            cotationText.Text = cotation.ToString("C2");
                        }

                        // Adicionar a nova linha à tabela
                        table.Append(newRow);
                    }
                }


                wordDoc.MainDocumentPart.Document.Save(); // Salvar o documento
            }

            MessageBox.Show($"Orçamento criado na pasta: {outputPath}", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            
            //ConverterParaPdf(outputPath);
        }

        private double CalcularCotationParaDuracao(int duration)
        {
            // Obter o valor base por músico para a duração específica
            double baseValue = OrcamentoModel.Instance.ValuePerMusician[duration];

            // Calcular o valor total base para o número de músicos
            double totalBaseValue = baseValue * OrcamentoModel.Instance.NumberMusicians;

            // Calcular despesas de viagem
            double travelExpenses = OrcamentoModel.Instance.KilometerPrice * OrcamentoModel.Instance.Distance;

            // Calcular o salário extra baseado na distância
            double extraSalary = CalcularExtraSalary(OrcamentoModel.Instance.Distance);

            // Calcular poupança do grupo e salário do manager
            double groupSavings = totalBaseValue * OrcamentoModel.Instance.GroupSavings;
            double managerSalary = totalBaseValue * OrcamentoModel.Instance.ManagerSalary;

            // Calcular a cotação final
            double cotation = totalBaseValue + extraSalary + travelExpenses + groupSavings + managerSalary + OrcamentoModel.Instance.AlimentationExpenses + OrcamentoModel.Instance.ExtraExpenses;

            // Arredondar para o valor mais próximo de 50 ou 00
            return Math.Ceiling(cotation / 50) * 50;
        }

        private void ConverterParaPdf(string docPath)
        {
            // Usar namespace explícito para o Application do Word Interop
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document wordDoc = null;

            try
            {
                // Usar System.IO.Path para manipular o caminho
                string pdfPath = System.IO.Path.ChangeExtension(docPath, ".pdf"); // Altera a extensão para PDF

                // Abrir o documento Word
                wordDoc = wordApp.Documents.Open(docPath);
                wordDoc.ExportAsFixedFormat(pdfPath, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);

                MessageBox.Show($"Orçamento em PDF salvo em: {pdfPath}", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao converter para PDF: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Fechar o documento e o aplicativo Word
                wordDoc?.Close();
                wordApp.Quit();
            }
        }


    }

}
