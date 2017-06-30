using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MemoryMonitor
{
    class MemoryViewModel
    {
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

        Memory CasheMemory = new Memory();
        Memory AvailableMemory = new Memory();
        Memory UseMemory = new Memory();
        Memory CPU = new Memory();
        GetData Data = new GetData();

        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public MemoryViewModel()
        { 
            CPU.Count = Data.GetCurrentCpuUsage();
            CPU.Name = "CPU usage" ;
            CPUspeed.Add(CPU);

            CasheMemory.Count = Math.Round(Data.GetCurrentMemoryAvailability(), 2);
            AvailableMemory.Count = Math.Round(Data.GetCasheMemory(), 2);
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
            CPU.Count = Data.GetCurrentCpuUsage();



            CasheMemory.Count = Math.Round(Data.GetCurrentMemoryAvailability(), 2);
            AvailableMemory.Count = Math.Round(Data.GetCasheMemory(), 2);
            UseMemory.Count = Math.Round(Data.GetTotalRAM() - Data.GetCurrentMemoryAvailability() - Data.GetCasheMemory(), 2);
            //CasheMemory.Name = "Cashe " + CasheMemory.Count + " GB";
            //AvailableMemory.Name = "Available " + AvailableMemory.Count + " GB";
            //UseMemory.Name = "Use memory " + UseMemory.Count + " GB";
        }
    }
}
