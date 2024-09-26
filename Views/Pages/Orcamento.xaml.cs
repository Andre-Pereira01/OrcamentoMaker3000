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
            OrcamentoModel.Instance.ValuePerMusician = new System.Collections.Generic.Dictionary<int, decimal>
            {
                { 15, 25.00m },
                { 30, 40.00m },
                { 45, 60.00m },
                { 60, 75.00m },
                { 90, 90.00m }
            };

            OrcamentoModel.Instance.ExtraSalary = new System.Collections.Generic.Dictionary<int, decimal>
            {
                { 30, 15.00m },
                { 100, 20.00m },
                { 200, 30.00m },
                {999, 40.00m }
            };
        }

        // Função que processa o orçamento e gera o resultado
        private void GerarOrcamento(object sender, RoutedEventArgs e)
        {
            // Preencher os valores no modelo
            OrcamentoModel.Instance.ClientName = ClientNameTextBox.Text;
            OrcamentoModel.Instance.ClientPhone = ClientPhoneTextBox.Text ?? "N/A";
            OrcamentoModel.Instance.ClientEmail = ClientEmailTextBox.Text ?? "N/A";
            OrcamentoModel.Instance.ServiceType = Lock_timeout_ms.Text;
            OrcamentoModel.Instance.Date = DatePicker.SelectedDate ?? DateTime.Now;
            OrcamentoModel.Instance.Schedule = HorarioBox.Text ?? "A definir";
            OrcamentoModel.Instance.Location = LocalTextBox.Text;
            OrcamentoModel.Instance.Duration = int.Parse(DuraçãoCombo.SelectedValue.ToString());
            OrcamentoModel.Instance.CreationDate = DateTime.Now;
            OrcamentoModel.Instance.Distance = decimal.Parse(DistanciaText.Text);

            // Cálculo do valor base por músico
            decimal baseValue = OrcamentoModel.Instance.ValuePerMusician[OrcamentoModel.Instance.Duration];
            decimal totalBaseValue = baseValue * OrcamentoModel.Instance.NumberMusicians;
            OrcamentoModel.Instance.TotalBaseValue = totalBaseValue;

            // Calcular despesas de viagem
            decimal travelExpenses = OrcamentoModel.Instance.KilometerPrice * OrcamentoModel.Instance.Distance;
            OrcamentoModel.Instance.TravelExpenses = travelExpenses;

            // Calcular salário extra baseado na distância
            decimal extraSalary = CalcularExtraSalary(OrcamentoModel.Instance.Distance);

            // Calcular a poupança do grupo e o salário do manager
            decimal groupSavings = totalBaseValue * OrcamentoModel.Instance.GroupSavings;
            decimal managerSalary = totalBaseValue * OrcamentoModel.Instance.ManagerSalary;


            // Cotação final
            decimal cotation = totalBaseValue + extraSalary + travelExpenses + groupSavings + managerSalary + OrcamentoModel.Instance.AlimentationExpenses;
            // Arredondar para o valor mais próximo de 50 ou 00
            OrcamentoModel.Instance.Cotation = Math.Ceiling(cotation / 50) * 50;

            MessageBox.Show($"Orçamento gerado com sucesso!\nCotação: {OrcamentoModel.Instance.Cotation:C2}", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Função para calcular o salário extra com base na distância
        private decimal CalcularExtraSalary(decimal distance)
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
    }

   
    
}
