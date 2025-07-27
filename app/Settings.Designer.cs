using GHelper.UI;

namespace GHelper
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelGamma = new Panel();
            sliderGamma = new Slider();
            panelGammaTitle = new Panel();
            labelGamma = new Label();
            pictureGamma = new PictureBox();
            labelGammaTitle = new Label();
            panelVersion = new Panel();
            labelVersion = new Label();
            panelGamma.SuspendLayout();
            panelGammaTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureGamma).BeginInit();
            panelVersion.SuspendLayout();
            SuspendLayout();
            // 
            // panelGamma
            // 
            panelGamma.AutoSize = true;
            panelGamma.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panelGamma.Controls.Add(sliderGamma);
            panelGamma.Controls.Add(panelGammaTitle);
            panelGamma.Dock = DockStyle.Top;
            panelGamma.Location = new Point(11, 11);
            panelGamma.Margin = new Padding(0);
            panelGamma.Name = "panelGamma";
            panelGamma.Padding = new Padding(20, 11, 20, 11);
            panelGamma.Size = new Size(827, 102);
            panelGamma.TabIndex = 9;
            panelGamma.Visible = false;
            // 
            // sliderGamma
            // 
            sliderGamma.Dock = DockStyle.Top;
            sliderGamma.Location = new Point(20, 51);
            sliderGamma.Margin = new Padding(4);
            sliderGamma.Max = 100;
            sliderGamma.Min = 0;
            sliderGamma.Name = "sliderGamma";
            sliderGamma.Size = new Size(787, 40);
            sliderGamma.Step = 5;
            sliderGamma.TabIndex = 20;
            sliderGamma.Text = "sliderGamma";
            sliderGamma.Value = 100;
            sliderGamma.Visible = false;
            // 
            // panelGammaTitle
            // 
            panelGammaTitle.Controls.Add(labelGamma);
            panelGammaTitle.Controls.Add(pictureGamma);
            panelGammaTitle.Controls.Add(labelGammaTitle);
            panelGammaTitle.Dock = DockStyle.Top;
            panelGammaTitle.Location = new Point(20, 11);
            panelGammaTitle.Margin = new Padding(4);
            panelGammaTitle.Name = "panelGammaTitle";
            panelGammaTitle.Size = new Size(787, 40);
            panelGammaTitle.TabIndex = 40;
            panelGammaTitle.Paint += panelGammaTitle_Paint;
            // 
            // labelGamma
            // 
            labelGamma.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelGamma.Location = new Point(675, 0);
            labelGamma.Margin = new Padding(4, 0, 4, 0);
            labelGamma.Name = "labelGamma";
            labelGamma.Size = new Size(107, 32);
            labelGamma.TabIndex = 40;
            labelGamma.Text = "                ";
            labelGamma.TextAlign = ContentAlignment.TopRight;
            // 
            // pictureGamma
            // 
            pictureGamma.BackgroundImage = Properties.Resources.icons8_brightness_32;
            pictureGamma.BackgroundImageLayout = ImageLayout.Zoom;
            pictureGamma.Location = new Point(8, 3);
            pictureGamma.Margin = new Padding(4);
            pictureGamma.Name = "pictureGamma";
            pictureGamma.Size = new Size(32, 32);
            pictureGamma.TabIndex = 38;
            pictureGamma.TabStop = false;
            // 
            // labelGammaTitle
            // 
            labelGammaTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelGammaTitle.Location = new Point(43, 0);
            labelGammaTitle.Margin = new Padding(4, 0, 4, 0);
            labelGammaTitle.Name = "labelGammaTitle";
            labelGammaTitle.Size = new Size(540, 32);
            labelGammaTitle.TabIndex = 37;
            labelGammaTitle.Text = "Flicker-free Dimming";
            // 
            // panelVersion
            // 
            panelVersion.AutoSize = true;
            panelVersion.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panelVersion.Controls.Add(labelVersion);
            panelVersion.Dock = DockStyle.Top;
            panelVersion.Location = new Point(11, 113);
            panelVersion.MinimumSize = new Size(0, 50);
            panelVersion.Name = "panelVersion";
            panelVersion.Padding = new Padding(20, 5, 20, 5);
            panelVersion.Size = new Size(827, 50);
            panelVersion.TabIndex = 10;
            // 
            // labelVersion
            // 
            labelVersion.Cursor = Cursors.Hand;
            labelVersion.Dock = DockStyle.Left;
            labelVersion.Font = new Font("Segoe UI", 9F, FontStyle.Underline);
            labelVersion.ForeColor = SystemColors.ControlDark;
            labelVersion.Location = new Point(20, 5);
            labelVersion.Margin = new Padding(0);
            labelVersion.Name = "labelVersion";
            labelVersion.Padding = new Padding(5, 0, 5, 0);
            labelVersion.Size = new Size(399, 40);
            labelVersion.TabIndex = 38;
            labelVersion.Text = "v.0";
            labelVersion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(192F, 192F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(849, 171);
            Controls.Add(panelVersion);
            Controls.Add(panelGamma);
            Margin = new Padding(8, 4, 8, 4);
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            MinimumSize = new Size(821, 71);
            Name = "SettingsForm";
            Padding = new Padding(11);
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "G-Helper";
            panelGamma.ResumeLayout(false);
            panelGammaTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureGamma).EndInit();
            panelVersion.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel panelGamma;
        private Slider sliderGamma;
        private Panel panelGammaTitle;
        private PictureBox pictureGamma;
        private Panel panelVersion;
        private Label labelVersion;
        private Label labelGammaTitle;
        private Label labelGamma;
    }
}
