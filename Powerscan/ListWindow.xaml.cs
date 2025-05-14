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
    /// Interaktionslogik für ListWindow.xaml
    /// </summary>
    public partial class ListWindow : Window
    {
        public ViewModel VM { get; set; }
        public ListWindow()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler in InitializeComponent():\n" + ex.Message, "XAML-Fehler");
            }
            try
            {
                VM = new ViewModel();
                this.DataContext = VM;
                VM.LoadData();
                VM.StartConsumptionSimulation();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden:\n" + ex.Message, "Fehler",MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearSelectionArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.DataContext is ViewModel vm)
            {
                vm.SelectedEntry = null;
            }
        }

        private void schliessenbtn_Click(object sender, RoutedEventArgs e)
        {
            Window mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void zuruecksetzenbtn_Click(object sender, RoutedEventArgs e)
        {
            VM.SetCounterBack();
        }

        private void neuItem_Click(object sender, RoutedEventArgs e)
        {
            EntryWindow entryWindow = new EntryWindow();
            entryWindow.Show();
            this.Close();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            VM.DeleteSelectedEntry();
        }

        private void filterbtn_Click(object sender, RoutedEventArgs e)
        {
            FilterWindow filterWindow = new FilterWindow();
            filterWindow.Owner = this; 
            filterWindow.ShowDialog();
        }

        private void suchenbtn_Click(object sender, RoutedEventArgs e)
        {
            SearchWindow sw = new SearchWindow();
            sw.Owner = this;
            sw.ShowDialog();
        }

        private void vollstaendigelistebtn_Click(object sender, RoutedEventArgs e)
        {
            VM.ShowAllEntrys();
        }
    }
    
}
