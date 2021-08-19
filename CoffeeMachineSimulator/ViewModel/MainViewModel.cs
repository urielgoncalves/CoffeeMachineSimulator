using CoffeeMachine.EventHub.Sender;
using CoffeeMachine.EventHub.Sender.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoffeeMachine.UI.ViewModel
{
    public class MainViewModel : BindableBase
    {
        private int _counterCappuccino;
        private int _counterEspresso;
        private string _city;
        private string _serialNumber;
        private readonly ICoffeeMachineDataSender _coffeeMachineDataSender;

        public MainViewModel(ICoffeeMachineDataSender coffeeMachineDataSender)
        {
            _coffeeMachineDataSender = coffeeMachineDataSender;
            SerialNumber = Guid.NewGuid().ToString().Substring(0, 8);
            MakeCappuccinoCommand = new DelegateCommand(MakeCappuccino);
            MakeEspressoCommand = new DelegateCommand(MakeEspresso);
            Logs = new ObservableCollection<string>();
        }

        public ICommand MakeCappuccinoCommand { get; }
        public ICommand MakeEspressoCommand { get; }

        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                RaisePropertyChanged();
            }
        }

        public string SerialNumber
        {
            get { return _serialNumber; }
            set
            {
                _serialNumber = value;
                RaisePropertyChanged();
            }
        }

        public int CounterCappuccino
        {
            get { return _counterCappuccino; }
            set
            {
                _counterCappuccino = value;
                RaisePropertyChanged();
            }
        }

        public int CounterEspresso
        {
            get { return _counterEspresso; }
            set
            {
                _counterEspresso = value;
                RaisePropertyChanged();
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

        private void WriteLog(string text)
        {
            //Insert always on the top of the list view
            Logs.Insert(0, text);
        }
    }
}
