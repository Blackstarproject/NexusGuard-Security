namespace FuturisticAntivirus
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 661);
            this.Text = "NexusGuard Security";
            this.MinimumSize = new System.Drawing.Size(800, 600);

            // Main Layout & Panels
            var mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.topPanel = new System.Windows.Forms.Panel();
            var buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();

            this.lblTitle = new System.Windows.Forms.Label();
            this.lblApiKey = new System.Windows.Forms.Label();
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.chkThemeToggle = new System.Windows.Forms.CheckBox();
            this.chkLowCpu = new System.Windows.Forms.CheckBox();

            mainLayoutPanel.SuspendLayout();
            this.topPanel.SuspendLayout();
            buttonPanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();

            // Main Layout Panel
            mainLayoutPanel.ColumnCount = 1;
            mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            mainLayoutPanel.RowCount = 4;
            mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            mainLayoutPanel.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);

            // Top Panel
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topPanel.Controls.Add(this.lblTitle);
            this.topPanel.Controls.Add(this.lblApiKey);
            this.topPanel.Controls.Add(this.txtApiKey);
            this.topPanel.Controls.Add(this.chkThemeToggle);
            this.topPanel.Controls.Add(this.chkLowCpu);

            this.lblTitle.Text = "NexusGuard Security";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Light", 24F);
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(10, 10);

            this.lblApiKey.Text = "VirusTotal API Key:";
            this.lblApiKey.AutoSize = true;
            this.lblApiKey.Location = new System.Drawing.Point(15, 65);

            this.txtApiKey.Width = 300;
            this.txtApiKey.Location = new System.Drawing.Point(130, 62);

            this.chkThemeToggle.Text = "Light Mode";
            this.chkThemeToggle.AutoSize = true;
            this.chkThemeToggle.Location = new System.Drawing.Point(750, 20);
            this.chkThemeToggle.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);

            // --- FIX IS HERE ---
            this.chkLowCpu.Text = "Low CPU usage";
            this.chkLowCpu.AutoSize = true;
            this.chkLowCpu.Location = new System.Drawing.Point(750, 45);
            this.chkLowCpu.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);

            // Button Panel
            this.btnScanFile = new System.Windows.Forms.Button();
            this.btnScanDirectory = new System.Windows.Forms.Button();
            this.btnFullScan = new System.Windows.Forms.Button();
            this.btnPauseScan = new System.Windows.Forms.Button();
            this.btnCancelScan = new System.Windows.Forms.Button();
            this.btnQuarantine = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();

            buttonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.btnScanFile.Text = "Scan File";
            this.btnScanFile.Size = new System.Drawing.Size(120, 40);
            this.btnScanDirectory.Text = "Scan Directory";
            this.btnScanDirectory.Size = new System.Drawing.Size(120, 40);
            this.btnFullScan.Text = "Full System Scan";
            this.btnFullScan.Size = new System.Drawing.Size(120, 40);
            this.btnPauseScan.Text = "Pause Scan";
            this.btnPauseScan.Size = new System.Drawing.Size(120, 40);
            this.btnPauseScan.Enabled = false;
            this.btnCancelScan.Text = "Cancel Scan";
            this.btnCancelScan.Size = new System.Drawing.Size(120, 40);
            this.btnCancelScan.Enabled = false;
            this.btnQuarantine.Text = "View Quarantine";
            this.btnQuarantine.Size = new System.Drawing.Size(120, 40);
            this.btnExport.Text = "Export Results";
            this.btnExport.Size = new System.Drawing.Size(120, 40);

            buttonPanel.Controls.Add(this.btnScanFile);
            buttonPanel.Controls.Add(this.btnScanDirectory);
            buttonPanel.Controls.Add(this.btnFullScan);
            buttonPanel.Controls.Add(this.btnPauseScan);
            buttonPanel.Controls.Add(this.btnCancelScan);
            buttonPanel.Controls.Add(this.btnQuarantine);
            buttonPanel.Controls.Add(this.btnExport);

            // Results ListView
            this.lvResults = new System.Windows.Forms.ListView();
            this.lvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvResults.View = System.Windows.Forms.View.Details;
            this.lvResults.FullRowSelect = true;
            this.lvResults.Columns.Add("File Path", 400);
            this.lvResults.Columns.Add("Status", 120);
            this.lvResults.Columns.Add("Detections", 100);
            this.lvResults.Columns.Add("Scan Date", 150);

            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.quarantineMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.quarantineMenuItem });
            this.quarantineMenuItem.Text = "Quarantine File";

            this.pbScanProgress = new System.Windows.Forms.ProgressBar();
            this.pbScanProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbScanProgress.Height = 15;

            // Status Strip
            this.lblFilesScanned = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblThreatsFound = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCurrentAction = new System.Windows.Forms.ToolStripStatusLabel();

            this.statusStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusStrip.SizingGrip = false;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.lblCurrentAction, this.lblFilesScanned, this.lblThreatsFound });

            this.lblCurrentAction.Name = "lblCurrentAction";
            this.lblCurrentAction.Size = new System.Drawing.Size(550, 17);
            this.lblCurrentAction.Spring = true;
            this.lblCurrentAction.Text = "Ready";
            this.lblCurrentAction.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.lblFilesScanned.Name = "lblFilesScanned";
            this.lblFilesScanned.Size = new System.Drawing.Size(100, 17);
            this.lblFilesScanned.Text = "Files Scanned: 0";

            this.lblThreatsFound.Name = "lblThreatsFound";
            this.lblThreatsFound.Size = new System.Drawing.Size(90, 17);
            this.lblThreatsFound.Text = "Threats Found: 0";

            // Final Assembly
            mainLayoutPanel.Controls.Add(this.topPanel, 0, 0);
            mainLayoutPanel.Controls.Add(buttonPanel, 0, 1);
            mainLayoutPanel.Controls.Add(this.lvResults, 0, 2);
            mainLayoutPanel.Controls.Add(this.pbScanProgress, 0, 3);
            this.Controls.Add(mainLayoutPanel);
            this.Controls.Add(this.statusStrip);

            mainLayoutPanel.ResumeLayout(false);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            buttonPanel.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

            // Event Wiring
            this.chkThemeToggle.CheckedChanged += new System.EventHandler(this.ChkThemeToggle_CheckedChanged);
            this.btnScanFile.Click += new System.EventHandler(this.BtnScanFile_Click);
            this.btnScanDirectory.Click += new System.EventHandler(this.BtnScanDirectory_Click);
            this.btnFullScan.Click += new System.EventHandler(this.BtnFullScan_Click);
            this.btnPauseScan.Click += new System.EventHandler(this.BtnPauseScan_Click);
            this.btnCancelScan.Click += new System.EventHandler(this.BtnCancelScan_Click);
            this.btnQuarantine.Click += new System.EventHandler(this.BtnQuarantine_Click);
            this.btnExport.Click += new System.EventHandler(this.BtnExport_Click);
            this.lvResults.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LvResults_MouseClick);
            this.lvResults.DoubleClick += new System.EventHandler(this.LvResults_DoubleClick);
            this.quarantineMenuItem.Click += new System.EventHandler(this.QuarantineMenuItem_Click);
        }
        #endregion
    }
}