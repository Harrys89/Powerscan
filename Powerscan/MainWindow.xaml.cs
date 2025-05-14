using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Powerscan
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

        private void eintragenbtn_Click(object sender, RoutedEventArgs e)
        {
            Window entryWindow = new EntryWindow();
            entryWindow.Show();
            this.Close();
        }

        private void listebtn_Click(object sender, RoutedEventArgs e)
        {
            Window listWindow = new ListWindow();
            listWindow.Show();
            this.Close();
        }

        private void progschliessenbtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}