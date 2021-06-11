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

namespace Extender01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        int counter = 0;
        List<TextBox> tbList = new List<TextBox>();


        private void FindNameButton_Click(object sender, RoutedEventArgs e)
        {
            object item = LayoutRoot.FindName(TextBox1.Text);

            if (item is TextBox)
            {
                TextBox txt = (TextBox)item;
                txt.Text = "Aha! you found me!";
                txt.Background = new SolidColorBrush(Colors.LightYellow);
            }

            else if (item is ListBox)
            {
                ListBox lst = (ListBox)item;
                lst.Items.Add("Aha! you found me!");
            }

            else if (item is Button)
            {
                Button btn = (Button)item;
                btn.Content = "Aha! you found me!";
                btn.Background = new SolidColorBrush(Colors.LightSeaGreen);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            counter++;
            TextBox tB = new TextBox();
            tB.Name = $"tb{counter}";

            tB.Width = 60; tB.Height = 60;

            tbList.Add(tB);
            sPan.Children.Add(tB);
        }

        private void BtnRzlt_Click(object sender, RoutedEventArgs e)
        {
            int total = 0;

            foreach (var tb in tbList)
            {
                //tbRzlt.AppendText(tb.Text + Environment.NewLine);   

                if (int.TryParse(tb.Text, out total))
                {
                    total += int.Parse(tb.Text);
                }

            }
            tbRzlt.AppendText(total.ToString());

            //object item = LayoutRoot.FindName("tb0");
            ////TextBox txt = (TextBox)item;
            ////txt.Text = "Aha! you found me!";
            //if (item is TextBox)
            //{
            //    TextBox txt = (TextBox)item;

            //    if (txt.Name == "tb0")
            //    {
            //        txt.Text = "55";

            //        tbRzlt.AppendText("You found me!");//tb0.Name + " " + tb0.Text);
            //    }
            //}
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbList.Add(tb0);
        }
    }
}


//this.Grid.Add(new TextBox() { Text = "Babau" });
