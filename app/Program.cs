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


        public static SettingsForm settingsForm = new SettingsForm();

        // The main entry point for the application
        public static void Main(string[] args)
        {
            //set default config values if they don't exist
            if (AppConfig.GetString("topmost")==null) AppConfig.Set("topmost",0);
            if (AppConfig.GetString("hide_with_focus")==null) AppConfig.Set("hide_with_focus",1);

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

            Application.EnableVisualStyles();

            trayIcon.MouseClick += TrayIcon_MouseClick;

            Startup.StartupCheck();


            Application.Run();

        }

        public static void SettingsToggle(bool checkForFocus = true, bool trayClick = false)
        {
            if (settingsForm.Visible)
            {
                // If helper window is not on top, this just focuses on the app again
                // Pressing the ghelper button again will hide the app
                if (!settingsForm.HasAnyFocus(trayClick) && !AppConfig.Is("topmost"))
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
            }
        }

        static void TrayIcon_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && SettingsForm.allowTray)
                SettingsToggle(trayClick: true);
        }

        static void OnExit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }

    }
}