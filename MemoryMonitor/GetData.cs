﻿using System;
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

namespace MemoryMonitor
{
    class GetData
    {
        
       
        
        
        

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
            return ((int)(((double)Memory) / 1024 / 1024 / 1024)).ToString(); 
        }

        public string ToGBytes(float memory)
        {
            return  Math.Round(((double)(memory) / 1024 / 1024 / 1024), 2).ToString();
        }
        public string GetChargeStatus()
        {
            object obj = BatteryProperty("BatteryLifePercent");
            double BatteryPercent;
            if (obj == null || !Double.TryParse(obj.ToString(), out BatteryPercent))
                return "WARNING! Battery don't found";
            else
                return ((BatteryPercent*100).ToString() + "%");
        }

        public string GetChargeTime()
        {
            object obj = BatteryProperty("BatteryLifeRemaining");
            int BatteryTime;
            if (obj == null || !Int32.TryParse(obj.ToString(), out BatteryTime))
                return "WARNING! Battery don't found";
            else
                return ((BatteryTime/60/60).ToString() + "h " + (BatteryTime % (60 * 60) / 60).ToString() + "min");
        }
        public string GetPageMemory()
        {
            PerformanceCounter perfPageBCounter = new PerformanceCounter("Memory", "Pool Paged Bytes", null);
            return ToGBytes(perfPageBCounter.NextValue());
        }
        public string GetCasheMemory()
        {
            PerformanceCounter perfCasheCounter = new PerformanceCounter("Memory", "Cache Bytes", null);
            return ToGBytes(perfCasheCounter.NextValue());
        }
        public string GetNPageMemory()
        {
            PerformanceCounter perfNPageBCounter = new PerformanceCounter("Memory", "Pool Nonpaged Bytes", null);
            return ToGBytes(perfNPageBCounter.NextValue());
        }

        public string GetCommitedGB()
        {
            PerformanceCounter perfCommitCounter = new PerformanceCounter("Memory", "Committed Bytes", null);
            return ToGBytes(perfCommitCounter.NextValue());
        }

        public string GetMaxCommitedGB()
        {
            PerformanceCounter perfMaxCommitCounter = new PerformanceCounter("Memory", "Commit Limit", null);
            return ToGBytes(perfMaxCommitCounter.NextValue());
        }


        public int GetCurrentCpuUsage()
        {
            PerformanceCounter perfCPUCounter = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            return (int)perfCPUCounter.NextValue();
        }
        public int GetCurrentMemoryAvailability()
        {
            PerformanceCounter perfMemCounter = new PerformanceCounter("Memory", "Available MBytes");
            return (int)perfMemCounter.NextValue();
        }

        public string GetTotalRAM()
        {
            ComputerInfo TPM = new ComputerInfo();
            return ToGBytes((float)TPM.TotalPhysicalMemory);
        }
        public string GetTimeWorkSystem()
        {
            PerformanceCounter perfSystemCounter = new PerformanceCounter("System", "System Up Time");
            perfSystemCounter.NextValue();
            int seconds_time = (int)perfSystemCounter.NextValue();
            
           return (seconds_time/3600).ToString() + "h " + ((seconds_time % 3600) / 60) + "min";
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

        public string GetDiskSize()
        {
            return Convert.ToString(ToGBytes(  GetComponent("Win32_LogicalDisk", "Size"))+ "GB");
        }

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
            }
            return FreeSpace;
        }
    }
}
