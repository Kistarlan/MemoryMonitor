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
using System.Collections.ObjectModel;
using MahApps.Metro.Controls;

namespace MemoryMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow :Window
    {
        ObservableCollection<KeyValuePair<DateTime, float>> valueList = new ObservableCollection<KeyValuePair<DateTime, float>>();
        private void showColumnChart()
        {
            //Setting data for line chart
           //CPUlineChart.DataContext = valueList;
        }

        public delegate void CPUEventHandler(object sender, float args);
        public event CPUEventHandler CPUEvent;

        DispatcherTimer timer = new DispatcherTimer();

        private static float cpuCurrent;

        void TriggerCPUEvent(float args)
        {
            if (CPUEvent != null)
            {
                CPUEvent(this, args);
            }
        }
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public void TimeChanged(object sender, EventArgs e)
        {
            cpuCurrent = (float)Data.GetCurrentCpuUsage();

            TriggerCPUEvent(cpuCurrent);

            valueList.Add(new KeyValuePair<DateTime, float>(DateTime.Now, cpuCurrent));
        }

        
        
        //PerformanceCounter perfSwapFileCounter = new PerformanceCounter("Swap File", "% Usage", "\\??\\C:\\paqefile.sys");
        GetData Data = new GetData();
        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0,0,0,0,10);
            //StartUpTimeText.Text = " " + Data.GetTimeWorkSystem() + " Hours";
            StartUpTimeText1.Text = " " + Data.GetTimeWorkSystem();
            //MessageBox.Show("Cashe: " + Data.GetCasheMemory() + 
            //    "\nCommited mamory: " + Data.GetCommitedGB() + "/" + Data.GetMaxCommitedGB());
            ComputerName.Text ="Computer name: " + Environment.MachineName;
            UserName.Text = "User name: " + Environment.UserName;
            RAMTitle.ChartSubTitle = "Total RAM: " + Data.GetTotalRAM() + "GB";
            CpuSpeed.ChartSubTitle ="Maximum speed: " + Data.CPUSpeed().ToString() + " GHz";
            dispatcherTimer.Start();
        }


        private void System_Data()
        {
            MessageBox.Show("CPU: " + Data.GetCurrentCpuUsage() + "\nPhysical Memory: " + Data.GetPhysicalMemoryName() +
                "\nVideo Controller: " + Data.GetVideoControllerName() +
                "\nDisk mamory: " + Data.GetDiskSize() +
                "\nFree space: " + Data.GetFreeDiskSpace() +
                "\nCharge Status: " + Data.GetChargeStatus() +
                "\nRemaining Time: " + Data.GetChargeTime() +
                "\nPool Page Gb: " + Data.GetPageMemory() +
                "\nNon Pool Page Gb: " + Data.GetNPageMemory());
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //CPUProgressBar.Value = Data.GetCurrentCpuUsage();
            //TextCPU.Text = System.String.Format("{0,3:N2}%",CPUProgressBar.Value);
            //RAMProgressBar.Value = Data.GetCommitedInUse();
            //TextRAM.Text = System.String.Format("{0,3:N2}%", RAMProgressBar.Value);
        }

    }
}
