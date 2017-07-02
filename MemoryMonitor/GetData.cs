using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Management;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.VisualBasic.Devices;
using System.Collections;
using Microsoft.Win32;

namespace MemoryMonitor
{
    class GetData
    {
        public GetData()
        {
            GetFreeDiskSpace();
        }

        private object BatteryProperty(string PropertyName)
        {
            Type t = typeof(System.Windows.Forms.PowerStatus);
            PropertyInfo[] pi = t.GetProperties();
            PropertyInfo prop = null;
            foreach (PropertyInfo propinf in pi)
            {
                if (propinf.Name == PropertyName)//"BatteryLifePercent")
                    prop = propinf;
            }
            if (prop == null)
                return null;
            else
                return prop.GetValue(SystemInformation.PowerStatus, null);
        }

        public string ToGBytes(string memory)
        {
            Int64 Memory;
            Int64.TryParse(memory, out Memory);
            return (Math.Round((((double)Memory) / 1024 / 1024 / 1024),2)).ToString();
        }

        public double ToGBytes(float memory)
        {
            return Math.Round(((double)(memory) / 1024 / 1024 / 1024), 1);
        }
        public double ToMBytes(float memory)
        {
            return Math.Round(((double)(memory) / 1024 / 1024 ));
        }
        public string GetChargeStatus()
        {
            object obj = BatteryProperty("BatteryLifePercent");
            double BatteryPercent;
            if (obj == null || !Double.TryParse(obj.ToString(), out BatteryPercent))
                return "WARNING! Battery don't found";
            else
                return ((BatteryPercent * 100).ToString() + "%");
        }

        public string GetChargeTime()
        {
            object obj = BatteryProperty("BatteryLifeRemaining");
            int BatteryTime;
            if (obj == null || !Int32.TryParse(obj.ToString(), out BatteryTime))
                return "WARNING! Battery don't found";
            else
                return ((BatteryTime / 60 / 60).ToString() + "h " + (BatteryTime % (60 * 60) / 60).ToString() + "min");
        }
        PerformanceCounter perfPageBCounter = new PerformanceCounter("Memory", "Pool Paged Bytes", null);
        public double GetPageMemory()
        {
            return ToMBytes(perfPageBCounter.NextValue());
        }
        PerformanceCounter perfCasheCounter = new PerformanceCounter("Memory", "Cache Bytes", null);
        public double GetCasheMemory()
        {
            return ToGBytes(perfCasheCounter.NextValue());
        }
        PerformanceCounter perfNPageBCounter = new PerformanceCounter("Memory", "Pool Nonpaged Bytes", null);
        public double GetNPageMemory()
        {
            return ToMBytes(perfNPageBCounter.NextValue());
        }

        PerformanceCounter perfCommitCounter = new PerformanceCounter("Memory", "Committed Bytes", null);
        public double GetCommitedGB()
        {
            return ToGBytes(perfCommitCounter.NextValue());
        }

        PerformanceCounter perfCommitCounterInUse = new PerformanceCounter("Memory", "% Committed Bytes In Use", null);
        public double GetCommitedInUse()
        {
            return perfCommitCounterInUse.NextValue();
        }
        PerformanceCounter perfMaxCommitCounter = new PerformanceCounter("Memory", "Commit Limit", null);
        public double GetMaxCommitedGB()
        {
            return ToGBytes(perfMaxCommitCounter.NextValue());
        }

        PerformanceCounter perfCPUCounter = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
        PerformanceCounter perfMemCounter = new PerformanceCounter("Memory", "Available MBytes");
        public double GetCurrentCpuUsage()
        {
            return perfCPUCounter.NextValue();
        }
        public double GetCurrentMemoryAvailability()
        {
            return perfMemCounter.NextValue()/1024;
        }

        public double GetTotalRAM()
        {
            ComputerInfo TPM = new ComputerInfo();
            return ToGBytes((float)TPM.TotalPhysicalMemory);
        }
        public string GetTimeWorkSystem()
        {
            PerformanceCounter perfSystemCounter = new PerformanceCounter("System", "System Up Time");
            perfSystemCounter.NextValue();
            int seconds_time = (int)perfSystemCounter.NextValue();

            return (seconds_time / 3600).ToString() + "h " + ((seconds_time % 3600) / 60) + "min";
        }

        public string GetComponent(string hwclass, string syntax)
        {
            ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM " + hwclass);
            string Name = "";
            foreach (ManagementObject mj in mos.Get())
            {
                Name = String.Concat(Name, Convert.ToString(mj[syntax]));
                Name = String.Concat(Name, " ");
            }
            //System.Windows.MessageBox.Show(Name);
            return Name;
        }

        public string GetProcessorName()
        {
            return GetComponent("Win32_Processor", "Name");
        }

        public string GetVideoControllerName()
        {
            return GetComponent("Win32_VideoController", "Name");
        }

        public string GetPhysicalMemoryName()
        {
            return GetComponent("Win32_PhysicalMemory", "Name");
        }

        public double GetDiskSize()
        {
            double disksize;
            double.TryParse(GetComponent("Win32_LogicalDisk", "Size"), out disksize);

            return ToGBytes((float)disksize);
        }

        double diskspace = 0;

        public string GetFreeDiskSpace()
        {
            const int HARD_DISK = 3;
            string strComputer = ".";

            ManagementScope namespaceScope = new ManagementScope("\\\\" + strComputer + "\\ROOT\\CIMV2");
            ObjectQuery diskQuery = new ObjectQuery("SELECT * FROM Win32_LogicalDisk WHERE DriveType = " + HARD_DISK + "");
            ManagementObjectSearcher mgmtObjSearcher = new ManagementObjectSearcher(namespaceScope, diskQuery);
            ManagementObjectCollection colDisks = mgmtObjSearcher.Get();

            string FreeSpace = "";

            foreach (ManagementObject objDisk in colDisks)  
            {
                FreeSpace = String.Concat(FreeSpace, objDisk["DeviceID"] + " ");
                string temp = string.Concat(objDisk["FreeSpace"]);
                FreeSpace = String.Concat(FreeSpace, ToGBytes(temp) + "GB");
                FreeSpace = String.Concat(FreeSpace, "\n");
                double t;
                Double.TryParse( ToGBytes(temp), out t);
                diskspace += t;
            }
            return FreeSpace;
        }

        public double DiskSpace
        {
            get
            {
                return diskspace;
            }
        }

        public double CPUSpeed()
        {
            ManagementObject Mo = new ManagementObject("Win32_Processor.DeviceID='CPU0'");
            uint sp = (uint)(Mo["CurrentClockSpeed"]);
            Mo.Dispose();
            return ((double)sp)/1000;
        }

        

        public int ProcessCount()
        {

                Process[] _processes =Process.GetProcesses();
                return _processes.Count();
        }

        public double GetCpuClockSpeed()
        {
            return (double)((int)Registry.GetValue(@"HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\System\CentralProcessor\0", "~MHz", 0))/1000;
        }

        private PerformanceCounter _cpuUtilityCounter = new PerformanceCounter("Processor Information", "% Processor Utility", "_Total");
        public int CPUUtilization
        {
            get
            {
                return (int)_cpuUtilityCounter.NextValue();
            }
        }

        public int LogicalProcessors
        {
            get
            {
                return Environment.ProcessorCount;
            }
        }
    }
}
