using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HelperWPF
{/// <summary> Benchmarking </summary>
    public class Compare
    {
        public Compare(){}

        public void CsvSaver(string content, string form_A, string form_B)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Таблица|*.csv";
            sfd.FileName = $"{form_A}-vs-{form_B} ({DateTime.Now.ToString("dd.MM.yy")})";

            if (sfd.ShowDialog() == true)
            {
                File.WriteAllText(sfd.FileName, content, Encoding.UTF8);
            }
        }

        public void DistrictsStats(List<string> districts, DirectoryInfo datFolder, string section, string row, string clm, int count_c, Grid mainGrid, int[] output )
        {
            foreach (FileInfo fI in datFolder.GetFiles()) //looping through DAT folder 
            {
                if (fI.Name.Substring(fI.Name.IndexOf('.') + 1) == section)
                {
                    string[] dat_txt = File.ReadAllLines(fI.FullName);
                    int[][] dat_MX = new int[dat_txt.Length][];

                    for (int i = 0; i < districts.Count - 1; i++) //for each district 
                    {
                        if (districts[i].Substring(0, 1) == "+")
                        {
                            if (fI.Name.Substring(0, fI.Name.IndexOf('.')) == districts[i].Substring(1, districts[i].IndexOf(' ') - 1))
                            {
                                //read dat file into array
                                for (int r = 2; r < dat_txt.Length; r++) //skipping 2 rows
                                {
                                    dat_MX[r] = (dat_txt[r].Split(' ')).Select(int.Parse).ToArray(); //every row is filled by "deviding" to columns
                                }

                                for (int c = 0; c <= count_c; c++)  //count_A
                                {

                                    int i_row = int.Parse((mainGrid.FindName($"{row}{c}") as TextBox).Text) + 1;
                                    int j_clm = int.Parse((mainGrid.FindName($"{clm}{c}") as TextBox).Text);

                                    output[i] += dat_MX[i_row][j_clm];
                                }
                            }
                        }
                        else
                        {
                            if (districts[i + 1].Substring(0, 1) == "+") //"Root" district-element
                            {
                                output[i] = 0;
                            }
                            else
                            {//regular districts without total-summ
                                if (fI.Name.Substring(0, fI.Name.IndexOf('.')) == districts[i].Substring(0, districts[i].IndexOf(' ')))
                                {
                                    for (int r = 2; r < dat_txt.Length; r++)
                                    {
                                        dat_MX[r] = (dat_txt[r].Split(' ')).Select(int.Parse).ToArray();
                                    }

                                    for (int c = 0; c <= count_c; c++)
                                    {
                                        int i_row = int.Parse((mainGrid.FindName($"{row}{c}") as TextBox).Text) + 1;
                                        int j_clm = int.Parse((mainGrid.FindName($"{clm}{c}") as TextBox).Text);

                                        output[i] += dat_MX[i_row][j_clm];
                                    }

                                }
                            }
                        }
                    }
                }
            }

            //Sum data from districts:
            int summ_Up = 0; //intermediate sum for "sub-districts"

            for (int i = districts.Count - 2; i > 0; i--) //for each Shif element
            {
                if (districts[i].Substring(0, 1) == "+")
                {
                    summ_Up += output[i];
                }
                else
                {
                    if (districts[i + 1].Substring(0, 1) == "+")  //"Root" district-element
                    {
                        output[i] = summ_Up;
                        summ_Up = 0;
                    }
                }
            }

        }

        public string TestTb(TextBox tb)
        {
            string tbDone = "";
            byte x;
            if (byte.TryParse(tb.Text, out x))
                tbDone = (x < 10) ? "0" + x.ToString() : x.ToString();
            else
                MessageBox.Show("Раздел введен не верно!");

            return tbDone;
        }
    }
}