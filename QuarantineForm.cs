using System;
using System.Drawing;
using System.Windows.Forms;

namespace FuturisticAntivirus
{
    public partial class QuarantineForm : Form
    {
        private readonly Theme _currentTheme;

        public QuarantineForm(Theme theme)
        {
            _currentTheme = theme;
            InitializeComponent();
            ApplyTheme();
            LoadQuarantinedFiles();
        }

        private void LoadQuarantinedFiles()
        {
            lvQuarantine.Items.Clear();
            var files = QuarantineManager.GetQuarantinedFiles();
            foreach (var file in files)
            {
                var item = new ListViewItem(file.OriginalFileName);
                item.SubItems.Add(file.OriginalPath);
                item.SubItems.Add(file.DateQuarantined.ToString("yyyy-MM-dd HH:mm:ss"));
                item.Tag = file;
                lvQuarantine.Items.Add(item);
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (lvQuarantine.SelectedItems.Count == 0) return;

            var item = lvQuarantine.SelectedItems[0];
            var fileInfo = (QuarantinedFileInfo)item.Tag;

            if (QuarantineManager.RestoreFile(fileInfo.QuarantinedFileName))
            {
                MessageBox.Show("File restored successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadQuarantinedFiles(); // Refresh the list
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvQuarantine.SelectedItems.Count == 0) return;

            var item = lvQuarantine.SelectedItems[0];
            var fileInfo = (QuarantinedFileInfo)item.Tag;

            var result = MessageBox.Show($"Are you sure you want to permanently delete this file?\n\n{fileInfo.OriginalFileName}",
                "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                if (QuarantineManager.DeleteFile(fileInfo.QuarantinedFileName))
                {
                    MessageBox.Show("File permanently deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadQuarantinedFiles(); // Refresh the list
                }
            }
        }

        private void ApplyTheme()
        {
            Color background = _currentTheme == Theme.Dark ? ThemeColors.DarkBackground : ThemeColors.LightBackground;
            Color surface = _currentTheme == Theme.Dark ? ThemeColors.DarkSurface : ThemeColors.LightSurface;
            Color text = _currentTheme == Theme.Dark ? ThemeColors.DarkPrimaryText : ThemeColors.LightPrimaryText;
            Color accent = _currentTheme == Theme.Dark ? ThemeColors.DarkAccent : ThemeColors.LightAccent;

            this.BackColor = background;
            this.ForeColor = text;
            lvQuarantine.BackColor = surface;
            lvQuarantine.ForeColor = text;

            var buttons = new[] { btnRestore, btnDelete };
            foreach (var btn in buttons)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = accent;
                btn.BackColor = surface;
                btn.ForeColor = text;
            }
        }
    }
}