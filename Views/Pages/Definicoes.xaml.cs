﻿using Newtonsoft.Json;
using OrcamentoMaker3000.Models;
using System.IO;
using System.Windows.Controls;

namespace OrcamentoMaker3000.Views.Pages
{
    public partial class Definicoes : Page
    {
        string _savePath = OrcamentoModel.Instance.SavePath;
        public Definicoes()
        {
            InitializeComponent();
            CarregarConfiguracoes();
        }

        private void CarregarConfiguracoes()
        {
            // Definir o caminho do arquivo config.json
            string configFilePath = System.IO.Path.Combine(_savePath, "config.json");

            if (File.Exists(configFilePath))
            {
                // Ler o JSON do arquivo config.json
                string json = File.ReadAllText(configFilePath);

                // Deserializar o JSON para o objeto Config
                var config = JsonConvert.DeserializeObject<Config>(json);

                // Inicializar os dicionários caso estejam nulos
                OrcamentoModel.Instance.ValuePerMusician = config.ValuePerMusician ?? new Dictionary<int, double>();
                OrcamentoModel.Instance.ExtraSalary = config.ExtraSalary ?? new Dictionary<int, double>();
                //OrcamentoModel.Instance.KilometerPrice = config.KilometerPrice;
                OrcamentoModel.Instance.GroupSavings = config.GroupSavings;
                OrcamentoModel.Instance.ManagerSalary = config.ManagerSalary;

                // Atualizar a interface com os valores carregados
                //PrecokmText.Text = OrcamentoModel.Instance.KilometerPrice.ToString();
                GrupoText.Text = (OrcamentoModel.Instance.GroupSavings * 100).ToString(); // Poupança em %
                ManagerBox.Text = (OrcamentoModel.Instance.ManagerSalary * 100).ToString(); // Salário do Manager em %
                AlimentaçãoBox.Text = OrcamentoModel.Instance.AlimentationExpenses.ToString();

                // Atualizar valores por duração
                ValueFor15MinTextBox.Text = OrcamentoModel.Instance.ValuePerMusician[15].ToString();
                ValueFor30MinTextBox.Text = OrcamentoModel.Instance.ValuePerMusician[30].ToString();
                ValueFor45MinTextBox.Text = OrcamentoModel.Instance.ValuePerMusician[45].ToString();
                ValueFor60MinTextBox.Text = OrcamentoModel.Instance.ValuePerMusician[60].ToString();
                ValueFor90MinTextBox.Text = OrcamentoModel.Instance.ValuePerMusician[90].ToString();

                // Carregar os valores do dicionário e preencher a UI
                var distanceKeys = OrcamentoModel.Instance.ExtraSalary.Keys.OrderBy(k => k).ToList();
                DistanceLimit1TextBox.Text = distanceKeys[0].ToString();
                Salary1TextBox.Text = OrcamentoModel.Instance.ExtraSalary[distanceKeys[0]].ToString();

                DistanceLimit2TextBox.Text = distanceKeys[1].ToString();
                Salary2TextBox.Text = OrcamentoModel.Instance.ExtraSalary[distanceKeys[1]].ToString();

                DistanceLimit3TextBox.Text = distanceKeys[2].ToString();
                Salary3TextBox.Text = OrcamentoModel.Instance.ExtraSalary[distanceKeys[2]].ToString();

                DistanceLimit4TextBox.Text = distanceKeys[3].ToString();
                Salary4TextBox.Text = OrcamentoModel.Instance.ExtraSalary[distanceKeys[3]].ToString();
            }
            else
            {
                MessageBox.Show("Ficheiro de configuração não encontrado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Save_Definitions(object sender, RoutedEventArgs e)
        {
            try
            {
                // Definir o caminho do arquivo config.json
                string configFilePath = System.IO.Path.Combine(_savePath, "config.json");

                // Verificar se o diretório existe, senão criar
                string directoryPath = System.IO.Path.GetDirectoryName(configFilePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                Config config;
                if (File.Exists(configFilePath))
                {
                    // Carregar o JSON existente
                    string json = File.ReadAllText(configFilePath);
                    config = JsonConvert.DeserializeObject<Config>(json);
                }
                else
                {
                    config = new Config(); // Se não existe, cria um novo
                }

                //// Atualizar apenas os valores de definições principais
                //if (double.TryParse(PrecokmText.Text, out var kilometerPrice))
                //{
                //    config.KilometerPrice = kilometerPrice;
                //}

                if (double.TryParse(GrupoText.Text, out var groupSavings))
                {
                    config.GroupSavings = groupSavings / 100; // Converter para porcentagem
                }

                if (double.TryParse(ManagerBox.Text, out var managerSalary))
                {
                    config.ManagerSalary = managerSalary / 100; // Converter para porcentagem
                }

                if (double.TryParse(AlimentaçãoBox.Text, out var alimentationExpenses))
                {
                    OrcamentoModel.Instance.AlimentationExpenses = alimentationExpenses; // Salvar no modelo também
                }

                // Salvar o JSON atualizado no arquivo
                string updatedJson = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(configFilePath, updatedJson);

                // Verificação: Leitura para conferir se os valores estão sendo salvos corretamente
                string testJson = File.ReadAllText(configFilePath);
                MessageBox.Show($"Valores atualizados com sucesso", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao guardar valores principais: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Save_Extra(object sender, RoutedEventArgs e)
        {
            try
            {
                string configFilePath = System.IO.Path.Combine(_savePath, "config.json");

                // Carregar ou criar novo arquivo de configuração
                Config config;
                if (File.Exists(configFilePath))
                {
                    string json = File.ReadAllText(configFilePath);
                    config = JsonConvert.DeserializeObject<Config>(json);
                }
                else
                {
                    config = new Config();
                }

                // Capturar os valores de distância e salário da UI
                if (int.TryParse(DistanceLimit1TextBox.Text, out var distance1) &&
                    int.TryParse(DistanceLimit2TextBox.Text, out var distance2) &&
                    int.TryParse(DistanceLimit3TextBox.Text, out var distance3) &&
                    int.TryParse(DistanceLimit4TextBox.Text, out var distance4) &&
                    double.TryParse(Salary1TextBox.Text, out var salary1) &&
                    double.TryParse(Salary2TextBox.Text, out var salary2) &&
                    double.TryParse(Salary3TextBox.Text, out var salary3) &&
                    double.TryParse(Salary4TextBox.Text, out var salary4))
                {
                    // Validar se as distâncias estão em ordem crescente
                    if (distance1 < distance2 && distance2 < distance3 && distance3 < distance4)
                    {
                        // Substituir os valores no dicionário de ExtraSalary
                        config.ExtraSalary.Clear(); // Limpar os valores antigos
                        config.ExtraSalary[distance1] = salary1;
                        config.ExtraSalary[distance2] = salary2;
                        config.ExtraSalary[distance3] = salary3;
                        config.ExtraSalary[distance4] = salary4;

                        // Salvar o JSON atualizado no arquivo
                        string updatedJson = JsonConvert.SerializeObject(config, Formatting.Indented);
                        File.WriteAllText(configFilePath, updatedJson);

                        MessageBox.Show("Salários extras atualizados!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Os valores de distância devem estar em ordem crescente.", "Erro de Validação", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Erro ao ler os valores de distância ou salário.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao guardar salários extras: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Save_ValoresBase(object sender, RoutedEventArgs e)
        {
            try
            {
                // Definir o caminho do arquivo config.json
                string configFilePath = System.IO.Path.Combine(_savePath, "config.json");

                Config config;
                if (File.Exists(configFilePath))
                {
                    // Carregar o JSON existente
                    string json = File.ReadAllText(configFilePath);
                    config = JsonConvert.DeserializeObject<Config>(json);
                }
                else
                {
                    config = new Config(); // Se não existe, cria um novo
                }

                // Atualizar apenas os valores por duração
                if (double.TryParse(ValueFor15MinTextBox.Text, out var value15))
                {
                    config.ValuePerMusician[15] = value15;
                }

                if (double.TryParse(ValueFor30MinTextBox.Text, out var value30))
                {
                    config.ValuePerMusician[30] = value30;
                }

                if (double.TryParse(ValueFor45MinTextBox.Text, out var value45))
                {
                    config.ValuePerMusician[45] = value45;
                }

                if (double.TryParse(ValueFor60MinTextBox.Text, out var value60))
                {
                    config.ValuePerMusician[60] = value60;
                }

                if (double.TryParse(ValueFor90MinTextBox.Text, out var value90))
                {
                    config.ValuePerMusician[90] = value90;
                }

                // Salvar o JSON atualizado no arquivo
                string updatedJson = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(configFilePath, updatedJson);

                MessageBox.Show("Valores por duração atualizados!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao guardar valores por duração: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}


