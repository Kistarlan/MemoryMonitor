using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Globalization;

namespace MemoryMonitor
{
    class MemoryViewModel
    {
        private readonly ObservableCollection<Memory> _Disk = new ObservableCollection<Memory>();
        public ObservableCollection<Memory> DiskMemory
        {
            get
            {
                return _Disk;
            }
        }

        private readonly ObservableCollection<CPUUsage> _CPUUsage = new ObservableCollection<CPUUsage>();
        public ObservableCollection<CPUUsage> CPUusage
        {
            get
            {
                return _CPUUsage;
            }
        }

        private readonly ObservableCollection<Memory> _RAMmemory = new ObservableCollection<Memory>();
        public ObservableCollection<Memory> RAMMemory
        {
            get
            {
                return _RAMmemory;
            }
        }

        private readonly ObservableCollection<Memory> _CPUspeed = new ObservableCollection<Memory>();
        public ObservableCollection<Memory> CPUspeed
        {
            get
            {
                return _CPUspeed;
            }
        }

        Memory FreeDisk = new Memory();
        Memory Disk = new Memory();



        Memory CasheMemory = new Memory();
        Memory AvailableMemory = new Memory();
        Memory UseMemory = new Memory();
        Memory CPU = new Memory();


        GetData Data = new GetData();
        Time MyTime = new Time();

        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public MemoryViewModel()
        {
            //_CPUUsage.Add(new CPUUsage() { Count = Data.GetCurrentCpuUsage(), ThisTime = MyTime.ToString()});
            
            //localDate.TimeOfDay;

            FreeDisk.Count = Data.DiskSpace;
            Disk.Count = Data.GetDiskSize() - FreeDisk.Count;
            FreeDisk.Name = "Free disk space";
            Disk.Name = "Size: " + Data.GetDiskSize() + " GB";
            _Disk.Add(Disk);
            _Disk.Add(FreeDisk);


            CPU.Count = Data.GetCurrentCpuUsage();
            //CPU.Name = "CPU usage" ;
            CPUspeed.Add(CPU);

            AvailableMemory.Count = Math.Round(Data.GetCurrentMemoryAvailability(), 2);
            CasheMemory.Count = Math.Round(Data.GetCasheMemory(), 2);
            UseMemory.Count = Math.Round(Data.GetTotalRAM() - Data.GetCurrentMemoryAvailability() - Data.GetCasheMemory(), 2);
            CasheMemory.Name = "Cashe ";
            AvailableMemory.Name = "Available ";
            UseMemory.Name = "Use memory";

            _RAMmemory.Add(CasheMemory);
            _RAMmemory.Add(UseMemory);
            _RAMmemory.Add(AvailableMemory);


            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherTimer.Start();
        }




        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            FreeDisk.Count = Data.DiskSpace;
            Disk.Count = Data.GetDiskSize() - FreeDisk.Count;


            CPU.Count = Data.GetCurrentCpuUsage();
            _CPUUsage.Add(new CPUUsage() { Count = Data.GetCurrentCpuUsage()});

            AvailableMemory.Count = Math.Round(Data.GetCurrentMemoryAvailability(), 2);
            CasheMemory.Count = Math.Round(Data.GetCasheMemory(), 2);
            UseMemory.Count = Math.Round(Data.GetTotalRAM() - Data.GetCurrentMemoryAvailability() - Data.GetCasheMemory(), 2);

        }
    }
}
