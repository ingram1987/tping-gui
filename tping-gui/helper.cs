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
    public class helper
    {
        public static int _pingCountValue = 0; //Number of pings to run
        public MainWindow _window;
        public DispatcherTimer _t1 = new DispatcherTimer();
        private int _pingCount = 0; //Number of pings that have been ran
        public int _pingSum = 0; //Sum of all pings RoundTripTime
        private string fileName = null; //Temporary location to store the ping data in the output window
        private static StreamWriter processedData;
        
        
        public void PingHost(MainWindow window, int pingValue)
        {
            fileName = Path.GetTempFileName();
            _window = window;
            _pingCountValue = pingValue;
            _pingSum = 0;
            _window.avgPingTime.Content = null;
            _window.numberOfPings.Content = null;
            //Checks to see if IPHostnameValue is in a valid IP or Hostname format, and then starts timedEvent
            if(Uri.CheckHostName(MainWindow.IpHostnameValue) != UriHostNameType.Unknown)
            {
                _window.StartPing.IsEnabled = false;
                _t1.Start();
                _window.Output.Focus(); //Sets the focus to the output window, so it will scroll with the results
            }
            else
            {
                MessageBox.Show("Please enter a valid hostname or IP");
            }
            

        }

        public void createTimer()
        {
            _t1.Tick += new EventHandler(timedEvent);
            _t1.Interval = new TimeSpan(0, 0, 2);

        }
        public void timedEvent(object sender, EventArgs e)
        {
            _pingCount++;
            processedData = new StreamWriter(@fileName, true);
            _window.progress.Maximum = _pingCountValue; //Sets maximum value for the progress bar
            _window.stop.IsEnabled = true; //Enables the stop button
            //Runs a ping until the pingCountValue is reached
            if(_pingCount <= _pingCountValue)
            {
                
                Ping ping = new Ping();
                PingReply pingReply = ping.Send(MainWindow.IpHostnameValue);
                _pingSum += Convert.ToInt32(pingReply.RoundtripTime);
                string pingCurrentLine = String.Format("{0}, " + "{1}, " + "{2}, " + DateTime.Now.TimeOfDay, MainWindow.IpHostnameValue, pingReply.RoundtripTime, pingReply.Status);
                processedData.WriteLine(pingCurrentLine); //Sends ping data to the output window
                _window.Output.AppendText(pingCurrentLine + Environment.NewLine);
                _window.avgPingTime.Content = _pingSum / _pingCount; //Updates the average ping RoundtripTime
                _window.numberOfPings.Content = _pingCount; //Updates the number of pings ran
                _window.progress.Value = _pingCount; //Updates the progress bar with the number of pings ran
                _window.Output.SelectionStart = int.MaxValue; //Forces the output window to scroll down
                processedData.Close();
                
                
            }
            //Stop pinging when pingCountValue is reached
            else
            {
                _t1.Stop();
                _window.Output.AppendText("Finished" + Environment.NewLine);
                _window.stop.IsEnabled = false; //Disables stop button
                _window.StartPing.IsEnabled = true; //Enables start button
                _pingCount = 0; //Resets pingCount to 0
                
            }
        }
        //Clears output window and statistics
        public void ClearForm(MainWindow window)
        {
            _window = window;
            _window.Output.Text = string.Empty; //Clears output window
        }
        //Stops pinging
        public void stopPing()
        {
            _t1.Stop();
            _window.Output.AppendText("Finished" + Environment.NewLine);
            _window.stop.IsEnabled = false; //Disables stop button
            _window.StartPing.IsEnabled = true; //Enables start button
            _pingCount = 0; //Sets pingCount to 0
            
        }
        //Exports the data from the output window to a csv file
        public void saveCSV()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "CSV|*.csv";
            saveFileDialog1.Title = "Export to CSV";

            string dateString = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss");
            saveFileDialog1.FileName = dateString; //Sets default save as file name to current date and time
            Nullable<bool> saveFileResult = saveFileDialog1.ShowDialog();
            //If saveFileResut is null (user clicked cancel) then do nothing
            if (saveFileResult == false)
            {

            }
            //Copy the temporary file with the ping data to a csv file
            else
            {
                File.Copy(fileName, saveFileDialog1.FileName);
            }
        }
    }  
}
