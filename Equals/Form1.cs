using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Equals
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists($@"{AppDomain.CurrentDomain.BaseDirectory}\SHIF_plus.txt"))
            {
                //[код формы]\[номер раздела]\[номер строки]\[номер графы]
                //A=>  501\12\2\1 = 221\3\74\2 + 221\3\74\4
                //B=>  501\12\2\2 = 221\3\74\3 + 221\3\74\5
                //C=>  501\12\2\3 = 221\3\74\6
                //D=>  501\12\2\5 = 221\3\74\7

                string d1section = "12"; //Наше:
                int d1row_A = 3;  int d1cln1_A = 1; 
                                  int d1cln1_B = 2;
                                  int d1cln1_C = 3;
                                  int d1cln1_D = 5;

                string d2section = "03"; //СУСК-овское:
                int d2row_A = 75; int d2cln1_A = 2; int d2cln2_A = 4;
                                  int d2cln1_B = 3; int d2cln2_B = 5;
                                  int d2cln1_C = 6;
                                  int d2cln1_D = 7;

                string[] districts = File.ReadAllLines($@"{AppDomain.CurrentDomain.BaseDirectory}\SHIF_plus.txt", Encoding.Default); //reads Shif

                int[] out1_A = new int[districts.Length]; //prepares plaseholders for the outcome
                int[] out1_B = new int[districts.Length];
                int[] out1_C = new int[districts.Length];
                int[] out1_D = new int[districts.Length];

                int[] out2_A = new int[districts.Length]; 
                int[] out2_B = new int[districts.Length];
                int[] out2_C = new int[districts.Length];
                int[] out2_D = new int[districts.Length];

                var dat1 = new DirectoryInfo($@"{AppDomain.CurrentDomain.BaseDirectory}\DAT_2006.501\"); 
                var dat2 = new DirectoryInfo($@"{AppDomain.CurrentDomain.BaseDirectory}\DAT_2006.221\");

                //using (StreamReader sR = new StreamReader($@"C:\Users\rh\source\repos\Equals\Equals\bin\Debug\DAT_2006.221\1.03"))

                for (int i = 0; i < districts.Length; i++) //для каждого элемента Shif
                {
                    ////### Наши ###
                    if (districts[i].Substring(0, 1) == "+")  
                    {
                        foreach (FileInfo fI in dat1.GetFiles())
                        {
                            string[] dat_txt1 = null; //создаем масив для dat файла
                            if (fI.Name.Substring(0, fI.Name.IndexOf('.')) == districts[i].Substring(1, districts[i].IndexOf(' '))) //Файл соотв-ет № в Shif
                            {
                                if (fI.Name.Substring(fI.Name.IndexOf('.') + 1) == d1section) //Раздел соотв-ет искомому  //no need for ( ,2) it trims till the end anyway
                                {
                                    dat_txt1 = File.ReadAllLines(fI.FullName);  //Reads each line, store as "dat_txt1" array

                                    int[][] d1MX = new int[dat_txt1.Length][]; //rows as many as in dat_txt1[], and columns - unspecified

                                    for (int l = 2; l < dat_txt1.Length; l++)
                                    {
                                        d1MX[l] = (dat_txt1[l].Split(' ')).Select(int.Parse).ToArray(); //every row is filled by "deviding" to columns
                                    }

                                    out1_A[i] = d1MX[d1row_A][d1cln1_A];
                                    out1_B[i] = d1MX[d1row_A][d1cln1_B];
                                    out1_C[i] = d1MX[d1row_A][d1cln1_C];
                                    out1_D[i] = d1MX[d1row_A][d1cln1_D];
                                }
                            }
                        }
                    }
                    //else
                    //{
                    //    out2_A[i] = 0;
                    //}


                    ////### СУСК ###
                    //if (districts[i].Substring(0, 1) != "+")  //у СУСК не складывается
                    //{
                    //    foreach (FileInfo fI in dat2.GetFiles())
                    //    {
                    //        string[] dat_txt2 = null; //создаем масив для dat файла
                    //        if (fI.Name.Substring(0, fI.Name.IndexOf('.')) == districts[i].Substring(0, districts[i].IndexOf(' '))) //Файл соотв-ет № в Shif
                    //        {
                    //            if (fI.Name.Substring(fI.Name.IndexOf('.') + 1) == d2section) //Раздел соотв-ет искомому  //no need ,2 it trims till the end anyway
                    //            {
                    //                dat_txt2 = File.ReadAllLines(fI.FullName);  //Read each line, store as "dat_txt2" array

                    //                int[][] d2MX = new int[dat_txt2.Length][]; //rows as many as in dat_txt2[], and columns - unspecified

                    //                for (int l = 2; l < dat_txt2.Length; l++)
                    //                {
                    //                    d2MX[l] = (dat_txt2[l].Split(' ')).Select(int.Parse).ToArray(); //every row is filled by "deviding" to columns
                    //                }

                    //                out2_A[i] = d2MX[d2row_A][d2cln1_A] + d2MX[d2row_A][d2cln2_A];
                    //                out2_B[i] = d2MX[d2row_A][d2cln1_B] + d2MX[d2row_A][d2cln2_B];
                    //                out2_C[i] = d2MX[d2row_A][d2cln1_C];
                    //                out2_D[i] = d2MX[d2row_A][d2cln1_D];
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    out2_A[i] = 0;
                    //}

                    tbRzlt.AppendText($"{districts[i]};{out1_A[i]};{out2_A[i]};{out1_B[i]};{out2_B[i]};{out1_C[i]};{out2_C[i]};{out1_D[i]};{out2_D[i]};{Environment.NewLine}");
                }
            }
            else
            {
                MessageBox.Show("Не удалось найти файл SHIF :( ");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Таблица-Блокнот|*.csv";
            sfd.FileName = $"501-vs-221 ({DateTime.Now.ToString("dd.MM.yy")})";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, tbRzlt.Text, Encoding.UTF8);
            }
        }
    }
}

