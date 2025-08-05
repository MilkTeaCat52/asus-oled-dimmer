using GOLED.Helpers;
using Microsoft.Win32;
using System.Management;

namespace GOLED.Display
{
    public enum SplendidCommand : int
    {
        None = -1,

        VivoNormal = 1,
        VivoVivid = 2,
        VivoManual = 6,
        VivoEycare = 7,

        Init = 10,
        DimmingVivo = 9,
        DimmingVisual = 19,
        DimmingDuo = 109,

        GamutMode = 200,

        Default = 11,
        Racing = 21,
        Scenery = 22,
        RTS = 23,
        FPS = 24,
        Cinema = 25,
        Vivid = 13,
        Eyecare = 17,
        Disabled = 18,
    }
    public static class VisualControl
    {
        public static DisplayGammaRamp? gammaRamp;

        private static int _brightness = 100;
        private static bool _init = true;
        private static string? _splendidPath = null;

        private static System.Timers.Timer brightnessTimer = new System.Timers.Timer(200);

        static VisualControl()
        {
            brightnessTimer.Elapsed += BrightnessTimerTimer_Elapsed;
        }

        const string GameVisualKey = @"HKEY_CURRENT_USER\Software\ASUS\ARMOURY CRATE Service\GameVisual";
        const string GameVisualValue = "ActiveGVStatus";

        public static void SetRegStatus(int status = 1)
        {
            Registry.SetValue(GameVisualKey, GameVisualValue, status, RegistryValueKind.DWord);
        }

        private static string GetSplendidPath()
        {
            if (_splendidPath == null)
            {
                try
                {
                    using (var searcher = new ManagementObjectSearcher(@"Select * from Win32_SystemDriver WHERE Name='ATKWMIACPIIO'"))
                    {
                        foreach (var driver in searcher.Get())
                        {
                            string path = driver["PathName"].ToString();
                            _splendidPath = Path.GetDirectoryName(path);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(ex.Message);
                }
            }

            return _splendidPath;
        }

        private static int RunSplendid(SplendidCommand command, int? param1 = null, int? param2 = null)
        {
            string splendidPath = GetSplendidPath();
            string splendidExe = $"{splendidPath}\\AsusSplendid.exe";
            bool isVivo = AppConfig.IsVivoZenPro();
            bool isSplenddid = File.Exists(splendidExe);

            if (isSplenddid)
            {
                var result = ProcessHelper.RunCMD(splendidExe, (int)command + " " + param1 + " " + param2, splendidPath);
                if (result.Contains("file not exist") || (result.Length == 0 && !isVivo)) return 1;
                if (result.Contains("return code: -1")) return -1;
                if (result.Contains("Visual is disabled"))
                {
                    SetRegStatus(1);
                    return 1;
                }
            }

            return 0;
        }

        private static void BrightnessTimerTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            brightnessTimer.Stop();

            var dimmingCommand = AppConfig.IsVivoZenPro() ? SplendidCommand.DimmingVivo : SplendidCommand.DimmingVisual;
            var dimmingLevel = (int)(40 + _brightness * 0.6);

            if (AppConfig.IsDUO()) RunSplendid(SplendidCommand.DimmingDuo, 0, dimmingLevel);
            if (RunSplendid(dimmingCommand, 0, dimmingLevel) == 0) return;

            if (_init)
            {
                _init = false;
                RunSplendid(SplendidCommand.Init);
                RunSplendid(SplendidCommand.Init, 4);
                if (RunSplendid(dimmingCommand, 0, dimmingLevel) == 0) return;
            }

            // GammaRamp Fallback
            SetGamma(_brightness);
        }

        private static bool IsOnBattery()
        {
            return AppConfig.SaveDimming() && SystemInformation.PowerStatus.PowerLineStatus != PowerLineStatus.Online;
        }

        public static int GetBrightness()
        {
            return AppConfig.Get(IsOnBattery() ? "brightness_battery" : "brightness", 100);
        }

        public static int SetBrightness(int brightness = -1, int delta = 0)
        {
            if (!AppConfig.IsOLED()) return -1;
            if (brightness < 0) brightness = GetBrightness();

            _brightness = Math.Max(0, Math.Min(100, brightness + delta));
            AppConfig.Set(IsOnBattery() ? "brightness_battery" : "brightness", _brightness);

            brightnessTimer.Start();

            Program.settingsForm.VisualiseBrightness();
            //if (brightness < 100) ResetGamut();

            return _brightness;
        }


        public static void SetGamma(int brightness = 100)
        {
            var bright = Math.Round((float)brightness / 200 + 0.5, 2);

            var screenName = ScreenNative.FindLaptopScreen();
            if (screenName is null) return;

            try
            {
                var handle = ScreenNative.CreateDC(screenName, screenName, null, IntPtr.Zero);
                if (gammaRamp is null)
                {
                    var gammaDump = new GammaRamp();
                    if (ScreenNative.GetDeviceGammaRamp(handle, ref gammaDump))
                    {
                        gammaRamp = new DisplayGammaRamp(gammaDump);
                        //Logger.WriteLine("Gamma R: " + string.Join("-", gammaRamp.Red));
                        //Logger.WriteLine("Gamma G: " + string.Join("-", gammaRamp.Green));
                        //Logger.WriteLine("Gamma B: " + string.Join("-", gammaRamp.Blue));
                    }
                }

                if (gammaRamp is null || !gammaRamp.IsOriginal())
                {
                    Logger.WriteLine("Not default Gamma");
                    gammaRamp = new DisplayGammaRamp();
                }

                var ramp = gammaRamp.AsBrightnessRamp(bright);
                bool result = ScreenNative.SetDeviceGammaRamp(handle, ref ramp);

                Logger.WriteLine("Gamma " + bright.ToString() + ": " + result);

            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex.ToString());
            }

            //ScreenBrightness.Set(60 + (int)(40 * bright));
        }

    }
}
