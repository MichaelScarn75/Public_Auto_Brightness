using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoBrightness
{
    internal class MonitorBrightnessController
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll")]
        static extern IntPtr GetDesktopWindow();

        [DllImport("dxva2.dll", SetLastError = true)]
        static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, out uint pdwNumberOfPhysicalMonitors);

        [DllImport("dxva2.dll", SetLastError = true)]
        static extern bool GetPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, uint dwPhysicalMonitorArraySize, [Out] PHYSICAL_MONITOR[] pPhysicalMonitorArray);

        [DllImport("dxva2.dll", SetLastError = true)]
        static extern bool SetMonitorBrightness(IntPtr hMonitor, uint dwNewBrightness);

        [DllImport("dxva2.dll", SetLastError = true)]
        static extern bool GetMonitorBrightness(IntPtr hMonitor, out uint pdwMinimumBrightness, out uint pdwCurrentBrightness, out uint pdwMaximumBrightness);

        [DllImport("dxva2.dll", SetLastError = true)]
        static extern bool DestroyPhysicalMonitors(uint dwPhysicalMonitorArraySize, PHYSICAL_MONITOR[] pPhysicalMonitorArray);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct PHYSICAL_MONITOR
        {
            public IntPtr hPhysicalMonitor;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szPhysicalMonitorDescription;
        }

        public static void SetBrightness(uint brightness)
        {
            IntPtr hMonitor = MonitorFromWindow(GetDesktopWindow(), 2);
            GetNumberOfPhysicalMonitorsFromHMONITOR(hMonitor, out uint monitorCount);
            PHYSICAL_MONITOR[] physicalMonitors = new PHYSICAL_MONITOR[monitorCount];
            GetPhysicalMonitorsFromHMONITOR(hMonitor, monitorCount, physicalMonitors);

            foreach (var monitor in physicalMonitors)
            {
                SetMonitorBrightness(monitor.hPhysicalMonitor, brightness);
            }

            DestroyPhysicalMonitors(monitorCount, physicalMonitors);
        }

        public static uint ShowCurrentBrightness()
        {
            IntPtr hMonitor = MonitorFromWindow(GetDesktopWindow(), 2);
            GetNumberOfPhysicalMonitorsFromHMONITOR(hMonitor, out uint monitorCount);
            PHYSICAL_MONITOR[] physicalMonitors = new PHYSICAL_MONITOR[monitorCount];
            GetPhysicalMonitorsFromHMONITOR(hMonitor, monitorCount, physicalMonitors);

            foreach (var monitor in physicalMonitors)
            {
                GetMonitorBrightness(monitor.hPhysicalMonitor, out uint min, out uint current, out uint max);
                Console.WriteLine($"Brightness: {current} (Min: {min}, Max: {max})");
            }

            DestroyPhysicalMonitors(monitorCount, physicalMonitors);

            GetMonitorBrightness(physicalMonitors[0].hPhysicalMonitor, out _, out uint currentBrightness, out _);

            return currentBrightness;
        }
    }
}
