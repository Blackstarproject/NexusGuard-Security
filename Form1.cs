using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FuturisticAntivirus
{
    public partial class Form1 : Form
    {
        #region Fields and Properties
        private readonly VirusTotalScanner _virusTotalScanner;
        private readonly FileSystemScanner _fileSystemScanner;
        private CancellationTokenSource _cancellationSource;
        private Theme _currentTheme = Theme.Dark;
        private bool _isScanPaused = false;

        // UI Controls are declared here and initialized in the Designer.cs file
        private Button btnScanFile, btnScanDirectory, btnFullScan, btnPauseScan, btnCancelScan, btnExport, btnQuarantine;
        private Label lblTitle, lblApiKey;
        private TextBox txtApiKey;
        private ListView lvResults;
        private ProgressBar pbScanProgress;
        private CheckBox chkThemeToggle, chkLowCpu;
        private Panel topPanel;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblFilesScanned, lblThreatsFound, lblCurrentAction;
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem quarantineMenuItem;
        #endregion

        #region Constructor
        public Form1()
        {
            // Initializes all controls defined in the designer file.
            InitializeComponent();

            // Custom initialization that cannot be in the designer.
            lvResults.SmallImageList = CreateStatusImageList();

            // Initialize backend components.
            _virusTotalScanner = new VirusTotalScanner();
            _fileSystemScanner = new FileSystemScanner();

            // Load saved settings.
            txtApiKey.Text = SettingsManager.LoadApiKey();
            if (!string.IsNullOrEmpty(txtApiKey.Text))
            {
                _virusTotalScanner.ApiKey = txtApiKey.Text;
            }

            // Apply the default look and feel.
            ApplyTheme(_currentTheme);
            UpdateStatusLabel("Ready. Select a scan option to begin.");
        }
        #endregion

        #region Theming and Icon Creation
        private void ApplyTheme(Theme theme)
        {
            Color background = theme == Theme.Dark ? ThemeColors.DarkBackground : ThemeColors.LightBackground;
            Color surface = theme == Theme.Dark ? ThemeColors.DarkSurface : ThemeColors.LightSurface;
            Color text = theme == Theme.Dark ? ThemeColors.DarkPrimaryText : ThemeColors.LightPrimaryText;
            Color secondaryText = theme == Theme.Dark ? ThemeColors.DarkSecondaryText : ThemeColors.LightSecondaryText;
            Color accent = theme == Theme.Dark ? ThemeColors.DarkAccent : ThemeColors.LightAccent;

            this.BackColor = background;
            this.ForeColor = text;
            this.statusStrip.BackColor = surface;
            topPanel.BackColor = surface;
            lblTitle.ForeColor = text;
            lblApiKey.ForeColor = secondaryText;
            lblFilesScanned.ForeColor = secondaryText;
            lblThreatsFound.ForeColor = secondaryText;
            lblCurrentAction.ForeColor = secondaryText;
            txtApiKey.BackColor = background;
            txtApiKey.ForeColor = text;
            txtApiKey.BorderStyle = BorderStyle.FixedSingle;
            lvResults.BackColor = surface;
            lvResults.ForeColor = text;

            var buttons = new[] { btnScanFile, btnScanDirectory, btnFullScan, btnCancelScan, btnExport, btnQuarantine, btnPauseScan };
            foreach (var btn in buttons)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = accent;
                btn.BackColor = surface;
                btn.ForeColor = text;
            }
            btnCancelScan.FlatAppearance.BorderColor = ThemeColors.DarkDanger;
            chkThemeToggle.ForeColor = secondaryText;
            chkLowCpu.ForeColor = secondaryText;
        }

        private ImageList CreateStatusImageList()
        {
            var imageList = new ImageList
            {
                ColorDepth = ColorDepth.Depth32Bit
            };
            var bmp = new Bitmap(16, 16);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                g.FillEllipse(Brushes.Green, 2, 2, 12, 12);
                imageList.Images.Add("clean", (Image)bmp.Clone());
                g.Clear(Color.Transparent);
                g.FillEllipse(Brushes.Red, 2, 2, 12, 12);
                imageList.Images.Add("malicious", (Image)bmp.Clone());
                g.Clear(Color.Transparent);
                g.FillEllipse(Brushes.Gray, 2, 2, 12, 12);
                imageList.Images.Add("unknown", (Image)bmp.Clone());
            }
            bmp.Dispose();
            return imageList;
        }
        #endregion

        #region Event Handlers
        private void ChkThemeToggle_CheckedChanged(object sender, EventArgs e)
        {
            _currentTheme = chkThemeToggle.Checked ? Theme.Light : Theme.Dark;
            chkThemeToggle.Text = chkThemeToggle.Checked ? "Dark Mode" : "Light Mode";
            ApplyTheme(_currentTheme);
            foreach (ListViewItem item in lvResults.Items)
            {
                UpdateItemColor(item);
            }
        }

        private async void BtnScanFile_Click(object sender, EventArgs e)
        {
            if (!IsApiKeyValid()) return;
            using (var ofd = new OpenFileDialog { Title = "Select a file to scan" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    await StartScan(new List<string> { ofd.FileName });
                }
            }
        }

        private async void BtnScanDirectory_Click(object sender, EventArgs e)
        {
            if (!IsApiKeyValid()) return;
            using (var fbd = new FolderBrowserDialog { Description = "Select a directory to scan" })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    UpdateStatusLabel("Discovering files...");
                    var files = await _fileSystemScanner.GetFilesInDirectoryAsync(fbd.SelectedPath);
                    await StartScan(files);
                }
            }
        }

        private async void BtnFullScan_Click(object sender, EventArgs e)
        {
            if (!IsApiKeyValid()) return;
            var result = MessageBox.Show("A full system scan can take a very long time. Are you sure?", "Confirm Full Scan", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                UpdateStatusLabel("Discovering files...");
                var files = await _fileSystemScanner.GetAllSystemFilesAsync();
                await StartScan(files);
            }
        }

        private void BtnPauseScan_Click(object sender, EventArgs e)
        {
            _isScanPaused = !_isScanPaused;
            btnPauseScan.Text = _isScanPaused ? "Resume Scan" : "Pause Scan";
        }

        private void BtnCancelScan_Click(object sender, EventArgs e)
        {
            if (_cancellationSource != null && !_cancellationSource.IsCancellationRequested)
            {
                _cancellationSource.Cancel();
            }
        }

        private void BtnQuarantine_Click(object sender, EventArgs e)
        {
            using (var quarantineForm = new QuarantineForm(_currentTheme))
            {
                quarantineForm.ShowDialog();
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (lvResults.Items.Count == 0) return;
            using (var sfd = new SaveFileDialog { Filter = "CSV files (*.csv)|*.csv", RestoreDirectory = true, FileName = $"ScanResults_{DateTime.Now:yyyyMMdd_HHmmss}.csv" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var sb = new StringBuilder();
                        sb.AppendLine("File Path,Status,Detections,Scan Date");
                        foreach (ListViewItem item in lvResults.Items)
                        {
                            sb.AppendLine($"\"{item.SubItems[0].Text}\",{item.SubItems[1].Text},{item.SubItems[2].Text},{item.SubItems[3].Text}");
                        }
                        File.WriteAllText(sfd.FileName, sb.ToString());
                        MessageBox.Show("Results exported successfully!", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to export results: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void LvResults_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && lvResults.FocusedItem != null && lvResults.FocusedItem.Bounds.Contains(e.Location))
            {
                var result = (ScanResult)lvResults.FocusedItem.Tag;
                quarantineMenuItem.Visible = result.Detections > 0 && result.Status != "Quarantined";
                contextMenu.Show(Cursor.Position);
            }
        }

        private void LvResults_DoubleClick(object sender, EventArgs e)
        {
            if (lvResults.SelectedItems.Count == 1)
            {
                var result = (ScanResult)lvResults.SelectedItems[0].Tag;
                using (var detailForm = new DetailForm(result, _currentTheme))
                {
                    detailForm.ShowDialog();
                }
            }
        }

        private void QuarantineMenuItem_Click(object sender, EventArgs e)
        {
            if (lvResults.SelectedItems.Count > 0)
            {
                var item = lvResults.SelectedItems[0];
                var result = (ScanResult)item.Tag;
                if (MessageBox.Show($"Are you sure you want to quarantine this file?\n\n{result.FilePath}", "Confirm Quarantine", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (QuarantineManager.QuarantineFile(result.FilePath))
                    {
                        result.Status = "Quarantined";
                        item.SubItems[1].Text = "Quarantined";
                        UpdateItemColor(item);
                        MessageBox.Show("File successfully quarantined.", "Quarantine Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
        #endregion

        #region Core Scan Logic
        private async Task StartScan(List<string> filesToScan)
        {
            if (filesToScan == null || filesToScan.Count == 0)
            {
                MessageBox.Show("No files were found to scan.", "Scan Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            lvResults.Items.Clear();
            SetScanInProgress(true);
            _cancellationSource = new CancellationTokenSource();
            _isScanPaused = false;

            // Set scanner mode based on UI checkbox
            _virusTotalScanner.SetLowCpuMode(chkLowCpu.Checked);

            pbScanProgress.Maximum = filesToScan.Count;
            pbScanProgress.Value = 0;
            int filesScanned = 0;
            int threatsFound = 0;
            UpdateScanStats(0, 0);

            var scanTasks = new List<Task>();
            foreach (var filePath in filesToScan)
            {
                if (_cancellationSource.IsCancellationRequested) break;

                var scanTask = Task.Run(async () =>
                {
                    this.Invoke(new Action(() => UpdateStatusLabel($"Scanning: {Path.GetFileName(filePath)}")));

                    while (_isScanPaused)
                    {
                        _cancellationSource.Token.ThrowIfCancellationRequested();
                        await Task.Delay(100, _cancellationSource.Token);
                    }
                    _cancellationSource.Token.ThrowIfCancellationRequested();

                    var result = await _virusTotalScanner.ScanFileAsync(filePath, _cancellationSource.Token);

                    if (result != null)
                    {
                        this.Invoke(new Action(() =>
                        {
                            if (result.Detections > 0) threatsFound++;
                            AddResultToListView(result);
                            filesScanned++;
                            UpdateScanStats(filesScanned, threatsFound);
                            pbScanProgress.Value = filesScanned;
                        }));
                    }
                }, _cancellationSource.Token);

                scanTasks.Add(scanTask);
            }

            try
            {
                await Task.WhenAll(scanTasks);
            }
            catch (OperationCanceledException) { /* Expected */ }
            catch (Exception ex)
            {
                var realException = ex is AggregateException ? ex.InnerException : ex;
                if (!(realException is OperationCanceledException))
                {
                    MessageBox.Show($"An error occurred: {realException.Message}", "Scan Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                if (_cancellationSource.IsCancellationRequested)
                {
                    UpdateStatusLabel($"Scan cancelled. Scanned {filesScanned} of {filesToScan.Count} file(s).");
                }
                else
                {
                    UpdateStatusLabel($"Scan complete. Found {threatsFound} potential threat(s).");
                }
                SetScanInProgress(false);
                _cancellationSource.Dispose();
            }
        }

        private void AddResultToListView(ScanResult result)
        {
            string status = result.IsFromCache ? $"{result.Status} (cached)" : result.Status;

            string displayDate = "N/A";
            if (DateTime.TryParse(result.ScanDate, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.RoundtripKind, out var dt))
            {
                displayDate = dt.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
            }

            var item = new ListViewItem(new[] { result.FilePath, status, result.Detections.ToString(), displayDate })
            {
                Tag = result
            };
            if (result.Detections > 0) item.ImageKey = "malicious";
            else if (result.Status == "Clean") item.ImageKey = "clean";
            else item.ImageKey = "unknown";

            UpdateItemColor(item);
            lvResults.Items.Add(item);
        }

        private void UpdateItemColor(ListViewItem item)
        {
            if (!(item.Tag is ScanResult result)) return;
            var theme = _currentTheme;
            Color backColor;
            if (result.Status == "Quarantined") backColor = theme == Theme.Dark ? Color.FromArgb(100, 100, 0) : Color.LightGoldenrodYellow;
            else if (result.Detections > 0) backColor = theme == Theme.Dark ? ThemeColors.DarkDanger : ThemeColors.LightDanger;
            else if (result.Status == "Clean") backColor = theme == Theme.Dark ? ThemeColors.DarkSuccess : ThemeColors.LightSuccess;
            else backColor = theme == Theme.Dark ? ThemeColors.DarkSurface : ThemeColors.LightSurface;
            item.BackColor = backColor;
        }
        #endregion

        #region UI Helper Methods
        private bool IsApiKeyValid()
        {
            if (string.IsNullOrWhiteSpace(txtApiKey.Text))
            {
                MessageBox.Show("Please enter your VirusTotal API key.", "API Key Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            SettingsManager.SaveApiKey(txtApiKey.Text);
            _virusTotalScanner.ApiKey = txtApiKey.Text;
            return true;
        }

        private void SetScanInProgress(bool inProgress)
        {
            btnScanFile.Enabled = !inProgress;
            btnScanDirectory.Enabled = !inProgress;
            btnFullScan.Enabled = !inProgress;
            txtApiKey.Enabled = !inProgress;
            btnQuarantine.Enabled = !inProgress;
            btnExport.Enabled = !inProgress;
            chkLowCpu.Enabled = !inProgress; // Disable CPU option during scan
            btnPauseScan.Enabled = inProgress;
            btnCancelScan.Enabled = inProgress;
            if (!inProgress)
            {
                btnPauseScan.Text = "Pause Scan";
                _isScanPaused = false;
            }
        }

        private void UpdateScanStats(int scanned, int threats)
        {
            if (statusStrip.InvokeRequired)
            {
                statusStrip.Invoke(new Action(() =>
                {
                    lblFilesScanned.Text = $"Files Scanned: {scanned}";
                    lblThreatsFound.Text = $"Threats Found: {threats}";
                }));
            }
            else
            {
                lblFilesScanned.Text = $"Files Scanned: {scanned}";
                lblThreatsFound.Text = $"Threats Found: {threats}";
            }
        }

        private void UpdateStatusLabel(string text)
        {
            if (statusStrip.InvokeRequired)
            {
                statusStrip.Invoke(new Action(() => lblCurrentAction.Text = text));
            }
            else
            {
                lblCurrentAction.Text = text;
            }
        }
        #endregion
    }
}