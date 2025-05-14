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
using System.Windows.Shapes;

namespace Powerscan
{
    /// <summary>
    /// Interaktionslogik für FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {
        public FilterWindow()
        {
            InitializeComponent();
        }

        private void filterabbrechenbtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void filteranwendenbtn_Click(object sender, RoutedEventArgs e)
        {
            {
                if (Owner is ListWindow listWindow)
                {
                    ApplyColumnVisibility(listWindow.datalist);
                    this.Close();
                }
            }
        }


        public void ApplyColumnVisibility(DataGrid grid)
        {
            foreach (var column in grid.Columns)
            {
                switch (column.Header.ToString())
                {
                    case "ID.Nr.":
                        column.Visibility = idNrchkbx.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                        break;
                    case "UnterzählerID":
                        column.Visibility = unterzaehkerIdchkbx.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                        break;
                    case "PLZ":
                        column.Visibility = plzchkbx.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                        break;
                    case "Ort":
                        column.Visibility = ortchkbx.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                        break;
                    case "Straße":
                        column.Visibility = strassechkbx.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                        break;
                    case "Hausnummer":
                        column.Visibility = hausnummerchkbx.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                        break;
                    case "Gebäude":
                        column.Visibility = Gebaeudechkbx.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                        break;
                    case "Etage":
                        column.Visibility = etagechkbx.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                        break;
                    case "Raum":
                        column.Visibility = raumchkbx.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                        break;
                }
            }
        }
    }
    
}
