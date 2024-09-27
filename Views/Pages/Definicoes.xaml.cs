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
using Newtonsoft.Json;
using System.IO;

namespace OrcamentoMaker3000.Views.Pages
{
    /// <summary>
    /// Interaction logic for Definicoes.xaml
    /// </summary>
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
                OrcamentoModel.Instance.KilometerPrice = config.KilometerPrice;
                OrcamentoModel.Instance.GroupSavings = config.GroupSavings;
                OrcamentoModel.Instance.ManagerSalary = config.ManagerSalary;

                // Atualizar a interface com os valores carregados
                PrecokmText.Text = OrcamentoModel.Instance.KilometerPrice.ToString();
                GrupoText.Text = (OrcamentoModel.Instance.GroupSavings * 100).ToString(); // Poupança em %
                ManagerBox.Text = (OrcamentoModel.Instance.ManagerSalary * 100).ToString(); // Salário do Manager em %
                AlimentaçãoBox.Text = OrcamentoModel.Instance.AlimentationExpenses.ToString();

                // Atualizar valores por duração
                ValueFor15MinTextBox.Text = OrcamentoModel.Instance.ValuePerMusician[15].ToString();
                ValueFor30MinTextBox.Text = OrcamentoModel.Instance.ValuePerMusician[30].ToString();
                ValueFor45MinTextBox.Text = OrcamentoModel.Instance.ValuePerMusician[45].ToString();
                ValueFor60MinTextBox.Text = OrcamentoModel.Instance.ValuePerMusician[60].ToString();
                ValueFor90MinTextBox.Text = OrcamentoModel.Instance.ValuePerMusician[90].ToString();

                // Atualizar valores de salários extras
                DistanceLimit1TextBox.Text = "30"; // Se você quer tornar essas variáveis dinâmicas, pode carregar do arquivo
                Salary1TextBox.Text = OrcamentoModel.Instance.ExtraSalary[30].ToString();

                DistanceLimit2TextBox.Text = "100";
                Salary2TextBox.Text = OrcamentoModel.Instance.ExtraSalary[100].ToString();

                DistanceLimit3TextBox.Text = "200";
                Salary3TextBox.Text = OrcamentoModel.Instance.ExtraSalary[200].ToString();

                DistanceLimit4TextBox.Text = "999";
                Salary4TextBox.Text = OrcamentoModel.Instance.ExtraSalary[999].ToString();
            }
            else
            {
                MessageBox.Show("Arquivo de configuração não encontrado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Save_Definitions(object sender, RoutedEventArgs e)
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

                // Atualizar apenas os valores de definições principais
                if (double.TryParse(PrecokmText.Text, out var kilometerPrice))
                {
                    config.KilometerPrice = kilometerPrice;
                }

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

                MessageBox.Show("Valores principais atualizados!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar valores principais: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Save_Extra(object sender, RoutedEventArgs e)
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

                // Atualizar apenas as faixas de salário extra
                if (int.TryParse(DistanceLimit1TextBox.Text, out var distance1) &&
                    double.TryParse(Salary1TextBox.Text, out var salary1))
                {
                    config.ExtraSalary[distance1] = salary1;
                }

                if (int.TryParse(DistanceLimit2TextBox.Text, out var distance2) &&
                    double.TryParse(Salary2TextBox.Text, out var salary2))
                {
                    config.ExtraSalary[distance2] = salary2;
                }

                if (int.TryParse(DistanceLimit3TextBox.Text, out var distance3) &&
                    double.TryParse(Salary3TextBox.Text, out var salary3))
                {
                    config.ExtraSalary[distance3] = salary3;
                }

                if (double.TryParse(Salary4TextBox.Text, out var salary4))
                {
                    config.ExtraSalary[999] = salary4;
                }

                // Salvar o JSON atualizado no arquivo
                string updatedJson = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(configFilePath, updatedJson);

                MessageBox.Show("Salários extras atualizados!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar salários extras: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Erro ao salvar valores por duração: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}


