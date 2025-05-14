using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Powerscan
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<AdressEntry> Daten { get; set; } = new();
        private List<AdressEntry> _allEntries = new();
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _entryId;
        private string _zipCode;
        private string _city;
        private string _street;
        private string _housenumber;
        private string _building;
        private string _floor;
        private string _room;
        private string _meterId;
        private string _consumption;
        private double _costs;
        private double _costsCalculation;
        private double _consumptionSinceReset;
        private int _daysSinceReset;
        private AdressEntry _selectedEntry;
        // for realteim consutmpion simulation
        private DispatcherTimer _consumptionTimer;
        private Random _random = new Random();

        public int EntryId
        {
            get => _entryId;
            set
            {
                if (_entryId != value)
                {
                    _entryId = value;
                    OnPropertyChanged(nameof(EntryId));
                }
            }
        }
        public string ZipCode
        {
            get => _zipCode;
            set
            {
                if (_zipCode != value)
                {
                    _zipCode = value;
                    OnPropertyChanged(nameof(ZipCode));
                }
            }
        }
        public string City
        {
            get => _city;
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged(nameof(City));
                }
            }
        }
        public string Street
        {
            get => _street;
            set
            {
                if (_street != value)
                {
                    _street = value;
                    OnPropertyChanged(nameof(Street));
                }
            }
        }
        public string Housenumber
        {
            get => _housenumber;
            set
            {
                if (_housenumber != value)
                {
                    _housenumber = value;
                    OnPropertyChanged(nameof(Housenumber));
                }
            }
        }
        public string Building
        {
            get => _building;
            set
            {
                if (_building != value)
                {
                    _building = value;
                    OnPropertyChanged(nameof(Building));
                }
            }
        }
        public string Floor
        {
            get => _floor;
            set
            {
                if (_floor != value)
                {
                    _floor = value;
                    OnPropertyChanged(nameof(Floor));
                }
            }
        }
        public string Room
        {
            get => _room;
            set
            {
                if (_room != value)
                {
                    _room = value;
                    OnPropertyChanged(nameof(Room));
                }
            }
        }
        public string MeterId
        {
            get => _meterId;
            set
            {
                if (_meterId != value)
                {
                    _meterId = value;
                    OnPropertyChanged(nameof(MeterId));
                }
            }
        }
        public double Costs
        {
            get => _costs;
            set
            {
                if (_costs != value)
                {
                    _costs = value;
                    OnPropertyChanged(nameof(Costs));
                }
            }
        }
        public double CostsCalculation
        {
            get => _costsCalculation;
            set
            {
                if (_costsCalculation != value)
                {
                    _costsCalculation = value;
                    OnPropertyChanged(nameof(CostsCalculation));
                }
            }
        }
        public double ConsumptionSinceReset
        {
            get => _consumptionSinceReset;
            set
            {
                if (_consumptionSinceReset != value)
                {
                    _consumptionSinceReset = value;
                    OnPropertyChanged(nameof(ConsumptionSinceReset));
                }
            }
        }
        public int DaysSinceReset
        {
            get => _daysSinceReset;
            set
            {
                if (_daysSinceReset != value)
                {
                    _daysSinceReset = value;
                    OnPropertyChanged(nameof(DaysSinceReset));
                }
            }
        }
        public AdressEntry SelectedEntry
        {
            get => _selectedEntry;
            set
            {
                if (_selectedEntry != value)
                {
                    _selectedEntry = value;
                    OnPropertyChanged(nameof(SelectedEntry));

                    if (SelectedEntry != null)
                    {
                        double currennt = _selectedEntry.TotalConsumbtion;
                        CostsCalculation = CalculateCostsSinceReset(_selectedEntry, currennt);
                        double diff = SelectedEntry.TotalConsumbtion - SelectedEntry.LastResetConsumption;
                        ConsumptionSinceReset = diff >= 0 ? Math.Round(diff, 2) : 0;
                        if (SelectedEntry.LastResetDateTime > DateTime.MinValue)
                        {
                            DaysSinceReset = (int)(DateTime.Now - SelectedEntry.LastResetDateTime).TotalDays;
                        }
                        else
                        {
                            DaysSinceReset = 0;
                        }
                    }
                    else
                    {
                        CostsCalculation = 0;
                    }
                }
            }
        }


        public void SaveData()
        {
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string folderPath = Path.Combine(basePath, "Powerscan");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, "Data.json");

            List<AdressEntry> entrys = new();
            if (File.Exists(filePath))
            {
                string jsonOld = File.ReadAllText(filePath);
                try
                {
                    var loaded = JsonSerializer.Deserialize<List<AdressEntry>>(jsonOld);
                    if (loaded != null)
                        entrys = loaded;
                }
                catch
                {
                    MessageBox.Show("Fehler beim Lesen der Datei.");
                }
            }

            // Vor dem Hinzufügen prüfen, ob es den Eintrag schon gibt
            if (entrys.Any(e =>
                !string.IsNullOrWhiteSpace(e.MeterId) &&
                !string.IsNullOrWhiteSpace(this.MeterId) &&
                e.MeterId.Trim().Equals(this.MeterId.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Diesen Eintrag gibt es schon und kann nicht noch mal gespeichert werden!");
                return;
            }

            var newEntry = new AdressEntry
            {
                EntryId = BaseEntity.GetNextId(),
                ZipCode = this.ZipCode?.Trim(),
                City = this.City?.Trim(),
                Street = this.Street?.Trim(),
                Housenumber = this.Housenumber?.Trim(),
                Building = this.Building?.Trim(),
                Floor = this.Floor?.Trim(),
                Room = this.Room?.Trim(),
                MeterId = this.MeterId?.Trim(),
                Costs = this.Costs,
                CostsPerKWh = this.Costs,
                CreatedAt = DateTime.Now,
                TotalConsumbtion = Math.Round(new Random().NextDouble() * 1800 + 200, 2)
            };

            entrys.Add(newEntry);
            _allEntries.Add(newEntry);
            string jsonNew = JsonSerializer.Serialize(entrys, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonNew);

            Daten.Add(newEntry); // Daten ist dein ObservableCollection
            MessageBox.Show("Daten wurden gespeichert.");
        }



        public void ShowData()
        {
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string folerPath = Path.Combine(basePath, "Powerscan");
            string filePath = Path.Combine(folerPath, "Data.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var entry = JsonSerializer.Deserialize<AdressEntry>(json);

                if (entry != null)
                {
                    this.ZipCode = entry.ZipCode;
                    this.City = entry.City;
                    this.Street = entry.Street;
                    this.Housenumber = entry.Housenumber;
                    this.Building = entry.Building;
                    this.Floor = entry.Floor;
                    this.Room = entry.Room;
                    this.MeterId = entry.MeterId;
                    this.Costs = entry.Costs;
                }
            }
            else
            {
                MessageBox.Show("Eintrag wurde nicht gefunden!!");
            }
        }
        public void LoadData()
        {
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string folderPath = Path.Combine(basePath, "Powerscan");
            string filePath = Path.Combine(folderPath, "Data.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);

                try
                {
                    // Versuch: Liste von Einträgen laden
                    var entrys = JsonSerializer.Deserialize<List<AdressEntry>>(json);

                    if (entrys != null)
                    {
                        _allEntries = entrys;
                        Daten.Clear();
                        foreach (var entry in entrys)
                        {
                            Daten.Add(entry);
                        }
                        if (_allEntries.Any())
                        {
                            int maxId = entrys.Max(e => e.EntryId);
                            BaseEntity.SetNextId(maxId + 1);
                        }

                        return;
                    }
                }
                catch (JsonException)
                {
                    // Fallback: A single entry (e.g., an old file)
                    try
                    {
                        var einzelner = JsonSerializer.Deserialize<AdressEntry>(json);
                        if (einzelner != null)
                        {
                            Daten.Clear();
                            Daten.Add(einzelner);
                            BaseEntity.SetNextId(einzelner.EntryId + 1);

                        }
                    }
                    catch
                    {
                        MessageBox.Show("Fehler beim Laden der Daten. Die Datei ist ungültig.", "Fehler");
                    }
                }
            }
        }



        public void DeleteSelectedEntry()
        {
            if (SelectedEntry == null)
            {
                MessageBox.Show("Kein Eintrag ausgewählt zum Löschen.");
                return;
            }

            if (MessageBox.Show("Möchtest du den ausgewählten Eintrag wirklich löschen?",
                "Löschen bestätigen", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Daten.Remove(SelectedEntry);
                _allEntries.Remove(SelectedEntry);

                // JSON-Datei aktualisieren
                string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string folderPath = Path.Combine(basePath, "Powerscan");
                string filePath = Path.Combine(folderPath, "Data.json");

                string json = JsonSerializer.Serialize(Daten.ToList(), new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);

                SelectedEntry = null;
                MessageBox.Show("Eintrag erfolgreich gelöscht.");
            }
        }
        //Resets the Calculation of Costs when consumption is setting back
        public double CalculateCostsSinceReset(AdressEntry entry, double currentMeterValue)
        {
            entry.TotalConsumbtion = currentMeterValue;

            double differenc = currentMeterValue - entry.LastResetConsumption;
            if (differenc < 0) differenc = 0; // if calcilation is reset

            double kosten = Math.Round(differenc * entry.CostsPerKWh, 2);
            return kosten;
        }

        private void CalculateCosts()
        {
            if (SelectedEntry != null)
            {
                double currentConsumptionOfDevice = 825.4;

                double costs = CalculateCostsSinceReset(SelectedEntry, currentConsumptionOfDevice);
            }
        }

        public void SetCounterBack()
        {
            if (SelectedEntry != null)
            {
                SelectedEntry.LastResetConsumption = SelectedEntry.TotalConsumbtion;
                SelectedEntry.LastResetDateTime = DateTime.Now;

                // Saves List Without new entry
                string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string folderPath = Path.Combine(basePath, "Powerscan");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string filePath = Path.Combine(folderPath, "Data.json");

                string json = JsonSerializer.Serialize(Daten.ToList(), new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);

                MessageBox.Show("Zähler wurde zurückgesetzt.");
            }
        }
        /// <summary>
        /// Simulation for random KWh Consumption
        /// </summary>
        public void RunConsumptionSimulationForAll()
        {
            Random rnd = new Random();

            foreach (var entry in Daten.Where(e => e.TotalConsumbtion == 0))
            {

                entry.TotalConsumbtion = Math.Round(rnd.NextDouble() * 1800 + 200, 2);
            }


            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string folderPath = Path.Combine(basePath, "Powerscan");
            string filePath = Path.Combine(folderPath, "Data.json");

            string json = JsonSerializer.Serialize(Daten.ToList(), new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);

            MessageBox.Show("Zufälliger Stromverbrauch wurde allen Einträgen zugewiesen.");
        }

        //Starting timer for simulation
        public void StartConsumptionSimulation()
        {
            _consumptionTimer = new DispatcherTimer();
            _consumptionTimer.Interval = TimeSpan.FromSeconds(5); // every 2 seconds
            _consumptionTimer.Tick += ConsumptionTimer_Tick;
            _consumptionTimer.Start();
        }

        //Timer 
        private void ConsumptionTimer_Tick(object sender, EventArgs e)
        {
            foreach (var entry in Daten)
            {
                //Simulating a smal consumption increase
                double consumptionIncrease = Math.Round(_random.NextDouble() * 0.02 + 0.01, 2);
                entry.TotalConsumbtion = Math.Round(entry.TotalConsumbtion + consumptionIncrease, 2);
            }
            SaveAllEntries();
            if (SelectedEntry != null)
            {
                double current = SelectedEntry.TotalConsumbtion;
                CostsCalculation = CalculateCostsSinceReset(SelectedEntry, current);
                double diff = current - SelectedEntry.LastResetConsumption;
                ConsumptionSinceReset = diff >= 0 ? Math.Round(diff, 2) : 0;
                if (SelectedEntry.LastResetDateTime > DateTime.MinValue)
                {
                    DaysSinceReset = (int)(DateTime.Now - SelectedEntry.LastResetDateTime).TotalDays;
                }
                else
                {
                    DaysSinceReset = 0;
                }
            }
        }

        public void SaveAllEntries()
        {
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string folderPath = Path.Combine(basePath, "Powerscan");
            string filePath = Path.Combine(folderPath, "Data.json");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string json = JsonSerializer.Serialize(Daten.ToList(), new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public List<AdressEntry> GetAllEntries()
        {
            return _allEntries.ToList(); // eine Kopie, keine direkte Referenz
        }
        public void ShowAllEntrys()
        {
            MessageBox.Show($"_allEntries enthält: {_allEntries.Count} Einträge");
            Daten.Clear();
            foreach (var eintrag in _allEntries)
            {
                Daten.Add(eintrag);
            }
        }
    }
}
