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
       
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        GetData Data = new GetData();
        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0,0,0,0,100);
            //StartUpTimeText.Text = " " + Data.GetTimeWorkSystem() + " Hours";
            StartUpTimeText1.Text = " " + Data.GetTimeWorkSystem();
            //MessageBox.Show("Cashe: " + Data.GetCasheMemory() + 
            //    "\nCommited mamory: " + Data.GetCommitedGB() + "/" + Data.GetMaxCommitedGB());
            ComputerName.Text ="Computer name: " + Environment.MachineName;
            UserName.Text = "User name: " + Environment.UserName;
            RAMTitle.ChartSubTitle = "Total RAM: " + Data.GetTotalRAM() + "GB";
            CpuSpeed.ChartSubTitle ="Maximum speed: " + Data.CPUSpeed().ToString() + " GHz";
            ProcessorsCount.Text = "Logical processors: " + Data.LogicalProcessors;
            ProcessorName.Text = Data.GetProcessorName();
            dispatcherTimer.Start();
            Memory_Data();
        }


        private void Memory_Data()
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
            ProcessesCount.Text = "Processes: " + Data.ProcessCount().ToString();
            ProcessorUtilization.Text = "Utilization: " + Data.CPUUtilization + "%";
            RAMUse.Text = "In use: " + Math.Round((Data.GetTotalRAM() - Data.GetCurrentMemoryAvailability()), 1) + " GB";
            RAMAvailable.Text = "Available: " + Math.Round(Data.GetCurrentMemoryAvailability(), 2) + " GB";
            Commited.Text = "Commited: " + Data.GetCommitedGB() + "/" + Data.GetMaxCommitedGB() + " GB";
            PagedPool.Text = "Paged pool: " + Data.GetPageMemory() + "MB";
            NonPagedPool.Text = "Non-paged pool: " + Data.GetNPageMemory() + "MB";
            Cashed.Text = "Cahsed: " + Data.GetCasheMemory() + "GB";
            //CPUProgressBar.Value = Data.GetCurrentCpuUsage();
            //TextCPU.Text = System.String.Format("{0,3:N2}%",CPUProgressBar.Value);
            //RAMProgressBar.Value = Data.GetCommitedInUse();
            //TextRAM.Text = System.String.Format("{0,3:N2}%", RAMProgressBar.Value);
        }

    }
}
