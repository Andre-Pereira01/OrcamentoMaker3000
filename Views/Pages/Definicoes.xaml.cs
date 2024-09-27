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

namespace OrcamentoMaker3000.Views.Pages
{
    /// <summary>
    /// Interaction logic for Definicoes.xaml
    /// </summary>
    public partial class Definicoes : Page
    {
        public Definicoes()
        {
            InitializeComponent();

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

            PrecokmText.Text = OrcamentoModel.Instance.KilometerPrice.ToString();
            GrupoText.Text = (OrcamentoModel.Instance.GroupSavings * 100).ToString(); // Poupança em %
            ManagerBox.Text = (OrcamentoModel.Instance.ManagerSalary * 100).ToString(); // Salário do Manager em %
            AlimentaçãoBox.Text = OrcamentoModel.Instance.AlimentationExpenses.ToString();

            // Preencher os valores de distância e salário extra
            DistanceLimit1TextBox.Text = "30";
            Salary1TextBox.Text = OrcamentoModel.Instance.ExtraSalary[30].ToString();

            DistanceLimit2TextBox.Text = "100";
            Salary2TextBox.Text = OrcamentoModel.Instance.ExtraSalary[100].ToString();

            DistanceLimit3TextBox.Text = "200";
            Salary3TextBox.Text = OrcamentoModel.Instance.ExtraSalary[200].ToString();

            DistanceLimit4TextBox.Text = "999"; // Valor representando acima de 150km
            Salary4TextBox.Text = OrcamentoModel.Instance.ExtraSalary[999].ToString();

            // Preencher os valores base no TextBox com os valores atuais do modelo
            ValueFor15MinTextBox.Text = OrcamentoModel.Instance.ValuePerMusician[15].ToString();
            ValueFor30MinTextBox.Text = OrcamentoModel.Instance.ValuePerMusician[30].ToString();
            ValueFor45MinTextBox.Text = OrcamentoModel.Instance.ValuePerMusician[45].ToString();
            ValueFor60MinTextBox.Text = OrcamentoModel.Instance.ValuePerMusician[60].ToString();
            ValueFor90MinTextBox.Text = OrcamentoModel.Instance.ValuePerMusician[90].ToString();


        }

        private void Save_Definitions(object sender, RoutedEventArgs e)
        {
            try
            {
                // Alterar os valores no modelo
                if (double.TryParse(PrecokmText.Text, out var kilometerPrice))
                    OrcamentoModel.Instance.KilometerPrice = kilometerPrice;

                if (double.TryParse(GrupoText.Text, out var groupSavings))
                    OrcamentoModel.Instance.GroupSavings = groupSavings / 100; // Converter para double

                if (double.TryParse(ManagerBox.Text, out var managerSalary))
                    OrcamentoModel.Instance.ManagerSalary = managerSalary / 100; // Converter para double

                if (double.TryParse(AlimentaçãoBox.Text, out var alimentationExpenses))
                    OrcamentoModel.Instance.AlimentationExpenses = alimentationExpenses;
                if (double.TryParse(ExtraBox.Text, out var extraExpenses))
                    OrcamentoModel.Instance.ExtraExpenses = extraExpenses;

                MessageBox.Show("Valores atualizados!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar valores: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Save_Extra(object sender, RoutedEventArgs e)
        {
            try
            {

                if (int.TryParse(DistanceLimit1TextBox.Text, out var distance1) &&
                    double.TryParse(Salary1TextBox.Text, out var salary1))
                {
                    // Atualiza o dicionário para a primeira faixa
                    OrcamentoModel.Instance.ExtraSalary[distance1] = salary1;
                }

                if (int.TryParse(DistanceLimit2TextBox.Text, out var distance2) &&
                    double.TryParse(Salary2TextBox.Text, out var salary2))
                {
                    // Atualiza o dicionário para a segunda faixa
                    OrcamentoModel.Instance.ExtraSalary[distance2] = salary2;
                }

                if (int.TryParse(DistanceLimit3TextBox.Text, out var distance3) &&
                    double.TryParse(Salary3TextBox.Text, out var salary3))
                {
                    // Atualiza o dicionário para a terceira faixa
                    OrcamentoModel.Instance.ExtraSalary[distance3] = salary3;
                }

                // Para a faixa de distâncias acima de 150km, definimos como 999 no dicionário
                if (double.TryParse(Salary4TextBox.Text, out var salary4))
                {
                    OrcamentoModel.Instance.ExtraSalary[999] = salary4;
                }

                MessageBox.Show("Valores atualizados!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar valores: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Save_ValoresBase(object sender, RoutedEventArgs e)
        {
            try
            {
                // Verifica e atualiza os valores de cada duração
                if (double.TryParse(ValueFor15MinTextBox.Text, out var value15))
                {
                    OrcamentoModel.Instance.ValuePerMusician[15] = value15;
                }

                if (double.TryParse(ValueFor30MinTextBox.Text, out var value30))
                {
                    OrcamentoModel.Instance.ValuePerMusician[30] = value30;
                }

                if (double.TryParse(ValueFor45MinTextBox.Text, out var value45))
                {
                    OrcamentoModel.Instance.ValuePerMusician[45] = value45;
                }

                if (double.TryParse(ValueFor60MinTextBox.Text, out var value60))
                {
                    OrcamentoModel.Instance.ValuePerMusician[60] = value60;
                }

                if (double.TryParse(ValueFor90MinTextBox.Text, out var value90))
                {
                    OrcamentoModel.Instance.ValuePerMusician[90] = value90;
                }

                MessageBox.Show("Valores atualizados!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar valores: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}

