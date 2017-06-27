using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Threading;
using System.Runtime.InteropServices;

namespace MemoryMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        
        //PerformanceCounter perfSwapFileCounter = new PerformanceCounter("Swap File", "% Usage", "\\??\\C:\\paqefile.sys");
        GetData Data = new GetData();

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }
        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0,0,0,0,100);
            LoadBarChartData();
            System_Data();
            dispatcherTimer.Start();
        }

        private void LoadBarChartData()
        {
            ((ColumnSeries)mcChart.Series[0]).ItemsSource =
                new KeyValuePair<string, int>[]{
        new KeyValuePair<string,int>("Project Manager", 12),
        new KeyValuePair<string,int>("CEO", 25),
        new KeyValuePair<string,int>("Software Engg.", 5),
        new KeyValuePair<string,int>("Team Leader", 6),
        new KeyValuePair<string,int>("Project Leader", 10),
        new KeyValuePair<string,int>("Developer", 4) };
        }

        private void System_Data()
        {
            textBlock1.Text = ("CPU: " + Data.Processor() + "\nPhysical Memory: " + Data.PhysicalMemoryName() +
                "\nVideo Controller: " + Data.VideoController()+
                "\nDisk mamory: " + Data.DiskSize() +
                "\nFree space: " + Data.FreeDiskSpace() +
                "\nCharge Status: " + Data.GetChargeStatus() +
                "\nRemaining Time: " + Data.GetChargeTime() +
                "\nPool Page Gb: " + Data.PageMemory() +
                "\nNon Pool Page Gb: " + Data.NPageMemory());
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            textBlock.Text = "CPU:" + " " + Data.CPUPercent() + "%" +
                "\n" + "Available Memory:" + " " + Data.AMemory() + "MB" +
                "\n" + "System Up Time:" + " " + Data.SysCounter() + " Hours";
        }

    }
}
