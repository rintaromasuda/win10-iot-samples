using Microsoft.IoT.Lightning.Providers;
using System;
using Windows.Devices;
using Windows.Devices.Pwm;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ServoSg90
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const int OUTPUT_PIN = 18;
        private PwmController _pwmController;
        private PwmPin _outputPin;

        private double _pwmCycle = 20.0; // http://akizukidenshi.com/catalog/g/gM-08761/
        private double _minPulse = 0.5;  // http://akizukidenshi.com/catalog/g/gM-08761/
        private double _maxPulse = 2.4;  // http://akizukidenshi.com/catalog/g/gM-08761/

        public MainPage()
        { 
            this.InitializeComponent();

            InitGPIO();
        }

        private async void InitGPIO()
        {
            if (LightningProvider.IsLightningEnabled)
            {
                LowLevelDevicesController.DefaultProvider = LightningProvider.GetAggregateProvider();
                var pwmControllers = await PwmController.GetControllersAsync(LightningPwmProvider.GetPwmProvider());
                _pwmController = pwmControllers[1];

                _outputPin = _pwmController.OpenPin(18);
                _outputPin.SetActiveDutyCyclePercentage(0.012);
                _outputPin.Start();
            }
        }

        private void Left90Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var percent = _minPulse / _pwmCycle;
            _outputPin.SetActiveDutyCyclePercentage(percent);
        }

        private void Left45Buttono_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var length = (((_minPulse + _maxPulse) / 2) + _minPulse) / 2;
            var percent = length / _pwmCycle;
            _outputPin.SetActiveDutyCyclePercentage(percent);
        }

        private void CenterButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var length = (_minPulse + _maxPulse) / 2;
            var percent = length / _pwmCycle;
            _outputPin.SetActiveDutyCyclePercentage(percent);
        }

        private void Right45Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var length = (((_minPulse + _maxPulse) / 2) + _maxPulse) / 2;
            var percent = length / _pwmCycle;
            _outputPin.SetActiveDutyCyclePercentage(percent);
        }

        private void Right90Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var percent = _maxPulse / _pwmCycle;
            _outputPin.SetActiveDutyCyclePercentage(percent);
        }
    }
}
