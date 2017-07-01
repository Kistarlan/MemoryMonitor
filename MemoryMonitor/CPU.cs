using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MemoryMonitor
{
    class CPUUsage : INotifyPropertyChanged
    {
        private int _time = 0;
        private double _count = 0;



        public int ThisTime
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
                NotifyPropertyChanged("Time");
            }
        }

        public double Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
                NotifyPropertyChanged("Count");
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
