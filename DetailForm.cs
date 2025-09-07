using System;
using System.Drawing;
using System.Windows.Forms;

namespace FuturisticAntivirus
{
    public partial class DetailForm : Form
    {
        private readonly ScanResult _result;
        private readonly Theme _currentTheme;

        public DetailForm(ScanResult result, Theme theme)
        {
            _result = result;
            _currentTheme = theme;
            InitializeComponent();
            PopulateDetails();
            ApplyTheme();
        }

        private void PopulateDetails()
        {
            lblFileName.Text = System.IO.Path.GetFileName(_result.FilePath);
            txtFilePath.Text = _result.FilePath;
            txtHash.Text = _result.Sha256Hash;
            lblScanDate.Text = $"Scanned on: {_result.ScanDate}";
            lblDetections.Text = $"Detections: {_result.Detections} / {_result.TotalScanners}";

            lvDetections.Items.Clear();
            foreach (var detection in _result.PositiveDetections)
            {
                if (detection.Value != null) // Only show vendors that flagged the file
                {
                    var item = new ListViewItem(detection.Key); // Antivirus engine name
                    item.SubItems.Add(detection.Value); // Result (e.g., "Win.Trojan.Generic-12345")
                    lvDetections.Items.Add(item);
                }
            }
        }

        private void ApplyTheme()
        {
            Color background = _currentTheme == Theme.Dark ? ThemeColors.DarkBackground : ThemeColors.LightBackground;
            Color surface = _currentTheme == Theme.Dark ? ThemeColors.DarkSurface : ThemeColors.LightSurface;
            Color text = _currentTheme == Theme.Dark ? ThemeColors.DarkPrimaryText : ThemeColors.LightPrimaryText;
            Color secondaryText = _currentTheme == Theme.Dark ? ThemeColors.DarkSecondaryText : ThemeColors.LightSecondaryText;

            this.BackColor = background;
            this.ForeColor = text;

            // Apply to all labels
            foreach (Control c in this.Controls)
            {
                if (c is Label) c.ForeColor = text;
            }

            lblFileName.ForeColor = text;
            lblScanDate.ForeColor = secondaryText;
            lblDetections.ForeColor = (_result.Detections > 0) ? ThemeColors.DarkDanger : ThemeColors.DarkSuccess;

            // Apply to TextBoxes
            txtFilePath.BackColor = surface;
            txtFilePath.ForeColor = secondaryText;
            txtHash.BackColor = surface;
            txtHash.ForeColor = secondaryText;

            // Apply to ListView
            lvDetections.BackColor = surface;
            lvDetections.ForeColor = text;

            // Apply to Button
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderColor = secondaryText;
            btnClose.BackColor = surface;
            btnClose.ForeColor = text;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}