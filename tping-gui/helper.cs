using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net.NetworkInformation;
using System.IO;
using System.Timers;
using System.Windows.Threading;
using Microsoft.Win32;


namespace tping_gui
{
    public class Helper
    {
        public static int _pingCountValue = 0; //Number of pings to run
        public MainWindow _window;
        public DispatcherTimer _timer1 = new DispatcherTimer();
        private int _completedPingCounter = 0;
        public int _pingRoundTripSum = 0;
        private string _temporaryPingDataFile = null;
        private static StreamWriter _processedData;
        
        
        public void PingHost(MainWindow window, int pingValue)
        {
            _temporaryPingDataFile = Path.GetTempFileName();
            _window = window;
            _pingCountValue = pingValue;
            _pingRoundTripSum = 0;
            _window.avgPingTime.Content = null;
            _window.numberOfPings.Content = null;
            //Checks to see if IPHostnameValue is in a valid IP or Hostname format, and then starts timedEvent
            if(Uri.CheckHostName(MainWindow.ipHostnameValue) != UriHostNameType.Unknown)
            {
                _window.StartPing.IsEnabled = false;
                _timer1.Start();
                //Sets the focus to the output window, so it will scroll with the results
                _window.Output.Focus();
            }
            else
            {
                MessageBox.Show("Please enter a valid hostname or IP");
            }
            

        }

        public void CreateTimer()
        {
            _timer1.Tick += new EventHandler(TimedPingEvent);
            _timer1.Interval = new TimeSpan(0, 0, 2);

        }
        public void TimedPingEvent(object sender, EventArgs e)
        {
            _completedPingCounter++;
            _processedData = new StreamWriter(_temporaryPingDataFile, true);
            _window.progress.Maximum = _pingCountValue;
            _window.stop.IsEnabled = true;
            //Runs a ping until the pingCountValue is reached
            if(_completedPingCounter <= _pingCountValue)
            {
                
                Ping ping = new Ping();
                PingReply pingReply = ping.Send(MainWindow.ipHostnameValue);
                _pingRoundTripSum += Convert.ToInt32(pingReply.RoundtripTime);
                string pingCurrentLine = String.Format("{0}, " + "{1}, " + "{2}, " + DateTime.Now.TimeOfDay, MainWindow.ipHostnameValue, pingReply.RoundtripTime, pingReply.Status);
                _processedData.WriteLine(pingCurrentLine);
                _window.Output.AppendText(pingCurrentLine + Environment.NewLine);
                _window.avgPingTime.Content = _pingRoundTripSum / _completedPingCounter;
                _window.numberOfPings.Content = _completedPingCounter;
                _window.progress.Value = _completedPingCounter;
                //Forces the output window to scroll down
                _window.Output.SelectionStart = int.MaxValue;
                _processedData.Close();
                
                
            }
            //Stop pinging when pingCountValue is reached
            else
            {
                _timer1.Stop();
                _window.Output.AppendText("Finished" + Environment.NewLine);
                _window.stop.IsEnabled = false;
                _window.StartPing.IsEnabled = true;
                _completedPingCounter = 0;
                
            }
        }
        //Clears output window and statistics
        public void ClearOutputWindow(MainWindow window)
        {
            window.Output.Text = string.Empty; //Clears output window
        }
        //Stops pinging
        public void stopPing()
        {
            _timer1.Stop();
            _window.Output.AppendText("Finished" + Environment.NewLine);
            _window.stop.IsEnabled = false;
            _window.StartPing.IsEnabled = true;
            _completedPingCounter = 0;
            
        }
        //Exports the data from the output window to a csv file
        public void ExportToCSV()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "CSV|*.csv";
            saveFileDialog1.Title = "Export to CSV";

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss");
            saveFileDialog1.FileName = currentDateTime;
            Nullable<bool> saveFileResult = saveFileDialog1.ShowDialog();
            //If saveFileResut is null (user clicked cancel) then do nothing
            if (saveFileResult == false)
            {
                return;
            }
            //Copy the temporary file with the ping data to a csv file
            else
            {
                File.Copy(_temporaryPingDataFile, saveFileDialog1.FileName);
            }
        }
    }  
}
