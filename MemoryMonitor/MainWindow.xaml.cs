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

namespace MemoryMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        PerformanceCounter perfCPUCounter = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
        PerformanceCounter perfMemCounter = new PerformanceCounter("Memory", "Available MBytes");
        PerformanceCounter perfSystemCounter = new PerformanceCounter("System", "System Up Time");

        private void button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("CPU:" + " " + (int)perfCPUCounter.NextValue() + "%" +
                "\n" + "Available Memory:" + " " + (int)perfMemCounter.NextValue() + "MB" +
                "\n" + "System Up Time:" + " " + (int)perfSystemCounter.NextValue() / 60 / 60 + " Hours"
                );
        }
        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0,0,0,0,100);
            LoadBarChartData();
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

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            textBlock.Text = "CPU:" + " " + (int)perfCPUCounter.NextValue() + "%" +
                "\n" + "Available Memory:" + " " + (int)perfMemCounter.NextValue() + "MB" +
                "\n" + "System Up Time:" + " " + (int)perfSystemCounter.NextValue() / 60 / 60 + " Hours";
        }


    }
}
