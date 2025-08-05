using System.Collections;
using System.Runtime.InteropServices;
using static GOLED.Display.ScreenInterrogatory;

namespace GOLED.Display
{
    internal class ScreenNative
    {

        [DllImport("gdi32", CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateDC(string driver, string device, string port, IntPtr deviceMode);

        [DllImport("gdi32")]
        internal static extern bool SetDeviceGammaRamp(IntPtr dcHandle, ref GammaRamp ramp);

        [DllImport("gdi32")]
        internal static extern bool GetDeviceGammaRamp(IntPtr dcHandle, ref GammaRamp ramp);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;

            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public int dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;

            public short dmLogPixels;
            public short dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        };

        [Flags()]
        public enum DisplaySettingsFlags : int
        {
            CDS_UPDATEREGISTRY = 1,
            CDS_TEST = 2,
            CDS_FULLSCREEN = 4,
            CDS_GLOBAL = 8,
            CDS_SET_PRIMARY = 0x10,
            CDS_RESET = 0x40000000,
            CDS_NORESET = 0x10000000
        }

        // PInvoke declaration for EnumDisplaySettings Win32 API
        [DllImport("user32.dll")]
        public static extern int EnumDisplaySettingsEx(
             string lpszDeviceName,
             int iModeNum,
             ref DEVMODE lpDevMode);

        // PInvoke declaration for ChangeDisplaySettings Win32 API
        [DllImport("user32.dll")]
        public static extern int ChangeDisplaySettingsEx(
                string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd,
                DisplaySettingsFlags dwflags, IntPtr lParam);

        public static DEVMODE CreateDevmode()
        {
            DEVMODE dm = new DEVMODE();
            dm.dmDeviceName = new String(new char[32]);
            dm.dmFormName = new String(new char[32]);
            dm.dmSize = (short)Marshal.SizeOf(dm);
            return dm;
        }

        public enum COLORPROFILETYPE
        {
            CPT_ICC,
            CPT_DMP,
            CPT_CAMP,
            CPT_GMMP
        }
        public enum COLORPROFILESUBTYPE
        {
            CPST_PERCEPTUAL,
            CPST_RELATIVE_COLORIMETRIC,
            CPST_SATURATION,
            CPST_ABSOLUTE_COLORIMETRIC,
            CPST_NONE,
            CPST_RGB_WORKING_SPACE,
            CPST_CUSTOM_WORKING_SPACE,
            CPST_STANDARD_DISPLAY_COLOR_MODE,
            CPST_EXTENDED_DISPLAY_COLOR_MODE
        }
        public enum WCS_PROFILE_MANAGEMENT_SCOPE
        {
            WCS_PROFILE_MANAGEMENT_SCOPE_SYSTEM_WIDE,
            WCS_PROFILE_MANAGEMENT_SCOPE_CURRENT_USER
        }

        public const int ENUM_CURRENT_SETTINGS = -1;
        public const string defaultDevice = @"\\.\DISPLAY1";


        public static string? FindInternalName(bool log = false)
        {
            try
            {
                var devicesList = GetAllDevices();
                var devices = devicesList.ToArray();
                string internalName = AppConfig.GetString("internal_display");

                foreach (var device in devices)
                {
                    if (device.outputTechnology == DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_INTERNAL ||
                        device.outputTechnology == DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_DISPLAYPORT_EMBEDDED ||
                        device.monitorFriendlyDeviceName == internalName)
                    {
                        if (log) Logger.WriteLine(device.monitorDevicePath + " " + device.outputTechnology);

                        AppConfig.Set("internal_display", device.monitorFriendlyDeviceName);
                        var names = device.monitorDevicePath.Split("#");
                        
                        if (names.Length > 1) return names[1];
                        else return "";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex.Message);
            }

            return null;
        }

        static string ExtractDisplay(string input)
        {
            int index = input.IndexOf('\\', 4); // Start searching from index 4 to skip ""

            if (index != -1)
            {
                string extracted = input.Substring(0, index);
                return extracted;
            }

            return input;
        }

        public static string? FindLaptopScreen(bool log = false)
        {
            string? laptopScreen = null;
            string? internalName = FindInternalName(log);

            if (internalName == null)
            {
                Logger.WriteLine("Internal screen off");
                return null;
            }

            try
            {
                var displays = GetDisplayDevices().ToArray();
                foreach (var display in displays)
                {
                    if (log) Logger.WriteLine(display.DeviceID + " " + display.DeviceName);
                    if (display.DeviceID.Contains(internalName))
                    {
                        laptopScreen = ExtractDisplay(display.DeviceName);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex.ToString());
            }

            if (laptopScreen is null)
            {
                Logger.WriteLine("Default internal screen");
                laptopScreen = Screen.PrimaryScreen.DeviceName;
            }

            return laptopScreen;
        }


    }
}
