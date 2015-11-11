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
using System.IO;
using System.Net.NetworkInformation;
using System.Windows.Threading;


namespace tping_gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    public partial class MainWindow : Window
    {
        public static string ipHostnameValue;
        private Helper callPing = new Helper();
        
        public MainWindow()
        {
            InitializeComponent();
            callPing.CreateTimer();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            
        }
        
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            //Start pinging
            ipHostnameValue = IP.Text;
            var requestedPings = int.Parse(pingCount.Text);
            callPing.ClearOutputWindow(this);
            callPing.PingHost(this, requestedPings);
        }

        public void Output_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            //Clears the output window
            callPing.ClearOutputWindow(this);
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            //Stops pinging
            callPing.stopPing();
        }

        private void exportCSV_Click(object sender, RoutedEventArgs e)
        {
            //Exports the output windows to a CSV file
            callPing.ExportToCSV();
        }
    }
}
