using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Pwm;
using Windows.Devices.Pwm.Provider;
using Windows.Devices.Lights;
using Microsoft.IoT.Lightning.Providers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LEDbyPWM
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const int OUTPUT_PIN = 25;
        private PwmController _pwmController;
        private PwmPin _outputPin;

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

                _outputPin = _pwmController.OpenPin(22);
                _outputPin.SetActiveDutyCyclePercentage(0.5);
                _outputPin.Start();
            }
        }

        private void DutyCycleSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (_pwmController != null)
            {
                var slider = (Slider)sender;
                var value = slider.Value;
                _outputPin.SetActiveDutyCyclePercentage(value);
            }
        }
    }
}
