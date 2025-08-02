/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace AutoBrightness
{
    internal class HDRclass
    {
        public static void EnableHDR()
        {

            var sim = new InputSimulator();

            // Press Ctrl+S
            sim.Keyboard.ModifiedKeyStroke(
                new List<VirtualKeyCode> { VirtualKeyCode.LWIN, VirtualKeyCode.LMENU },
                VirtualKeyCode.VK_B);
        }
    }

    internal class HDRObject
    {
        internal bool HDREnabled { get; set; }
        internal bool HDRSupported { get; set; }
    }

    class HDRCheck
    {
        const int QDC_ALL_PATHS = 1;
        const int DISPLAYCONFIG_DEVICE_INFO_GET_ADVANCED_COLOR_INFO = 0x0000000D;

        [DllImport("user32.dll")]
        static extern int GetDisplayConfigBufferSizes(uint flags, out uint numPathArrayElements, out uint numModeInfoArrayElements);

        [DllImport("user32.dll")]
        static extern int QueryDisplayConfig(uint flags, ref uint numPathArrayElements, IntPtr pathInfoArray, ref uint numModeInfoArrayElements, IntPtr modeInfoArray, IntPtr currentTopologyId);

        [DllImport("user32.dll")]
        static extern int DisplayConfigGetDeviceInfo(ref DISPLAYCONFIG_GET_ADVANCED_COLOR_INFO requestPacket);

        [StructLayout(LayoutKind.Sequential)]
        struct DISPLAYCONFIG_GET_ADVANCED_COLOR_INFO
        {
            public DISPLAYCONFIG_DEVICE_INFO_HEADER header;
            public uint value;
            public uint colorEncoding;
            public uint bitsPerColorChannel;
            public bool advancedColorSupported;
            public bool advancedColorEnabled;
            public uint wideColorEnforced;
            public uint advancedColorForceDisabled;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct DISPLAYCONFIG_DEVICE_INFO_HEADER
        {
            public int type;
            public int size;
            public LUID adapterId;
            public uint id;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct LUID
        {
            public uint LowPart;
            public int HighPart;
        }

        public static HDRObject? CheckHDRStatus()
        {
            uint numPaths = 0, numModes = 0;
            int result = GetDisplayConfigBufferSizes(QDC_ALL_PATHS, out numPaths, out numModes);
            if (result != 0)
            {
                Debug.WriteLine("GetDisplayConfigBufferSizes failed.");
                return null;
            }

            int sizeOfPathInfo = Marshal.SizeOf(typeof(DISPLAYCONFIG_PATH_INFO));
            int sizeOfModeInfo = Marshal.SizeOf(typeof(DISPLAYCONFIG_MODE_INFO));

            IntPtr pathInfoPtr = Marshal.AllocHGlobal((int)(sizeOfPathInfo * numPaths));
            IntPtr modeInfoPtr = Marshal.AllocHGlobal((int)(sizeOfModeInfo * numModes));

            result = QueryDisplayConfig(QDC_ALL_PATHS, ref numPaths, pathInfoPtr, ref numModes, modeInfoPtr, IntPtr.Zero);
            if (result != 0)
            {
                Debug.WriteLine("QueryDisplayConfig failed.");
                return null;
            }

            for (int i = 0; i < numPaths; i++)
            {
                DISPLAYCONFIG_PATH_INFO pathInfo = Marshal.PtrToStructure<DISPLAYCONFIG_PATH_INFO>(pathInfoPtr + i * sizeOfPathInfo);

                var request = new DISPLAYCONFIG_GET_ADVANCED_COLOR_INFO
                {
                    header = new DISPLAYCONFIG_DEVICE_INFO_HEADER
                    {
                        type = DISPLAYCONFIG_DEVICE_INFO_GET_ADVANCED_COLOR_INFO,
                        size = Marshal.SizeOf(typeof(DISPLAYCONFIG_GET_ADVANCED_COLOR_INFO)),
                        adapterId = pathInfo.targetInfo.adapterId,
                        id = pathInfo.targetInfo.id
                    }
                };

                HDRObject hDRObject = new();
                result = DisplayConfigGetDeviceInfo(ref request);
                if (result == 0)
                {
                    Debug.WriteLine($"Monitor {i + 1}: HDR Supported: {request.advancedColorSupported}, HDR Enabled: {request.advancedColorEnabled}");
                    hDRObject.HDREnabled = request.advancedColorEnabled;
                    hDRObject.HDRSupported = request.advancedColorSupported;
                    return hDRObject; 
                }
                else
                {
                    Debug.WriteLine($"Monitor {i + 1}: Failed to get HDR info.");
                    return null;
                }
            }

            Marshal.FreeHGlobal(pathInfoPtr);
            Marshal.FreeHGlobal(modeInfoPtr);
            Debug.WriteLine("test3");
            return null;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct DISPLAYCONFIG_PATH_INFO
        {
            public DISPLAYCONFIG_PATH_SOURCE_INFO sourceInfo;
            public DISPLAYCONFIG_PATH_TARGET_INFO targetInfo;
            public uint flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct DISPLAYCONFIG_PATH_SOURCE_INFO
        {
            public LUID adapterId;
            public uint id;
            public uint modeInfoIdx;
            public uint statusFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct DISPLAYCONFIG_PATH_TARGET_INFO
        {
            public LUID adapterId;
            public uint id;
            public uint modeInfoIdx;
            public DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY outputTechnology;
            public DISPLAYCONFIG_ROTATION rotation;
            public DISPLAYCONFIG_SCALING scaling;
            public DISPLAYCONFIG_RATIONAL refreshRate;
            public DISPLAYCONFIG_SCANLINE_ORDERING scanLineOrdering;
            public bool targetAvailable;
            public uint statusFlags;
        }

        enum DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY : uint { Unknown = 0xFFFFFFFF }
        enum DISPLAYCONFIG_ROTATION : uint { Identity = 1 }
        enum DISPLAYCONFIG_SCALING : uint { Identity = 1 }
        enum DISPLAYCONFIG_SCANLINE_ORDERING : uint { Progressive = 1 }

        [StructLayout(LayoutKind.Sequential)]
        struct DISPLAYCONFIG_RATIONAL
        {
            public uint Numerator;
            public uint Denominator;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct DISPLAYCONFIG_MODE_INFO
        {
            public uint infoType;
            public uint id;
            public LUID adapterId;
            public DISPLAYCONFIG_TARGET_MODE targetMode;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct DISPLAYCONFIG_TARGET_MODE
        {
            public DISPLAYCONFIG_VIDEO_SIGNAL_INFO targetVideoSignalInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct DISPLAYCONFIG_VIDEO_SIGNAL_INFO
        {
            public ulong pixelRate;
            public DISPLAYCONFIG_RATIONAL hSyncFreq;
            public DISPLAYCONFIG_RATIONAL vSyncFreq;
            public DISPLAYCONFIG_2D_REGION activeSize;
            public DISPLAYCONFIG_2D_REGION totalSize;
            public uint videoStandard;
            public DISPLAYCONFIG_SCANLINE_ORDERING scanLineOrdering;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct DISPLAYCONFIG_2D_REGION
        {
            public uint cx;
            public uint cy;
        }
    }
}
*/
