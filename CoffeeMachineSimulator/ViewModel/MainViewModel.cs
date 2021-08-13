using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoffeeMachineSimulator.ViewModel
{
    public class MainViewModel : BindableBase
    {
        private int _counterCappuccino;
        private int _counterEspresso;
        private string _city;
        private string _serialNumber;

        public ICommand MakeCappuccinoCommand { get; }
        public ICommand MakeEspressoCommand { get; }

        public MainViewModel()
        {
            SerialNumber = Guid.NewGuid().ToString().Substring(0, 8);
            MakeCappuccinoCommand = new DelegateCommand(MakeCappuccino);
            MakeEspressoCommand = new DelegateCommand(MakeEspresso);
        }

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


        private void MakeCappuccino()
        {
            CounterCappuccino++;
        }

        private void MakeEspresso()
        {
            CounterEspresso++;
        }

    }
}
