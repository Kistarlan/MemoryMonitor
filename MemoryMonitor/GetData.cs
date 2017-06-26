using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Management;

namespace MemoryMonitor
{
    class GetData
    {
        PerformanceCounter perfCPUCounter = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
        PerformanceCounter perfMemCounter = new PerformanceCounter("Memory", "Available MBytes");
        PerformanceCounter perfSystemCounter = new PerformanceCounter("System", "System Up Time");

        public string ToGBytes(string memory)
        {
            Int64 Memory;
            Int64.TryParse(memory, out Memory);
            return ((int)(((double)Memory) / 1024 / 1024 / 1024)).ToString();
          
        }

        public int CPUPercent()
        {
            return (int)perfCPUCounter.NextValue();
        }
        public int AMemory()
        {
            return (int)perfMemCounter.NextValue();
        }

        public int SysCounter()
        {
           return (int)perfSystemCounter.NextValue() / 60 / 60;
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
            return Name;
        }

        public string Processor()
        {
            return GetComponent("Win32_Processor", "Name");
        }

        public string VideoController()
        {
            return GetComponent("Win32_VideoController", "Name");
        }

        public string PhysicalMemoryName()
        {
            return GetComponent("Win32_PhysicalMemory", "Name");
        }

        public string DiskSize()
        {
            return Convert.ToString(ToGBytes(  GetComponent("Win32_LogicalDisk", "Size"))+ "GB");
        }

        public string FreeDiskSpace()
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
            }
            return FreeSpace;
        }
    }
}
