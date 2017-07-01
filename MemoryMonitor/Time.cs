using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace MemoryMonitor
{
    class Time
    {
        private DateTime localtime;
        // TODO: don't forget to add validation
        public int Hours
        {
            get {
                return localtime.Hour;
            }
        }
        public int Minutes
        {
            get
            {
                return localtime.Minute;
            }
        }
        public int Seconds
        {
            get
            {
                return localtime.Second;
            }
        }
        public override string ToString()
        {
            return String.Format(
                "{0:00}:{1:00}:{2:00}",
                this.Hours, this.Minutes, this.Seconds);
        }
    }
}
