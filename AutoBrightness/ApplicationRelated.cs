using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using AutoBrightness.Model;
using System.CodeDom.Compiler;


namespace AutoBrightness
{
    class ForegroundAppInfo
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        // Delegate for window enumeration callback
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        public static string ReturnActiveApplicationProcessName()
        {
            IntPtr handle = GetForegroundWindow();

            if (handle == IntPtr.Zero)
            {
                Debug.WriteLine("No active window.");
                return string.Empty;
            }

            GetWindowThreadProcessId(handle, out uint processId);
            Process? process = null;

            try
            {
                process = Process.GetProcessById((int)processId);
                Debug.WriteLine($"Active App: {process.ProcessName} ({process.MainWindowTitle}) - PID: {process.Id}");
                return process.ProcessName;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Could not get process info: {ex.Message}");
                return string.Empty;
            }
        }
        public static string? GetActiveWindowTitle()
        {
            IntPtr hWnd = GetForegroundWindow();
            if (hWnd == IntPtr.Zero)
                return null;

            GetWindowThreadProcessId(hWnd, out uint pid);

            try
            {
                var proc = Process.GetProcessById((int)pid);
                return proc.MainWindowTitle?.ToLower();
            }
            catch
            {
                return null;
            }
        }

        public static ObservableCollection<ApplicationSettingsModel> ShowAllApplications()
        {
            Debug.WriteLine("List of running applications with visible windows:\n");
            ObservableCollection<ApplicationSettingsModel> temp = new();

            EnumWindows((hWnd, lParam) =>
            {
                if (!IsWindowVisible(hWnd))
                    return true;

                var sb = new System.Text.StringBuilder(256);
                GetWindowText(hWnd, sb, sb.Capacity);
                string windowTitle = sb.ToString();

                if (string.IsNullOrWhiteSpace(windowTitle))
                    return true;

                GetWindowThreadProcessId(hWnd, out uint pid);

                try
                {
                    Process process = Process.GetProcessById((int)pid);
                    Debug.WriteLine("process: " + process.ProcessName + "title: " + windowTitle);
                    temp.Add(new ApplicationSettingsModel(process.ProcessName, windowTitle, 0, 0));
                }
                catch
                {
                    // Skip processes that can't be accessed
                }

                return true; // Continue enumeration
            }, IntPtr.Zero);

            return temp;
        }
    }
}
