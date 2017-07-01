using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Threading;

namespace MemoryMonitor
{
    /// <summary>
    /// Interaction logic for MemoryChart.xaml
    /// </summary>
    public partial class MemoryChart
    {
        GetData Data = new GetData();
        int seconds;

        ObservableCollection<CPUUsage> valueList = new ObservableCollection<CPUUsage>();

        public MemoryChart()
        {

            InitializeComponent();

            

            InitMemoryWatch();
            DataContext = this;
        }
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public void InitMemoryWatch()
        {

            for (seconds = -60; seconds < 0; seconds++)
            {
                valueList.Add(new CPUUsage() { Count = 0, ThisTime = seconds });
            }
            ((AreaSeries)MemoryChartComponent.Series[0]).ItemsSource = valueList;
            LinearAxis dependentaxis = new LinearAxis();
            dependentaxis.Orientation = AxisOrientation.Y;
            dependentaxis.ShowGridLines = true;
            dependentaxis.Maximum = 100;
            dependentaxis.Minimum = 0;
            dependentaxis.Title = "100%(n)";
            dependentaxis.ShowGridLines = true;
            ((AreaSeries)MemoryChartComponent.Series[0]).DependentRangeAxis = dependentaxis;
          
            dispatcherTimer.Tick += DispatcherTimerTick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }


        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            seconds++;
            valueList.Add(new CPUUsage() { Count = Data.GetCurrentCpuUsage(), ThisTime = seconds });
            valueList.Remove(valueList.First());
        }
    }
}
