using GHelper;
using System.Management;
using System.Runtime.InteropServices;

public enum AsusFan
{
    CPU = 0,
    GPU = 1,
    Mid = 2,
    XGM = 3
}

public class AsusACPI
{

    const string FILE_NAME = @"\\.\\ATKACPI";
    const uint CONTROL_CODE = 0x0022240C;

    const uint DSTS = 0x53545344;
    const uint DEVS = 0x53564544;
    const uint INIT = 0x54494E49;
    const uint WDOG = 0x474F4457;

    public const uint UniversalControl = 0x00100021;

    public const int Airplane = 0x88;
    public const int KB_Light_Up = 0xc4;
    public const int KB_Light_Down = 0xc5;
    public const int Brightness_Down = 0x10;
    public const int Brightness_Up = 0x20;
    public const int KB_Sleep = 0x6c;

    public const int KB_TouchpadToggle = 0x6b;
    public const int KB_MuteToggle = 0x7c;
    public const int KB_FNlockToggle = 0x4e;

    public const int KB_DUO_PgUpDn = 0x4B;
    public const int KB_DUO_SecondDisplay = 0x6A;

    public const int Touchpad_Toggle = 0x6B;

    public const int ChargerMode = 0x0012006C;

    public const int ChargerUSB = 2;
    public const int ChargerBarrel = 1;

    public const uint CPU_Fan = 0x00110013;
    public const uint GPU_Fan = 0x00110014;
    public const uint Mid_Fan = 0x00110031;

    public const uint BatteryDischarge = 0x0012005A;

    public const uint PerformanceMode = 0x00120075; // Performance modes
    public const uint VivoBookMode = 0x00110019; // Vivobook performance modes

    public const uint GPUEcoROG = 0x00090020;
    public const uint GPUEcoVivo = 0x00090120;

    public const uint GPUXGConnected = 0x00090018;
    public const uint GPUXG = 0x00090019;

    public const uint GPUMuxROG = 0x00090016;
    public const uint GPUMuxVivo = 0x00090026;

    public const uint BatteryLimit = 0x00120057;

    public const uint ScreenOverdrive = 0x00050019;
    public const uint ScreenMiniled1 = 0x0005001E;
    public const uint ScreenMiniled2 = 0x0005002E;
    public const uint ScreenFHD = 0x0005001C;

    public const uint ScreenOptimalBrightness = 0x0005002A;
    public const uint ScreenInit = 0x00050011; // ?

    public const uint DevsCPUFan = 0x00110022;
    public const uint DevsGPUFan = 0x00110023;

    public const uint DevsCPUFanCurve = 0x00110024;
    public const uint DevsGPUFanCurve = 0x00110025;
    public const uint DevsMidFanCurve = 0x00110032;

    public const int Temp_CPU = 0x00120094;
    public const int Temp_GPU = 0x00120097;

    public const int PPT_APUA0 = 0x001200A0;  // sPPT (slow boost limit) / PL2
    public const int PPT_EDCA1 = 0x001200A1;  // CPU EDC
    public const int PPT_TDCA2 = 0x001200A2;  // CPU TDC
    public const int PPT_APUA3 = 0x001200A3;  // SPL (sustained limit) / PL1

    public const int PPT_CPUB0 = 0x001200B0;  // CPU PPT on 2022 (PPT_LIMIT_APU)
    public const int PPT_CPUB1 = 0x001200B1;  // Total PPT on 2022 (PPT_LIMIT_SLOW)

    public const int PPT_GPUC0 = 0x001200C0;  // NVIDIA GPU Boost
    public const int PPT_APUC1 = 0x001200C1;  // fPPT (fast boost limit)
    public const int PPT_GPUC2 = 0x001200C2;  // NVIDIA GPU Temp Target (75.. 87 C) 

    public const uint CORES_CPU = 0x001200D2; // Intel E-core and P-core configuration in a format 0x0[E]0[P]
    public const uint CORES_MAX = 0x001200D3; // Maximum Intel E-core and P-core availability

    public const uint GPU_BASE = 0x00120099;  // Base part GPU TGP
    public const uint GPU_POWER = 0x00120098;  // Additonal part of GPU TGP

    public const int APU_MEM = 0x000600C1;

    public const int TUF_KB_BRIGHTNESS = 0x00050021;
    public const int KBD_BACKLIGHT_OOBE = 0x0005002F;

    public const int TUF_KB = 0x00100056;
    public const int TUF_KB2 = 0x0010005a;

    public const int TUF_KB_STATE = 0x00100057;

    public const int MicMuteLed = 0x00040017;
    public const int SoundMuteLed = 0x0004001C;

    public const int SlateMode = 0x00120063;
    public const int TabletState = 0x00060077;
    public const int TentState = 0x00060062;
    public const int FnLock = 0x00100023;

    public const int ScreenPadToggle = 0x00050031;
    public const int ScreenPadBrightness = 0x00050032;

    public const int CameraShutter = 0x00060078;
    public const int CameraLed = 0x00060079;
    public const int StatusLed = 0x000600C2;

    public const int BootSound = 0x00130022;

    public const int Tablet_Notebook = 0;
    public const int Tablet_Tablet = 1;
    public const int Tablet_Tent = 2;
    public const int Tablet_Rotated = 3;

