using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace CarConfigurator
{
    public class CarConfiguration : INotifyPropertyChanged
    {
        private string _selectedModel = "Toyota Camry";
        public string SelectedModel
        {
            get { return _selectedModel; }
            set
            {
                if (_selectedModel != value)
                {
                    _selectedModel = value;
                    OnPropertyChanged("SelectedModel");
                    OnPropertyChanged("BasePrice");
                    OnPropertyChanged("TotalPrice");
                    OnPropertyChanged("DownPaymentAmount");
                    OnPropertyChanged("LoanAmount");
                    OnPropertyChanged("MonthlyPayment");
                }
            }
        }

        private string _selectedEngine = "Бензин";
        public string SelectedEngine
        {
            get { return _selectedEngine; }
            set
            {
                if (_selectedEngine != value)
                {
                    _selectedEngine = value;
                    OnPropertyChanged("SelectedEngine");
                    OnPropertyChanged("EnginePrice");
                    OnPropertyChanged("TotalPrice");
                    OnPropertyChanged("DownPaymentAmount");
                    OnPropertyChanged("LoanAmount");
                    OnPropertyChanged("MonthlyPayment");
                }
            }
        }

        private string _selectedColor = "Белый";
        public string SelectedColor
        {
            get { return _selectedColor; }
            set
            {
                if (_selectedColor != value)
                {
                    _selectedColor = value;
                    OnPropertyChanged("SelectedColor");
                    OnPropertyChanged("ColorPrice");
                    OnPropertyChanged("TotalPrice");
                    OnPropertyChanged("DownPaymentAmount");
                    OnPropertyChanged("LoanAmount");
                    OnPropertyChanged("MonthlyPayment");
                }
            }
        }

        private bool _leatherSeats;
        public bool LeatherSeats
        {
            get { return _leatherSeats; }
            set
            {
                if (_leatherSeats != value)
                {
                    _leatherSeats = value;
                    OnPropertyChanged("LeatherSeats");
                    OnPropertyChanged("OptionsPrice");
                    OnPropertyChanged("TotalPrice");
                    OnPropertyChanged("DownPaymentAmount");
                    OnPropertyChanged("LoanAmount");
                    OnPropertyChanged("MonthlyPayment");
                }
            }
        }

        private bool _sunroof;
        public bool Sunroof
        {
            get { return _sunroof; }
            set
            {
                if (_sunroof != value)
                {
                    _sunroof = value;
                    OnPropertyChanged("Sunroof");
                    OnPropertyChanged("OptionsPrice");
                    OnPropertyChanged("TotalPrice");
                    OnPropertyChanged("DownPaymentAmount");
                    OnPropertyChanged("LoanAmount");
                    OnPropertyChanged("MonthlyPayment");
                }
            }
        }

        private bool _navigation;
        public bool Navigation
        {
            get { return _navigation; }
            set
            {
                if (_navigation != value)
                {
                    _navigation = value;
                    OnPropertyChanged("Navigation");
                    OnPropertyChanged("OptionsPrice");
                    OnPropertyChanged("TotalPrice");
                    OnPropertyChanged("DownPaymentAmount");
                    OnPropertyChanged("LoanAmount");
                    OnPropertyChanged("MonthlyPayment");
                }
            }
        }

        private bool _heatedSeats;
        public bool HeatedSeats
        {
            get { return _heatedSeats; }
            set
            {
                if (_heatedSeats != value)
                {
                    _heatedSeats = value;
                    OnPropertyChanged("HeatedSeats");
                    OnPropertyChanged("OptionsPrice");
                    OnPropertyChanged("TotalPrice");
                    OnPropertyChanged("DownPaymentAmount");
                    OnPropertyChanged("LoanAmount");
                    OnPropertyChanged("MonthlyPayment");
                }
            }
        }

        public decimal BasePrice
        {
            get
            {
                switch (SelectedModel)
                {
                    case "Toyota Camry": return 2500000m;
                    case "BMW X5": return 5500000m;
                    case "Skoda Octavia": return 1800000m;
                    default: return 0m;
                }
            }
        }

        public decimal EnginePrice
        {
            get
            {
                switch (SelectedEngine)
                {
                    case "Бензин": return 0m;
                    case "Дизель": return 150000m;
                    case "Гибрид": return 300000m;
                    case "Электрический": return 500000m;
                    default: return 0m;
                }
            }
        }

        public decimal ColorPrice
        {
            get
            {
                switch (SelectedColor)
                {
                    case "Белый": return 0m;
                    case "Чёрный": return 25000m;
                    case "Серебристый": return 20000m;
                    case "Синий": return 15000m;
                    case "Красный": return 30000m;
                    default: return 0m;
                }
            }
        }

        public decimal OptionsPrice
        {
            get
            {
                decimal total = 0m;
                if (LeatherSeats) total += 150000m;
                if (Sunroof) total += 200000m;
                if (Navigation) total += 80000m;
                if (HeatedSeats) total += 50000m;
                return total;
            }
        }

        public decimal TotalPrice
        {
            get { return BasePrice + EnginePrice + ColorPrice + OptionsPrice; }
        }

        private decimal _downPaymentPercent = 20m;
        public decimal DownPaymentPercent
        {
            get { return _downPaymentPercent; }
            set
            {
                if (_downPaymentPercent != value && value >= 10 && value <= 90)
                {
                    _downPaymentPercent = value;
                    OnPropertyChanged("DownPaymentPercent");
                    OnPropertyChanged("DownPaymentAmount");
                    OnPropertyChanged("LoanAmount");
                    OnPropertyChanged("MonthlyPayment");
                }
            }
        }

        private int _loanTerm = 36;
        public int LoanTerm
        {
            get { return _loanTerm; }
            set
            {
                if (_loanTerm != value && value >= 12 && value <= 96)
                {
                    _loanTerm = value;
                    OnPropertyChanged("LoanTerm");
                    OnPropertyChanged("MonthlyPayment");
                }
            }
        }

        public decimal DownPaymentAmount
        {
            get { return Math.Round(TotalPrice * (DownPaymentPercent / 100m), 2); }
        }

        public decimal LoanAmount
        {
            get { return Math.Round(TotalPrice - DownPaymentAmount, 2); }
        }

        public decimal MonthlyPayment
        {
            get
            {
                if (LoanTerm <= 0 || LoanAmount <= 0) return 0m;

                double S = (double)LoanAmount;           
                double r = 8.0;                          
                double i = r / 100 / 12;             
                double n = LoanTerm;              

                double numerator = S * i * Math.Pow(1 + i, n);
                double denominator = Math.Pow(1 + i, n) - 1;

                if (denominator == 0) return 0m;

                double monthlyPayment = numerator / denominator;
                return Math.Round((decimal)monthlyPayment, 2);
            }
        }

        private string _customerName = "";
        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                if (_customerName != value)
                {
                    _customerName = value;
                    OnPropertyChanged("CustomerName");
                    OnPropertyChanged("IsFormValid");
                }
            }
        }

        private string _phone = "";
        public string Phone
        {
            get { return _phone; }
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    OnPropertyChanged("Phone");
                    OnPropertyChanged("IsFormValid");
                }
            }
        }

        private string _email = "";
        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged("Email");
                    OnPropertyChanged("IsFormValid");
                }
            }
        }

        public bool IsFormValid
        {
            get
            {
                if (string.IsNullOrWhiteSpace(CustomerName) || CustomerName.Length < 2)
                    return false;

                if (string.IsNullOrWhiteSpace(Phone) || Phone.Length < 10)
                    return false;

                foreach (char c in Phone)
                {
                    if (!char.IsDigit(c))
                        return false;
                }

                if (string.IsNullOrWhiteSpace(Email))
                    return false;

                if (!Email.Contains("@") || !Email.Contains("."))
                    return false;

                return true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string GetConfigurationSummary()
        {
            return $"Модель: {SelectedModel}\n" +
                   $"Двигатель: {SelectedEngine}\n" +
                   $"Цвет: {SelectedColor}\n" +
                   $"Опции: {(LeatherSeats ? "Кожаный салон " : "")}" +
                          $"{(Sunroof ? "Панорамная крыша " : "")}" +
                          $"{(Navigation ? "Навигация " : "")}" +
                          $"{(HeatedSeats ? "Подогрев сидений" : "")}\n" +
                   $"Базовая цена: {BasePrice:C}\n" +
                   $"Двигатель: +{EnginePrice:C}\n" +
                   $"Цвет: +{ColorPrice:C}\n" +
                   $"Опции: +{OptionsPrice:C}\n" +
                   $"ИТОГО: {TotalPrice:C}\n" +
                   $"Первоначальный взнос ({DownPaymentPercent}%): {DownPaymentAmount:C}\n" +
                   $"Сумма кредита: {LoanAmount:C}\n" +
                   $"Срок: {LoanTerm} месяцев\n" +
                   $"Ежемесячный платеж: {MonthlyPayment:C}";
        }
    }
}