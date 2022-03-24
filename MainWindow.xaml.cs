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

namespace WPFkalendarZpisnoy
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += loadMain;
        }

        private void loadMain(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < daysGrid.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < daysGrid.ColumnDefinitions.Count; j++)
                {
                    int labelContent = j + i * 7 + 1;
                    if (labelContent > 31) continue;
                    var label = new Label();
                    label.Margin = new Thickness(5);
                    label.Background = new SolidColorBrush(Colors.Aquamarine);
                    label.Content = labelContent;
                    daysGrid.Children.Add(label);
                    Grid.SetRow(label, i);
                    Grid.SetColumn(label, j);
                }
            }
        }
    }
}