    public const int PerformanceBalanced = 0;
    public const int PerformanceTurbo = 1;
    public const int PerformanceSilent = 2;
    public const int PerformanceManual = 4;

    public const int GPUModeEco = 0;
    public const int GPUModeStandard = 1;
    public const int GPUModeUltimate = 2;

    public const int MinTotal = 5;

    public static int MaxTotal = 150;
    public static int DefaultTotal = 80;

    public const int MinCPU = 5;
    public const int MaxCPU = 100;
    public const int DefaultCPU = 80;

    public const int MinGPUBoost = 5;
    public static int MaxGPUBoost = 25;

    public static int MinGPUPower = 0;
    public static int MaxGPUPower = 70;

    public const int MinGPUTemp = 75;
    public const int MaxGPUTemp = 87;

    public const int PCoreMin = 4;
    public const int ECoreMin = 0;

    public const int PCoreMax = 16;
    public const int ECoreMax = 16;

    private bool? _allAMD = null;
    private bool? _overdrive = null;

    public static uint GPUEco => AppConfig.IsVivoZenPro() ? GPUEcoVivo : GPUEcoROG;
    public static uint GPUMux => AppConfig.IsVivoZenPro() ? GPUMuxVivo : GPUMuxROG;

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern IntPtr CreateFile(
        string lpFileName,
        uint dwDesiredAccess,
        uint dwShareMode,
        IntPtr lpSecurityAttributes,
        uint dwCreationDisposition,
        uint dwFlagsAndAttributes,
        IntPtr hTemplateFile
    );

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool DeviceIoControl(
        IntPtr hDevice,
        uint dwIoControlCode,
        byte[] lpInBuffer,
        uint nInBufferSize,
        byte[] lpOutBuffer,
        uint nOutBufferSize,
        ref uint lpBytesReturned,
        IntPtr lpOverlapped
    );

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(IntPtr hObject);

    private const uint GENERIC_READ = 0x80000000;
    private const uint GENERIC_WRITE = 0x40000000;
    private const uint OPEN_EXISTING = 3;
    private const uint FILE_ATTRIBUTE_NORMAL = 0x80;
    private const uint FILE_SHARE_READ = 1;
    private const uint FILE_SHARE_WRITE = 2;

    private IntPtr handle;

    // Event handling attempt

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool WaitForSingleObject(IntPtr hHandle, int dwMilliseconds);

    private IntPtr eventHandle;
    private bool _connected = false;

    // still works only with asus optimization service on , if someone knows how to get ACPI events from asus without that - let me know
    public void RunListener()
    {

        eventHandle = CreateEvent(IntPtr.Zero, false, false, "ATK4001");

        byte[] outBuffer = new byte[16];
        byte[] data = new byte[8];
        bool result;

        data[0] = BitConverter.GetBytes(eventHandle.ToInt32())[0];
        data[1] = BitConverter.GetBytes(eventHandle.ToInt32())[1];

        Control(0x222400, data, outBuffer);
        Logger.WriteLine("ACPI :" + BitConverter.ToString(data) + "|" + BitConverter.ToString(outBuffer));

        while (true)
        {
            WaitForSingleObject(eventHandle, Timeout.Infinite);
            Control(0x222408, new byte[0], outBuffer);
            int code = BitConverter.ToInt32(outBuffer);
            Logger.WriteLine("ACPI Code: " + code);
        }
    }

    public bool IsConnected()
    {
        return _connected;
    }

    public AsusACPI()
    {
        try
        {
            handle = CreateFile(
                FILE_NAME,
                GENERIC_READ | GENERIC_WRITE,
                FILE_SHARE_READ | FILE_SHARE_WRITE,
                IntPtr.Zero,
                OPEN_EXISTING,
                FILE_ATTRIBUTE_NORMAL,
                IntPtr.Zero
            );

            //handle = new IntPtr(-1);
            //throw new Exception("ERROR");
            _connected = true;

        }
        catch (Exception ex)
        {
            Logger.WriteLine($"Can't connect to ACPI: {ex.Message}");
        }

        if (AppConfig.IsAdvantageEdition())
        {
            MaxTotal = 250;
        }

        if (AppConfig.IsG14AMD())
        {
            DefaultTotal = 125;
        }

        if (AppConfig.IsX13())
        {
            MaxTotal = 75;
            DefaultTotal = 50;
        }

        if (AppConfig.IsAlly())
        {
            MaxTotal = 50;
            DefaultTotal = 30;
        }

        if (AppConfig.IsIntelHX())
        {
            MaxTotal = 175;
        }

        if (AppConfig.DynamicBoost5())
        {
            MaxGPUBoost = 5;
        }

        if (AppConfig.DynamicBoost15())
        {
            MaxGPUBoost = 15;
        }

        if (AppConfig.DynamicBoost20())
        {
            MaxGPUBoost = 20;
        }

        if (AppConfig.IsAMDLight())
        {
            MaxTotal = 90;
        }



    }

    public void Control(uint dwIoControlCode, byte[] lpInBuffer, byte[] lpOutBuffer)
    {

        uint lpBytesReturned = 0;
        DeviceIoControl(
            handle,
            dwIoControlCode,
            lpInBuffer,
            (uint)lpInBuffer.Length,
            lpOutBuffer,
            (uint)lpOutBuffer.Length,
            ref lpBytesReturned,
            IntPtr.Zero
        );
    }
}
