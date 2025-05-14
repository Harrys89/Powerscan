using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powerscan
{
    public class AdressEntry : BaseEntity, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        private string _zipCode;
        private string _city;
        private string _street;
        private string _housenumber;
        private string _building;
        private string _floor;
        private string _room;
        private string _meterId;
        private double _costs;
        private double _costsPerKwh;
        private double _totalConsumbtion;
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
                if (_street  != value)
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
        public string MeterId { get; set; }
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
        public DateTime CreatedAt { get; set; }
        public double LastResetConsumption { get; set; }
        public DateTime LastResetDateTime { get; set; }
        public double CostsPerKWh {  get; set; }
        

        public double TotalConsumbtion
        {
            get => _totalConsumbtion;
            set
            {
                if (_totalConsumbtion != value)
                {
                    _totalConsumbtion = value;
                    OnPropertyChanged(nameof(TotalConsumbtion));
                }
            }
        }
    }
}
