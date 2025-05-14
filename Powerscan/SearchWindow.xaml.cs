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
    /// Interaktionslogik für SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public SearchWindow()
        {
            InitializeComponent();
        }


        private void Searchaction()
        {
            // 1. Suchbegriff holen
            string suchbegriff = suchentxtbx.Text?.Trim();

            // 2. Alle Checkboxen prüfen
            var aktiveCheckboxen = new List<CheckBox>
    {
        idNrSuchenchkbx,
        unterzaehlerIdSuchenchkbx,
        postleitzahlSuchenchkbx,
        ortSuchenchkbx,
        strasseSuchenchkbx,
        hausnummerSuchenchkbx,
        etageSuchenchkbx,
        gebaeudeSuchenchkbx,
        raumSuchenchkbx
    }.Where(cb => cb.IsChecked == true).ToList();

            // 3. Prüfen: genau 1 Checkbox & nicht leer
            if (aktiveCheckboxen.Count != 1)
            {
                MessageBox.Show("Bitte genau **eine** Kategorie auswählen, nach der gesucht werden soll.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(suchbegriff))
            {
                MessageBox.Show("Bitte einen Suchbegriff eingeben.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 4. Suche in Liste (Beispiel: Zugriff auf ViewModel von Owner)
            if (Owner is ListWindow listWindow && listWindow.VM is ViewModel vm)
            {
                var treffer = vm.GetAllEntries().Where(entry =>
                {
                    string zielwert = aktiveCheckboxen[0].Name switch
                    {
                        nameof(idNrSuchenchkbx) => entry.EntryId.ToString(),
                        nameof(unterzaehlerIdSuchenchkbx) => entry.MeterId,
                        nameof(postleitzahlSuchenchkbx) => entry.ZipCode,
                        nameof(ortSuchenchkbx) => entry.City,
                        nameof(strasseSuchenchkbx) => entry.Street,
                        nameof(hausnummerSuchenchkbx) => entry.Housenumber,
                        nameof(etageSuchenchkbx) => entry.Floor,
                        nameof(gebaeudeSuchenchkbx) => entry.Building,
                        nameof(raumSuchenchkbx) => entry.Room,
                        _ => ""
                    };

                    return zielwert != null && zielwert.Equals(suchbegriff, StringComparison.OrdinalIgnoreCase);
                }).ToList();

                vm.Daten.Clear();
                foreach (var eintrag in treffer)
                    vm.Daten.Add(eintrag);

                this.Close(); // Fenster schließen nach erfolgreicher Suche
            }
        }

        private void suchenbtn_Click(object sender, RoutedEventArgs e)
        {
            Searchaction();
        }
    }
}
