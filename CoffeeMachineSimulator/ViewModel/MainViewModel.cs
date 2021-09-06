using CoffeeMachine.EventHub.Model;
using CoffeeMachine.EventHub.Sender;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace CoffeeMachine.UI.ViewModel
{
    public class MainViewModel : BindableBase
    {
        private int _counterCappuccino;
        private int _counterEspresso;
        private string _city;
        private string _serialNumber;
        private int _boilerTemp;
        private int _beanLevel;
        private bool _isSendingPeriodically;
        private readonly ICoffeeMachineDataSender _coffeeMachineDataSender;
        private readonly DispatcherTimer _dispatcherTimer;

        public MainViewModel(ICoffeeMachineDataSender coffeeMachineDataSender)
        {
            _coffeeMachineDataSender = coffeeMachineDataSender;
            SerialNumber = Guid.NewGuid().ToString().Substring(0, 8);
            MakeCappuccinoCommand = new DelegateCommand(MakeCappuccino);
            MakeEspressoCommand = new DelegateCommand(MakeEspresso);
            Logs = new ObservableCollection<string>();
            _dispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
        }

        private async void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            CoffeeMachineData boilerTempData = CreateCoffeMachineData(nameof(BoilerTemp), BoilerTemp);
            CoffeeMachineData beanLevelData = CreateCoffeMachineData(nameof(BeanLevel), BeanLevel);

            await SendDataAsync(new[] { boilerTempData, beanLevelData });
        }

        public ICommand MakeCappuccinoCommand { get; }
        public ICommand MakeEspressoCommand { get; }

        public string City
        {
            get => _city;
            set
            {
                _city = value;
                RaisePropertyChanged();
            }
        }

        public string SerialNumber
        {
            get => _serialNumber;
            set
            {
                _serialNumber = value;
                RaisePropertyChanged();
            }
        }

        public int CounterCappuccino
        {
            get => _counterCappuccino;
            set
            {
                _counterCappuccino = value;
                RaisePropertyChanged();
            }
        }

        public int CounterEspresso
        {
            get => _counterEspresso;
            set
            {
                _counterEspresso = value;
                RaisePropertyChanged();
            }
        }

        public int BoilerTemp
        {
            get => _boilerTemp;
            set
            {
                _boilerTemp = value;
                RaisePropertyChanged();
            }
        }

        public int BeanLevel
        {
            get => _beanLevel;
            set
            {
                _beanLevel = value;
                RaisePropertyChanged();
            }
        }

        public bool IsSendingPeriodically
        {
            get => _isSendingPeriodically;
            set
            {
                if (_isSendingPeriodically != value)
                {
                    _isSendingPeriodically = value;

                    if (_isSendingPeriodically)
                    {
                        _dispatcherTimer.Start();
                    }
                    else
                    {
                        _dispatcherTimer.Stop();
                    }

                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<string> Logs { get; }

        private async void MakeCappuccino()
        {
            CounterCappuccino++;
            CoffeeMachineData coffeeMachineData = CreateCoffeMachineData(nameof(CounterCappuccino), CounterCappuccino);
            await SendDataAsync(coffeeMachineData);
        }

        private async void MakeEspresso()
        {
            CounterEspresso++;
            CoffeeMachineData coffeeMachineData = CreateCoffeMachineData(nameof(CounterEspresso), CounterEspresso);
            await SendDataAsync(coffeeMachineData);
        }

        private CoffeeMachineData CreateCoffeMachineData(string sensorType, int sensorValue)
        {
            return new()
            {
                City = City,
                SerialNumber = SerialNumber,
                SensorType = sensorType,
                SensorValue = sensorValue,
                RecordingTime = DateTime.Now
            };
        }

        private async Task SendDataAsync(CoffeeMachineData data)
        {
            try
            {
            await _coffeeMachineDataSender.SendDataAsync(data);
            WriteLog($"Sent data: {data}");

            }
            catch(Exception ex)
            {
                WriteLog($"Exception: {ex.Message}");
            }
        }

        private async Task SendDataAsync(IEnumerable<CoffeeMachineData> data)
        {
            try
            {
                await _coffeeMachineDataSender.SendDataAsync(data);

                foreach (CoffeeMachineData item in data)
                {
                    WriteLog($"Sent data: {item}");
                }
            }
            catch (Exception ex)
            {
                WriteLog($"Exception: {ex.Message}");
            }
        }

        private void WriteLog(string text)
        {
            //Insert always on the top of the list view
            Logs.Insert(0, text);
        }
    }
}
