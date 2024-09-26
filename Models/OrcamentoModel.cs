using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoMaker3000.Models
{
    public class OrcamentoModel : INotifyPropertyChanged
    {
        private static OrcamentoModel _instance;

        public static OrcamentoModel Instance => _instance ?? (_instance = new OrcamentoModel());

        private int _numberOrc;
        private string _clientName;
        private string _clientPhone;
        private string _clientEmail;
        private string _serviceType;
        private DateTime _date;
        private string _schedule;
        private string _location;
        private int _duration;
        private decimal _cotation;
        private DateTime _creationDate;
        private Dictionary<int, decimal> _valuePerMusician;
        private int _numberMusicians = 6;
        private decimal _kilometerPrice = 0.2m;
        private decimal _distance;
        private decimal _travelExpenses;
        private decimal _totalBaseValue;
        private decimal _groupSavings = 0.10m;
        private decimal _managerSalary = 0.08m;
        private decimal _alimentationExpenses;
        private Dictionary<int, decimal> _extraSalary;


        public int NumberOrc
        {
            get => _numberOrc;
            set
            {
                _numberOrc = value;
                OnPropertyChanged(nameof(NumberOrc));
            }
        }
        public string ClientName
        {
            get => _clientName;
            set
            {
                _clientName = value;
                OnPropertyChanged(nameof(ClientName));
            }
        }
        public string ClientPhone
        {
            get => _clientPhone;
            set
            {
                _clientPhone = value;
                OnPropertyChanged(nameof(ClientPhone));
            }
        }
        public string ClientEmail
        {
            get => _clientEmail;
            set
            {
                _clientEmail = value;
                OnPropertyChanged(nameof(ClientEmail));
            }
        }
        public string ServiceType
        {
            get => _serviceType;
            set
            {
                _serviceType = value;
                OnPropertyChanged(nameof(ServiceType));
            }
        }
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }
        public string Schedule
        {
            get => _schedule;
            set
            {
                _schedule = value;
                OnPropertyChanged(nameof(Schedule));
            }
        }
        public string Location
        {
            get => _location;
            set
            {
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }
        public int Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }
        public decimal Cotation
        {
            get => _cotation;
            set
            {
                _cotation = value;
                OnPropertyChanged(nameof(Cotation));
            }
        }
        public DateTime CreationDate
        {
            get => _creationDate;
            set
            {
                _creationDate = value;
                OnPropertyChanged(nameof(CreationDate));
            }
        }
        public Dictionary<int, decimal> ValuePerMusician
        {
            get => _valuePerMusician;
            set
            {
                _valuePerMusician = value;
                OnPropertyChanged(nameof(ValuePerMusician));
            }
        }
        public int NumberMusicians
        {
            get => _numberMusicians;
            set
            {
                _numberMusicians = value;
                OnPropertyChanged(nameof(NumberMusicians));
            }
        }
        public decimal KilometerPrice
        {
            get => _kilometerPrice;
            set
            {
                _kilometerPrice = value;
                OnPropertyChanged(nameof(KilometerPrice));
            }
        }
        public decimal Distance
        {
            get => _distance;
            set
            {
                _distance = value;
                OnPropertyChanged(nameof(Distance));
            }
        }
        public decimal TravelExpenses
        {
            get => _travelExpenses;
            set
            {
                _travelExpenses = value;
                OnPropertyChanged(nameof(TravelExpenses));
            }
        }
        public decimal TotalBaseValue
        {
            get => _totalBaseValue;
            set
            {
                _totalBaseValue = value;
                OnPropertyChanged(nameof(TotalBaseValue));
            }
        }
        public decimal GroupSavings
        {
            get => _groupSavings;
            set
            {
                _groupSavings = value;
                OnPropertyChanged(nameof(GroupSavings));
            }
        }
        public decimal ManagerSalary
        {
            get => _managerSalary;
            set
            {
                _managerSalary = value;
                OnPropertyChanged(nameof(ManagerSalary));
            }
        }
        public decimal AlimentationExpenses
        {
            get => _alimentationExpenses;
            set
            {
                _alimentationExpenses = value;
                OnPropertyChanged(nameof(AlimentationExpenses));
            }
        }
        public Dictionary<int, decimal> ExtraSalary
        {
            get => _extraSalary;
            set
            {
                _extraSalary = value;
                OnPropertyChanged(nameof(ExtraSalary));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
