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

        private int _numberOrc = 1;
        private string _clientName;
        private string _clientPhone;
        private string _clientEmail;
        private string _serviceType;
        private DateTime _date;
        private string _schedule;
        private string _location;
        private int _duration;
        private double _cotation;
        private DateTime _creationDate;
        private Dictionary<int, double> _valuePerMusician;
        private int _numberMusicians = 6;
        private double _kilometerPrice = 0.20;
        private double _distance;
        private double _travelExpenses;
        private double _totalBaseValue;
        private double _groupSavings = 0.10;
        private double _managerSalary = 0.08;
        private double _alimentationExpenses;
        private double _extraExpenses;
        private string _savePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private Dictionary<int, double> _extraSalary;


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
        public double Cotation
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
        public Dictionary<int, double> ValuePerMusician
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
        public double KilometerPrice
        {
            get => _kilometerPrice;
            set
            {
                _kilometerPrice = value;
                OnPropertyChanged(nameof(KilometerPrice));
            }
        }
        public double Distance
        {
            get => _distance;
            set
            {
                _distance = value;
                OnPropertyChanged(nameof(Distance));
            }
        }
        public double TravelExpenses
        {
            get => _travelExpenses;
            set
            {
                _travelExpenses = value;
                OnPropertyChanged(nameof(TravelExpenses));
            }
        }
        public double TotalBaseValue
        {
            get => _totalBaseValue;
            set
            {
                _totalBaseValue = value;
                OnPropertyChanged(nameof(TotalBaseValue));
            }
        }
        public double GroupSavings
        {
            get => _groupSavings;
            set
            {
                _groupSavings = value;
                OnPropertyChanged(nameof(GroupSavings));
            }
        }
        public double ManagerSalary
        {
            get => _managerSalary;
            set
            {
                _managerSalary = value;
                OnPropertyChanged(nameof(ManagerSalary));
            }
        }
        public double AlimentationExpenses
        {
            get => _alimentationExpenses;
            set
            {
                _alimentationExpenses = value;
                OnPropertyChanged(nameof(AlimentationExpenses));
            }
        }
        public Dictionary<int, double> ExtraSalary
        {
            get => _extraSalary;
            set
            {
                _extraSalary = value;
                OnPropertyChanged(nameof(ExtraSalary));
            }
        }

        public double ExtraExpenses
        {
            get => _extraExpenses;
            set
            {
                _extraExpenses = value;
                OnPropertyChanged(nameof(ExtraExpenses));
            }
        }

        public string SavePath
        {
            get => _savePath;
            set
            {
                _savePath = value;
                OnPropertyChanged(nameof(SavePath));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
