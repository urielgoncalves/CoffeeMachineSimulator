using CoffeeMachine.EventHub.Sender;
using CoffeeMachine.UI.ViewModel;
using System.Configuration;
using System.Windows;

namespace CoffeeMachine.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            string eventHubConnectionString = ConfigurationManager.AppSettings.Get("EventHubConnectionString");
            DataContext = new MainViewModel(new CoffeeMachineDataSender(eventHubConnectionString)); //TODO: ADD DEPENDENCY INJECTION
        }
    }
}
