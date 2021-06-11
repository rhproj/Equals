using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HelperWPF
{
    /// <summary>Interaction logic for Universal comparison</summary>
    public partial class Universal : Window
    {
        int count_A = 0;  int count_B = 0;
        string shif = "";
        string datFolder_A = ""; string datFolder_B = ""; //gets populated from comboboxes
        string form_A; string form_B;

        Compare compare = new Compare();

        public Universal()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var datFolders = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory);
            foreach (var d in datFolders)
            {
                var datName = new DirectoryInfo(d).Name;
                comboA.Items.Add(datName);
                comboB.Items.Add(datName);
            }

            foreach (FileInfo fI in (new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory)).GetFiles())
            {
                if (fI.Name == "SHIF_plus.txt")
                {
                    shif = fI.FullName;
                    lblShif.Content = fI.Name;
                }
            }
        }

        private void BtnCompare_Click(object sender, RoutedEventArgs e)
        {
            tbUniRzlt.Text = "";

            if (shif != "")
            {
                if (datFolder_A != "" && datFolder_B != "")
                {
                    List<string> districts = File.ReadAllLines(shif, Encoding.Default).ToList();  //reads districts from shif.txt     
                    districts.Add("The end of the List");

                    string sectionA = compare.TestTb(tbSect_A);
                    string row_A = "tbRow_A";
                    string clm_A = "tbClm_A"; 

                    string sectionB = compare.TestTb(tbSect_B);
                    string row_B = "tbRow_B";
                    string clm_B = "tbClm_B";

                    var dat_A = new DirectoryInfo($@"{AppDomain.CurrentDomain.BaseDirectory}\{datFolder_A}\");
                    var dat_B = new DirectoryInfo($@"{AppDomain.CurrentDomain.BaseDirectory}\{datFolder_B}\");

                    int[] out_A = new int[districts.Count];     //prepares plaseholders for the outcome
                    int[] out_B = new int[districts.Count];
                    string[] eq = new string[districts.Count];    //equal signs

                    try
                    {
                        compare.DistrictsStats(districts, dat_A, sectionA, row_A, clm_A, count_A, gridUni, out_A);
                        compare.DistrictsStats(districts, dat_B, sectionB, row_B, clm_B, count_B, gridUni, out_B);
                    }
                    catch (Exception )
                    {
                        MessageBox.Show($"Не полностью заполнены номера строк/граф!");
                    }

                    //Results
                    tbUniRzlt.AppendText($"Форма.Раздел; {form_A}.{sectionA} ;?; {form_B}.{sectionB} {Environment.NewLine}"); //header
                    if (count_A == 0 && count_B == 0)
                    {
                        tbUniRzlt.AppendText($"Строка/Графа; {tbRow_A0.Text}/{tbClm_A0.Text} ; ; {tbRow_B0.Text}/{tbClm_B0.Text} {Environment.NewLine}"); //header
                    }

                    for (int i = 0; i < districts.Count - 1; i++)
                    {
                        if (districts[i].Substring(0, 1) != "+")
                        {
                            if (out_A[i] == out_B[i])
                                eq[i] = "=";
                            else if (out_A[i] > out_B[i])
                                eq[i] = ">";
                            else
                                eq[i] = "<";
                        }

                        tbUniRzlt.AppendText($"{districts[i]};{out_A[i]};{eq[i]};{out_B[i]}{Environment.NewLine}");
                    }
                }
                else
                {
                    MessageBox.Show("Не выбрана папка DAT");
                }
            }
            else
            {
                MessageBox.Show("Не выбран файл Shif");
            }
        }

        private void ComboA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            datFolder_A = comboA.SelectedValue.ToString();
            lblFormA.Content = datFolder_A.Substring(datFolder_A.IndexOf('.') + 1);
            form_A = lblFormA.Content.ToString();
        }

        private void ComboB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            datFolder_B = comboB.SelectedValue.ToString();
            lblFormB.Content = datFolder_B.Substring(datFolder_B.IndexOf('.') + 1);
            form_B = lblFormB.Content.ToString();
        }

        private void BtnShif_Click(object sender, RoutedEventArgs e)  //reads shif file
        {
            OpenFileDialog ofD = new OpenFileDialog();
            ofD.Filter = "shif txt file|*.txt";
            ofD.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (ofD.ShowDialog() == true)
            {
                shif = ofD.FileName;
                lblShif.Content = ofD.SafeFileName;
            }
        }

        private void BtnPlusA_Click(object sender, RoutedEventArgs e)
        {
            count_A++;

            TextBox tb_Row = new TextBox();
            tb_Row.Margin = new Thickness(0, 0, 5, 0);
            tb_Row.Width = 30;
            tb_Row.Height = 27;
            tb_Row.TextAlignment = TextAlignment.Center;
            tb_Row.FontSize = 18;

            stPanRowA.RegisterName($"tbRow_A{count_A}", tb_Row);
            stPanRowA.Children.Add(tb_Row);

            TextBox tb_Clm = new TextBox();
            tb_Clm.Margin = new Thickness(0, 0, 5, 0);
            tb_Clm.Width = 30;
            tb_Clm.Height = 27;
            tb_Clm.TextAlignment = TextAlignment.Center;
            tb_Clm.FontSize = 18;

            stPanClmA.RegisterName($"tbClm_A{count_A}", tb_Clm);
            stPanClmA.Children.Add(tb_Clm);
        }

        private void BtnPlusB_Click(object sender, RoutedEventArgs e)
        {
            count_B++;

            TextBox tb_Row = new TextBox();
            tb_Row.Margin = new Thickness(0, 0, 5, 0);
            tb_Row.Width = 30;
            tb_Row.Height = 27;
            tb_Row.TextAlignment = TextAlignment.Center;
            tb_Row.FontSize = 18;

            stPanRowB.RegisterName($"tbRow_B{count_B}", tb_Row);
            stPanRowB.Children.Add(tb_Row);  

            TextBox tb_Clm = new TextBox();
            tb_Clm.Margin = new Thickness(0, 0, 5, 0);
            tb_Clm.Width = 30;
            tb_Clm.Height = 27;
            tb_Clm.TextAlignment = TextAlignment.Center;
            tb_Clm.FontSize = 18;

            stPanClmB.RegisterName($"tbClm_B{count_B}", tb_Clm);
            stPanClmB.Children.Add(tb_Clm);       

        }

        private void BtnRemoveA_Click(object sender, RoutedEventArgs e) 
        {
            if (stPanRowA.Children.Count > 0)
            {
                UnregisterName($"tbRow_A{count_A}");
                stPanRowA.Children.RemoveAt(stPanRowA.Children.Count - 1);
            }

            if (stPanClmA.Children.Count > 0)
            {
                UnregisterName($"tbClm_A{count_A}");
                stPanClmA.Children.RemoveAt(stPanClmA.Children.Count - 1);
                count_A--;
            }
        }

        private void BtnRemoveB_Click(object sender, RoutedEventArgs e)
        {
            if (stPanRowB.Children.Count > 0)
            {
                UnregisterName($"tbRow_B{count_B}");
                stPanRowB.Children.RemoveAt(stPanRowB.Children.Count - 1);
            }

            if (stPanClmB.Children.Count > 0)
            {
                UnregisterName($"tbClm_B{count_B}");
                stPanClmB.Children.RemoveAt(stPanClmB.Children.Count - 1);
                count_B--;
            }
        }

        private void TbRow_A0_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            compare.CsvSaver(tbUniRzlt.Text, form_A, form_B);
        }
    }
}