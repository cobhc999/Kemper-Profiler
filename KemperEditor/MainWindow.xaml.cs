using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Midi;


namespace KemperEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Instanzvariablen
        Midi.OutputDevice outputDevice = Midi.OutputDevice.InstalledDevices[1];
        Midi.InputDevice inputDevice = Midi.InputDevice.InstalledDevices[0];


        public MainWindow()
        {
            InitializeComponent();
            if (inputDevice.IsReceiving)
            {
                inputDevice.StopReceiving();
            }
            
            if (outputDevice.IsOpen)
            {
                outputDevice.Close();
            }
           

            if (inputDevice.IsOpen)
            {
                inputDevice.Close();
            }

            outputDevice.Open();
            inputDevice.Open();
            
           
        }


        private void b_Tuner_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void b_A_Click(object sender, RoutedEventArgs e)
        {
            LED_A.IsChecked = true;

        }

        void b_Stomps_Click(object sender, RoutedEventArgs e)
        {
           
            
            
            outputDevice.SendControlChange(Channel.Channel1, Midi.Control.Tuner, 1);

            var sevenItems = new byte[] { 0xF0, 0x00, 0x20, 0x33, 0x02, 0x7F, 0x03, 0x00, 0x00, 0x01, 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x00, 0xF7 }; //Hello Rigname
            //var sevenItems = new byte[] { 0xF0, 0x00, 0x20, 0x33, 0x02, 0x7F, 0x01, 0x00, 0x4A, 0x04, 0x40,0x00, 0xF7 }; //Delay auf 0dB/50%

            outputDevice.SendSysEx(sevenItems);

        }

        private void b_B_Click(object sender, RoutedEventArgs e)
        {
            inputDevice.SysEx += new Midi.InputDevice.SysExHandler(SysEx);
            inputDevice.StartReceiving(null);
            outputDevice.SendControlChange(Channel.Channel1, Midi.Control.Tuner, 0);
            var sevenItems = new byte[] { 0xF0, 0x00, 0x20, 0x33, 0x02, 0x7F, 0x41, 0x00, 0x4A, 0x04, 0xF7 }; //Anfrage Delay Volume
            outputDevice.SendSysEx(sevenItems);



                

        }


        public void SysEx(SysExMessage msg)
        {
            Textpanel.Text = "Test";
            Textpanel.Text = msg.Device.Name.ToString();
            //THeoretische Antowrt:  SYSX: F0 00 20 33 00 00 01 00 4A 04 40 00 F7
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // Close the output device if we have one.
            if (outputDevice != null && outputDevice.IsOpen)
            {
                outputDevice.Close();
            }
            // Stop and close the input device if we have one.
            if (inputDevice != null && inputDevice.IsOpen)
            {
                if (inputDevice.IsReceiving)
                {
                    inputDevice.StopReceiving();
                }
                inputDevice.Close();
            }
        }

        private void b_MIDIsettings_Click(object sender, RoutedEventArgs e)
        {
            if (MidiPopup.IsOpen == true)  MidiPopup.IsOpen = false;
            else  MidiPopup.IsOpen = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MidiPopup.IsOpen = false;
        }


    }
}
