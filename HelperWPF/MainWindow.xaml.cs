using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace HelperWPF
{
    public partial class MainWindow : Window
    {
        string shif = "";
        string datFolder_A = ""; string datFolder_B = ""; 
        string form_A = "501"; string form_B = "221";

        Compare compare = new Compare();

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (FileInfo fI in (new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory)).GetFiles())
            {
                if (fI.Name == "SHIF_plus.txt")
                {
                    shif = fI.FullName;
                    lblShif.Content = fI.Name;
                }
            }

            var datFolders = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory);
            foreach (var d in datFolders)
            {
                var datName = new DirectoryInfo(d).Name;

                if (datName.Substring(datName.IndexOf('.') + 1) == form_A)
                {
                    datFolder_A = datName;
                    tbRzlt.AppendText(datFolder_A + "\r\n");
                }
                if (datName.Substring(datName.IndexOf('.') + 1) == form_B)
                {
                    datFolder_B = datName;
                    tbRzlt.AppendText(datFolder_B + "\r\n");
                }
            }
        }

        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            tbRzlt.Text = "";
            if (shif != "")
            {
                if (datFolder_A != "" && datFolder_B != "")
                {
                    #region Stats layout
                    //[код формы]\[номер раздела]\[номер строки]\[номер графы]
                    //A=>  501\12\2\1 = 221\3\74\2 + 221\3\74\4
                    //B=>  501\12\2\2 = 221\3\74\3 + 221\3\74\5
                    //C=>  501\12\2\3 = 221\3\74\6
                    //D=>  501\12\2\5 = 221\3\74\7 
                    #endregion

                    string d1section = "12";
                    int d1row_A = 3; int d1cln1_A = 1;
                    int d1cln1_B = 2;
                    int d1cln1_C = 3;
                    int d1cln1_D = 5;

                    string d2section = "03"; //СУСК:
                    int d2row_A = 75; int d2cln1_A = 2; int d2cln2_A = 4;
                    int d2cln1_B = 3; int d2cln2_B = 5;
                    int d2cln1_C = 6;
                    int d2cln1_D = 7;

                    var dat1 = new DirectoryInfo($@"{AppDomain.CurrentDomain.BaseDirectory}\{datFolder_A}\");
                    var dat2 = new DirectoryInfo($@"{AppDomain.CurrentDomain.BaseDirectory}\{datFolder_B}\");

                    string[] dat_txt1 = null; //array for dat file
                    int[][] d1MX = null; //Matrix for dat file

                    string[] dat_txt2 = null;
                    int[][] d2MX = null;

                    List<string> districts = File.ReadAllLines(shif, Encoding.Default).ToList();  //reads shif file
                    districts.Add("The end of the List");

                    int[] out1_A = new int[districts.Count]; //prepares plaseholders for the outcome
                    int[] out1_B = new int[districts.Count];
                    int[] out1_C = new int[districts.Count];
                    int[] out1_D = new int[districts.Count];

                    int[] out2_A = new int[districts.Count]; //prepares plaseholders for the outcome
                    int[] out2_B = new int[districts.Count];
                    int[] out2_C = new int[districts.Count];
                    int[] out2_D = new int[districts.Count];

                    string[] eq_A = new string[districts.Count];  //equal signs
                    string[] eq_B = new string[districts.Count];
                    string[] eq_C = new string[districts.Count];
                    string[] eq_D = new string[districts.Count];

                    #region The Prosecutors Office Stats
                    foreach (FileInfo fI in dat1.GetFiles()) //every file in DAT folder
                    {
                        if (fI.Name.Substring(fI.Name.IndexOf('.') + 1) == d1section)
                        {
                            dat_txt1 = File.ReadAllLines(fI.FullName);  //Reads each line, store as "dat_txt1" array
                            d1MX = new int[dat_txt1.Length][]; //rows as many as in dat_txt1[], and columns - unspecified

                            for (int i = 0; i < districts.Count - 1; i++) //for each district in Shif file
                            {
                                if (districts[i].Substring(0, 1) == "+")
                                {
                                    if (fI.Name.Substring(0, fI.Name.IndexOf('.')) == districts[i].Substring(1, districts[i].IndexOf(' ') - 1))
                                    {
                                        //equals go to array
                                        for (int r = 2; r < dat_txt1.Length; r++)
                                        {
                                            d1MX[r] = (dat_txt1[r].Split(' ')).Select(int.Parse).ToArray(); //every row is filled by "deviding" to columns
                                        }
                                        out1_A[i] = d1MX[d1row_A][d1cln1_A];
                                        out1_B[i] = d1MX[d1row_A][d1cln1_B];
                                        out1_C[i] = d1MX[d1row_A][d1cln1_C];
                                        out1_D[i] = d1MX[d1row_A][d1cln1_D];
                                    }
                                }
                                else
                                {
                                    if (districts[i + 1].Substring(0, 1) == "+") //root-district element
                                    {
                                        out1_A[i] = 0;
                                        out1_B[i] = 0;
                                        out1_C[i] = 0;
                                        out1_D[i] = 0;
                                    }
                                    else
                                    {
                                        if (fI.Name.Substring(0, fI.Name.IndexOf('.')) == districts[i].Substring(0, districts[i].IndexOf(' ')))
                                        {
                                            for (int r = 2; r < dat_txt1.Length; r++)
                                            {
                                                d1MX[r] = (dat_txt1[r].Split(' ')).Select(int.Parse).ToArray(); //every row is filled by "deviding" to columns
                                            }
                                            out1_A[i] = d1MX[d1row_A][d1cln1_A];
                                            out1_B[i] = d1MX[d1row_A][d1cln1_B];
                                            out1_C[i] = d1MX[d1row_A][d1cln1_C];
                                            out1_D[i] = d1MX[d1row_A][d1cln1_D];
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //Sum data from deistricts:
                    int summ_A = 0; int summ_B = 0; int summ_C = 0; int summ_D = 0;  //intermediate result for "sub-districts"

                    for (int i = districts.Count - 2; i > 0; i--) //every Shif element
                    {
                        if (districts[i].Substring(0, 1) == "+")
                        {
                            summ_A += out1_A[i];
                            summ_B += out1_B[i];
                            summ_C += out1_C[i];
                            summ_D += out1_D[i];
                        }
                        else
                        {
                            if (districts[i + 1].Substring(0, 1) == "+")
                            {
                                out1_A[i] = summ_A;
                                out1_B[i] = summ_B;
                                out1_C[i] = summ_C;
                                out1_D[i] = summ_D;

                                summ_A = 0; summ_B = 0; summ_C = 0; summ_D = 0;
                            }
                        }
                    }
                    #endregion

                    #region Investigative Committee Stats
                    foreach (FileInfo fI in dat2.GetFiles()) //для каждого файла в DAT
                    {
                        if (fI.Name.Substring(fI.Name.IndexOf('.') + 1) == d2section) //Раздел соотв-ет искомому  //no need for ( ,2) it trims till the end anyway
                        {
                            //dat_txt2 = File.ReadAllLines(fI.FullName);  //Reads each line, store as "dat_txt2" array
                            //d2MX = new int[dat_txt2.Length][]; //rows as many as in dat_txt2[], and columns - unspecified

                            for (int i = 0; i < districts.Count - 1; i++) //для каждого элемента Shif 
                            {
                                if (fI.Name.Substring(0, fI.Name.IndexOf('.')) == districts[i].Substring(0, districts[i].IndexOf(' ')))  //у СУСК не складывается
                                {
                                    dat_txt2 = File.ReadAllLines(fI.FullName);  //in this case we can move these 2 here  
                                    d2MX = new int[dat_txt2.Length][];

                                    for (int l = 2; l < dat_txt2.Length; l++)
                                    {
                                        d2MX[l] = (dat_txt2[l].Split(' ')).Select(int.Parse).ToArray(); //every row is filled by "deviding" to columns
                                    }

                                    out2_A[i] = d2MX[d2row_A][d2cln1_A] + d2MX[d2row_A][d2cln2_A];
                                    out2_B[i] = d2MX[d2row_A][d2cln1_B] + d2MX[d2row_A][d2cln2_B];
                                    out2_C[i] = d2MX[d2row_A][d2cln1_C];
                                    out2_D[i] = d2MX[d2row_A][d2cln1_D];
                                }
                            }
                        }
                    }

                    //###Результат###
                    tbRzlt.AppendText($"Форма.Раздел; {form_A}.{d1section} ;?; {form_B}.{d2section} ;{form_A}.{d1section} ;?; {form_B}.{d2section}; {form_A}.{d1section};?; {form_B}.{d2section}; {form_A}.{d1section};?; {form_B}.{d2section} ;{Environment.NewLine}"); //шапка
                    tbRzlt.AppendText($"Строка/Графа; {d1row_A - 1}/{d1cln1_A} ; ; {d2row_A - 1}/{d2cln1_A}+{d2cln2_A} ; {d1row_A - 1}/{d1cln1_B} ;; {d2row_A - 1}/{d2cln1_B}+{d2cln2_B} ; {d1row_A - 1}/{d1cln1_C} ;; {d2row_A - 1}/{d2cln1_C} ;  {d1row_A - 1}/{d1cln1_D} ;; {d2row_A - 1}/{d2cln1_D} ;{Environment.NewLine}"); //шапка

                    for (int i = 0; i < districts.Count - 1; i++)
                    {
                        if (districts[i].Substring(0, 1) != "+")
                        {
                            if (out1_A[i] == out2_A[i])
                                eq_A[i] = "=";
                            else if (out1_A[i] > out2_A[i])
                                eq_A[i] = ">";
                            else
                                eq_A[i] = "<";
                            //
                            if (out1_B[i] == out2_B[i])
                                eq_B[i] = "=";
                            else if (out1_B[i] > out2_B[i])
                                eq_B[i] = ">";
                            else
                                eq_B[i] = "<";
                            //
                            if (out1_C[i] == out2_C[i])
                                eq_C[i] = "=";
                            else if (out1_A[i] > out2_A[i])
                                eq_C[i] = ">";
                            else
                                eq_C[i] = "<";
                            //
                            if (out1_D[i] == out2_D[i])
                                eq_D[i] = "=";
                            else if (out1_D[i] > out2_D[i])
                                eq_D[i] = ">";
                            else
                                eq_D[i] = "<";
                        }

                        tbRzlt.AppendText($"{districts[i]};{out1_A[i]};{eq_A[i]};{out2_A[i]};{out1_B[i]};{eq_B[i]};{out2_B[i]};{out1_C[i]};{eq_C[i]};{out2_C[i]};{out1_D[i]};{eq_D[i]};{out2_D[i]};{Environment.NewLine}");
                    } 
                    #endregion

                    compare.CsvSaver(tbRzlt.Text, form_A, form_B);
                }
                else
                {
                    MessageBox.Show("Отсутствуют подходящие папки DAT для 501 или 221 формы");
                }
            }
            else
            {
                MessageBox.Show("Не выбран файл Shif :(");
            }
        }

        private void BtnPlus_Click(object sender, RoutedEventArgs e)
        {
            Universal uni = new Universal();
            uni.ShowDialog();
        }

        private void BtnShif_Click(object sender, RoutedEventArgs e)
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
    }
}
