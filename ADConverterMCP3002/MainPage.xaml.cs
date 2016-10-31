using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.Devices.Spi;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ADConverterMCP3002
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private SpiDevice _spiDevice;
        private DispatcherTimer _timer;

        public MainPage()
        {
            this.InitializeComponent();

            InitSpi();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private async void InitSpi()
        {
            try
            {
                var deviceSelector = SpiDevice.GetDeviceSelector("SPI0");
                var deviceInfo = await DeviceInformation.FindAllAsync(deviceSelector);
                var spiSettings = new SpiConnectionSettings(0x00);
                _spiDevice = await SpiDevice.FromIdAsync(deviceInfo[0].Id, spiSettings);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            byte[] readBuffer = new byte[3];
            byte[] writeBuffer = new byte[3] { 0x68, 0x00, 0x00 };

            _spiDevice.TransferFullDuplex(writeBuffer, readBuffer);
            var val = readBuffer[0] & 0x03;
            val <<= 8;
            val += readBuffer[1];

            DigitalValue.Text = val.ToString();
        }
    }
}
