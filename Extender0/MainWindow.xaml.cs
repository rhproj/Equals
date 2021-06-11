using Extender.UserControls;
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


namespace Extender0
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

        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            RowColumn rowColumn = new RowColumn();
            stackPanel.Children.Add(rowColumn);
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            RowColumn rowColumn = new RowColumn();
            //stackPanel.Children.Remove(rowColumn);
            if (stackPanel.Children.Count > 0)
            {
                stackPanel.Children.RemoveAt(stackPanel.Children.Count - 1);
            }
        }
    }
}
