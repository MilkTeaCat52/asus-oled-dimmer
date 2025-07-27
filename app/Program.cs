using GHelper.Helpers;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace GHelper
{

    static class Program
    {
        public static NotifyIcon trayIcon = new NotifyIcon
        {
            Text = "G-Helper",
            Icon = Properties.Resources.standard,
            Visible = true
        };

        public static AsusACPI acpi;

        public static SettingsForm settingsForm = new SettingsForm();

        // The main entry point for the application
        public static void Main(string[] args)
        {
            string language = AppConfig.GetString("language");

            if (language != null && language.Length > 0)
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(language);
            else
            {
                var culture = CultureInfo.CurrentUICulture;
                if (culture.ToString() == "kr") culture = CultureInfo.GetCultureInfo("ko");
                Thread.CurrentThread.CurrentUICulture = culture;
            }

            ProcessHelper.CheckAlreadyRunning();

            Logger.WriteLine("------------");
            Logger.WriteLine("App launched: " + AppConfig.GetModel() + " :" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + CultureInfo.CurrentUICulture + (ProcessHelper.IsUserAdministrator() ? "." : ""));

            var startCount = AppConfig.Get("start_count") + 1;
            AppConfig.Set("start_count", startCount);
            Logger.WriteLine("Start Count: " + startCount);

            acpi = new AsusACPI();

            if (!acpi.IsConnected() && AppConfig.IsASUS())
            {
                DialogResult dialogResult = MessageBox.Show(Properties.Strings.ACPIError, Properties.Strings.StartupError, MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Process.Start(new ProcessStartInfo("https://www.asus.com/support/FAQ/1047338/") { UseShellExecute = true });
                }

                Application.Exit();
                return;
            }

            //ProcessHelper.KillByName("ASUSSmartDisplayControl");

            Application.EnableVisualStyles();

            trayIcon.MouseClick += TrayIcon_MouseClick;


            if (Environment.CurrentDirectory.Trim('\\') == Application.StartupPath.Trim('\\'))
            {
                SettingsToggle(false);
            }

            Startup.StartupCheck();


            Application.Run();

        }

        public static void SettingsToggle(bool checkForFocus = true, bool trayClick = false)
        {
            if (settingsForm.Visible)
            {
                // If helper window is not on top, this just focuses on the app again
                // Pressing the ghelper button again will hide the app
                if (checkForFocus && !settingsForm.HasAnyFocus(trayClick) && !AppConfig.Is("topmost"))
                {
                    settingsForm.ShowAll();
                }
                else
                {
                    settingsForm.HideAll();
                }
            }
            else
            {
                var screen = Screen.PrimaryScreen;
                if (screen is null) screen = Screen.FromControl(settingsForm);

                settingsForm.Location = screen.WorkingArea.Location;
                settingsForm.Left = screen.WorkingArea.Width - 10 - settingsForm.Width;
                settingsForm.Top = screen.WorkingArea.Height - 105 - settingsForm.Height;

                settingsForm.Show();
                settingsForm.ShowAll();

                //settingsForm.Left = screen.WorkingArea.Width - 10 - settingsForm.Width;
                //settingsForm.Top = screen.WorkingArea.Height - 40 - settingsForm.Height;
            }
        }

        static void TrayIcon_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                SettingsToggle(trayClick: true);

        }

        static void OnExit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }

    }
}