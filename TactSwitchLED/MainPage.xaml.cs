using System;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TactSwitchLED
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const int INPUT_PIN =  24;
        private const int OUTPUT_PIN = 25;
        private GpioPin _inputPin;
        private GpioPin _outputPin;
        private DispatcherTimer _timer;

        public MainPage()
        {
            this.InitializeComponent();

            if (InitGPIO())
            {
                _timer = new DispatcherTimer();
                _timer.Interval = TimeSpan.FromMilliseconds(500);
                _timer.Tick += _timer_Tick;
                _timer.Start();
            }
        }
   
        private bool InitGPIO()
        {
            var gpioController = GpioController.GetDefault();
            if (gpioController == null)
            {
                return false;
            }

            _inputPin = gpioController.OpenPin(INPUT_PIN);
            _inputPin.Write(GpioPinValue.Low);
            _inputPin.SetDriveMode(GpioPinDriveMode.Input);

            _outputPin = gpioController.OpenPin(OUTPUT_PIN);
            _outputPin.Write(GpioPinValue.Low);
            _outputPin.SetDriveMode(GpioPinDriveMode.Output);

            return true;
        }

        private void _timer_Tick(object sender, object e)
        {
            var inputVal = _inputPin.Read();
            if (inputVal == GpioPinValue.High)
            {
                _outputPin.Write(GpioPinValue.High);
                ButtonStatus.Text = "Tact switch DOWN";
            }
            else
            {
                _outputPin.Write(GpioPinValue.Low);
                ButtonStatus.Text = "Tact switch UP";
            }
        }
    }
}
