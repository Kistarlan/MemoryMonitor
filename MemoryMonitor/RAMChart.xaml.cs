using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;

using System.Windows.Threading;

namespace MemoryMonitor
{
    /// <summary>
    /// Interaction logic for RAMChart.xaml
    /// </summary>
    public partial class RAMChart : UserControl
    {
        GetData Data = new GetData();
        int seconds;

        ObservableCollection<CPUUsage> valueList = new ObservableCollection<CPUUsage>();

        public RAMChart()
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
            ((AreaSeries)RAMChartComponent.Series[0]).ItemsSource = valueList;
            LinearAxis dependentaxis = new LinearAxis();
            dependentaxis.Orientation = AxisOrientation.Y;
            dependentaxis.ShowGridLines = true;
            dependentaxis.Maximum = Data.GetTotalRAM();
            dependentaxis.Minimum = 0;
            dependentaxis.Title = "RAM";
            dependentaxis.ShowGridLines = true;
            ((AreaSeries)RAMChartComponent.Series[0]).DependentRangeAxis = dependentaxis;

            dispatcherTimer.Tick += DispatcherTimerTick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }


        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            seconds++;
            valueList.Add(new CPUUsage() { Count = Data.GetTotalRAM() - Data.GetCurrentMemoryAvailability(), ThisTime = seconds });
            valueList.Remove(valueList.First());
        }
    }
}

