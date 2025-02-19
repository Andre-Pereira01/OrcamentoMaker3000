﻿using DocumentFormat.OpenXml.Packaging;
using Newtonsoft.Json;
using Ookii.Dialogs.Wpf;
using OrcamentoMaker3000.Models;
using OrcamentoMaker3000.Views.Windows;
using Spire.Doc;
using System.IO;
using System.Windows.Controls;

namespace OrcamentoMaker3000.Views.Pages
{
    public partial class Orcamento : System.Windows.Controls.Page
    {
        public Orcamento()
        {
            InitializeComponent();

            LoadConfig();
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Lista para guardar as durações selecionadas
            var selectedDurations = new List<string>();

            // Verifica cada CheckBox e adiciona as durações selecionadas
            if (Duration15CheckBox.IsChecked == true) selectedDurations.Add("15");
            if (Duration30CheckBox.IsChecked == true) selectedDurations.Add("30");
            if (Duration45CheckBox.IsChecked == true) selectedDurations.Add("45");
            if (Duration60CheckBox.IsChecked == true) selectedDurations.Add("60");
            if (Duration90CheckBox.IsChecked == true) selectedDurations.Add("90");

            // Atualiza o texto do ComboBox principal com as durações selecionadas
            DurationComboBox.Text = selectedDurations.Count > 0
                ? string.Join(", ", selectedDurations) + " minutos"
                : "";
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
                //OrcamentoModel.Instance.KilometerPrice = config.KilometerPrice;
                OrcamentoModel.Instance.GroupSavings = config.GroupSavings;
                OrcamentoModel.Instance.ManagerSalary = config.ManagerSalary;
            }
            else
            {
                MessageBox.Show("Ficheiro de configuração não encontrado.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
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
            //if (string.IsNullOrWhiteSpace(DistanciaText.Text))
            //{
            //    MessageBox.Show("Por favor, preencha a distância do evento.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

            if (string.IsNullOrWhiteSpace(HorarioBox.Text))
            {
                HorarioBox.Text = "A definir";
            }
            if (string.IsNullOrWhiteSpace(ClientPhoneTextBox.Text))
            {
                ClientPhoneTextBox.Text = "N/A";
            }
            if (string.IsNullOrWhiteSpace(ClientEmailTextBox.Text))
            {
                ClientEmailTextBox.Text = "N/A";
            }

            // Verifica se o diretório existe
            if (!Directory.Exists(OrcamentoModel.Instance.SavePath))
            {
                // Se não existir, cria o diretório
                Directory.CreateDirectory(OrcamentoModel.Instance.SavePath);
            }


            // Preencher os valores no modelo
            OrcamentoModel.Instance.ClientName = ClientNameTextBox.Text;
            OrcamentoModel.Instance.ClientPhone = ClientPhoneTextBox.Text;
            OrcamentoModel.Instance.ClientEmail = ClientEmailTextBox.Text;
            OrcamentoModel.Instance.ServiceType = ServicoText.Text;
            OrcamentoModel.Instance.Date = DatePicker.SelectedDate ?? DateTime.Now;
            OrcamentoModel.Instance.Schedule = HorarioBox.Text;
            OrcamentoModel.Instance.Location = LocalTextBox.Text;
            //calcularDistancia(OrcamentoModel.Instance.Location);
            OrcamentoModel.Instance.CreationDate = DateTime.Now;
            

            OrcamentoModel.Instance.SelectedDurations.Clear();
            // Verificar quais durações foram selecionadas
            if (Duration15CheckBox.IsChecked == true) OrcamentoModel.Instance.SelectedDurations.Add(15);
            if (Duration30CheckBox.IsChecked == true) OrcamentoModel.Instance.SelectedDurations.Add(30);
            if (Duration45CheckBox.IsChecked == true) OrcamentoModel.Instance.SelectedDurations.Add(45);
            if (Duration60CheckBox.IsChecked == true) OrcamentoModel.Instance.SelectedDurations.Add(60);
            if (Duration90CheckBox.IsChecked == true) OrcamentoModel.Instance.SelectedDurations.Add(90);

            //OrcamentoModel.Instance.Duration = int.Parse(DuraçãoCombo.SelectedValue.ToString());
            //OrcamentoModel.Instance.Distance = double.Parse(DistanciaText.Text);

            if (!OrcamentoModel.Instance.SelectedDurations.Any())
            {
                MessageBox.Show("Por favor, selecione pelo menos uma duração para o orçamento.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            AtualizarNumberOrc();
            PreencherDocumentoWord();
        }
        private void AtualizarNumberOrc()
        {
            // Inicializar o número do orçamento com 20

            int orcamentoYear = OrcamentoModel.Instance.Date.Year;
            int numberOrc = 20;

            // Definir o diretório onde os orçamentos estão salvos
            string directoryPath = System.IO.Path.Combine(_savePath, orcamentoYear.ToString());

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Obter todos os arquivos do diretório, excluindo "modelo.docx" e "config.json"
            var files = Directory.GetFiles(directoryPath, "*.docx")/*.Where(f => !f.EndsWith("modelo.docx") && !f.EndsWith("config.json"))*/;

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

            // Incrementar o número do orçamento
            OrcamentoModel.Instance.NumberOrc = numberOrc + 1;;
        }
        private double CalcularExtraSalary(double distance, string outputPath)
        {
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
                Description = "Selecione a pasta onde os orçamentos serão guardados",
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
            string orcamentoYear = OrcamentoModel.Instance.Date.Year.ToString();
            string outputPath = System.IO.Path.Combine(_savePath,orcamentoYear, $"Orcamento_{OrcamentoModel.Instance.NumberOrc}_{OrcamentoModel.Instance.ClientName}.docx");

            File.Copy(templatePath, outputPath, true); // Copiar o arquivo de modelo para o local de saída

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(outputPath, true))
            {
                var body = wordDoc.MainDocumentPart.Document.Body;
                foreach (var text in body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>())/*Text*/
                {
                    // Substituir placeholders pelos valores reais
                    if (text.Text.Contains("neneim")) text.Text = text.Text.Replace("neneim", "0" + OrcamentoModel.Instance.NumberOrc.ToString());
                    if (text.Text.Contains("yearVar")) text.Text = text.Text.Replace("yearVar", DateTime.Now.Year.ToString());
                    if (text.Text.Contains("clientName")) text.Text = text.Text.Replace("clientName", OrcamentoModel.Instance.ClientName);
                    if (text.Text.Contains("clientPhone")) text.Text = text.Text.Replace("clientPhone", OrcamentoModel.Instance.ClientPhone);
                    if (text.Text.Contains("clientEmail")) text.Text = text.Text.Replace("clientEmail", OrcamentoModel.Instance.ClientEmail);
                    if (text.Text.Contains("thisVar")) text.Text = text.Text.Replace("thisVar", OrcamentoModel.Instance.ServiceType);
                    if (text.Text.Contains("date"))
                    {
                        string formattedDate1 = OrcamentoModel.Instance.Date.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("pt-PT"));
                        int startIndex = formattedDate1.IndexOf("de", 3) + 3; // Pega o índice depois do segundo "de"
                        string capitalizedMonthDate1 = formattedDate1.Substring(0, startIndex) + char.ToUpper(formattedDate1[startIndex]) + formattedDate1.Substring(startIndex + 1);

                        text.Text = text.Text.Replace("date", capitalizedMonthDate1);
                    }
                    if (text.Text.Contains("schedule")) text.Text = text.Text.Replace("schedule", OrcamentoModel.Instance.Schedule);
                    if (text.Text.Contains("location")) text.Text = text.Text.Replace("location", OrcamentoModel.Instance.Location);
                    if (text.Text.Contains("creationDate"))
                    {
                        string formattedDate2 = OrcamentoModel.Instance.CreationDate.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("pt-PT"));
                        int startIndex = formattedDate2.IndexOf("de", 3) + 3; // Pega o índice depois do segundo "de"
                        string capitalizedMonthDate2 = formattedDate2.Substring(0, startIndex) + char.ToUpper(formattedDate2[startIndex]) + formattedDate2.Substring(startIndex + 1);

                        text.Text = text.Text.Replace("creationDate", capitalizedMonthDate2);
                    }

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
                        double cotation = CalcularCotationParaDuracao(duration,outputPath);

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

            if (TogglePdfBtn.IsChecked == true)
            {
                ConverterParaPdf(outputPath);
            }
        }
        private double CalcularCotationParaDuracao(int duration, string outputPath)
        {
            // Obter o valor base por músico para a duração específica
            double baseValue = OrcamentoModel.Instance.ValuePerMusician[duration];
            Logger($"{DateTime.Now}:Valor p/ musico para {duration} min: {baseValue}", outputPath);

            // Calcular o valor total base para o número de músicos
            double totalBaseValue = baseValue * OrcamentoModel.Instance.NumberMusicians;
            Logger($"{DateTime.Now}:Valor Base do grupo para {duration} min: {totalBaseValue}", outputPath);

            //// Calcular despesas de viagem
            //double travelExpenses = OrcamentoModel.Instance.KilometerPrice * OrcamentoModel.Instance.Distance;
            double travelExpenses = OrcamentoModel.Instance.TravelExpenses; //*4
            Logger($"{DateTime.Now}:Despesas de Viagem para {OrcamentoModel.Instance.Location} ({OrcamentoModel.Instance.Distance}km) segundo guia Michelin = {travelExpenses/4}€ por carro só ida, o que multiplicado ida e volta x2 carros dá {travelExpenses}€", outputPath);
           
            // Calcular o salário extra baseado na distância
            double extraSalary = CalcularExtraSalary(OrcamentoModel.Instance.Distance, outputPath);
            Logger($"{DateTime.Now}:Salário extra por distância: {extraSalary}",outputPath);


            // Calcular poupança do grupo e salário do manager
            double groupSavings = totalBaseValue * OrcamentoModel.Instance.GroupSavings;
            Logger($"{DateTime.Now}:Percentagem para o grupo: Base({totalBaseValue}) x {OrcamentoModel.Instance.GroupSavings} = {groupSavings}", outputPath);

            double managerSalary = totalBaseValue * OrcamentoModel.Instance.ManagerSalary;
            Logger($"{DateTime.Now}:Percentagem para o manager: Base({totalBaseValue}) x {OrcamentoModel.Instance.ManagerSalary} = {managerSalary}", outputPath);

            // Calcular a cotação final
            double cotation = totalBaseValue + 6*extraSalary + travelExpenses + groupSavings + managerSalary + OrcamentoModel.Instance.AlimentationExpenses + OrcamentoModel.Instance.ExtraExpenses;

            Logger($"{DateTime.Now}:Base: {totalBaseValue} + Extra (dist): {extraSalary} + Transporte: {travelExpenses} + Poupança para o Grupo: {groupSavings} + Manager : {managerSalary} + Alimentacao: {OrcamentoModel.Instance.AlimentationExpenses} + Despesas Extra: {OrcamentoModel.Instance.ExtraExpenses} = {cotation}€", outputPath);
            // Arredondar para o valor mais próximo de 50 ou 00
            //cotation = Math.Ceiling(cotation / 50) * 50;
            cotation = Math.Ceiling(cotation / 50.0) * 50 + 50;
            Logger($"{DateTime.Now}:Arredonda  para: {cotation}€", outputPath);
            Logger(Environment.NewLine,outputPath);

            return cotation;
        }
        private void ConverterParaPdf(string docPath)
        {
            try
            {
                // Cria uma instância do documento
                Document document = new Document();

                // Carrega o ficheiro Word existente
                document.LoadFromFile(docPath);

                // Define o caminho para salvar o PDF
                string pdfPath = System.IO.Path.ChangeExtension(docPath, ".pdf");

                // Converte e salva o documento como PDF
                document.SaveToFile(pdfPath, FileFormat.PDF);

                MessageBox.Show($"Orçamento em PDF guardado em: {pdfPath}", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao converter para PDF: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Logger(string message, string outputPath)
        {
            // Verificar se o diretório de logs existe
            VerificarECriarDiretorioLogs();

            // Definir o caminho do arquivo de log
            string logFilePath = Path.Combine(OrcamentoModel.Instance.SavePath, "logs", $"{Path.GetFileNameWithoutExtension(outputPath)}.txt");

            // Adicionar a mensagem ao log com data e hora
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{message}");
            }
        }
        private void VerificarECriarDiretorioLogs()
        {
            string logPath = Path.Combine(OrcamentoModel.Instance.SavePath, "logs");

            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
        }
        private void CalcViagem(object sender, RoutedEventArgs e)
        {
            // iniciar uma janela WebViewWindow
            OrcamentoModel.Instance.Location = LocalTextBox.Text;
            WebViewWindow webViewWindow = new WebViewWindow();
            webViewWindow.Show();
        }
    }

}
