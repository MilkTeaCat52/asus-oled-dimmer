using GHelper.Display;
using GHelper.Helpers;
using GHelper.UI;

namespace GHelper
{
    public partial class SettingsForm : RForm
    {
        ContextMenuStrip contextMenuStrip = new CustomContextMenu();
        ToolStripMenuItem menuStartup, menuOnTop, menuFocus;

        static long lastLostFocus;
        static public bool allowTray = true;

        bool sliderGammaIgnore = false;

        public SettingsForm()
        {

            InitializeComponent();
            InitTheme(true);

            // Accessible Labels

            FormClosing += SettingsForm_FormClosing;
            Deactivate += SettingsForm_LostFocus;

            labelVersion.ForeColor = Color.FromArgb(128, Color.Gray);
            SetVersionLabel("0.219.0");


            Text = "G-Helper " + (ProcessHelper.IsUserAdministrator() ? "—" : "-") + " " + AppConfig.GetModelShort();
            TopMost = AppConfig.Is("topmost");

            //This will auto position the window again when it resizes. Might mess with position if people drag the window somewhere else.
            this.Resize += SettingsForm_Resize;
            SetContextMenu();

            Program.trayIcon.MouseDown += TrayIcon_MouseDown;

            InitVisual();
        }

        public void InitVisual()
        {
            panelGamma.Visible = true;
            sliderGamma.Visible = true;
            labelGammaTitle.Text = Properties.Strings.FlickerFreeDimming;

            VisualiseBrightness();

            sliderGamma.ValueChanged += SliderGamma_ValueChanged;
            sliderGamma.MouseUp += SliderGamma_ValueChanged;
        }

        public void VisualiseBrightness()
        {
            Invoke(delegate
            {
                sliderGammaIgnore = true;
                sliderGamma.Value = VisualControl.GetBrightness();
                labelGamma.Text = sliderGamma.Value + "%";
                sliderGammaIgnore = false;
            });
        }

        private void SliderGamma_ValueChanged(object? sender, EventArgs e)
        {
            if (sliderGammaIgnore) return;
            VisualControl.SetBrightness(sliderGamma.Value);
        }

        private void SettingsForm_Resize(object? sender, EventArgs e)
        {
            Left = Screen.FromControl(this).WorkingArea.Width - 10 - Width;
            Top = Screen.FromControl(this).WorkingArea.Height - 105 - Height;
        }

        public void SetContextMenu()
        {
            contextMenuStrip.Items.Clear();
            Padding padding = new Padding(15, 5, 5, 5);

            menuStartup = new ToolStripMenuItem("Run On Startup");
            menuStartup.Click += ButtonStartup_Click;
            menuStartup.Margin = padding;
            menuStartup.Checked = (Startup.IsScheduled());
            contextMenuStrip.Items.Add(menuStartup);

            contextMenuStrip.Items.Add("-");

            menuOnTop = new ToolStripMenuItem("Always On Top");
            menuOnTop.Click += ButtonOnTop_Click;
            menuOnTop.Margin = padding;
            menuOnTop.Checked = AppConfig.Is("topmost");
            contextMenuStrip.Items.Add(menuOnTop);
                                                  
            menuFocus = new ToolStripMenuItem("Hide When Focus Lost");
            menuFocus.Click += ButtonFocus_Click;
            menuFocus.Margin = padding;
            menuFocus.Checked = AppConfig.Is("hide_with_focus");
            contextMenuStrip.Items.Add(menuFocus);

            contextMenuStrip.Items.Add("-");

            var quit = new ToolStripMenuItem(Properties.Strings.Quit);
            quit.Click += ButtonQuit_Click;
            quit.Margin = padding;
            contextMenuStrip.Items.Add(quit);

            //contextMenuStrip.ShowCheckMargin = true;
            contextMenuStrip.RenderMode = ToolStripRenderMode.System;

            if (darkTheme)
            {
                contextMenuStrip.BackColor = this.BackColor;
                contextMenuStrip.ForeColor = this.ForeColor;
            }

            Program.trayIcon.ContextMenuStrip = contextMenuStrip;
        }

        public void SetVersionLabel(string label, bool update = false)
        {
            if (InvokeRequired)
                Invoke(delegate
                {
                    labelVersion.Text = label;
                    if (update) labelVersion.ForeColor = colorTurbo;
                });
            else
            {
                labelVersion.Text = label;
                if (update) labelVersion.ForeColor = colorTurbo;
            }
        }

        private void ButtonStartup_Click(object? sender, EventArgs e)
        {
            if (Startup.IsScheduled())
            {
                Startup.UnSchedule();
                menuStartup.Checked = false;
            }
            else
            {
                Startup.Schedule();
                menuStartup.Checked = true;
            }
        }
        private void ButtonOnTop_Click(object? sender, EventArgs e)
        {
            if (AppConfig.Get("topmost") == 1)
            {
                AppConfig.Set("topmost", 0);
                menuOnTop.Checked = false;
                TopMost = false;

            }
            else
            {
                AppConfig.Set("topmost", 1);
                menuOnTop.Checked = true;
                TopMost = true;
            }
        }
        private void ButtonFocus_Click(object? sender, EventArgs e)
        {
            if (AppConfig.Get("hide_with_focus") == 1)
            {
                AppConfig.Set("hide_with_focus", 0);
                menuFocus.Checked = false;

            }
            else
            {
                AppConfig.Set("hide_with_focus", 1);
                menuFocus.Checked = true;
            }     
        }

        private void ButtonQuit_Click(object? sender, EventArgs e)
        {
            Close();
            Program.trayIcon.Visible = false;
            Application.Exit();
        }

        private void TrayIcon_MouseDown(object? sender, EventArgs e)
        {
            if (AppConfig.Is("hide_with_focus") )//this.HasAnyFocus())
            {
                allowTray = false;

                if (!this.Visible && Math.Abs(DateTimeOffset.Now.ToUnixTimeMilliseconds() - lastLostFocus) > 300)
                {
                    allowTray = true;
                }
            }
            else
            {
                allowTray = true;
            }
        }

        private void SettingsForm_LostFocus(object? sender, EventArgs e)
        {
            lastLostFocus = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (AppConfig.Is("hide_with_focus"))
            {
                HideAll();
                allowTray = false;
            }
            
        }

        /// <summary>
        /// Closes all forms except the settings. Hides the settings
        /// </summary>
        public void HideAll()
        {
            this.Hide();
        }

        /// <summary>
        /// Brings all visible windows to the top, with settings being the focus
        /// </summary>
        public void ShowAll()
        {
            this.Activate();
            this.TopMost = true;
            this.TopMost = AppConfig.Is("topmost");
        }

        /// <summary>
        /// Check if any of fans, keyboard, update, or itself has focus
        /// </summary>
        /// <returns>Focus state</returns>
        public bool HasAnyFocus(bool lostFocusCheck = false)
        {
            return this.ContainsFocus ||
                   (lostFocusCheck && Math.Abs(DateTimeOffset.Now.ToUnixTimeMilliseconds() - lastLostFocus) < 300);
        }

        private void SettingsForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                HideAll();
            }
        }

        private void panelGammaTitle_Paint(object sender, PaintEventArgs e)
        {

        }
    }


}
